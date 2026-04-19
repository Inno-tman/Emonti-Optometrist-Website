using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class Orders : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadOrders();
            }
        }

        protected void rptOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                OrderInfo order = (OrderInfo)e.Item.DataItem;
                Repeater rptOrderItems = (Repeater)e.Item.FindControl("rptOrderItems");
                
                if (rptOrderItems != null && order.Items != null)
                {
                    rptOrderItems.DataSource = order.Items;
                    rptOrderItems.DataBind();
                }
            }
        }

        private void LoadOrders()
        {
            try
            {
                string customerId = Session["Cust_ID"]?.ToString();
                if (string.IsNullOrEmpty(customerId))
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                // Load real orders from database
                List<OrderInfo> orders = GetCustomerOrders(customerId);
                
                if (orders.Count > 0)
                {
                    rptOrders.DataSource = orders;
                    rptOrders.DataBind();
                    pnlOrders.Visible = true;
                    pnlNoOrders.Visible = false;
                    lblOrderCount.Text = $"({orders.Count} orders)";
                }
                else
                {
                    pnlOrders.Visible = false;
                    pnlNoOrders.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Handle error
                pnlOrders.Visible = false;
                pnlNoOrders.Visible = true;
                System.Diagnostics.Debug.WriteLine($"Error loading orders: {ex.Message}");
            }
        }

        private List<OrderInfo> GetCustomerOrders(string customerId)
        {
            List<OrderInfo> orders = new List<OrderInfo>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Query to get orders for the specific customer
                    string query = @"
                        SELECT o.OrderID, o.Order_Date, o.Order_Total, o.Order_Status, o.Delivery_Address
                        FROM [Order] o
                        WHERE o.CustID = @CustomerId
                        ORDER BY o.Order_Date DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.CommandTimeout = 30; // Set timeout to 30 seconds
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int orderId = Convert.ToInt32(reader["OrderID"]);
                                
                                OrderInfo order = new OrderInfo
                                {
                                    OrderId = orderId,
                                    OrderNumber = $"EL-{orderId.ToString().PadLeft(3, '0')}",
                                    OrderDate = reader.IsDBNull(reader.GetOrdinal("Order_Date")) 
                                        ? DateTime.Now 
                                        : Convert.ToDateTime(reader["Order_Date"]),
                                    Status = reader.IsDBNull(reader.GetOrdinal("Order_Status")) 
                                        ? "Unknown" 
                                        : reader["Order_Status"].ToString(),
                                    Total = reader.IsDBNull(reader.GetOrdinal("Order_Total")) 
                                        ? 0m 
                                        : Convert.ToDecimal(reader["Order_Total"]),
                                    Items = new List<OrderItem>() // Initialize empty list first
                                };
                                
                                // Get order items separately to avoid nested connection issues
                                try
                                {
                                    order.Items = GetOrderItems(orderId);
                                }
                                catch (Exception itemEx)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error loading items for order {orderId}: {itemEx.Message}");
                                    order.Items = new List<OrderItem>(); // Set empty list on error
                                }
                                
                                orders.Add(order);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting customer orders: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
            
            return orders;
        }

        private List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT oi.Product_Name, oi.Product_Brand, oi.Product_Category, 
                               oi.Quantity, oi.Unit_Price, oi.Subtotal,
                               p.Picture1, p.Picture2
                        FROM OrderItems oi
                        LEFT JOIN Products2 p ON oi.Product_ID = p.Product_ID
                        WHERE oi.OrderID = @OrderId
                        ORDER BY oi.OrderItemID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderId", orderId);
                        cmd.CommandTimeout = 30; // Set timeout to 30 seconds
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Get picture values - handle DBNull properly
                                object pic1 = reader.IsDBNull(reader.GetOrdinal("Picture1")) ? null : reader["Picture1"];
                                object pic2 = reader.IsDBNull(reader.GetOrdinal("Picture2")) ? null : reader["Picture2"];
                                
                                string productName = reader.IsDBNull(reader.GetOrdinal("Product_Name")) 
                                    ? "Unknown Product" 
                                    : reader["Product_Name"].ToString();
                                
                                string imagePath = GetProductImage(pic1, pic2);
                                
                                // Debug logging
                                System.Diagnostics.Debug.WriteLine($"Order Item - Product: {productName}, Pic1: {pic1?.ToString() ?? "NULL"}, Pic2: {pic2?.ToString() ?? "NULL"}, ImagePath: {imagePath}");
                                
                                OrderItem item = new OrderItem
                                {
                                    ProductName = productName,
                                    Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) 
                                        ? 0 
                                        : Convert.ToInt32(reader["Quantity"]),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Unit_Price")) 
                                        ? 0m 
                                        : Convert.ToDecimal(reader["Unit_Price"]),
                                    ProductImage = imagePath
                                };
                                
                                items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting order items for order {orderId}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                // Return empty list if there's an error to prevent page crash
            }
            
            return items;
        }

        // Helper method to get the best available image (same logic as Shop and ProductDetails)
        protected string GetProductImage(object picture1, object picture2)
        {
            string img1 = picture1?.ToString();
            string img2 = picture2?.ToString();
            
            // Handle the image path correctly
            if (!string.IsNullOrEmpty(img1))
            {
                // If the path already contains Images/Products, use it as is
                if (img1.StartsWith("Images\\Products\\") || img1.StartsWith("Images/Products/"))
                {
                    return $"~/{img1.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{img1}";
                }
            }
            else if (!string.IsNullOrEmpty(img2))
            {
                // If the path already contains Images/Products, use it as is
                if (img2.StartsWith("Images\\Products\\") || img2.StartsWith("Images/Products/"))
                {
                    return $"~/{img2.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{img2}";
                }
            }
            else
            {
                return "~/Images/Products/placeholder.jpg"; // Default placeholder image
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Implement search functionality
            LoadOrders(); // For now, just reload all orders
        }


        protected void btnStartShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Shop.aspx");
        }

        // Public method to get product image for data binding (kept for backward compatibility)
        public string GetProductImage(object productName)
        {
            if (productName == null) return "Images/Products/default-product.png";
            
            string product = productName.ToString();
            
            // Simple mapping based on product names - you can expand this
            if (product.Contains("Holbrook"))
                return "Images/Products/ID_2_Picture1.png";
            else if (product.Contains("Biotrue"))
                return "Images/Products/ID_37_Picture1.png";
            else if (product.Contains("Adjustable"))
                return "Images/Products/ID_49_Picture1.png";
            else if (product.Contains("Gaming"))
                return "Images/Products/ID_15_Picture1.png";
            else if (product.Contains("Roebling"))
                return "Images/Products/ID_9_Picture1.png";
            else if (product.Contains("Microfiber"))
                return "Images/Products/ID_44_Picture1.png";
            else if (product.Contains("TF5401"))
                return "Images/Products/ID_7_Picture1.png";
            else if (product.Contains("BOSS"))
                return "Images/Products/ID_24_Picture1.png";
            else if (product.Contains("VE3284"))
                return "Images/Products/ID_20_Picture1.png";
            else if (product.Contains("Varilux"))
                return "Images/Products/ID_30_Picture1.png";
            else if (product.Contains("Razer"))
                return "Images/Products/ID_16_Picture1.png";
            else if (product.Contains("Aviator"))
                return "Images/Products/ID_1_Picture1.png";
            else if (product.Contains("Peahi"))
                return "Images/Products/ID_4_Picture1.png";
            else if (product.Contains("GG0061O"))
                return "Images/Products/ID_21_Picture1.png";
            else if (product.Contains("PO0649"))
                return "Images/Products/ID_3_Picture1.png";
            else
                return "Images/Products/default-product.png";
        }
    }

    // Helper classes for order data
    public class OrderInfo
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public List<OrderItem> Items { get; set; }
    }

    public class OrderItem
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductImage { get; set; }
    }
}
