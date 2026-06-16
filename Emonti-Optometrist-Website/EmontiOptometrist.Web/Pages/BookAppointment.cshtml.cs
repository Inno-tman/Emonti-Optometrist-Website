using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class BookAppointmentModel : PageModel
{
    private readonly IConfiguration _configuration;

    public BookAppointmentModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [BindProperty]
    public AppointmentInput Input { get; set; } = new();

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    public List<TimeSlotItem> TimeSlots { get; set; } = new()
    {
        new() { Value = "1", Text = "08:00 - 09:00", Group = "Morning" },
        new() { Value = "2", Text = "09:00 - 10:00", Group = "Morning" },
        new() { Value = "3", Text = "10:00 - 11:00", Group = "Morning" },
        new() { Value = "4", Text = "11:00 - 12:00", Group = "Morning" },
        new() { Value = "5", Text = "13:00 - 14:00", Group = "Afternoon" },
        new() { Value = "6", Text = "14:00 - 15:00", Group = "Afternoon" },
        new() { Value = "7", Text = "15:00 - 16:00", Group = "Afternoon" },
    };

    public List<StaffItem> Optometrists { get; set; } = new();

    public void OnGet()
    {
        Input.PreferredDate = DateTime.Today;
        LoadOptometrists();
    }

    private void LoadOptometrists()
    {
        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Staff_ID, Staff_Name, Staff_Surname FROM Staff WHERE Staff_Role = 'Optometrist' ORDER BY Staff_Name, Staff_Surname";
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                Optometrists.Add(new StaffItem { Value = rdr["Staff_ID"].ToString()!, Text = $"{rdr["Staff_Name"]} {rdr["Staff_Surname"]}" });
        }
        catch { }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
        {
            ErrorMessage = "Please log in to book an appointment.";
            return Page();
        }

        LoadOptometrists();

        var date = Input.PreferredDate.Date;

        if (date < DateTime.Today)
        {
            ErrorMessage = "Please select a future date for your appointment.";
            return Page();
        }

        if (date.DayOfWeek == DayOfWeek.Sunday)
        {
            ErrorMessage = "We are closed on Sundays. Please select a different date.";
            return Page();
        }

        var slot = TimeSlots.FirstOrDefault(s => s.Value == Input.PreferredTime);
        if (slot == null)
        {
            ErrorMessage = "Please select a valid time slot.";
            return Page();
        }

        if (!DateTime.TryParse(slot.Text.Split(" - ")[0], out var parsedStart))
        {
            ErrorMessage = "Invalid time slot.";
            return Page();
        }

        var slotStart = parsedStart.TimeOfDay;
        var businessClose = date.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(14, 0, 0) : new TimeSpan(17, 0, 0);
        if (slotStart < new TimeSpan(8, 0, 0) || slotStart >= businessClose)
        {
            ErrorMessage = "The selected time slot is outside our business hours.";
            return Page();
        }

        if (date == DateTime.Today)
        {
            var slotDateTime = DateTime.Today.Add(slotStart);
            if (DateTime.Now >= slotDateTime)
            {
                ErrorMessage = "This time slot has already passed. Please select a future time slot.";
                return Page();
            }
            if (slotDateTime <= DateTime.Now.AddHours(2))
            {
                ErrorMessage = "Same-day appointments require at least 2 hours advance notice.";
                return Page();
            }
        }

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using var perDayCmd = conn.CreateCommand();
            perDayCmd.CommandText = @"
                SELECT COUNT(*) FROM Appointment
                WHERE Cust_ID = @CustId AND Appointment_Date = @AppointmentDate
                AND Appoinment_Status != 'Cancelled'";
            perDayCmd.Parameters.AddWithValue("@CustId", custId);
            perDayCmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("o"));
            if ((long)perDayCmd.ExecuteScalar()! > 0)
            {
                ErrorMessage = "You already have an appointment scheduled on this date. You can only book one appointment per day.";
                return Page();
            }

            using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = @"
                SELECT COUNT(*) FROM Appointment
                WHERE Appointment_Date = @AppointmentDate
                AND AppointmentTimeID = @AppointmentTimeId
                AND Appoinment_Status != 'Cancelled'";
            checkCmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("o"));
            checkCmd.Parameters.AddWithValue("@AppointmentTimeId", Input.PreferredTime);
            if ((long)checkCmd.ExecuteScalar()! > 0)
            {
                ErrorMessage = "This time slot is already booked. Please select a different date or time.";
                return Page();
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Appointment
                    (Cust_ID, Staff_ID, Appointment_Date, AppointmentTimeID, Appoinment_Status)
                VALUES
                    (@CustId, @StaffId, @AppointmentDate, @AppointmentTimeId, @Status)";
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.Parameters.AddWithValue("@StaffId", Input.OptometristId);
            cmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("o"));
            cmd.Parameters.AddWithValue("@AppointmentTimeId", Input.PreferredTime);
            cmd.Parameters.AddWithValue("@Status", "Pending");

            cmd.ExecuteNonQuery();

            SuccessMessage = $"Appointment booked successfully! We look forward to seeing you on {date:dddd, MMMM dd, yyyy}.";
            ModelState.Clear();
            Input = new AppointmentInput { PreferredDate = DateTime.Today };
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to book appointment: {ex.Message}";
            return Page();
        }
    }
}

public class AppointmentInput
{
    [Required(ErrorMessage = "Please select an appointment type")]
    [Display(Name = "Appointment Type")]
    public string AppointmentType { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select an optometrist")]
    [Display(Name = "Optometrist")]
    public string OptometristId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select a date")]
    [Display(Name = "Preferred Date")]
    [DataType(DataType.Date)]
    public DateTime PreferredDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Please select a time slot")]
    [Display(Name = "Preferred Time")]
    public string PreferredTime { get; set; } = string.Empty;

    [Required(ErrorMessage = "Patient name is required")]
    [Display(Name = "Patient Name")]
    [StringLength(100)]
    public string PatientName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number")]
    [Display(Name = "Phone")]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;

    [Display(Name = "Notes / Comments")]
    [StringLength(500)]
    public string? Notes { get; set; }
}

public class TimeSlotItem
{
    public string Value { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
}

public class StaffItem
{
    public string Value { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
