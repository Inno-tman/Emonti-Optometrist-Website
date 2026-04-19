using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Emonti_Optometrist_Website
{
    public static class CartDatabase
    {
        private static string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        /// <summary>
        /// Verify that the required database tables exist
        /// </summary>
        public static bool VerifyTablesExist()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if Cart table exists
                    string checkCartTable = @"
                        SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Cart'";
                    
                    using (SqlCommand cmd = new SqlCommand(checkCartTable, conn))
                    {
                        int cartTableExists = Convert.ToInt32(cmd.ExecuteScalar());
                        if (cartTableExists == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR: Cart table does not exist in database");
                            return false;
                        }
                    }
                    
                    // Check if CartItem table exists
                    string checkCartItemTable = @"
                        SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'CartItem'";
                    
                    using (SqlCommand cmd = new SqlCommand(checkCartItemTable, conn))
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

        /// <summary>
        /// Get or create an active cart for a logged-in user
        /// </summary>
        /// <param name="custId">Customer ID (required for database carts)</param>
        /// <returns>Cart ID</returns>
        public static int GetOrCreateCart(string custId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // First, try to get existing active cart
                    string getCartQuery = @"
                        SELECT Cart_ID FROM Cart 
                        WHERE Cust_ID = @CustId AND Status = 'Active'";

                    using (SqlCommand cmd = new SqlCommand(getCartQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        conn.Open();
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }

                    // If no active cart exists, create a new one
                    string createCartQuery = @"
                        INSERT INTO Cart (Cust_ID, Status, Created_At, Updated_At) 
                        VALUES (@CustId, 'Active', @CreatedAt, @UpdatedAt);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(createCartQuery, conn))
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

        /// <summary>
        /// Add or update an item in the cart
        /// </summary>
        public static void AddItemToCart(int cartId, int productId, int quantity, decimal price)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"AddItemToCart: CartId={cartId}, ProductId={productId}, Quantity={quantity}, Price={price}");
                
                // Verify tables exist before proceeding
                if (!VerifyTablesExist())
                {
                    throw new Exception("Required database tables (Cart, CartItem) do not exist");
                }
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Open connection once at the beginning
                    System.Diagnostics.Debug.WriteLine("Database connection opened successfully");
                    
                    // Check if item already exists in cart
                    string checkQuery = @"
                        SELECT CartItem_ID, Quantity FROM CartItem 
                        WHERE Cart_ID = @CartId AND Product_ID = @ProductId";

                    int existingItemId = 0;
                    int existingQuantity = 0;

                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
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
                        // Update existing item
                        System.Diagnostics.Debug.WriteLine($"Updating existing cart item: CartItemId={existingItemId}, NewQuantity={existingQuantity + quantity}");
                        string updateQuery = @"
                            UPDATE CartItem 
                            SET Quantity = @NewQuantity, Price = @Price 
                            WHERE CartItem_ID = @CartItemId";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
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
                        // Add new item
                        System.Diagnostics.Debug.WriteLine($"Adding new cart item: CartId={cartId}, ProductId={productId}");
                        string insertQuery = @"
                            INSERT INTO CartItem (Cart_ID, Product_ID, Quantity, Price) 
                            VALUES (@CartId, @ProductId, @Quantity, @Price)";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@CartId", cartId);
                            cmd.Parameters.AddWithValue("@ProductId", productId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.Parameters.AddWithValue("@Price", price);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            System.Diagnostics.Debug.WriteLine($"Insert query executed, rows affected: {rowsAffected}");
                        }
                    }

                    // Update cart's Updated_At timestamp
                    string updateCartQuery = @"
                        UPDATE Cart SET Updated_At = @UpdatedAt WHERE Cart_ID = @CartId";

                    using (SqlCommand cmd = new SqlCommand(updateCartQuery, conn))
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

        /// <summary>
        /// Get all items in a cart with product details
        /// </summary>
        public static List<CartItem> GetCartItems(int cartId)
        {
            List<CartItem> cartItems = new List<CartItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT ci.CartItem_ID, ci.Product_ID, ci.Quantity, ci.Price,
                               p.Product_Name, p.Product_Brand, p.Product_Category, p.Picture1
                        FROM CartItem ci
                        INNER JOIN Products2 p ON ci.Product_ID = p.Product_ID
                        WHERE ci.Cart_ID = @CartId
                        ORDER BY ci.CartItem_ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CartId", cartId);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
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

        /// <summary>
        /// Update quantity of a cart item
        /// </summary>
        public static void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    if (quantity <= 0)
                    {
                        // Remove item if quantity is 0 or negative
                        RemoveCartItem(cartItemId);
                    }
                    else
                    {
                        string query = @"
                            UPDATE CartItem 
                            SET Quantity = @Quantity 
                            WHERE CartItem_ID = @CartItemId";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
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

        /// <summary>
        /// Remove an item from the cart
        /// </summary>
        public static void RemoveCartItem(int cartItemId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM CartItem WHERE CartItem_ID = @CartItemId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
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

        /// <summary>
        /// Clear a cart (mark as Cleared)
        /// </summary>
        public static void ClearCart(int cartId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE Cart 
                        SET Status = 'Cleared', Updated_At = @UpdatedAt 
                        WHERE Cart_ID = @CartId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
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

        /// <summary>
        /// Get total number of items in a cart
        /// </summary>
        public static int GetCartItemCount(int cartId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT ISNULL(SUM(Quantity), 0) 
                        FROM CartItem 
                        WHERE Cart_ID = @CartId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
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

        /// <summary>
        /// Merge session cart items into user's database cart
        /// </summary>
        public static void MergeSessionCartWithUserCart(string sessionId, string custId)
        {
            try
            {
                // Get session cart items
                var sessionCartItems = CartTransfer.GetCart(sessionId);
                
                if (sessionCartItems.Count > 0)
                {
                    // Get or create user's database cart
                    int userCartId = GetOrCreateCart(custId);
                    
                    // Add each session item to database cart
                    foreach (var item in sessionCartItems)
                    {
                        int productId = Convert.ToInt32(item.ProductId);
                        AddItemToCart(userCartId, productId, item.Quantity, item.Price);
                    }
                    
                    // Clear session cart
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
