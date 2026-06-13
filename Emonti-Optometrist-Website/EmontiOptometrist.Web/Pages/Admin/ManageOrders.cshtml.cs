using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

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

        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (string.IsNullOrEmpty(connStr))
            return;

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            string query = @"
                SELECT o.OrderID, o.CustID, o.Order_Date, o.Order_Total, o.Order_Status, o.Delivery_Address,
                       c.Customer_Name, c.Customer_Surname,
                       c.Customer_Email, c.Customer_Phone
                FROM [Order] o
                LEFT JOIN customer c ON o.CustID = c.Cust_ID
                WHERE 1=1";

            if (!string.IsNullOrEmpty(status))
                query += " AND o.Order_Status = @Status";
            if (!string.IsNullOrEmpty(search))
                query += " AND (o.CustID LIKE @Search OR CAST(o.OrderID AS TEXT) LIKE @Search OR c.Customer_Name LIKE @Search OR c.Customer_Surname LIKE @Search)";

            query += " ORDER BY o.Order_Date DESC";

            using var cmd = conn.CreateCommand();
            cmd.CommandText = query;
            if (!string.IsNullOrEmpty(status))
                cmd.Parameters.AddWithValue("@Status", status);
            if (!string.IsNullOrEmpty(search))
                cmd.Parameters.AddWithValue("@Search", $"%{search}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Orders.Add(new OrderListItem
                {
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    CustID = reader["CustID"].ToString(),
                    CustomerName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
                    CustomerEmail = reader["Customer_Email"]?.ToString() ?? "",
                    CustomerPhone = reader["Customer_Phone"]?.ToString() ?? "",
                    OrderDate = DateTime.Parse(reader["Order_Date"].ToString()),
                    OrderTotal = Convert.ToDecimal(reader["Order_Total"]),
                    OrderStatus = reader["Order_Status"].ToString(),
                    DeliveryAddress = reader["Delivery_Address"].ToString()
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ManageOrders error: {ex.Message}");
        }
    }

    public IActionResult OnPostUpdateStatus(int orderId, string status)
    {
        var connStr = _configuration.GetConnectionString("DefaultConnection") ?? "";
        if (!string.IsNullOrEmpty(connStr))
        {
            try
            {
                using var conn = new SqliteConnection(connStr);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE [Order] SET Order_Status = @Status WHERE OrderID = @OrderID";
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.ExecuteNonQuery();

                TempData["SuccessMessage"] = $"Order #EL-{orderId:D6} status updated to {status}.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating order: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"ManageOrders update error: {ex.Message}");
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
        }

        return RedirectToPage(new { status = StatusFilter, search = SearchTerm });
    }
}

public class OrderListItem
{
    public int OrderID { get; set; }
    public string CustID { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public DateTime OrderDate { get; set; }
    public decimal OrderTotal { get; set; }
    public string OrderStatus { get; set; } = "";
    public string DeliveryAddress { get; set; } = "";
    public string OrderNumber => "EL-" + OrderID.ToString("D6");
}
