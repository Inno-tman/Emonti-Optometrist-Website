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
                   COALESCE(o.OrderCount, 0) AS OrderCount,
                   IFNULL(c.Customer_DOB, '') AS Customer_DOB,
                   IFNULL(c.Customer_Gender, '') AS Customer_Gender,
                   IFNULL(c.Street_Number, '') AS Street_Number,
                   IFNULL(c.Street_Name, '') AS Street_Name,
                   IFNULL(c.Complex_Name, '') AS Complex_Name,
                   IFNULL(c.Unit_Number, '') AS Unit_Number,
                   IFNULL(c.Postal_Code, '') AS Postal_Code,
                   IFNULL(c.Customer_Address, '') AS Customer_Address,
                   IFNULL(c.Medical_Aid, '') AS Medical_Aid,
                   IFNULL(c.Medical_Aid_Number, '') AS Medical_Aid_Number,
                   IFNULL(c.Main_Member_Name, '') AS Main_Member_Name,
                   IFNULL(c.Main_Member_Surname, '') AS Main_Member_Surname,
                   IFNULL(c.Main_Member_ID, '') AS Main_Member_ID
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
            var streetNum = reader["Street_Number"]?.ToString() ?? "";
            var streetName = reader["Street_Name"]?.ToString() ?? "";
            var complex = reader["Complex_Name"]?.ToString() ?? "";
            var unit = reader["Unit_Number"]?.ToString() ?? "";
            var city = reader["City"]?.ToString() ?? "";
            var province = reader["Province"]?.ToString() ?? "";
            var postal = reader["Postal_Code"]?.ToString() ?? "";
            var custAddr = reader["Customer_Address"]?.ToString() ?? "";

            var addr = !string.IsNullOrWhiteSpace(custAddr)
                ? custAddr
                : string.Join(", ", new[] {
                    string.IsNullOrWhiteSpace(unit) ? "" : $"Unit {unit}",
                    string.IsNullOrWhiteSpace(complex) ? "" : complex,
                    string.IsNullOrWhiteSpace(streetNum) ? "" : $"{streetNum} {streetName}",
                    city,
                    province,
                    postal
                }.Where(p => !string.IsNullOrWhiteSpace(p)));

            list.Add(new CustomerListItem
            {
                CustId = reader["Cust_ID"]?.ToString() ?? "",
                Name = reader["Customer_Name"]?.ToString() ?? "",
                Surname = reader["Customer_Surname"]?.ToString() ?? "",
                Email = reader["Customer_Email"]?.ToString() ?? "",
                Phone = reader["Customer_Phone"]?.ToString() ?? "",
                City = city,
                Province = province,
                LastLogin = reader["Last_Login"]?.ToString() ?? "",
                OrderCount = Convert.ToInt32(reader["OrderCount"]),
                DOB = reader["Customer_DOB"]?.ToString() ?? "",
                Gender = reader["Customer_Gender"]?.ToString() ?? "",
                FullAddress = addr,
                MedicalAid = reader["Medical_Aid"]?.ToString() ?? "",
                MedicalAidNumber = reader["Medical_Aid_Number"]?.ToString() ?? "",
                MainMemberName = reader["Main_Member_Name"]?.ToString() ?? "",
                MainMemberSurname = reader["Main_Member_Surname"]?.ToString() ?? "",
                MainMemberID = reader["Main_Member_ID"]?.ToString() ?? ""
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
    public string DOB { get; set; } = "";
    public string Gender { get; set; } = "";
    public string FullAddress { get; set; } = "";
    public string MedicalAid { get; set; } = "";
    public string MedicalAidNumber { get; set; } = "";
    public string MainMemberName { get; set; } = "";
    public string MainMemberSurname { get; set; } = "";
    public string MainMemberID { get; set; } = "";
}
