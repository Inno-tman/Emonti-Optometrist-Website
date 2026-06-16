using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class OrderConfirmationModel : PageModel
{
    private readonly OrderDatabase _orderDb;
    private readonly ILogger<OrderConfirmationModel> _logger;

    public Order? Order { get; set; }
    public List<DatabaseOrderItem> OrderItems { get; set; } = new();
    public bool OrderNotFound { get; set; }
    public bool PaymentVerified { get; set; }

    public OrderConfirmationModel(OrderDatabase orderDb, ILogger<OrderConfirmationModel> logger)
    {
        _orderDb = orderDb;
        _logger = logger;
    }

    public IActionResult OnGet(int id, [FromQuery(Name = "ref")] string? paymentRef = null)
    {
        if (!string.IsNullOrEmpty(paymentRef))
        {
            bool verified = _orderDb.VerifyWithPaystackAPI(paymentRef);
            if (verified)
            {
                var order = _orderDb.GetOrder(id);
                if (order != null && order.Order_Status == "Pending")
                {
                    _orderDb.UpdateOrderStatus(id, "Processing");
                    _orderDb.UpdatePaymentStatus(id, "Paid", DateTime.Now);
                    _orderDb.RestoreOriginalAddress(id);
                    PaymentVerified = true;
                }
                else if (order != null)
                {
                    PaymentVerified = true;
                }
            }
            else
            {
                _logger.LogWarning("Paystack payment verification failed for ref: {Ref}, order: {Id}", paymentRef, id);
            }
        }

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
