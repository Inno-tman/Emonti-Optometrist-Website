using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services;

public class CartDatabase
{
    private readonly string _connectionString;

    public CartDatabase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public int GetOrCreateCart(string custId)
    {
        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT Cart_ID FROM Cart WHERE Cust_ID = @CustId AND Status = 'Active'";
            cmd.Parameters.AddWithValue("@CustId", custId);

            object result = cmd.ExecuteScalar();
            if (result != null)
                return Convert.ToInt32(result);

            cmd.CommandText = "INSERT INTO Cart (Cust_ID, Status, Created_At, Updated_At) VALUES (@CustId, 'Active', @CreatedAt, @UpdatedAt); SELECT last_insert_rowid()";
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("o"));

            object newCartId = cmd.ExecuteScalar();
            return Convert.ToInt32(newCartId);
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

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT CartItem_ID, Quantity FROM CartItem WHERE Cart_ID = @CartId AND Product_ID = @ProductId";
            cmd.Parameters.AddWithValue("@CartId", cartId);
            cmd.Parameters.AddWithValue("@ProductId", productId);

            int existingItemId = 0;
            int existingQuantity = 0;

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    existingItemId = Convert.ToInt32(reader["CartItem_ID"]);
                    existingQuantity = Convert.ToInt32(reader["Quantity"]);
                }
            }

            if (existingItemId > 0)
            {
                System.Diagnostics.Debug.WriteLine($"Updating existing cart item: CartItemId={existingItemId}, NewQuantity={existingQuantity + quantity}");
                cmd.CommandText = "UPDATE CartItem SET Quantity = @NewQuantity, Price = @Price WHERE CartItem_ID = @CartItemId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NewQuantity", existingQuantity + quantity);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@CartItemId", existingItemId);
                cmd.ExecuteNonQuery();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Adding new cart item: CartId={cartId}, ProductId={productId}");
                cmd.CommandText = "INSERT INTO CartItem (Cart_ID, Product_ID, Quantity, Price) VALUES (@CartId, @ProductId, @Quantity, @Price)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CartId", cartId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = "UPDATE Cart SET Updated_At = @UpdatedAt WHERE Cart_ID = @CartId";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@CartId", cartId);
            cmd.ExecuteNonQuery();
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
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT ci.CartItem_ID, ci.Product_ID, ci.Quantity, ci.Price,
                       p.Product_Name, p.Product_Brand, p.Product_Category, p.Picture1
                FROM CartItem ci
                INNER JOIN Products2 p ON ci.Product_ID = p.Product_ID
                WHERE ci.Cart_ID = @CartId
                ORDER BY ci.CartItem_ID";
            cmd.Parameters.AddWithValue("@CartId", cartId);

            using var reader = cmd.ExecuteReader();
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
            if (quantity <= 0)
            {
                RemoveCartItem(cartItemId);
            }
            else
            {
                using var conn = new SqliteConnection(_connectionString);
                conn.Open();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE CartItem SET Quantity = @Quantity WHERE CartItem_ID = @CartItemId";
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
                cmd.ExecuteNonQuery();
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
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM CartItem WHERE CartItem_ID = @CartItemId";
            cmd.Parameters.AddWithValue("@CartItemId", cartItemId);
            cmd.ExecuteNonQuery();
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
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE Cart SET Status = 'Cleared', Updated_At = @UpdatedAt WHERE Cart_ID = @CartId";
            cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now.ToString("o"));
            cmd.Parameters.AddWithValue("@CartId", cartId);
            cmd.ExecuteNonQuery();
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
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IFNULL(SUM(Quantity), 0) FROM CartItem WHERE Cart_ID = @CartId";
            cmd.Parameters.AddWithValue("@CartId", cartId);

            object result = cmd.ExecuteScalar();
            return Convert.ToInt32(result);
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
