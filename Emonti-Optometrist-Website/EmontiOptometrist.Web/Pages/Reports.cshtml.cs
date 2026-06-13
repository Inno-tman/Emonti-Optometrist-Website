using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace EmontiOptometrist.Web.Pages;

public class ReportsModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ReportsModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalProductsSold { get; set; }
    public int TotalCustomers { get; set; }
    public List<RecentOrderDisplay> RecentOrders { get; set; } = new();
    public List<PopularProductDisplay> PopularProducts { get; set; } = new();

    public void OnGet()
    {
        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (string.IsNullOrEmpty(connStr))
            return;

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM [Order]";
                TotalOrders = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT IFNULL(SUM(Order_Total), 0) FROM [Order]";
                TotalRevenue = Convert.ToDecimal(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT IFNULL(SUM(Quantity), 0) FROM OrderItems";
                TotalProductsSold = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(DISTINCT CustID) FROM [Order]";
                TotalCustomers = Convert.ToInt32(cmd.ExecuteScalar());
            }

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT OrderID, CustID, Order_Date, Order_Total, Order_Status
                    FROM [Order]
                    ORDER BY Order_Date DESC
                    LIMIT 10";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        RecentOrders.Add(new RecentOrderDisplay
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

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT Product_Name, Product_Brand,
                           SUM(Quantity) AS TotalSold,
                           SUM(Subtotal) AS TotalRevenue
                    FROM OrderItems
                    GROUP BY Product_Name, Product_Brand
                    ORDER BY TotalSold DESC
                    LIMIT 5";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PopularProducts.Add(new PopularProductDisplay
                        {
                            ProductName = reader["Product_Name"].ToString(),
                            Brand = reader["Product_Brand"].ToString(),
                            TotalSold = Convert.ToInt32(reader["TotalSold"]),
                            TotalRevenue = Convert.ToDecimal(reader["TotalRevenue"])
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Reports error: {ex.Message}");
        }
    }
}

public class RecentOrderDisplay
{
    public int OrderID { get; set; }
    public string CustID { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; } = "";
}

public class PopularProductDisplay
{
    public string ProductName { get; set; } = "";
    public string Brand { get; set; } = "";
    public int TotalSold { get; set; }
    public decimal TotalRevenue { get; set; }
}
