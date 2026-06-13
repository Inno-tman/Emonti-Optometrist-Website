using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services;

public class ProductDatabase
{
    private readonly string _connectionString;

    public ProductDatabase(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public List<Product> GetAllProducts()
    {
        var products = new List<Product>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Product_ID, Product_Name, Product_Brand, Product_Category,
                       Product_Price, QuantityOnHand, Picture1, Picture2
                FROM Products2
                ORDER BY Product_Name";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(MapProduct(reader));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProductDatabase.GetAllProducts error: {ex.Message}");
        }

        return products;
    }

    public List<string> GetCategories()
    {
        var categories = new List<string>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT DISTINCT Product_Category FROM Products2
                WHERE Product_Category IS NOT NULL AND Product_Category != ''
                ORDER BY Product_Category";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(reader["Product_Category"]?.ToString() ?? "");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProductDatabase.GetCategories error: {ex.Message}");
        }

        return categories;
    }

    public List<string> GetBrands()
    {
        var brands = new List<string>();

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT DISTINCT Product_Brand FROM Products2
                WHERE Product_Brand IS NOT NULL AND Product_Brand != ''
                ORDER BY Product_Brand";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                brands.Add(reader["Product_Brand"]?.ToString() ?? "");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProductDatabase.GetBrands error: {ex.Message}");
        }

        return brands;
    }

    private static Product MapProduct(SqliteDataReader reader)
    {
        var picture1 = reader["Picture1"]?.ToString();
        var picture2 = reader["Picture2"]?.ToString();

        return new Product
        {
            ProductId = Convert.ToInt32(reader["Product_ID"]),
            Name = reader["Product_Name"]?.ToString() ?? "",
            Brand = reader["Product_Brand"]?.ToString() ?? "",
            Category = reader["Product_Category"]?.ToString() ?? "",
            Price = Convert.ToDecimal(reader["Product_Price"]),
            Stock = Convert.ToInt32(reader["QuantityOnHand"]),
            ImageUrl = BuildImageUrl(picture1, picture2)
        };
    }

    private static string BuildImageUrl(string? picture1, string? picture2)
    {
        var pic = picture1 ?? picture2;
        if (string.IsNullOrEmpty(pic))
            return "~/Images/Products/default.png";

        pic = pic.Replace("\\", "/");
        if (pic.StartsWith("Images/Products/"))
            return $"~/{pic}";

        return $"~/Images/Products/{pic}";
    }
}
