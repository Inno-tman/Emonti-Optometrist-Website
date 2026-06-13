using Microsoft.Data.SqlClient;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services
{
    public class ProductDatabase
    {
        private readonly string _connectionString;

        public ProductDatabase(IConfiguration configuration)
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

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            if (string.IsNullOrEmpty(_connectionString)) return products;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT Product_ID, Product_Name, Product_Brand, Product_Category,
                               Product_Price, QuantityOnHand, Picture1, Picture2
                        FROM Products2
                        ORDER BY Product_Name";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                products.Add(MapProduct(reader));
                            }
                        }
                    }
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
            if (string.IsNullOrEmpty(_connectionString)) return categories;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT DISTINCT Product_Category FROM Products2
                        WHERE Product_Category IS NOT NULL AND Product_Category != ''
                        ORDER BY Product_Category";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(reader["Product_Category"].ToString() ?? "");
                            }
                        }
                    }
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
            if (string.IsNullOrEmpty(_connectionString)) return brands;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                        SELECT DISTINCT Product_Brand FROM Products2
                        WHERE Product_Brand IS NOT NULL AND Product_Brand != ''
                        ORDER BY Product_Brand";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                brands.Add(reader["Product_Brand"].ToString() ?? "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductDatabase.GetBrands error: {ex.Message}");
            }

            return brands;
        }

        private static Product MapProduct(SqlDataReader reader)
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
}
