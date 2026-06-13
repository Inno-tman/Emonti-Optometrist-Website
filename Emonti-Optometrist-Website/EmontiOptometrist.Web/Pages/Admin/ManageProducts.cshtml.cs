using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ManageProductsModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ProductDatabase _productDb;

    public ManageProductsModel(IConfiguration configuration, ProductDatabase productDb)
    {
        _configuration = configuration;
        _productDb = productDb;
    }

    public List<Product> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public List<string> Brands { get; set; } = new();

    [BindProperty]
    public ProductInput NewProduct { get; set; } = new();

    [BindProperty]
    public ProductInput EditProduct { get; set; } = new();

    private void LoadProducts()
    {
        Products = _productDb.GetAllProducts();
        Categories = Products.Select(p => p.Category).Where(c => !string.IsNullOrEmpty(c)).Distinct().OrderBy(c => c).ToList();
        Brands = Products.Select(p => p.Brand).Where(b => !string.IsNullOrEmpty(b)).Distinct().OrderBy(b => b).ToList();
    }

    public void OnGet()
    {
        LoadProducts();
    }

    public IActionResult OnPostAdd()
    {
        if (!ModelState.IsValid)
        {
            LoadProducts();
            return Page();
        }

        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";
        if (!string.IsNullOrEmpty(connStr))
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                        INSERT INTO Products2 (Product_Name, Product_Brand, Product_Category, Product_Price, QuantityOnHand)
                        VALUES (@Name, @Brand, @Category, @Price, @Stock)", conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", NewProduct.Name);
                        cmd.Parameters.AddWithValue("@Brand", NewProduct.Brand);
                        cmd.Parameters.AddWithValue("@Category", NewProduct.Category);
                        cmd.Parameters.AddWithValue("@Price", NewProduct.Price);
                        cmd.Parameters.AddWithValue("@Stock", NewProduct.Stock);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = $"Product \"{NewProduct.Name}\" added successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error adding product: {ex.Message}";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPostEdit()
    {
        if (!ModelState.IsValid)
        {
            LoadProducts();
            return Page();
        }

        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";
        if (!string.IsNullOrEmpty(connStr))
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(@"
                        UPDATE Products2
                        SET Product_Name = @Name, Product_Brand = @Brand, Product_Category = @Category,
                            Product_Price = @Price, QuantityOnHand = @Stock
                        WHERE Product_ID = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", EditProduct.Id);
                        cmd.Parameters.AddWithValue("@Name", EditProduct.Name);
                        cmd.Parameters.AddWithValue("@Brand", EditProduct.Brand);
                        cmd.Parameters.AddWithValue("@Category", EditProduct.Category);
                        cmd.Parameters.AddWithValue("@Price", EditProduct.Price);
                        cmd.Parameters.AddWithValue("@Stock", EditProduct.Stock);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = $"Product \"{EditProduct.Name}\" updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating product: {ex.Message}";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
        }

        return RedirectToPage();
    }

    public IActionResult OnPostDelete(int id)
    {
        var connStr = _configuration.GetConnectionString("ProductConnection") ?? "";
        if (!string.IsNullOrEmpty(connStr))
        {
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DELETE FROM Products2 WHERE Product_ID = @Id", conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = $"Product #{id} deleted.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting product: {ex.Message}";
            }
        }
        else
        {
            TempData["ErrorMessage"] = "Database connection not configured.";
        }

        return RedirectToPage();
    }
}

public class ProductInput
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Category { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
