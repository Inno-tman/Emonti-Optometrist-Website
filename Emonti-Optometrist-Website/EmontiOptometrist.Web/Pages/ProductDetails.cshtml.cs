using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class ProductDetailsModel : PageModel
{
    private readonly ProductDatabase _productDb;
    private readonly CartDatabase _cartDb;
    private readonly WishlistDatabase _wishlistDb;
    private readonly IConfiguration _configuration;

    public Product? Product { get; set; }
    public List<Product> RelatedProducts { get; set; } = new();
    public string ResultMessage { get; set; } = "";
    public bool IsInWishlist { get; set; }

    public ProductDetailsModel(ProductDatabase productDb, CartDatabase cartDb,
                                WishlistDatabase wishlistDb, IConfiguration configuration)
    {
        _productDb = productDb;
        _cartDb = cartDb;
        _wishlistDb = wishlistDb;
        _configuration = configuration;
    }

    public void OnGet(int productId)
    {
        LoadProduct(productId);
    }

    private void LoadProduct(int productId)
    {
        var allProducts = _productDb.GetAllProducts();
        Product = allProducts.FirstOrDefault(p => p.ProductId == productId);

        if (Product != null)
        {
            var description = GetProductDescription(productId);
            if (!string.IsNullOrEmpty(description))
                Product.Description = description;

            RelatedProducts = allProducts
                .Where(p => p.Category == Product.Category && p.ProductId != productId)
                .Take(4)
                .ToList();
        }

        if (AuthSession.IsCustomerLoggedIn(HttpContext) && Product != null)
        {
            var userIdClaim = AuthSession.GetCustId(HttpContext);
            if (int.TryParse(userIdClaim, out var custId))
                IsInWishlist = _wishlistDb.IsInWishlist(custId, productId);
        }
    }

    private string GetProductDescription(int productId)
    {
        var connStr = _configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connStr)) return "";

        try
        {
            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Product_Description FROM Products2 WHERE Product_ID = @Id";
            cmd.Parameters.AddWithValue("@Id", productId);
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? "";
        }
        catch
        {
            return "";
        }
    }

    public IActionResult OnPostAddToCart(int productId)
    {
        var allProducts = _productDb.GetAllProducts();
        var product = allProducts.FirstOrDefault(p => p.ProductId == productId);
        var displayName = product?.Name ?? "Product";

        if (AuthSession.IsCustomerLoggedIn(HttpContext))
        {
            try
            {
                var custId = AuthSession.GetCustId(HttpContext) ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                _cartDb.AddItemToCart(cartId, productId, 1, product?.Price ?? 0);
                ResultMessage = $"{displayName} added to cart!";
            }
            catch (Exception ex)
            {
                ResultMessage = $"Error adding to cart: {ex.Message}";
            }
        }
        else
        {
            var sessionId = HttpContext.Session?.Id ?? Guid.NewGuid().ToString();

            var cartItems = CartTransfer.GetCart(sessionId);
            var productIdStr = productId.ToString();
            var existing = cartItems.FirstOrDefault(c => c.ProductId == productIdStr);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                cartItems.Add(new CartItem
                {
                    CartItemId = 0,
                    ProductId = productIdStr,
                    ProductName = displayName,
                    Brand = product?.Brand ?? "",
                    Price = product?.Price ?? 0,
                    Quantity = 1,
                    ImageUrl = product?.ImageUrl ?? "/Images/Products/default.png"
                });
            }

            CartTransfer.SaveCart(sessionId, cartItems);
            ResultMessage = $"{displayName} added to cart!";
        }

        LoadProduct(productId);
        return Page();
    }

    public IActionResult OnPostAddToWishlist(int productId)
    {
        if (!AuthSession.IsCustomerLoggedIn(HttpContext))
        {
            ResultMessage = "Please log in to manage your wishlist.";
            LoadProduct(productId);
            return Page();
        }

        try
        {
            var userIdClaim = AuthSession.GetCustId(HttpContext);
            if (!int.TryParse(userIdClaim, out var custId))
            {
                ResultMessage = "Unable to manage wishlist with your account.";
                LoadProduct(productId);
                return Page();
            }

            if (_wishlistDb.IsInWishlist(custId, productId))
            {
                var items = _wishlistDb.GetWishlistItems(custId);
                var item = items.FirstOrDefault(i =>
                    int.TryParse(i.ProductId, out var pid) && pid == productId);

                if (item != null)
                {
                    _wishlistDb.RemoveItemFromWishlist(item.WishlistItemId);
                    ResultMessage = "Removed from wishlist.";
                }
            }
            else
            {
                _wishlistDb.AddItemToWishlist(custId, productId);
                ResultMessage = "Added to wishlist!";
            }
        }
        catch (Exception ex)
        {
            ResultMessage = $"Error updating wishlist: {ex.Message}";
        }

        LoadProduct(productId);
        return Page();
    }
}
