using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class CartModel : PageModel
{
    private readonly CartDatabase _cartDb;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public List<CartItem> CartItems { get; set; } = new();
    public decimal CartTotal { get; set; }
    public string Message { get; set; } = "";

    public CartModel(CartDatabase cartDb, IHttpContextAccessor httpContextAccessor)
    {
        _cartDb = cartDb;
        _httpContextAccessor = httpContextAccessor;
    }

    public void OnGet()
    {
        LoadCart();
    }

    public IActionResult OnPostUpdateQuantity(int cartItemId, string productId, int quantity)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (cartItemId > 0)
            {
                if (quantity <= 0)
                    _cartDb.RemoveCartItem(cartItemId);
                else
                    _cartDb.UpdateCartItemQuantity(cartItemId, quantity);
            }
        }
        else
        {
            var sessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "";
            var cartItems = CartTransfer.GetCart(sessionId);
            var item = cartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cartItems.Remove(item);
                else
                    item.Quantity = quantity;
                CartTransfer.SaveCart(sessionId, cartItems);
            }
        }

        LoadCart();
        return Page();
    }

    public IActionResult OnPostRemoveItem(int cartItemId, string productId)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (cartItemId > 0)
                _cartDb.RemoveCartItem(cartItemId);
        }
        else
        {
            var sessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "";
            var cartItems = CartTransfer.GetCart(sessionId);
            cartItems.RemoveAll(c => c.ProductId == productId);
            CartTransfer.SaveCart(sessionId, cartItems);
        }

        LoadCart();
        return Page();
    }

    private void LoadCart()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            try
            {
                var custId = User.Identity.Name ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                CartItems = _cartDb.GetCartItems(cartId);
            }
            catch (Exception ex)
            {
                Message = $"Error loading cart: {ex.Message}";
            }
        }
        else
        {
            var sessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "";
            CartItems = CartTransfer.GetCart(sessionId);
        }

        CartTotal = CartItems.Sum(c => c.Subtotal);
    }
}
