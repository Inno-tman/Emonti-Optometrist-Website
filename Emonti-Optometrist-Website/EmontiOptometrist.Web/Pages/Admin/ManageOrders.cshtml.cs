using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace EmontiOptometrist.Web.Pages.Admin;

[Authorize(Roles = "Admin,Staff")]
public class ManageOrdersModel : PageModel
{
    private readonly IConfiguration _configuration;

    public ManageOrdersModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public List<OrderListItem> Orders { get; set; } = new();
    public string? StatusFilter { get; set; }
    public string? SearchTerm { get; set; }

    public void OnGet(string? status, string? search)
    {
        StatusFilter = status;
        SearchTerm = search;

        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";

        using (var conn = new SqlConnection(connStr))
        {
            conn.Open();

            string query = @"
                SELECT OrderID, CustID, Order_Date, Order_Total, Order_Status, Delivery_Address
                FROM [Order]
                WHERE 1=1";

            if (!string.IsNullOrEmpty(status))
                query += " AND Order_Status = @Status";
            if (!string.IsNullOrEmpty(search))
                query += " AND (CustID LIKE @Search OR CAST(OrderID AS NVARCHAR) LIKE @Search)";

            query += " ORDER BY Order_Date DESC";

            using (var cmd = new SqlCommand(query, conn))
            {
                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@Status", status);
                if (!string.IsNullOrEmpty(search))
                    cmd.Parameters.AddWithValue("@Search", $"%{search}%");

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Orders.Add(new OrderListItem
                        {
                            OrderID = Convert.ToInt32(reader["OrderID"]),
                            CustID = reader["CustID"].ToString(),
                            OrderDate = Convert.ToDateTime(reader["Order_Date"]),
                            OrderTotal = Convert.ToDecimal(reader["Order_Total"]),
                            OrderStatus = reader["Order_Status"].ToString(),
                            DeliveryAddress = reader["Delivery_Address"].ToString()
                        });
                    }
                }
            }
        }
    }

    public IActionResult OnPostUpdateStatus(int orderId, string status)
    {
        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";

        using (var conn = new SqlConnection(connStr))
        {
            conn.Open();
            using (var cmd = new SqlCommand(
                "UPDATE [Order] SET Order_Status = @Status WHERE OrderID = @OrderID", conn))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.ExecuteNonQuery();
            }
        }

        TempData["SuccessMessage"] = $"Order #EL-{orderId:D6} status updated to {status}.";
        return RedirectToPage(new { status = StatusFilter, search = SearchTerm });
    }
}

public class OrderListItem
{
    public int OrderID { get; set; }
    public string CustID { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    public string OrderStatus { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
    public string OrderNumber => "EL-" + OrderID.ToString("D6");
}
