using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class BookAppointmentModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<BookAppointmentModel> _logger;
    private readonly BrevoEmailService _brevoEmail;

    public BookAppointmentModel(IConfiguration configuration, ILogger<BookAppointmentModel> logger, BrevoEmailService brevoEmail)
    {
        _configuration = configuration;
        _logger = logger;
        _brevoEmail = brevoEmail;
    }

    [BindProperty]
    public AppointmentInput Input { get; set; } = new();

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }
    public bool IsRebooking { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string CustomerPhone { get; set; } = "";

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

    public JsonResult OnGetCheckAvailability(string date, string time, string optometristId)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
            return new JsonResult(new { available = false, message = "Invalid date." });

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
            return new JsonResult(new { available = false, message = "Please log in to book." });

        if (parsedDate.Date < DateTime.Today)
            return new JsonResult(new { available = false, message = "This date has already passed. Please select a future date." });

        if (parsedDate.DayOfWeek == DayOfWeek.Sunday)
            return new JsonResult(new { available = false, message = "We are closed on Sundays. Please select a different date." });

        var slot = TimeSlots.FirstOrDefault(s => s.Value == time);
        if (slot == null)
            return new JsonResult(new { available = true, message = "" });

        if (!DateTime.TryParse(slot.Text.Split(" - ")[0], out var parsedStart))
            return new JsonResult(new { available = false, message = "Invalid time slot." });

        var slotStart = parsedStart.TimeOfDay;
        var businessClose = parsedDate.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(14, 0, 0) : new TimeSpan(17, 0, 0);
        if (slotStart < new TimeSpan(8, 0, 0) || slotStart >= businessClose)
        {
            var closeTime = parsedDate.DayOfWeek == DayOfWeek.Saturday ? "2:00 PM" : "5:00 PM";
            return new JsonResult(new { available = false, message = $"This slot is outside our business hours (8 AM - {closeTime})." });
        }

        if (parsedDate == DateTime.Today)
        {
            var slotDateTime = DateTime.Today.Add(slotStart);
            if (DateTime.Now >= slotDateTime)
                return new JsonResult(new { available = false, message = "This time has already passed. Please select a future slot." });
            if (slotDateTime <= DateTime.Now.AddHours(2))
                return new JsonResult(new { available = false, message = "Same-day bookings need at least 2 hours notice." });
        }

        if (!string.IsNullOrEmpty(time) && !string.IsNullOrEmpty(optometristId))
        {
            try
            {
                var connStr = _configuration.GetConnectionString("DefaultConnection");
                using var conn = new SqliteConnection(connStr);
                conn.Open();

                using var perDayCmd = conn.CreateCommand();
                perDayCmd.CommandText = @"
                    SELECT COUNT(*) FROM Appointment
                    WHERE Cust_ID = @CustId AND date(Appointment_Date) = date(@Date)
                    AND Appoinment_Status != 'Cancelled'";
                perDayCmd.Parameters.AddWithValue("@CustId", custId);
                perDayCmd.Parameters.AddWithValue("@Date", parsedDate.ToString("yyyy-MM-dd"));
                if ((long)perDayCmd.ExecuteScalar()! > 0)
                    return new JsonResult(new { available = false, message = "You already have an appointment on this date. Only one appointment per day allowed." });

                using var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = @"
                    SELECT
                        (SELECT COUNT(*) FROM Appointment
                         WHERE Staff_ID = @StaffId AND date(Appointment_Date) = date(@Date)
                         AND AppointmentTimeID = @TimeId AND Appoinment_Status != 'Cancelled') +
                        (SELECT COUNT(*) FROM BlockedTimeslots
                         WHERE Staff_ID = @StaffId AND date(Blocked_Date) = date(@Date)
                         AND TimeID = @TimeId) AS TotalCount";
                checkCmd.Parameters.AddWithValue("@StaffId", optometristId);
                checkCmd.Parameters.AddWithValue("@Date", parsedDate.ToString("yyyy-MM-dd"));
                checkCmd.Parameters.AddWithValue("@TimeId", time);
                if ((long)checkCmd.ExecuteScalar()! > 0)
                    return new JsonResult(new { available = false, message = "This slot is already booked or blocked by the optometrist." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { available = false, message = $"Error checking availability: {ex.Message}" });
            }
        }

        return new JsonResult(new { available = true, message = "This slot is available." });
    }

    public void OnGet(string? rebook)
    {
        LoadOptometrists();
        LoadCustomerDetails();

        if (!string.IsNullOrEmpty(rebook))
            LoadRebookingData(rebook);
    }

    private void LoadCustomerDetails()
    {
        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId)) return;

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Customer_Name, Customer_Surname, Customer_Email, Customer_Phone FROM customer WHERE Cust_ID = @Id";
            cmd.Parameters.AddWithValue("@Id", custId);
            using var rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                var firstName = rdr["Customer_Name"]?.ToString() ?? "";
                var surname = rdr["Customer_Surname"]?.ToString() ?? "";
                CustomerName = $"{firstName} {surname}".Trim();
                CustomerEmail = rdr["Customer_Email"]?.ToString() ?? "";
                CustomerPhone = rdr["Customer_Phone"]?.ToString() ?? "";
            }
        }
        catch { }
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

    private void LoadRebookingData(string appointmentId)
    {
        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId)) return;

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Staff_ID FROM Appointment
                WHERE Appointment_ID = @Id AND Cust_ID = @CustId
                AND Appoinment_Status != 'Cancelled'";
            cmd.Parameters.AddWithValue("@Id", appointmentId);
            cmd.Parameters.AddWithValue("@CustId", custId);
            var staffId = cmd.ExecuteScalar()?.ToString();
            if (!string.IsNullOrEmpty(staffId))
            {
                Input.OptometristId = staffId;
                IsRebooking = true;
            }
        }
        catch { }
    }

    public IActionResult OnPost()
    {
        LoadOptometrists();
        LoadCustomerDetails();

        if (!ModelState.IsValid)
            return Page();

        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
        {
            ErrorMessage = "Please log in to book an appointment.";
            return Page();
        }

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
            using var tx = conn.BeginTransaction();

            using var perDayCmd = conn.CreateCommand();
            perDayCmd.CommandText = @"
                SELECT COUNT(*) FROM Appointment
                WHERE Cust_ID = @CustId AND date(Appointment_Date) = date(@AppointmentDate)
                AND Appoinment_Status != 'Cancelled'";
            perDayCmd.Parameters.AddWithValue("@CustId", custId);
            perDayCmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("yyyy-MM-dd"));
            if ((long)perDayCmd.ExecuteScalar()! > 0)
            {
                ErrorMessage = "You already have an appointment scheduled on this date. You can only book one appointment per day.";
                return Page();
            }

            using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = @"
                SELECT
                    (SELECT COUNT(*) FROM Appointment
                     WHERE Staff_ID = @StaffId AND date(Appointment_Date) = date(@AppointmentDate)
                     AND AppointmentTimeID = @AppointmentTimeId AND Appoinment_Status != 'Cancelled') +
                    (SELECT COUNT(*) FROM BlockedTimeslots
                     WHERE Staff_ID = @StaffId AND date(Blocked_Date) = date(@AppointmentDate)
                     AND TimeID = @AppointmentTimeId) AS TotalCount";
            checkCmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("yyyy-MM-dd"));
            checkCmd.Parameters.AddWithValue("@AppointmentTimeId", Input.PreferredTime);
            checkCmd.Parameters.AddWithValue("@StaffId", Input.OptometristId);
            if ((long)checkCmd.ExecuteScalar()! > 0)
            {
                ErrorMessage = "This time slot is already booked or unavailable. Please select a different date or time.";
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
            cmd.Parameters.AddWithValue("@AppointmentDate", date.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@AppointmentTimeId", Input.PreferredTime);
            cmd.Parameters.AddWithValue("@Status", "Pending");

            cmd.ExecuteNonQuery();
            tx.Commit();

            var customerEmail = HttpContext.Session.GetString(AuthSession.UserEmail);
            if (!string.IsNullOrEmpty(customerEmail))
                SendConfirmationEmail(customerEmail, date, slot.Text);

            var optometristName = Optometrists.FirstOrDefault(o => o.Value == Input.OptometristId)?.Text ?? "";
            SendOptometristNotification(Input.OptometristId, CustomerName, CustomerPhone, date, slot.Text);
            if (IsRebooking)
                SuccessMessage = $"Rebooking successful! Your appointment has been confirmed for {date:dddd, MMMM dd, yyyy} at {slot.Text} with {optometristName}.";
            else
                SuccessMessage = $"Appointment booked successfully! A confirmation email will be sent to your email address. We look forward to seeing you on {date:dddd, MMMM dd, yyyy} at {slot.Text} with {optometristName}.";

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

    private void SendEmail(string toEmail, string subject, string htmlBody)
    {
        _ = _brevoEmail.SendEmailAsync(toEmail, null, subject, htmlBody);
    }

    private void SendOptometristNotification(string staffId, string patientName, string patientPhone, DateTime date, string slotText)
    {
        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Staff_Name, Staff_Surname, Staff_Email FROM Staff WHERE Staff_ID = @Id";
            cmd.Parameters.AddWithValue("@Id", staffId);
            using var rdr = cmd.ExecuteReader();
            if (!rdr.Read()) return;
            var staffEmail = rdr["Staff_Email"]?.ToString() ?? "";
            if (string.IsNullOrEmpty(staffEmail)) return;
            var staffName = $"{rdr["Staff_Name"]} {rdr["Staff_Surname"]}";

            var body = $@"<!DOCTYPE html>
<html><head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#28a745;padding:25px;text-align:center;"">
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">New Appointment Booking</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;"">Dear {staffName},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;"">A new appointment has been booked with you. Please log in to the staff dashboard to accept or manage this booking.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #28a745;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;width:120px;""><strong>Patient:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{patientName}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Phone:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{patientPhone}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{date:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#28a745;font-size:14px;font-weight:600;"">{slotText}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Status:</strong></td><td style=""padding:8px 0;color:#ffc107;font-size:14px;font-weight:600;"">Pending</td></tr>
</table></div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;"">Please review and accept this appointment at your earliest convenience.</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

            SendEmail(staffEmail, "New Appointment Booking - Emonti Optometrist", body);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send optometrist notification");
        }
    }

    private void SendConfirmationEmail(string customerEmail, DateTime date, string slotText)
    {
        var optometristName = Optometrists.FirstOrDefault(o => o.Value == Input.OptometristId)?.Text ?? "Selected Optometrist";
        var heading = IsRebooking ? "Appointment Rebooked" : "Appointment Confirmed";
        var body = $@"<!DOCTYPE html>
<html><head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#667eea;padding:25px;text-align:center;"">
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">{heading}</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;"">Dear Valued Customer,</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;"">Your appointment has been successfully confirmed.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #667eea;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;width:120px;""><strong>Optometrist:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{optometristName}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{date:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#667eea;font-size:14px;font-weight:600;"">{slotText}</td></tr>
</table></div>
<div style=""background-color:#e7f3ff;border-left:4px solid #2196F3;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0 0 8px 0;color:#1976D2;font-size:14px;font-weight:600;"">Important Reminders</p>
<ul style=""margin:0;padding-left:20px;color:#1565C0;font-size:13px;line-height:1.8;"">
<li>Please arrive <strong>15 minutes early</strong> for your appointment</li>
<li>Bring your current eyeglasses or contact lenses if you have them</li>
<li>Bring your medical aid card if applicable</li>
</ul>
</div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;"">We look forward to seeing you soon!</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

        SendEmail(customerEmail, $"Appointment {(IsRebooking ? "Rebooking" : "Confirmation")} - Emonti Optometrist", body);
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
