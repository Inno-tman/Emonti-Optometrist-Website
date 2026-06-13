using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace EmontiOptometrist.Web.Pages;

[Authorize]
public class PersonalDetailsModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PersonalDetailsModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    [BindProperty]
    public string FirstName { get; set; } = "";

    [BindProperty]
    public string LastName { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Phone { get; set; } = "";

    [BindProperty]
    public string Address { get; set; } = "";

    public string SuccessMessage { get; set; } = "";
    public string ErrorMessage { get; set; } = "";

    public void OnGet()
    {
        LoadCustomer();
    }

    public IActionResult OnPost()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            ErrorMessage = "Could not identify user. Please log in again.";
            return Page();
        }

        try
        {
            var connStr = _configuration.GetConnectionString("ProductConnection");
            using var conn = new SqlConnection(connStr);
            conn.Open();

            string query = @"
                UPDATE customer
                SET Cust_FirstName = @FirstName,
                    Cust_LastName = @LastName,
                    Cust_Email = @Email,
                    Cust_Phone = @Phone,
                    Cust_Address = @Address
                WHERE Cust_ID = @CustId";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@CustId", userId);

            cmd.ExecuteNonQuery();
            SuccessMessage = "Personal details updated successfully.";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to update details: {ex.Message}";
        }

        return Page();
    }

    private void LoadCustomer()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return;

        try
        {
            var connStr = _configuration.GetConnectionString("ProductConnection");
            using var conn = new SqlConnection(connStr);
            conn.Open();

            string query = @"
                SELECT Cust_FirstName, Cust_LastName, Cust_Email, Cust_Phone, Cust_Address
                FROM customer
                WHERE Cust_ID = @CustId";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CustId", userId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                FirstName = reader["Cust_FirstName"]?.ToString() ?? "";
                LastName = reader["Cust_LastName"]?.ToString() ?? "";
                Email = reader["Cust_Email"]?.ToString() ?? "";
                Phone = reader["Cust_Phone"]?.ToString() ?? "";
                Address = reader["Cust_Address"]?.ToString() ?? "";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load details: {ex.Message}";
        }
    }
}
