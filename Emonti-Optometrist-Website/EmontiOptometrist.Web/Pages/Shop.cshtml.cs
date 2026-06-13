using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;
using System.Security.Claims;

namespace EmontiOptometrist.Web.Pages;

public class ShopModel : PageModel
{
    private readonly ProductDatabase _productDb;
    private readonly CartDatabase _cartDb;
    private readonly WishlistDatabase _wishlistDb;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public List<Product> Products { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public List<string> Brands { get; set; } = new();
    public string? SearchTerm { get; set; }
    public string? SelectedCategory { get; set; }
    public string? SelectedBrand { get; set; }
    public string? SelectedPriceRange { get; set; }
    public string? SortBy { get; set; }
    public string ResultMessage { get; set; } = "";

    public ShopModel(ProductDatabase productDb, CartDatabase cartDb,
                     WishlistDatabase wishlistDb, IHttpContextAccessor httpContextAccessor)
    {
        _productDb = productDb;
        _cartDb = cartDb;
        _wishlistDb = wishlistDb;
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnGet()
    {
        Products = _productDb.GetAllProducts();
        Categories = Products.Select(p => p.Category).Where(c => !string.IsNullOrEmpty(c)).Distinct().OrderBy(c => c).ToList();
        Brands = Products.Select(p => p.Brand).Where(b => !string.IsNullOrEmpty(b)).Distinct().OrderBy(b => b).ToList();
    }

    public IActionResult OnPostFilter(string? searchTerm, string? category, string? brand,
                                       string? priceRange, string? sortBy)
    {
        SearchTerm = searchTerm;
        SelectedCategory = category;
        SelectedBrand = brand;
        SelectedPriceRange = priceRange;
        SortBy = sortBy;

        var all = _productDb.GetAllProducts();
        Categories = _productDb.GetCategories();
        Brands = _productDb.GetBrands();

        var filtered = all.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            filtered = filtered.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Brand.ToLower().Contains(term) ||
                p.Category.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(category) && !category.Equals("All", StringComparison.OrdinalIgnoreCase))
            filtered = filtered.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(brand) && !brand.Equals("All", StringComparison.OrdinalIgnoreCase))
            filtered = filtered.Where(p => p.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(priceRange) && !priceRange.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            if (priceRange.Contains('+'))
            {
                if (decimal.TryParse(priceRange.Replace("R", "").Replace("+", ""), out var minPrice))
                    filtered = filtered.Where(p => p.Price >= minPrice);
            }
            else if (priceRange.Contains('-'))
            {
                var parts = priceRange.Replace("R", "").Split('-');
                if (parts.Length == 2 &&
                    decimal.TryParse(parts[0], out var minVal) &&
                    decimal.TryParse(parts[1], out var maxVal))
                {
                    filtered = filtered.Where(p => p.Price >= minVal && p.Price <= maxVal);
                }
            }
        }

        Products = sortBy switch
        {
            "name_desc" => filtered.OrderByDescending(p => p.Name).ToList(),
            "price_asc" => filtered.OrderBy(p => p.Price).ToList(),
            "price_desc" => filtered.OrderByDescending(p => p.Price).ToList(),
            _ => filtered.OrderBy(p => p.Name).ToList()
        };

        return Page();
    }

    public IActionResult OnPostAddToCart(int productId, string? brand, string? name,
                                          decimal price, string? picture1, string? picture2)
    {
        var displayName = name ?? "Product";

        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var custId = User.Identity.Name ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                _cartDb.AddItemToCart(cartId, productId, 1, price);
                ResultMessage = $"{displayName} added to cart!";
            }
            catch (Exception ex)
            {
                ResultMessage = $"Error adding to cart: {ex.Message}";
            }
        }
        else
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var sessionId = httpContext?.Session?.Id ?? Guid.NewGuid().ToString();

            var cartItems = CartTransfer.GetCart(sessionId);
            var productIdStr = productId.ToString();
            var existing = cartItems.FirstOrDefault(c => c.ProductId == productIdStr);

            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                var pic = !string.IsNullOrEmpty(picture1)
                    ? $"/Images/Products/{picture1.Replace("\\", "/")}"
                    : !string.IsNullOrEmpty(picture2)
                        ? $"/Images/Products/{picture2.Replace("\\", "/")}"
                        : "/Images/Products/default.png";

                cartItems.Add(new CartItem
                {
                    CartItemId = 0,
                    ProductId = productIdStr,
                    ProductName = displayName,
                    Brand = brand ?? "",
                    Price = price,
                    Quantity = 1,
                    ImageUrl = pic
                });
            }

            CartTransfer.SaveCart(sessionId, cartItems);
            ResultMessage = $"{displayName} added to cart!";
        }

        OnGet();
        return Page();
    }

    public IActionResult OnPostToggleWishlist(int productId)
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            ResultMessage = "Please log in to manage your wishlist.";
            OnGet();
            return Page();
        }

        try
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var custId))
            {
                ResultMessage = "Unable to manage wishlist with your account.";
                OnGet();
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
                else
                {
                    _wishlistDb.RemoveItemFromWishlist(productId);
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

        OnGet();
        return Page();
    }
}
