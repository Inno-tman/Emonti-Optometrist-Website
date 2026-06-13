using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using EmontiOptometrist.Web.Models;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class CheckoutModel : PageModel
{
    private readonly CartDatabase _cartDb;
    private readonly OrderDatabase _orderDb;
    private readonly IHttpContextAccessor _httpContextAccessor;
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

    public bool IsLoggedIn => User.Identity?.IsAuthenticated == true;

    public CheckoutModel(CartDatabase cartDb, OrderDatabase orderDb, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _cartDb = cartDb;
        _orderDb = orderDb;
        _httpContextAccessor = httpContextAccessor;
        _connectionString = configuration.GetConnectionString("ProductConnection") ?? "";
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
        return RedirectToPage("/OrderConfirmation", new { id = orderId });
    }

    private void LoadCart()
    {
        if (IsLoggedIn)
        {
            try
            {
                var custId = User.Identity!.Name ?? "guest";
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
            var sessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "";
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
            var custId = User.Identity!.Name ?? "";
            using var conn = new SqlConnection(_connectionString);
            string query = @"
                SELECT Customer_Name, Customer_Surname, Customer_Email, Customer_Phone,
                       Street_Number, Street_Name, Complex_Name, Unit_Number,
                       City, Postal_Code
                FROM customer
                WHERE Cust_ID = @CustID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@CustID", custId);
            conn.Open();
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
        if (string.IsNullOrWhiteSpace(FirstName))
        { Message = "First name is required."; return false; }
        if (string.IsNullOrWhiteSpace(LastName))
        { Message = "Last name is required."; return false; }
        if (string.IsNullOrWhiteSpace(Email))
        { Message = "Email is required."; return false; }
        if (string.IsNullOrWhiteSpace(Phone))
        { Message = "Phone number is required."; return false; }
        if (string.IsNullOrWhiteSpace(Address))
        { Message = "Delivery address is required."; return false; }
        if (string.IsNullOrWhiteSpace(City))
        { Message = "City is required."; return false; }
        if (string.IsNullOrWhiteSpace(PostalCode))
        { Message = "Postal code is required."; return false; }

        if (PaymentMethod != "credit_card" && PaymentMethod != "cash_on_delivery" && PaymentMethod != "medical_aid")
        { Message = "Please select a valid payment method."; return false; }

        return true;
    }

    private int CreateOrder()
    {
        try
        {
            var custId = IsLoggedIn ? User.Identity!.Name ?? "guest" : "guest";

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

            using var conn = new SqlConnection(_connectionString);
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
                    string insertPayment = @"
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
                    using var pmtCmd = new SqlCommand(insertPayment, conn, transaction);
                    pmtCmd.Parameters.AddWithValue("@Cust_ID", custIdInt);
                    pmtCmd.Parameters.AddWithValue("@Order_ID", orderId);
                    pmtCmd.Parameters.AddWithValue("@Transaction_Number", transNumber);
                    pmtCmd.Parameters.AddWithValue("@Order_Payment", Total);
                    pmtCmd.Parameters.AddWithValue("@Total_Payable", Total);
                    pmtCmd.Parameters.AddWithValue("@Payment_Method", order.Payment_Method);
                    pmtCmd.Parameters.AddWithValue("@Created_Date", DateTime.Now);
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

    private void ClearCart()
    {
        try
        {
            if (IsLoggedIn)
            {
                var custId = User.Identity!.Name ?? "guest";
                int cartId = _cartDb.GetOrCreateCart(custId);
                _cartDb.ClearCart(cartId);
                HttpContext.Session.Remove("Cart_ID");
            }
            else
            {
                var sessionId = _httpContextAccessor.HttpContext?.Session?.Id ?? "";
                CartTransfer.ClearCart(sessionId);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error clearing cart: {ex.Message}");
        }
    }
}
