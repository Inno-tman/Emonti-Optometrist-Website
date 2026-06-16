using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class AppointmentsModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppointmentsModel> _logger;

    public AppointmentsModel(IConfiguration configuration, ILogger<AppointmentsModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public List<AppointmentViewModel> Appointments { get; set; } = new();
    public bool HasAppointments => Appointments.Count > 0;
    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        LoadAppointments();
    }

    private void LoadAppointments()
    {
        Appointments.Clear();
        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
            return;

        var connStr = _configuration.GetConnectionString("DefaultConnection");
        using var conn = new SqliteConnection(connStr);
        conn.Open();

        string query = @"
            SELECT a.Appointment_ID, a.Appointment_Date, a.Appoinment_Status,
                   s.Staff_Name, s.Staff_Surname, t.Timeslot
            FROM Appointment a
            LEFT JOIN Staff s ON a.Staff_ID = s.Staff_ID
            LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
            WHERE a.Cust_ID = @CustId
            ORDER BY a.Appointment_Date DESC";

        using var cmd = conn.CreateCommand();
        cmd.CommandText = query;
        cmd.Parameters.AddWithValue("@CustId", custId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var date = DateTime.Parse(reader["Appointment_Date"].ToString());
            var status = reader["Appoinment_Status"].ToString();

            string type;
            if (status == "Cancelled")
                type = "Cancelled";
            else if (date.Date < DateTime.Today)
                type = "Past";
            else
                type = "Upcoming";

            var timeSlot = reader["Timeslot"]?.ToString() ?? "";
            var canCancel = status != "Cancelled" && CanCancelAppointment(date, timeSlot);

            Appointments.Add(new AppointmentViewModel
            {
                AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                AppointmentDate = date,
                Status = status,
                Type = type,
                TimeSlot = timeSlot,
                DoctorName = reader["Staff_Name"]?.ToString() ?? "",
                CanCancel = canCancel
            });
        }
    }

    private bool CanCancelAppointment(DateTime appointmentDate, string timeSlot)
    {
        if (appointmentDate.Date < DateTime.Today)
            return false;

        var fullDateTime = appointmentDate.Date;
        if (!string.IsNullOrEmpty(timeSlot))
        {
            var startPart = timeSlot.Split('-')[0].Trim();
            if (DateTime.TryParse(startPart, out var parsed))
                fullDateTime = appointmentDate.Date.Add(parsed.TimeOfDay);
        }

        return fullDateTime > DateTime.Now.AddHours(2);
    }

    public IActionResult OnPostCancelAppointment(int appointmentId)
    {
        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
        {
            ErrorMessage = "Please log in to cancel an appointment.";
            LoadAppointments();
            return Page();
        }

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = @"
                SELECT Appointment_Date FROM Appointment
                WHERE Appointment_ID = @Id AND Cust_ID = @CustId
                AND Appoinment_Status != 'Cancelled'";
            checkCmd.Parameters.AddWithValue("@Id", appointmentId);
            checkCmd.Parameters.AddWithValue("@CustId", custId);

            using var reader = checkCmd.ExecuteReader();
            if (!reader.Read())
            {
                ErrorMessage = "Appointment not found or already cancelled.";
                LoadAppointments();
                return Page();
            }
            reader.Close();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Appointment SET Appoinment_Status = 'Cancelled' WHERE Appointment_ID = @Id";
            cmd.Parameters.AddWithValue("@Id", appointmentId);
            cmd.ExecuteNonQuery();

            SuccessMessage = "Appointment cancelled successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to cancel appointment {Id}", appointmentId);
            ErrorMessage = "Failed to cancel appointment.";
        }

        LoadAppointments();
        return Page();
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
    public bool CanCancel { get; set; }
}
