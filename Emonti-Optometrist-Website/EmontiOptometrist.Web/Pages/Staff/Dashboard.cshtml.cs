using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Staff;

public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;

    public DashboardModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string StaffName { get; set; } = "";
    public string StaffRole { get; set; } = "";
    public int TodayAppointments { get; set; }
    public int PendingOrders { get; set; }
    public int TotalPatients { get; set; }
    public int UpcomingCount { get; set; }
    public List<UpcomingAppointment> UpcomingAppointments { get; set; } = new();

    public IActionResult OnGet()
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        StaffName = HttpContext.Session.GetString("StaffName") ?? "Staff";
        StaffRole = HttpContext.Session.GetString("StaffRole") ?? "Staff";
        var staffId = HttpContext.Session.GetString("Staff_ID") ?? "";

        if (string.IsNullOrEmpty(connStr))
            return Page();

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT COUNT(*) FROM Appointment
                    WHERE Staff_ID = @StaffId
                    AND date(Appointment_Date) = date('now')
                    AND Appoinment_Status != 'Cancelled'";
                cmd.Parameters.AddWithValue("@StaffId", staffId);
                TodayAppointments = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM [Order] WHERE Order_Status IN ('Pending', 'Processing')";
                PendingOrders = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(DISTINCT Cust_ID) FROM Appointment";
                TotalPatients = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT a.Appointment_ID, a.Appointment_Date, a.Appoinment_Status,
                           t.Timeslot, c.Customer_Name, c.Customer_Surname
                    FROM Appointment a
                    INNER JOIN customer c ON a.Cust_ID = c.Cust_ID
                    LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                    WHERE a.Staff_ID = @StaffId
                    AND date(a.Appointment_Date) >= date('now')
                    AND a.Appoinment_Status != 'Cancelled'
                    ORDER BY a.Appointment_Date ASC, t.Timeslot ASC
                    LIMIT 5";
                cmd.Parameters.AddWithValue("@StaffId", staffId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UpcomingAppointments.Add(new UpcomingAppointment
                        {
                            AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                            AppointmentDate = DateTime.Parse(reader["Appointment_Date"].ToString()),
                            Status = reader["Appoinment_Status"]?.ToString() ?? "",
                            Timeslot = reader["Timeslot"]?.ToString() ?? "N/A",
                            PatientName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}"
                        });
                    }
                }
            }

            UpcomingCount = UpcomingAppointments.Count;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Staff Dashboard error: {ex.Message}");
        }

        return Page();
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
