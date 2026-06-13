using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace EmontiOptometrist.Web.Pages;

[Authorize]
public class BookAppointmentModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BookAppointmentModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
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

    public void OnGet()
    {
        Input.PreferredDate = DateTime.Today;
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                ErrorMessage = "Could not identify user. Please log in again.";
                return Page();
            }

            var connStr = _configuration.GetConnectionString("DefaultConnection");
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Appointment
                    (Cust_ID, Staff_ID, Appointment_Date, AppointmentTimeID, Appoinment_Status)
                VALUES
                    (@CustId, @StaffId, @AppointmentDate, @AppointmentTimeId, @Status)";
            cmd.Parameters.AddWithValue("@CustId", userId);
            cmd.Parameters.AddWithValue("@StaffId", "1");
            cmd.Parameters.AddWithValue("@AppointmentDate", Input.PreferredDate.Date.ToString("o"));
            cmd.Parameters.AddWithValue("@AppointmentTimeId", Input.PreferredTime);
            cmd.Parameters.AddWithValue("@Status", "Pending");

            cmd.ExecuteNonQuery();

            SuccessMessage = $"Appointment booked successfully! We look forward to seeing you on {Input.PreferredDate:dddd, MMMM dd, yyyy}.";
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
