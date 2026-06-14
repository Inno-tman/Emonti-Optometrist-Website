using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

public class DashboardModel : PageModel
{
    private readonly IConfiguration _configuration;

    public DashboardModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public int TotalOrdersToday { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int TotalProducts { get; set; }
    public int TotalStaff { get; set; }
    public int TodayAppointments { get; set; }
    public int NewCustomers { get; set; }
    public List<RecentOrderItem> RecentOrders { get; set; } = new();

    public IActionResult OnGet()
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

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
                    cmd.CommandText = "SELECT COUNT(*) FROM Staff";
                    TotalStaff = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Appointment WHERE date(Appointment_Date) = date('now')";
                    TodayAppointments = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM customer WHERE date(Customer_Create_Date) >= date('now', 'start of month')";
                    NewCustomers = Convert.ToInt32(cmd.ExecuteScalar());
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT o.OrderID, o.CustID, o.Order_Date, o.Order_Total, o.Order_Status,
                               c.Customer_Name, c.Customer_Surname
                        FROM [Order] o
                        LEFT JOIN customer c ON o.CustID = c.Cust_ID
                        ORDER BY o.Order_Date DESC
                        LIMIT 5";
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecentOrders.Add(new RecentOrderItem
                            {
                                OrderID = Convert.ToInt32(reader["OrderID"]),
                                CustID = reader["CustID"].ToString(),
                                CustomerName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
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

        return Page();
    }
}

public class RecentOrderItem
{
    public int OrderID { get; set; }
    public string CustID { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "";
}
