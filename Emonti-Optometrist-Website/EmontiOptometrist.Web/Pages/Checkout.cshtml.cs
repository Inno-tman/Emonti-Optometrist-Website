using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;
using System.Net;
using System.Net.Mail;

namespace EmontiOptometrist.Web.Pages;

public class CheckoutModel : PageModel
{
    private readonly CartDatabase _cartDb;
    private readonly OrderDatabase _orderDb;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CheckoutModel> _logger;
    private readonly string _connectionString;

    public List<CartItem> CartItems { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; } = 150.00m;
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public string Message { get; set; } = "";

    [BindProperty]
    public string FirstName { get; set; } = "";

    [BindProperty]
    public string LastName { get; set; } = "";

    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Phone { get; set; } = "";

    [BindProperty]
    public string Address { get; set; } = "";

    [BindProperty]
    public string City { get; set; } = "";

    [BindProperty]
    public string PostalCode { get; set; } = "";

    [BindProperty]
    public string PaymentMethod { get; set; } = "credit_card";

    public bool IsLoggedIn => AuthSession.IsCustomerLoggedIn(HttpContext);
    public string PaystackPublicKey => _configuration["Paystack:PublicKey"] ?? "";

    public CheckoutModel(CartDatabase cartDb, OrderDatabase orderDb, IConfiguration configuration, ILogger<CheckoutModel> logger)
    {
        _cartDb = cartDb;
        _orderDb = orderDb;
        _configuration = configuration;
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public IActionResult OnGet()
    {
        LoadCart();
        if (CartItems.Count == 0)
            return RedirectToPage("/Shop");

        if (IsLoggedIn)
            LoadCustomerAddress();

        return Page();
    }

    public IActionResult OnPost()
    {
        LoadCart();
        if (CartItems.Count == 0)
            return RedirectToPage("/Shop");

        if (!ValidateInput())
            return Page();

        var orderId = CreateOrder();
        if (orderId <= 0)
        {
            Message = "Failed to create order. Please try again.";
            return Page();
        }

        ClearCart();
        SendOrderConfirmationEmail(orderId, Email, FirstName);
        return RedirectToPage("/OrderConfirmation", new { id = orderId });
    }

    public JsonResult OnPostCreatePendingOrder()
    {
        LoadCart();
        if (CartItems.Count == 0)
            return new JsonResult(new { error = "Cart is empty." });

        if (!ValidateInput())
            return new JsonResult(new { error = Message });

        var orderId = CreateOrder();
        if (orderId <= 0)
            return new JsonResult(new { error = "Failed to create order." });

        ClearCart();
        return new JsonResult(new { orderId, total = Total, email = Email });
    }

    private void LoadCart()
    {
        if (IsLoggedIn)
        {
            try
            {
                var custId = AuthSession.GetCustId(HttpContext) ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                CartItems = _cartDb.GetCartItems(cartId);
            }
            catch (Exception ex)
            {
                Message = $"Error loading cart: {ex.Message}";
                CartItems = new();
            }
        }
        else
        {
            var sessionId = HttpContext.Session?.Id ?? "";
            CartItems = CartTransfer.GetCart(sessionId);
        }

        CalculateTotals();
    }

    private void CalculateTotals()
    {
        Subtotal = CartItems.Sum(c => c.Subtotal);
        Discount = 0;
        var promoCode = HttpContext.Session.GetString("PromoCode");
        if (!string.IsNullOrEmpty(promoCode) && promoCode == "SAVE10")
            Discount = Subtotal * 0.10m;
        Total = Subtotal + Shipping - Discount;
    }

    private void LoadCustomerAddress()
    {
        try
        {
            var custId = AuthSession.GetCustId(HttpContext) ?? "";
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Customer_Name, Customer_Surname, Customer_Email, Customer_Phone,
                       Street_Number, Street_Name, Complex_Name, Unit_Number,
                       City, Postal_Code
                FROM customer
                WHERE Cust_ID = @CustID";
            cmd.Parameters.AddWithValue("@CustID", custId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                FirstName = reader["Customer_Name"]?.ToString() ?? "";
                LastName = reader["Customer_Surname"]?.ToString() ?? "";
                Email = reader["Customer_Email"]?.ToString() ?? "";
                Phone = reader["Customer_Phone"]?.ToString() ?? "";

                var streetNum = reader["Street_Number"]?.ToString() ?? "";
                var streetName = reader["Street_Name"]?.ToString() ?? "";
                var complexName = reader["Complex_Name"]?.ToString() ?? "";
                var unitNum = reader["Unit_Number"]?.ToString() ?? "";

                var parts = new List<string>();
                if (!string.IsNullOrEmpty(streetNum) && !string.IsNullOrEmpty(streetName))
                    parts.Add($"{streetNum} {streetName}");
                else if (!string.IsNullOrEmpty(streetName))
                    parts.Add(streetName);
                if (!string.IsNullOrEmpty(complexName))
                    parts.Add(complexName);
                if (!string.IsNullOrEmpty(unitNum))
                    parts.Add($"Unit {unitNum}");

                Address = string.Join(", ", parts);
                City = reader["City"]?.ToString() ?? "";
                PostalCode = reader["Postal_Code"]?.ToString() ?? "";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading customer address: {ex.Message}");
        }
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(FirstName)) { Message = "First name is required."; return false; }
        if (string.IsNullOrWhiteSpace(LastName)) { Message = "Last name is required."; return false; }
        if (string.IsNullOrWhiteSpace(Email)) { Message = "Email is required."; return false; }
        if (string.IsNullOrWhiteSpace(Phone)) { Message = "Phone number is required."; return false; }
        if (string.IsNullOrWhiteSpace(Address)) { Message = "Delivery address is required."; return false; }
        if (string.IsNullOrWhiteSpace(City)) { Message = "City is required."; return false; }
        if (string.IsNullOrWhiteSpace(PostalCode)) { Message = "Postal code is required."; return false; }

        if (PaymentMethod != "credit_card" && PaymentMethod != "cash_on_delivery" && PaymentMethod != "medical_aid")
        { Message = "Please select a valid payment method."; return false; }

        return true;
    }

    private int CreateOrder()
    {
        try
        {
            var custId = IsLoggedIn ? AuthSession.GetCustId(HttpContext) ?? "guest" : "guest";

            var deliveryAddr = $"{FirstName} {LastName}, {Address}, {City}, {PostalCode}, Phone: {Phone}, Email: {Email}";
            if (deliveryAddr.Length > 250)
                deliveryAddr = deliveryAddr[..247] + "...";

            var order = new Order
            {
                CustID = custId,
                Order_Date = DateTime.Now,
                Order_Total = Total,
                Order_Status = "Pending",
                Delivery_Address = deliveryAddr,
                Payment_Method = PaymentMethod switch
                {
                    "credit_card" => "Credit Card",
                    "cash_on_delivery" => "Cash on Delivery",
                    "medical_aid" => "Medical Aid",
                    _ => "Credit Card"
                },
                Payment_Status = PaymentMethod == "cash_on_delivery" ? "pending" : "pending",
                Order_Number = _orderDb.GenerateOrderNumber(),
                Notes = ""
            };

            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                int orderId = _orderDb.CreateOrder(order, conn, transaction);

                foreach (var cartItem in CartItems)
                {
                    var orderItem = new DatabaseOrderItem
                    {
                        OrderID = orderId,
                        Product_ID = Convert.ToInt32(cartItem.ProductId),
                        Product_Name = cartItem.ProductName,
                        Product_Brand = cartItem.Brand,
                        Product_Category = cartItem.Category,
                        Quantity = cartItem.Quantity,
                        Unit_Price = cartItem.Price,
                        Subtotal = cartItem.Subtotal
                    };
                    _orderDb.AddOrderItem(orderItem, conn, transaction);
                }

                _orderDb.UpdateInventory(orderId, conn, transaction);

                if (PaymentMethod == "cash_on_delivery")
                {
                    int custIdInt = 0;
                    int.TryParse(custId, out custIdInt);
                    string transNumber = DateTime.Now.ToString("yyMMddHHmmss") + new Random().Next(0, 10);
                    using var pmtCmd = conn.CreateCommand();
                    pmtCmd.Transaction = transaction;
                    pmtCmd.CommandText = @"
                        INSERT INTO Payments (
                            Cust_ID, Order_ID, Appointment_ID, Transaction_Number, Payment_Date,
                            Consultation_Fee, Order_Payment, Total_Payable, Payment_Method,
                            Amount_Received, Change_Due, Created_Date, Created_By, Payment_Status,
                            Medical_Aid_Amount, Patient_Portion_Amount, Patient_Payment_Method,
                            Patient_Amount_Received, Patient_Change_Due, Medical_Aid_Reference
                        )
                        VALUES (
                            @Cust_ID, @Order_ID, NULL, @Transaction_Number, NULL,
                            NULL, @Order_Payment, @Total_Payable, @Payment_Method,
                            0, NULL, @Created_Date, @Created_By, 'Pending',
                            NULL, NULL, NULL, NULL, NULL, NULL
                        )";
                    pmtCmd.Parameters.AddWithValue("@Cust_ID", custIdInt);
                    pmtCmd.Parameters.AddWithValue("@Order_ID", orderId);
                    pmtCmd.Parameters.AddWithValue("@Transaction_Number", transNumber);
                    pmtCmd.Parameters.AddWithValue("@Order_Payment", Total);
                    pmtCmd.Parameters.AddWithValue("@Total_Payable", Total);
                    pmtCmd.Parameters.AddWithValue("@Payment_Method", order.Payment_Method);
                    pmtCmd.Parameters.AddWithValue("@Created_Date", DateTime.Now.ToString("o"));
                    pmtCmd.Parameters.AddWithValue("@Created_By", "WEBSITE");
                    pmtCmd.ExecuteNonQuery();
                }

                HttpContext.Session.SetInt32("LastOrderId", orderId);
                transaction.Commit();
                return orderId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error creating order: {ex.Message}");
            return 0;
        }
    }

    private void SendOrderConfirmationEmail(int orderId, string customerEmail, string firstName)
    {
        try
        {
            var smtpHost = _configuration["Smtp:Host"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
            var smtpEmail = _configuration["Smtp:Email"] ?? "";
            var smtpPassword = _configuration["Smtp:Password"] ?? "";
            var smtpFromName = _configuration["Smtp:FromName"] ?? "Emonti Optometrist";
            var enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"] ?? "true");

            if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP not configured, skipping order confirmation email");
                return;
            }

            using var smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
            smtp.EnableSsl = enableSsl;
            smtp.Timeout = 30000;

            using var message = new MailMessage();
            message.From = new MailAddress(smtpEmail, smtpFromName);
            message.To.Add(customerEmail);
            message.Subject = $"Order Confirmed - #{orderId} - Emonti Optometrist";

            var body = $@"<!DOCTYPE html>
<html><head><meta charset=""utf-8""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#667eea;padding:25px;text-align:center;"">
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Order Confirmed</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;"">Dear {firstName},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;"">Your order <strong>#{orderId}</strong> has been confirmed and is being processed.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #667eea;"">
<p style=""margin:0 0 10px 0;color:#666;font-size:14px;""><strong>Order Total:</strong> R{Total:F2}</p>
<p style=""margin:0 0 10px 0;color:#666;font-size:14px;""><strong>Payment Method:</strong> {PaymentMethod.Replace("_", " ")}</p>
<p style=""margin:0;color:#666;font-size:14px;""><strong>Delivery Address:</strong> {Address}, {City}, {PostalCode}</p>
</div>
<p style=""margin:20px 0 0 0;color:#555;font-size:14px;"">You can view your order details and track its status in your account dashboard.</p>
<p style=""margin:20px 0 0 0;color:#999;font-size:12px;"">If you have any questions, please contact us at {smtpEmail}</p>
</td></tr>
</table>
</td></tr>
</table>
</body></html>";
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send order confirmation email for order {OrderId}", orderId);
        }
    }

    private void ClearCart()
    {
        try
        {
            if (IsLoggedIn)
            {
                var custId = AuthSession.GetCustId(HttpContext) ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                _cartDb.ClearCart(cartId);
                HttpContext.Session.Remove("Cart_ID");
            }
            else
            {
                var sessionId = HttpContext.Session?.Id ?? "";
                CartTransfer.ClearCart(sessionId);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error clearing cart: {ex.Message}");
        }
    }
}
