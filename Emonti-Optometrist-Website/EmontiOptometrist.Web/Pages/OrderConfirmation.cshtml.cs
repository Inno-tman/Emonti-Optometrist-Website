using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class OrderConfirmationModel : PageModel
{
    private readonly OrderDatabase _orderDb;

    public Order? Order { get; set; }
    public List<DatabaseOrderItem> OrderItems { get; set; } = new();
    public bool OrderNotFound { get; set; }

    public OrderConfirmationModel(OrderDatabase orderDb)
    {
        _orderDb = orderDb;
    }

    public IActionResult OnGet(int id)
    {
        Order = _orderDb.GetOrder(id);
        if (Order == null)
        {
            OrderNotFound = true;
            return Page();
        }

        OrderItems = _orderDb.GetOrderItems(id);
        return Page();
    }
}
