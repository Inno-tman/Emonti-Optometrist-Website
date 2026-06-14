using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class LogoutModel : PageModel
{
    private readonly IConfiguration _configuration;

    public LogoutModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet()
    {
        var custId = AuthSession.GetCustId(HttpContext);
        if (!string.IsNullOrEmpty(custId))
        {
            string connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE customer SET Last_Login = datetime('now', 'localtime') WHERE Cust_ID = @CustId";
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.ExecuteNonQuery();
        }

        AuthSession.SignOut(HttpContext);
        return RedirectToPage("/Index");
    }
}
