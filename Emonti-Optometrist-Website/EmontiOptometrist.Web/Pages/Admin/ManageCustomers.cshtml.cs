using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

public class ManageCustomersModel : PageModel
{
    private readonly string _connectionString;

    public ManageCustomersModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public List<CustomerListItem> Customers { get; set; } = new();
    public string SearchQuery { get; set; } = "";

    public IActionResult OnGet(string search)
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        SearchQuery = search ?? "";
        LoadData();
        return Page();
    }

    private void LoadData()
    {
        var list = new List<CustomerListItem>();

        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();

        var sql = @"
            SELECT c.Cust_ID, c.Customer_Name, c.Customer_Surname, c.Customer_Email,
                   c.Customer_Phone, c.City, c.Province, IFNULL(c.Last_Login, '') AS Last_Login,
                   COALESCE(o.OrderCount, 0) AS OrderCount
            FROM customer c
            LEFT JOIN (SELECT CustID, COUNT(*) AS OrderCount FROM [Order] GROUP BY CustID) o ON c.Cust_ID = o.CustID";

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            sql += @" WHERE c.Customer_Name LIKE @q OR c.Customer_Surname LIKE @q
                       OR c.Customer_Email LIKE @q OR c.Customer_Phone LIKE @q";
        }

        sql += " ORDER BY c.Customer_Name, c.Customer_Surname";

        cmd.CommandText = sql;
        if (!string.IsNullOrWhiteSpace(SearchQuery))
            cmd.Parameters.AddWithValue("@q", $"%{SearchQuery.Trim()}%");

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new CustomerListItem
            {
                CustId = reader["Cust_ID"]?.ToString() ?? "",
                Name = reader["Customer_Name"]?.ToString() ?? "",
                Surname = reader["Customer_Surname"]?.ToString() ?? "",
                Email = reader["Customer_Email"]?.ToString() ?? "",
                Phone = reader["Customer_Phone"]?.ToString() ?? "",
                City = reader["City"]?.ToString() ?? "",
                Province = reader["Province"]?.ToString() ?? "",
                LastLogin = reader["Last_Login"]?.ToString() ?? "",
                OrderCount = Convert.ToInt32(reader["OrderCount"])
            });
        }

        Customers = list;
    }
}

public class CustomerListItem
{
    public string CustId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Surname { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
    public string Province { get; set; } = "";
    public string LastLogin { get; set; } = "";
    public int OrderCount { get; set; }
}
