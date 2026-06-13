using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace EmontiOptometrist.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;

    public DashboardModel(IConfiguration configuration, UserManager<IdentityUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public int TotalOrdersToday { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int TotalProducts { get; set; }
    public int TotalStaff { get; set; }
    public List<RecentOrderItem> RecentOrders { get; set; } = new();

    public void OnGet()
    {
        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";

        using (var conn = new SqlConnection(connStr))
        {
            conn.Open();

            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Order] WHERE CAST(Order_Date AS DATE) = CAST(GETDATE() AS DATE)", conn))
                TotalOrdersToday = Convert.ToInt32(cmd.ExecuteScalar());

            using (var cmd = new SqlCommand("SELECT ISNULL(SUM(Order_Total), 0) FROM [Order]", conn))
                TotalRevenue = Convert.ToDecimal(cmd.ExecuteScalar());

            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Order] WHERE Order_Status IN ('Pending', 'Processing')", conn))
                PendingOrders = Convert.ToInt32(cmd.ExecuteScalar());

            using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Products2", conn))
                TotalProducts = Convert.ToInt32(cmd.ExecuteScalar());

            using (var cmd = new SqlCommand(@"
                SELECT TOP 5 OrderID, CustID, Order_Date, Order_Total, Order_Status
                FROM [Order] ORDER BY Order_Date DESC", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RecentOrders.Add(new RecentOrderItem
                        {
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            CustID = reader["CustID"].ToString(),
                            OrderDate = Convert.ToDateTime(reader["Order_Date"]),
                            Total = Convert.ToDecimal(reader["Order_Total"]),
                            Status = reader["Order_Status"].ToString()
                        });
                    }
                }
            }
        }

        TotalStaff = _userManager.Users.Count();
    }
}

public class RecentOrderItem
{
    public int OrderID { get; set; }
    public string CustID { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "";
}
