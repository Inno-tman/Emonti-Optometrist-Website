using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Emonti_Optometrist_Website
{
    public static class WishlistDatabase
    {
        private static string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        /// <summary>
        /// Get or create a wishlist for a customer
        /// </summary>
        public static int GetOrCreateWishlist(int custId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if wishlist exists
                    string checkQuery = @"
                        SELECT Wishlist_ID FROM Wishlist 
                        WHERE Cust_ID = @CustId";

                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        object result = cmd.ExecuteScalar();
                        
                        if (result != null)
                        {
                            return Convert.ToInt32(result);
                        }
                    }

                    // Create new wishlist
                    string createQuery = @"
                        INSERT INTO Wishlist (Cust_ID, Created_At, Updated_At) 
                        VALUES (@CustId, @CreatedAt, @UpdatedAt);
                        SELECT SCOPE_IDENTITY();";

                    using (SqlCommand cmd = new SqlCommand(createQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        
                        object newWishlistId = cmd.ExecuteScalar();
                        return Convert.ToInt32(newWishlistId);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting/creating wishlist: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Add an item to the wishlist
        /// </summary>
        public static void AddItemToWishlist(int custId, int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Check if item already exists
                    string checkQuery = @"
                        SELECT COUNT(*) FROM WishlistItem 
                        WHERE Cust_ID = @CustId AND Product_ID = @ProductId";

                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        
                        int exists = Convert.ToInt32(cmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            return; // Item already in wishlist
                        }
                    }

                    // Add new item
                    string insertQuery = @"
                        INSERT INTO WishlistItem (Cust_ID, Product_ID, Added_At) 
                        VALUES (@CustId, @ProductId, @AddedAt)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@AddedAt", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }

                    // Update wishlist timestamp
                    string updateQuery = @"
                        UPDATE Wishlist SET Updated_At = @UpdatedAt WHERE Cust_ID = @CustId";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding item to wishlist: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Remove an item from the wishlist
        /// </summary>
        public static void RemoveItemFromWishlist(int wishlistItemId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string deleteQuery = @"
                        DELETE FROM WishlistItem WHERE WishlistItem_ID = @WishlistItemId";

                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@WishlistItemId", wishlistItemId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing item from wishlist: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Get all wishlist items with product details
        /// </summary>
        public static List<WishlistItem> GetWishlistItems(int custId)
        {
            List<WishlistItem> wishlistItems = new List<WishlistItem>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Query without Color column (in case it doesn't exist in database)
                    // If Color column exists, you can add it back: ISNULL(p.Color, 'N/A') as Color
                    string query = @"
                        SELECT wi.WishlistItem_ID, wi.Product_ID, wi.Added_At,
                               p.Product_Name, p.Product_Brand, p.Product_Category, 
                               p.Product_Price, p.QuantityOnHand, p.Picture1, p.Picture2
                        FROM WishlistItem wi
                        INNER JOIN Products2 p ON wi.Product_ID = p.Product_ID
                        WHERE wi.Cust_ID = @CustId
                        ORDER BY wi.Added_At DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int qty = 0;
                                int.TryParse(reader["QuantityOnHand"]?.ToString() ?? "0", out qty);
                                
                                string stockStatus = "In stock";
                                if (qty > 5)
                                {
                                    stockStatus = "In stock";
                                }
                                else if (qty <= 5 && qty >= 1)
                                {
                                    stockStatus = "Limited stock";
                                }
                                else
                                {
                                    stockStatus = "Out of stock";
                                }
                                
                                var item = new WishlistItem
                                {
                                    WishlistItemId = Convert.ToInt32(reader["WishlistItem_ID"]),
                                    ProductId = Convert.ToInt32(reader["Product_ID"]).ToString(),
                                    ProductName = reader["Product_Name"].ToString(),
                                    Brand = reader["Product_Brand"].ToString(),
                                    Category = reader["Product_Category"].ToString(),
                                    Price = Convert.ToDecimal(reader["Product_Price"]),
                                    ImageUrl = GetProductImage(reader["Picture1"]?.ToString(), reader["Picture2"]?.ToString()),
                                    AddedAt = Convert.ToDateTime(reader["Added_At"]),
                                    StockQuantity = qty,
                                    StockStatus = stockStatus,
                                    Color = "" // Color column not available in Products2 table
                                };
                                
                                wishlistItems.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting wishlist items: {ex.Message}");
                throw;
            }

            return wishlistItems;
        }

        /// <summary>
        /// Check if a product is in the user's wishlist
        /// </summary>
        public static bool IsInWishlist(int custId, int productId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query = @"
                        SELECT COUNT(*) FROM WishlistItem 
                        WHERE Cust_ID = @CustId AND Product_ID = @ProductId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking wishlist: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get wishlist item count for header display
        /// </summary>
        public static int GetWishlistItemCount(int custId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    string query = @"
                        SELECT COUNT(*) FROM WishlistItem 
                        WHERE Cust_ID = @CustId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", custId);
                        
                        object result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting wishlist count: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Helper method to get product image URL
        /// </summary>
        private static string GetProductImage(string picture1, string picture2)
        {
            if (!string.IsNullOrEmpty(picture1))
            {
                // If the path already contains Images/Products, use it as is
                if (picture1.StartsWith("Images\\Products\\") || picture1.StartsWith("Images/Products/"))
                {
                    return $"~/{picture1.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{picture1}";
                }
            }
            else if (!string.IsNullOrEmpty(picture2))
            {
                // If the path already contains Images/Products, use it as is
                if (picture2.StartsWith("Images\\Products\\") || picture2.StartsWith("Images/Products/"))
                {
                    return $"~/{picture2.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{picture2}";
                }
            }
            else
            {
                return "~/Images/Products/default.png";
            }
        }
    }

    /// <summary>
    /// Extended CartItem class for wishlist items
    /// </summary>
    public class WishlistItem : CartItem
    {
        public int WishlistItemId { get; set; }
        public DateTime AddedAt { get; set; }
        public int StockQuantity { get; set; }
        public string StockStatus { get; set; }
        public string Color { get; set; }
    }
}