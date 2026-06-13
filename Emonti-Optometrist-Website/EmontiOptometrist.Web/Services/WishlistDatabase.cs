using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services;

public class WishlistDatabase
{
    private readonly string _connectionString;

    public WishlistDatabase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public int GetOrCreateWishlist(int custId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT Wishlist_ID FROM Wishlist WHERE Cust_ID = @CustId";
            cmd.Parameters.AddWithValue("@CustId", custId);
            object result = cmd.ExecuteScalar();

            if (result != null)
                return Convert.ToInt32(result);

            cmd.CommandText = "INSERT INTO Wishlist (Cust_ID, Created_At, Updated_At) VALUES (@CustId, @CreatedAt, @UpdatedAt); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("o"));

            object newWishlistId = cmd.ExecuteScalar();
            return Convert.ToInt32(newWishlistId);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting/creating wishlist: {ex.Message}");
            throw;
        }
    }

    public void AddItemToWishlist(int custId, int productId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT COUNT(*) FROM WishlistItem WHERE Cust_ID = @CustId AND Product_ID = @ProductId";
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.Parameters.AddWithValue("@ProductId", productId);

            int exists = Convert.ToInt32(cmd.ExecuteScalar());
            if (exists > 0) return;

            cmd.CommandText = "INSERT INTO WishlistItem (Cust_ID, Product_ID, Added_At) VALUES (@CustId, @ProductId, @AddedAt)";
            cmd.Parameters.AddWithValue("@AddedAt", DateTime.Now.ToString("o"));
            cmd.ExecuteNonQuery();

            cmd.CommandText = "UPDATE Wishlist SET Updated_At = @UpdatedAt WHERE Cust_ID = @CustId";
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("o"));
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error adding item to wishlist: {ex.Message}");
            throw;
        }
    }

    public void RemoveItemFromWishlist(int wishlistItemId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM WishlistItem WHERE WishlistItem_ID = @WishlistItemId";
            cmd.Parameters.AddWithValue("@WishlistItemId", wishlistItemId);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error removing item from wishlist: {ex.Message}");
            throw;
        }
    }

    public List<WishlistItem> GetWishlistItems(int custId)
    {
        var wishlistItems = new List<WishlistItem>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT wi.WishlistItem_ID, wi.Product_ID, wi.Added_At,
                       p.Product_Name, p.Product_Brand, p.Product_Category, 
                       p.Product_Price, p.QuantityOnHand, p.Picture1, p.Picture2
                FROM WishlistItem wi
                INNER JOIN Products2 p ON wi.Product_ID = p.Product_ID
                WHERE wi.Cust_ID = @CustId
                ORDER BY wi.Added_At DESC";
            cmd.Parameters.AddWithValue("@CustId", custId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int qty = 0;
                int.TryParse(reader["QuantityOnHand"]?.ToString() ?? "0", out qty);

                string stockStatus = "In stock";
                if (qty > 5)
                    stockStatus = "In stock";
                else if (qty <= 5 && qty >= 1)
                    stockStatus = "Limited stock";
                else
                    stockStatus = "Out of stock";

                var item = new WishlistItem
                {
                    WishlistItemId = Convert.ToInt32(reader["WishlistItem_ID"]),
                    ProductId = Convert.ToInt32(reader["Product_ID"]).ToString(),
                    ProductName = reader["Product_Name"].ToString(),
                    Brand = reader["Product_Brand"].ToString(),
                    Category = reader["Product_Category"].ToString(),
                    Price = Convert.ToDecimal(reader["Product_Price"]),
                    ImageUrl = GetProductImage(reader["Picture1"]?.ToString(), reader["Picture2"]?.ToString()),
                    AddedAt = DateTime.Parse(reader["Added_At"].ToString()),
                    StockQuantity = qty,
                    StockStatus = stockStatus,
                    Color = ""
                };

                wishlistItems.Add(item);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting wishlist items: {ex.Message}");
            throw;
        }

        return wishlistItems;
    }

    public bool IsInWishlist(int custId, int productId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM WishlistItem WHERE Cust_ID = @CustId AND Product_ID = @ProductId";
            cmd.Parameters.AddWithValue("@CustId", custId);
            cmd.Parameters.AddWithValue("@ProductId", productId);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count > 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error checking wishlist: {ex.Message}");
            return false;
        }
    }

    public int GetWishlistItemCount(int custId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM WishlistItem WHERE Cust_ID = @CustId";
            cmd.Parameters.AddWithValue("@CustId", custId);

            object result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting wishlist count: {ex.Message}");
            return 0;
        }
    }

    private static string GetProductImage(string? picture1, string? picture2)
    {
        if (!string.IsNullOrEmpty(picture1))
        {
            if (picture1.StartsWith("Images\\Products\\") || picture1.StartsWith("Images/Products/"))
                return $"~/{picture1.Replace("\\", "/")}";
            else
                return $"~/Images/Products/{picture1}";
        }
        else if (!string.IsNullOrEmpty(picture2))
        {
            if (picture2.StartsWith("Images\\Products\\") || picture2.StartsWith("Images/Products/"))
                return $"~/{picture2.Replace("\\", "/")}";
            else
                return $"~/Images/Products/{picture2}";
        }
        else
        {
            return "~/Images/Products/default.png";
        }
    }
}
