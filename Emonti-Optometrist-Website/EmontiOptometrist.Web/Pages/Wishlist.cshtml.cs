using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class WishlistModel : PageModel
{
    private readonly WishlistDatabase _wishlistDb;

    public List<WishlistItem> WishlistItems { get; set; } = new();
    public string Message { get; set; } = "";

    public WishlistModel(WishlistDatabase wishlistDb)
    {
        _wishlistDb = wishlistDb;
    }

    public void OnGet()
    {
        LoadWishlist();
    }

    public IActionResult OnPostRemove(int wishlistItemId)
    {
        try
        {
            _wishlistDb.RemoveItemFromWishlist(wishlistItemId);
        }
        catch (Exception ex)
        {
            Message = $"Error removing item: {ex.Message}";
        }

        LoadWishlist();
        return Page();
    }

    private void LoadWishlist()
    {
        var userId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(userId))
        {
            Message = "Could not identify user.";
            return;
        }

        if (!int.TryParse(userId, out int custId))
        {
            Message = "Invalid user identifier.";
            return;
        }

        try
        {
            WishlistItems = _wishlistDb.GetWishlistItems(custId);
        }
        catch (Exception ex)
        {
            Message = $"Error loading wishlist: {ex.Message}";
        }
    }
}
