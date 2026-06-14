using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class OrdersModel : PageModel
{
    private readonly OrderDatabase _orderDb;

    public OrdersModel(OrderDatabase orderDb)
    {
        _orderDb = orderDb;
    }

    public List<OrderDisplay> Orders { get; set; } = new();
    public bool HasOrders => Orders.Count > 0;

    public void OnGet()
    {
        var custId = AuthSession.GetCustId(HttpContext);
        if (string.IsNullOrEmpty(custId))
            return;

        var orders = _orderDb.GetCustomerOrders(custId);

        Orders = orders.Select(o => new OrderDisplay
        {
            Order = o,
            ItemCount = _orderDb.GetOrderItems(o.OrderID).Count
        }).ToList();
    }
}

public class OrderDisplay
{
    public Order Order { get; set; } = new();
    public int ItemCount { get; set; }
}
