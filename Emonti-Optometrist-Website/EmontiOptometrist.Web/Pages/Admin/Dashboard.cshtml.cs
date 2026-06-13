using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardModel(IConfiguration configuration, UserManager<ApplicationUser> userManager)
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
        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (!string.IsNullOrEmpty(connStr))
        {
            try
            {
                using var conn = new SqliteConnection(connStr);
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM [Order] WHERE date(Order_Date) = date('now')";
                    TotalOrdersToday = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT IFNULL(SUM(Order_Total), 0) FROM [Order]";
                    TotalRevenue = Convert.ToDecimal(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM [Order] WHERE Order_Status IN ('Pending', 'Processing')";
                    PendingOrders = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Products2";
                    TotalProducts = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT OrderID, CustID, Order_Date, Order_Total, Order_Status
                        FROM [Order] ORDER BY Order_Date DESC
                        LIMIT 5";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecentOrders.Add(new RecentOrderItem
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustID = reader["CustID"].ToString(),
                                OrderDate = DateTime.Parse(reader["Order_Date"].ToString()),
                                Total = Convert.ToDecimal(reader["Order_Total"]),
                                Status = reader["Order_Status"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Admin Dashboard error: {ex.Message}");
            }
        }

        try { TotalStaff = _userManager.Users.Count(); } catch { }
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
