using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace EmontiOptometrist.Web.Pages.Staff;

[Authorize(Roles = "Admin,Staff")]
public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DashboardModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public string StaffName { get; set; } = "";
    public string StaffRole { get; set; } = "";
    public int TodayAppointments { get; set; }
    public int PendingOrders { get; set; }
    public int TotalPatients { get; set; }
    public int UpcomingCount { get; set; }
    public List<UpcomingAppointment> UpcomingAppointments { get; set; } = new();

    public void OnGet()
    {
        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";
        var staffId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        StaffName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Staff";
        StaffRole = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role) ?? "Staff";

        if (string.IsNullOrEmpty(connStr))
            return;

        try
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = new SqlCommand(@"
                    SELECT COUNT(*) FROM Appointment
                    WHERE Staff_ID = @StaffId
                    AND CAST(Appointment_Date AS DATE) = CAST(GETDATE() AS DATE)
                    AND Appoinment_Status != 'Cancelled'", conn))
                {
                    cmd.Parameters.AddWithValue("@StaffId", staffId);
                    TodayAppointments = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = new SqlCommand(@"
                    SELECT COUNT(*) FROM [Order]
                    WHERE Order_Status IN ('Pending', 'Processing')", conn))
                {
                    PendingOrders = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = new SqlCommand(@"
                    SELECT COUNT(DISTINCT Cust_ID) FROM Appointment", conn))
                {
                    TotalPatients = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = new SqlCommand(@"
                    SELECT TOP 5
                        a.Appointment_ID,
                        a.Appointment_Date,
                        a.Appoinment_Status,
                        t.Timeslot,
                        c.Customer_Name,
                        c.Customer_Surname
                    FROM Appointment a
                    INNER JOIN customer c ON a.Cust_ID = c.Cust_ID
                    LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                    WHERE a.Staff_ID = @StaffId
                    AND CAST(a.Appointment_Date AS DATE) >= CAST(GETDATE() AS DATE)
                    AND a.Appoinment_Status != 'Cancelled'
                    ORDER BY a.Appointment_Date ASC, t.Timeslot ASC", conn))
                {
                    cmd.Parameters.AddWithValue("@StaffId", staffId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UpcomingAppointments.Add(new UpcomingAppointment
                            {
                                AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                                AppointmentDate = Convert.ToDateTime(reader["Appointment_Date"]),
                                Status = reader["Appoinment_Status"]?.ToString() ?? "",
                                Timeslot = reader["Timeslot"]?.ToString() ?? "N/A",
                                PatientName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}"
                            });
                        }
                    }
                }

                UpcomingCount = UpcomingAppointments.Count;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Staff Dashboard error: {ex.Message}");
        }
    }
}

public class UpcomingAppointment
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "";
    public string Timeslot { get; set; } = "";
    public string PatientName { get; set; } = "";
}
