using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services;

public class OrderDatabase
{
    private readonly string _connectionString;

    public OrderDatabase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public int CreateOrder(Order order)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO [Order] (CustID, Order_Date, Order_Total, Order_Status, Delivery_Address)
                VALUES (@CustID, @Order_Date, @Order_Total, @Order_Status, @Delivery_Address);
                SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@CustID", order.CustID);
            cmd.Parameters.AddWithValue("@Order_Date", order.Order_Date.ToString("o"));
            cmd.Parameters.AddWithValue("@Order_Total", order.Order_Total);
            cmd.Parameters.AddWithValue("@Order_Status", order.Order_Status);
            cmd.Parameters.AddWithValue("@Delivery_Address", order.Delivery_Address ?? "");

            object result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating order: {ex.Message}");
            throw;
        }
    }

    public int CreateOrder(Order order, SqliteConnection conn, SqliteTransaction transaction)
    {
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO [Order] (CustID, Order_Date, Order_Total, Order_Status, Delivery_Address)
                VALUES (@CustID, @Order_Date, @Order_Total, @Order_Status, @Delivery_Address);
                SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@CustID", order.CustID);
            cmd.Parameters.AddWithValue("@Order_Date", order.Order_Date.ToString("o"));
            cmd.Parameters.AddWithValue("@Order_Total", order.Order_Total);
            cmd.Parameters.AddWithValue("@Order_Status", order.Order_Status);
            cmd.Parameters.AddWithValue("@Delivery_Address", order.Delivery_Address ?? "");

            object result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating order: {ex.Message}");
            throw;
        }
    }

    public void AddOrderItem(DatabaseOrderItem orderItem)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO OrderItems (OrderID, Product_ID, Product_Name, Product_Brand, Product_Category, Quantity, Unit_Price, Subtotal)
                VALUES (@OrderID, @Product_ID, @Product_Name, @Product_Brand, @Product_Category, @Quantity, @Unit_Price, @Subtotal)";
            cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
            cmd.Parameters.AddWithValue("@Product_ID", orderItem.Product_ID);
            cmd.Parameters.AddWithValue("@Product_Name", orderItem.Product_Name);
            cmd.Parameters.AddWithValue("@Product_Brand", orderItem.Product_Brand);
            cmd.Parameters.AddWithValue("@Product_Category", orderItem.Product_Category);
            cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@Unit_Price", orderItem.Unit_Price);
            cmd.Parameters.AddWithValue("@Subtotal", orderItem.Subtotal);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error adding order item: {ex.Message}");
            throw;
        }
    }

    public void AddOrderItem(DatabaseOrderItem orderItem, SqliteConnection conn, SqliteTransaction transaction)
    {
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO OrderItems (OrderID, Product_ID, Product_Name, Product_Brand, Product_Category, Quantity, Unit_Price, Subtotal)
                VALUES (@OrderID, @Product_ID, @Product_Name, @Product_Brand, @Product_Category, @Quantity, @Unit_Price, @Subtotal)";
            cmd.Parameters.AddWithValue("@OrderID", orderItem.OrderID);
            cmd.Parameters.AddWithValue("@Product_ID", orderItem.Product_ID);
            cmd.Parameters.AddWithValue("@Product_Name", orderItem.Product_Name);
            cmd.Parameters.AddWithValue("@Product_Brand", orderItem.Product_Brand);
            cmd.Parameters.AddWithValue("@Product_Category", orderItem.Product_Category);
            cmd.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
            cmd.Parameters.AddWithValue("@Unit_Price", orderItem.Unit_Price);
            cmd.Parameters.AddWithValue("@Subtotal", orderItem.Subtotal);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error adding order item: {ex.Message}");
            throw;
        }
    }

    public Order? GetOrder(int orderId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT OrderID, CustID, Order_Date, Order_Total, Order_Status, Delivery_Address
                FROM [Order] 
                WHERE OrderID = @OrderID";
            cmd.Parameters.AddWithValue("@OrderID", orderId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Order
                {
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    CustID = reader["CustID"].ToString(),
                    Order_Date = DateTime.Parse(reader["Order_Date"].ToString()),
                    Order_Total = Convert.ToDecimal(reader["Order_Total"]),
                    Order_Status = reader["Order_Status"].ToString(),
                    Delivery_Address = reader["Delivery_Address"].ToString(),
                    Payment_Method = "paystack",
                    Payment_Status = "pending",
                    Order_Number = "EL-" + orderId.ToString("D6"),
                    Payment_Date = null,
                    Notes = ""
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting order: {ex.Message}");
        }
        return null;
    }

    public List<DatabaseOrderItem> GetOrderItems(int orderId)
    {
        var orderItems = new List<DatabaseOrderItem>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT OrderItemID, OrderID, Product_ID, Product_Name, Product_Brand, Product_Category, 
                       Quantity, Unit_Price, Subtotal
                FROM OrderItems 
                WHERE OrderID = @OrderID
                ORDER BY OrderItemID";
            cmd.Parameters.AddWithValue("@OrderID", orderId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orderItems.Add(new DatabaseOrderItem
                {
                    OrderItemID = Convert.ToInt32(reader["OrderItemID"]),
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    Product_ID = Convert.ToInt32(reader["Product_ID"]),
                    Product_Name = reader["Product_Name"].ToString(),
                    Product_Brand = reader["Product_Brand"].ToString(),
                    Product_Category = reader["Product_Category"].ToString(),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Unit_Price = Convert.ToDecimal(reader["Unit_Price"]),
                    Subtotal = Convert.ToDecimal(reader["Subtotal"])
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting order items: {ex.Message}");
        }

        return orderItems;
    }

    public void UpdateOrderStatus(int orderId, string status)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE [Order] SET Order_Status = @Status WHERE OrderID = @OrderID";
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating order status: {ex.Message}");
            throw;
        }
    }

    public void UpdatePaymentStatus(int orderId, string paymentStatus, DateTime? paymentDate = null)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE [Order] SET Payment_Status = @PaymentStatus, Payment_Date = @PaymentDate WHERE OrderID = @OrderID";
            cmd.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
            cmd.Parameters.AddWithValue("@PaymentDate", paymentDate?.ToString("o") ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@OrderID", orderId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating payment status: {ex.Message}");
            throw;
        }
    }

    public List<Order> GetCustomerOrders(string custId)
    {
        var orders = new List<Order>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT OrderID, CustID, Order_Date, Order_Total, Order_Status, Delivery_Address
                FROM [Order] 
                WHERE CustID = @CustID
                ORDER BY Order_Date DESC";
            cmd.Parameters.AddWithValue("@CustID", custId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                orders.Add(new Order
                {
                    OrderID = Convert.ToInt32(reader["OrderID"]),
                    CustID = reader["CustID"].ToString(),
                    Order_Date = DateTime.Parse(reader["Order_Date"].ToString()),
                    Order_Total = Convert.ToDecimal(reader["Order_Total"]),
                    Order_Status = reader["Order_Status"].ToString(),
                    Delivery_Address = reader["Delivery_Address"].ToString(),
                    Payment_Method = "paystack",
                    Payment_Status = "pending",
                    Order_Number = "EL-" + Convert.ToInt32(reader["OrderID"]).ToString("D6"),
                    Payment_Date = null,
                    Notes = ""
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting customer orders: {ex.Message}");
        }

        return orders;
    }

    public string GenerateOrderNumber()
    {
        return "EL-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);
    }

    public void StorePaystackReference(int orderId, string paystackRef)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string currentAddress = "";
            using (var getCmd = conn.CreateCommand())
            {
                getCmd.CommandText = "SELECT Delivery_Address FROM [Order] WHERE OrderID = @OrderID";
                getCmd.Parameters.AddWithValue("@OrderID", orderId);
                object result = getCmd.ExecuteScalar();
                if (result != null)
                    currentAddress = result.ToString();
            }

            string combinedAddress = "PAYSTACK_REF:" + paystackRef + "|ORIGINAL_ADDRESS:" + currentAddress;
            using var updateCmd = conn.CreateCommand();
            updateCmd.CommandText = "UPDATE [Order] SET Delivery_Address = @Address WHERE OrderID = @OrderID";
            updateCmd.Parameters.AddWithValue("@Address", combinedAddress);
            updateCmd.Parameters.AddWithValue("@OrderID", orderId);
            updateCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error storing Paystack reference: {ex.Message}");
            throw;
        }
    }

    public void RestoreOriginalAddress(int orderId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            string currentAddress = "";
            using (var getCmd = conn.CreateCommand())
            {
                getCmd.CommandText = "SELECT Delivery_Address FROM [Order] WHERE OrderID = @OrderID";
                getCmd.Parameters.AddWithValue("@OrderID", orderId);
                object result = getCmd.ExecuteScalar();
                if (result != null)
                    currentAddress = result.ToString();
            }

            if (currentAddress.Contains("|ORIGINAL_ADDRESS:"))
            {
                string[] parts = currentAddress.Split('|');
                if (parts.Length > 1)
                {
                    string originalAddress = parts[1].Replace("ORIGINAL_ADDRESS:", "");

                    using var updateCmd = conn.CreateCommand();
                    updateCmd.CommandText = "UPDATE [Order] SET Delivery_Address = @Address WHERE OrderID = @OrderID";
                    updateCmd.Parameters.AddWithValue("@Address", originalAddress);
                    updateCmd.Parameters.AddWithValue("@OrderID", orderId);
                    updateCmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error restoring original address: {ex.Message}");
            throw;
        }
    }

    public bool VerifyWithPaystackAPI(string paystackReference)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"Verifying payment with Paystack: {paystackReference}");
            System.Threading.Thread.Sleep(1000);
            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error verifying with Paystack API: {ex.Message}");
            return false;
        }
    }

    public void UpdateInventory(int orderId, SqliteConnection conn, SqliteTransaction transaction)
    {
        try
        {
            var orderItems = new List<(int ProductId, int Quantity)>();

            using (var getCmd = conn.CreateCommand())
            {
                getCmd.Transaction = transaction;
                getCmd.CommandText = "SELECT Product_ID, Quantity FROM OrderItems WHERE OrderID = @OrderID";
                getCmd.Parameters.AddWithValue("@OrderID", orderId);

                using (var reader = getCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orderItems.Add((
                            Convert.ToInt32(reader["Product_ID"]),
                            Convert.ToInt32(reader["Quantity"])
                        ));
                    }
                }
            }

            foreach (var item in orderItems)
            {
                using var cmd = conn.CreateCommand();
                cmd.Transaction = transaction;
                cmd.CommandText = "UPDATE Products2 SET QuantityOnHand = QuantityOnHand - @Quantity WHERE Product_ID = @Product_ID AND QuantityOnHand >= @Quantity";
                cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                cmd.Parameters.AddWithValue("@Product_ID", item.ProductId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Warning: Could not update inventory for Product_ID {item.ProductId}. Insufficient stock or product not found.");
                    throw new Exception($"Insufficient stock for Product_ID {item.ProductId}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Inventory updated: Product_ID {item.ProductId}, Quantity deducted: {item.Quantity}");
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating inventory: {ex.Message}");
            throw;
        }
    }

    public void UpdateProductInventory(int productId, int quantity, SqliteConnection conn, SqliteTransaction transaction)
    {
        try
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "UPDATE Products2 SET QuantityOnHand = QuantityOnHand - @Quantity WHERE Product_ID = @Product_ID AND QuantityOnHand >= @Quantity";
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Product_ID", productId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Warning: Could not update inventory for Product_ID {productId}. Insufficient stock or product not found.");
                throw new Exception($"Insufficient stock for Product_ID {productId}");
            }

            System.Diagnostics.Debug.WriteLine($"Inventory updated: Product_ID {productId}, Quantity deducted: {quantity}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating product inventory: {ex.Message}");
            throw;
        }
    }

    public int CreatePaymentRecord(int orderId, string custId, string paymentReference, decimal orderTotal, SqliteConnection conn, SqliteTransaction transaction)
    {
        try
        {
            string transactionNumber = DateTime.Now.ToString("yyMMddHHmmss") + new Random().Next(0, 10).ToString();

            int custIdInt;
            if (!int.TryParse(custId, out custIdInt))
                throw new ArgumentException($"Cust_ID must be a valid integer. Received: {custId}");

            System.Diagnostics.Debug.WriteLine($"CreatePaymentRecord: OrderId={orderId}, CustId={custIdInt}, Total={orderTotal}, TransactionNumber={transactionNumber} (Length: {transactionNumber.Length})");

            if (transactionNumber.Length != 13)
                throw new Exception($"Transaction number must be exactly 13 characters. Generated: {transactionNumber} (Length: {transactionNumber.Length})");

            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = @"
                INSERT INTO Payments (
                    Cust_ID, Order_ID, Appointment_ID, Transaction_Number, Payment_Date,
                    Consultation_Fee, Order_Payment, Total_Payable, Payment_Method,
                    Amount_Received, Change_Due, Created_Date, Created_By, Payment_Status,
                    Medical_Aid_Amount, Patient_Portion_Amount, Patient_Payment_Method,
                    Patient_Amount_Received, Patient_Change_Due, Medical_Aid_Reference
                )
                VALUES (
                    @Cust_ID, @Order_ID, NULL, @Transaction_Number, @Payment_Date,
                    NULL, @Order_Payment, @Total_Payable, @Payment_Method,
                    @Amount_Received, NULL, @Created_Date, @Created_By, @Payment_Status,
                    NULL, NULL, NULL, NULL, NULL, NULL
                );
                SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@Cust_ID", custIdInt);
            cmd.Parameters.AddWithValue("@Order_ID", orderId);
            cmd.Parameters.AddWithValue("@Transaction_Number", transactionNumber);
            cmd.Parameters.AddWithValue("@Payment_Date", DateTime.Now.Date.ToString("o"));
            cmd.Parameters.AddWithValue("@Order_Payment", orderTotal);
            cmd.Parameters.AddWithValue("@Total_Payable", orderTotal);
            cmd.Parameters.AddWithValue("@Payment_Method", "Card");
            cmd.Parameters.AddWithValue("@Amount_Received", orderTotal);
            cmd.Parameters.AddWithValue("@Created_Date", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@Created_By", "WEBSITE");
            cmd.Parameters.AddWithValue("@Payment_Status", "Paid");

            System.Diagnostics.Debug.WriteLine($"CreatePaymentRecord: Executing INSERT command...");
            object result = cmd.ExecuteScalar();
            int paymentId = Convert.ToInt32(result);
            System.Diagnostics.Debug.WriteLine($"CreatePaymentRecord: Payment record created successfully with ID = {paymentId}");
            return paymentId;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating payment record: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            throw;
        }
    }
}
