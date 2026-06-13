using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace EmontiOptometrist.Web.Pages;

[Authorize]
public class AppointmentsModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentsModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<AppointmentViewModel> Appointments { get; set; } = new();
    public bool HasAppointments => Appointments.Count > 0;

    public void OnGet()
    {
        var custId = _httpContextAccessor.HttpContext?.User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(custId))
            return;

        var connStr = _configuration.GetConnectionString("ProductConnection");
        using var conn = new SqlConnection(connStr);
        conn.Open();

        string query = @"
            SELECT a.Appointment_ID, a.Appointment_Date, a.Appoinment_Status,
                   s.Staff_Name, s.Staff_Surname, t.Timeslot
            FROM Appointment a
            LEFT JOIN Staff s ON a.Staff_ID = s.Staff_ID
            LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
            WHERE a.Cust_ID = @CustId
            ORDER BY a.Appointment_Date DESC";

        using var cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@CustId", custId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var date = Convert.ToDateTime(reader["Appointment_Date"]);
            var now = DateTime.Now;

            string type;
            if (reader["Appoinment_Status"].ToString() == "Cancelled")
                type = "Cancelled";
            else if (date.Date < now.Date)
                type = "Past";
            else
                type = "Upcoming";

            Appointments.Add(new AppointmentViewModel
            {
                AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                AppointmentDate = date,
                Status = reader["Appoinment_Status"].ToString(),
                Type = type,
                TimeSlot = reader["Timeslot"]?.ToString() ?? "",
                DoctorName = reader["Staff_Name"]?.ToString() ?? ""
            });
        }
    }
}

public class AppointmentViewModel
{
    public int AppointmentId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string Status { get; set; } = "";
    public string Type { get; set; } = "";
    public string TimeSlot { get; set; } = "";
    public string DoctorName { get; set; } = "";
}
