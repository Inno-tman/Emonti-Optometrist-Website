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

    public IActionResult OnPostCancelAppointment(int appointmentId)
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
            cmd.CommandText = "UPDATE Appointment SET Appoinment_Status = 'Cancelled' WHERE Appointment_ID = @Id AND Appoinment_Status != 'Cancelled'";
            cmd.Parameters.AddWithValue("@Id", appointmentId);
            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            else
                TempData["ErrorMessage"] = "Appointment could not be cancelled (already cancelled or not found).";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error cancelling appointment: {ex.Message}";
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
