using System.Data;
using Microsoft.Data.SqlClient;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services
{
    public class CartDatabase
    {
        private readonly string _connectionString;

        public CartDatabase(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("ProductConnection");
            if (!string.IsNullOrEmpty(connStr))
            {
                var builder = new SqlConnectionStringBuilder(connStr);
                if (builder.ConnectTimeout > 3) builder.ConnectTimeout = 3;
                _connectionString = builder.ConnectionString;
            }
            else
            {
                _connectionString = "";
            }
        }

        public bool VerifyTablesExist()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string checkCartTable = @"
                        SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Cart'";

                    using (var cmd = new SqlCommand(checkCartTable, conn))
                    {
                        int cartTableExists = Convert.ToInt32(cmd.ExecuteScalar());
                        if (cartTableExists == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: Cart table does not exist in database");
                            return false;
                        }
                    }

                    string checkCartItemTable = @"
                        SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'CartItem'";

                    using (var cmd = new SqlCommand(checkCartItemTable, conn))
                    {
                        int cartItemTableExists = Convert.ToInt32(cmd.ExecuteScalar());
                        if (cartItemTableExists == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: CartItem table does not exist in database");
                            return false;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine("Database tables verification successful");
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error verifying database tables: {ex.Message}");
                return false;
            }
        }

        public int GetOrCreateCart(string custId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string getCartQuery = @"
                        SELECT Cart_ID FROM Cart 
                        WHERE Cust_ID = @CustId AND Status = 'Active'";

                    using (var cmd = new SqlCommand(getCartQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }

                    string createCartQuery = @"
                        INSERT INTO Cart (Cust_ID, Status, Created_At, Updated_At) 
                        VALUES (@CustId, 'Active', @CreatedAt, @UpdatedAt);
                        SELECT SCOPE_IDENTITY();";

                    using (var cmd = new SqlCommand(createCartQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                        object newCartId = cmd.ExecuteScalar();
                        return Convert.ToInt32(newCartId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting/creating cart: {ex.Message}");
                throw;
            }
        }

        public void AddItemToCart(int cartId, int productId, int quantity, decimal price)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"AddItemToCart: CartId={cartId}, ProductId={productId}, Quantity={quantity}, Price={price}");

                if (!VerifyTablesExist())
                {
                    throw new Exception("Required database tables (Cart, CartItem) do not exist");
                }

                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection opened successfully");

                    string checkQuery = @"
                        SELECT CartItem_ID, Quantity FROM CartItem 
                        WHERE Cart_ID = @CartId AND Product_ID = @ProductId";

                    int existingItemId = 0;
                    int existingQuantity = 0;

                    using (var cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                existingItemId = Convert.ToInt32(reader["CartItem_ID"]);
                                existingQuantity = Convert.ToInt32(reader["Quantity"]);
                            }
                        }
                    }

                    if (existingItemId > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"Updating existing cart item: CartItemId={existingItemId}, NewQuantity={existingQuantity + quantity}");
                        string updateQuery = @"
                            UPDATE CartItem 
                            SET Quantity = @NewQuantity, Price = @Price 
                            WHERE CartItem_ID = @CartItemId";

                        using (var cmd = new SqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@NewQuantity", existingQuantity + quantity);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@CartItemId", existingItemId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine($"Update query executed, rows affected: {rowsAffected}");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Adding new cart item: CartId={cartId}, ProductId={productId}");
                        string insertQuery = @"
                            INSERT INTO CartItem (Cart_ID, Product_ID, Quantity, Price) 
                            VALUES (@CartId, @ProductId, @Quantity, @Price)";

                        using (var cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CartId", cartId);
                            cmd.Parameters.AddWithValue("@ProductId", productId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.Parameters.AddWithValue("@Price", price);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine($"Insert query executed, rows affected: {rowsAffected}");
                        }
                    }

                    string updateCartQuery = @"
                        UPDATE Cart SET Updated_At = @UpdatedAt WHERE Cart_ID = @CartId";

                    using (var cmd = new SqlCommand(updateCartQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding item to cart: {ex.Message}");
                throw;
            }
        }

        public List<CartItem> GetCartItems(int cartId)
        {
            var cartItems = new List<CartItem>();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT ci.CartItem_ID, ci.Product_ID, ci.Quantity, ci.Price,
                               p.Product_Name, p.Product_Brand, p.Product_Category, p.Picture1
                        FROM CartItem ci
                        INNER JOIN Products2 p ON ci.Product_ID = p.Product_ID
                        WHERE ci.Cart_ID = @CartId
                        ORDER BY ci.CartItem_ID";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cartItems.Add(new CartItem
                                {
                                    CartItemId = Convert.ToInt32(reader["CartItem_ID"]),
                                    ProductId = reader["Product_ID"].ToString(),
                                    ProductName = reader["Product_Name"].ToString(),
                                    Brand = reader["Product_Brand"].ToString(),
                                    Category = reader["Product_Category"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    ImageUrl = reader["Picture1"]?.ToString() ?? ""
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting cart items: {ex.Message}");
            }

            return cartItems;
        }

        public void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (quantity <= 0)
                    {
                        RemoveCartItem(cartItemId);
                    }
                    else
                    {
                        string query = @"
                            UPDATE CartItem 
                            SET Quantity = @Quantity 
                            WHERE CartItem_ID = @CartItemId";

                        using (var cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating cart item quantity: {ex.Message}");
                throw;
            }
        }

        public void RemoveCartItem(int cartItemId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM CartItem WHERE CartItem_ID = @CartItemId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing cart item: {ex.Message}");
                throw;
            }
        }

        public void ClearCart(int cartId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        UPDATE Cart 
                        SET Status = 'Cleared', Updated_At = @UpdatedAt 
                        WHERE Cart_ID = @CartId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing cart: {ex.Message}");
                throw;
            }
        }

        public int GetCartItemCount(int cartId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT ISNULL(SUM(Quantity), 0) 
                        FROM CartItem 
                        WHERE Cart_ID = @CartId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting cart item count: {ex.Message}");
                return 0;
            }
        }

        public void MergeSessionCartWithUserCart(string sessionId, string custId)
        {
            try
            {
                var sessionCartItems = CartTransfer.GetCart(sessionId);

                if (sessionCartItems.Count > 0)
                {
                    int userCartId = GetOrCreateCart(custId);

                    foreach (var item in sessionCartItems)
                    {
                        int productId = Convert.ToInt32(item.ProductId);
                        AddItemToCart(userCartId, productId, item.Quantity, item.Price);
                    }

                    CartTransfer.ClearCart(sessionId);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error merging session cart: {ex.Message}");
                throw;
            }
        }
    }
}
