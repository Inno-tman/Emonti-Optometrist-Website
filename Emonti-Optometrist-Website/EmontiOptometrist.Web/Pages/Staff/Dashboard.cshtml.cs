using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Staff;

public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly BrevoEmailService _brevoEmail;

    public DashboardModel(IConfiguration configuration, BrevoEmailService brevoEmail)
    {
        _configuration = configuration;
        _brevoEmail = brevoEmail;
    }

    public string StaffName { get; set; } = "";
    public string StaffRole { get; set; } = "";
    public int TodayAppointments { get; set; }
    public int PendingOrders { get; set; }
    public int TotalPatients { get; set; }
    public int UpcomingCount { get; set; }
    public List<UpcomingAppointment> UpcomingAppointments { get; set; } = new();
    public string SuccessMessage { get; set; } = "";
    public string ErrorMessage { get; set; } = "";

    public IActionResult OnGet()
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        SuccessMessage = TempData["SuccessMessage"]?.ToString() ?? "";
        ErrorMessage = TempData["ErrorMessage"]?.ToString() ?? "";

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
                    SELECT COUNT(*) FROM Appointment
                    WHERE Staff_ID = @StaffId
                    AND date(Appointment_Date) >= date('now')
                    AND Appoinment_Status != 'Cancelled'";
                cmd.Parameters.AddWithValue("@StaffId", staffId);
                UpcomingCount = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT a.Appointment_ID, a.Appointment_Date, a.Appoinment_Status,
                           t.Timeslot, c.Customer_Name, c.Customer_Surname, c.Customer_Phone
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
                            PatientName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
                            PatientPhone = reader["Customer_Phone"]?.ToString() ?? ""
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Staff Dashboard error: {ex.Message}");
        }

        return Page();
    }

    public IActionResult OnPostCancelAppointment(int appointmentId, string reason)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (string.IsNullOrEmpty(connStr))
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
            return RedirectToPage();
        }

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            string? customerEmail = null;
            string? customerName = null;
            string? appointmentDate = null;
            string? appointmentTime = null;
            string? optometristName = null;

            using (var getInfo = conn.CreateCommand())
            {
                getInfo.CommandText = @"
                    SELECT c.Customer_Email, c.Customer_Name, a.Appointment_Date, t.Timeslot, s.Staff_Name, s.Staff_Surname
                    FROM Appointment a
                    INNER JOIN customer c ON a.Cust_ID = c.Cust_ID
                    LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                    LEFT JOIN Staff s ON a.Staff_ID = s.Staff_ID
                    WHERE a.Appointment_ID = @Id";
                getInfo.Parameters.AddWithValue("@Id", appointmentId);
                using (var rdr = getInfo.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        customerEmail = rdr["Customer_Email"]?.ToString();
                        customerName = $"{rdr["Customer_Name"]}";
                        appointmentDate = rdr["Appointment_Date"]?.ToString();
                        appointmentTime = rdr["Timeslot"]?.ToString() ?? "N/A";
                        optometristName = $"{rdr["Staff_Name"]} {rdr["Staff_Surname"]}";
                    }
                }
            }

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Appointment SET Appoinment_Status = 'Cancelled' WHERE Appointment_ID = @Id AND Appoinment_Status != 'Cancelled'";
            cmd.Parameters.AddWithValue("@Id", appointmentId);
            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                TempData["SuccessMessage"] = "Appointment cancelled successfully.";

                if (!string.IsNullOrEmpty(customerEmail))
                {
                    var reasonText = string.IsNullOrWhiteSpace(reason) ? "No reason provided." : reason;
                    var body = $@"<!DOCTYPE html>
<html><head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#dc3545;padding:25px;text-align:center;"">
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Appointment Cancelled</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;"">Dear {customerName},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;"">Your appointment has been cancelled.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #dc3545;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;width:120px;""><strong>Optometrist:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{optometristName}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{appointmentDate}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{appointmentTime}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;vertical-align:top;""><strong>Reason:</strong></td><td style=""padding:8px 0;color:#dc3545;font-size:14px;"">{reasonText}</td></tr>
</table></div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;"">We apologise for the inconvenience. Please contact us if you would like to reschedule.</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

                    _ = _brevoEmail.SendEmailAsync(customerEmail, customerName, "Appointment Cancelled - Emonti Optometrist", body);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Appointment could not be cancelled (already cancelled or not found).";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error cancelling appointment: {ex.Message}";
        }

        return RedirectToPage();
    }

    public IActionResult OnPostAcceptAppointment(int appointmentId)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (string.IsNullOrEmpty(connStr))
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
            return RedirectToPage();
        }

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Appointment SET Appoinment_Status = 'Confirmed' WHERE Appointment_ID = @Id AND Appoinment_Status = 'Pending'";
            cmd.Parameters.AddWithValue("@Id", appointmentId);
            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
                TempData["SuccessMessage"] = "Appointment accepted successfully.";
            else
                TempData["ErrorMessage"] = "Appointment could not be accepted (not found or already accepted/cancelled).";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error accepting appointment: {ex.Message}";
        }

        return RedirectToPage();
    }

    public JsonResult OnGetGetTimeslots(string date)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return new JsonResult(new { error = "Not authenticated" });

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        var staffId = HttpContext.Session.GetString("Staff_ID") ?? "";

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            var allSlots = new List<(string TimeId, string Label)>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT TimeID, Timeslot FROM tblTime ORDER BY TimeID";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                    allSlots.Add((reader["TimeID"].ToString(), reader["Timeslot"].ToString()));
            }

            var bookedIds = new HashSet<string>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT AppointmentTimeID FROM Appointment
                    WHERE date(Appointment_Date) = date(@Date)
                    AND Appoinment_Status != 'Cancelled'";
                cmd.Parameters.AddWithValue("@Date", date);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tid = reader["AppointmentTimeID"]?.ToString();
                    if (!string.IsNullOrEmpty(tid)) bookedIds.Add(tid);
                }
            }

            var blockedIds = new HashSet<string>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT TimeID FROM BlockedTimeslots
                    WHERE Staff_ID = @StaffId AND Blocked_Date = @Date";
                cmd.Parameters.AddWithValue("@StaffId", staffId);
                cmd.Parameters.AddWithValue("@Date", date);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tid = reader["TimeID"]?.ToString();
                    if (!string.IsNullOrEmpty(tid)) blockedIds.Add(tid);
                }
            }

            var slots = new List<object>();
            foreach (var (timeId, label) in allSlots)
            {
                string status;
                if (bookedIds.Contains(timeId))
                    status = "booked";
                else if (blockedIds.Contains(timeId))
                    status = "blocked";
                else
                    status = "available";

                slots.Add(new { timeId, label, status });
            }

            return new JsonResult(new { slots });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { error = ex.Message });
        }
    }

    public JsonResult OnGetToggleBlock(string date, string timeId)
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return new JsonResult(new { error = "Not authenticated" });

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        var staffId = HttpContext.Session.GetString("Staff_ID") ?? "";

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using var checkCmd = conn.CreateCommand();
            checkCmd.CommandText = @"
                SELECT COUNT(*) FROM BlockedTimeslots
                WHERE Staff_ID = @StaffId AND Blocked_Date = @Date AND TimeID = @TimeId";
            checkCmd.Parameters.AddWithValue("@StaffId", staffId);
            checkCmd.Parameters.AddWithValue("@Date", date);
            checkCmd.Parameters.AddWithValue("@TimeId", timeId);
            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists > 0)
            {
                using var delCmd = conn.CreateCommand();
                delCmd.CommandText = @"
                    DELETE FROM BlockedTimeslots
                    WHERE Staff_ID = @StaffId AND Blocked_Date = @Date AND TimeID = @TimeId";
                delCmd.Parameters.AddWithValue("@StaffId", staffId);
                delCmd.Parameters.AddWithValue("@Date", date);
                delCmd.Parameters.AddWithValue("@TimeId", timeId);
                delCmd.ExecuteNonQuery();
            }
            else
            {
                using var insCmd = conn.CreateCommand();
                insCmd.CommandText = @"
                    INSERT INTO BlockedTimeslots (Staff_ID, Blocked_Date, TimeID)
                    VALUES (@StaffId, @Date, @TimeId)";
                insCmd.Parameters.AddWithValue("@StaffId", staffId);
                insCmd.Parameters.AddWithValue("@Date", date);
                insCmd.Parameters.AddWithValue("@TimeId", timeId);
                insCmd.ExecuteNonQuery();
            }

            return new JsonResult(new { success = true });
        }
        catch (Exception ex)
        {
            return new JsonResult(new { error = ex.Message });
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
    public string PatientPhone { get; set; } = "";
}
