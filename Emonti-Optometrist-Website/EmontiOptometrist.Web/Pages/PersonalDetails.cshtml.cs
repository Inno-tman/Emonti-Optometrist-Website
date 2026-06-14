using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class PersonalDetailsModel : PageModel
{
    private readonly IConfiguration _configuration;

    public PersonalDetailsModel(IConfiguration configuration)
    {
        _configuration = configuration;
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
        var userId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(userId))
        {
            ErrorMessage = "Could not identify user. Please log in again.";
            return Page();
        }

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                UPDATE customer
                SET Customer_Name = @FirstName,
                    Customer_Surname = @LastName,
                    Customer_Email = @Email,
                    Customer_Phone = @Phone,
                    Cust_Address = @Address,
                    Cust_FirstName = @FirstName,
                    Cust_LastName = @LastName,
                    Cust_Email = @Email,
                    Cust_Phone = @Phone
                WHERE Cust_ID = @CustId";
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
        var userId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(userId))
            return;

        try
        {
            var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Customer_Name, Customer_Surname, Customer_Email, Customer_Phone, Cust_Address
                FROM customer
                WHERE Cust_ID = @CustId";
            cmd.Parameters.AddWithValue("@CustId", userId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                FirstName = reader["Customer_Name"]?.ToString() ?? "";
                LastName = reader["Customer_Surname"]?.ToString() ?? "";
                Email = reader["Customer_Email"]?.ToString() ?? "";
                Phone = reader["Customer_Phone"]?.ToString() ?? "";
                Address = reader["Cust_Address"]?.ToString() ?? "";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load details: {ex.Message}";
        }
    }
}
