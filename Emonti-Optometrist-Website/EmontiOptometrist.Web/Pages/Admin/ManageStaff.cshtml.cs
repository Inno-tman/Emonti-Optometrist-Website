using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

public class ManageStaffModel : PageModel
{
    private readonly string _connectionString;

    public ManageStaffModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public List<StaffUser> StaffUsers { get; set; } = new();

    public IActionResult OnGet()
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        LoadData();
        return Page();
    }

    public IActionResult OnPostAddStaff(string staffEmail, string staffName, string staffSurname, string staffPassword, string staffRole)
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(staffEmail) || string.IsNullOrWhiteSpace(staffName))
        {
            TempData["ErrorMessage"] = "Email and name are required.";
            LoadData();
            return Page();
        }

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var checkCmd = conn.CreateCommand();
        checkCmd.CommandText = "SELECT COUNT(*) FROM Staff WHERE Staff_Email = @Email";
        checkCmd.Parameters.AddWithValue("@Email", staffEmail.Trim());
        if ((long)checkCmd.ExecuteScalar() > 0)
        {
            TempData["ErrorMessage"] = "A staff member with this email already exists.";
            LoadData();
            return Page();
        }

        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Staff (Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Password, Staff_Role)
            VALUES (@id, @name, @surname, @email, @password, @role)";
        cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
        cmd.Parameters.AddWithValue("@name", staffName.Trim());
        cmd.Parameters.AddWithValue("@surname", string.IsNullOrWhiteSpace(staffSurname) ? "" : staffSurname.Trim());
        cmd.Parameters.AddWithValue("@email", staffEmail.Trim());
        cmd.Parameters.AddWithValue("@password", string.IsNullOrWhiteSpace(staffPassword) ? "Staff123" : staffPassword);
        cmd.Parameters.AddWithValue("@role", string.IsNullOrWhiteSpace(staffRole) ? "Staff" : staffRole);
        cmd.ExecuteNonQuery();

        TempData["SuccessMessage"] = $"{staffEmail} added as staff.";
        LoadData();
        return Page();
    }

    public IActionResult OnPostRemoveStaff(string staffId)
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Staff WHERE Staff_ID = @Id AND Staff_Role != 'Admin'";
        cmd.Parameters.AddWithValue("@Id", staffId);
        cmd.ExecuteNonQuery();

        TempData["SuccessMessage"] = "Staff member removed.";
        LoadData();
        return Page();
    }

    public IActionResult OnPostMakeAdmin(string staffId)
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE Staff SET Staff_Role = 'Admin' WHERE Staff_ID = @Id";
        cmd.Parameters.AddWithValue("@Id", staffId);
        cmd.ExecuteNonQuery();

        TempData["SuccessMessage"] = "Staff member promoted to admin.";
        LoadData();
        return Page();
    }

    private void LoadData()
    {
        var users = new List<StaffUser>();

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Role
            FROM Staff
            ORDER BY Staff_Name";

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var role = reader["Staff_Role"]?.ToString() ?? "Staff";
            users.Add(new StaffUser
            {
                StaffId = reader["Staff_ID"].ToString(),
                Name = reader["Staff_Name"]?.ToString() ?? "",
                Surname = reader["Staff_Surname"]?.ToString() ?? "",
                Email = reader["Staff_Email"]?.ToString() ?? "",
                Role = role,
                IsAdmin = role == "Admin"
            });
        }

        StaffUsers = users;
    }
}

public class StaffUser
{
    public string StaffId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public bool IsAdmin { get; set; }
}
