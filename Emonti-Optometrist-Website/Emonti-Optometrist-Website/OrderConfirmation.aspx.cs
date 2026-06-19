using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Net.Mail;
using Emonti_Optometrist_Website.Models;

namespace Emonti_Optometrist_Website
{
    public partial class OrderConfirmation : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if this is a payment confirmation callback
                string paymentRef = Request.QueryString["ref"];
                if (!string.IsNullOrEmpty(paymentRef))
                {
                    // Create order and process payment - returns the orderId if successful
                    int newOrderId = CreateOrderAndProcessPayment(paymentRef);
                    if (newOrderId > 0)
                    {
                        // Load and display the order details directly
                        LoadOrderDetails(newOrderId);
                    }
                    return;
                }
                
                // Load order details if we have an OrderId
                string orderIdStr = Request.QueryString["OrderId"];
                if (!string.IsNullOrEmpty(orderIdStr) && int.TryParse(orderIdStr, out int existingOrderId))
                {
                    LoadOrderDetails(existingOrderId);
                }
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                var order = OrderDatabase.GetOrder(orderId);
                if (order == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Order {orderId} not found in database");
                    return;
                }

                // Display order information
                litOrderNumber.Text = order.Order_Number;
                litOrderDate.Text = order.Order_Date.ToString("MMMM dd, yyyy");

                // Load and display order items
                var orderItems = OrderDatabase.GetOrderItems(orderId);
                if (orderItems != null && orderItems.Count > 0)
                {
                    rptOrderItems.DataSource = orderItems;
                    rptOrderItems.DataBind();
                    
                    decimal subtotal = orderItems.Sum(x => x.Subtotal);
                    decimal shipping = 150.00m;

                    litSubtotal.Text = subtotal.ToString("F2");
                    litShipping.Text = shipping.ToString("F2");
                    litTotal.Text = order.Order_Total.ToString("F2");
                }

                // Display shipping and payment info
                litShippingAddress.Text = order.Delivery_Address.Replace("\n", "<br>");
                litPaymentMethod.Text = GetPaymentMethodDisplay(order.Payment_Method);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading order details: {ex.Message}");
            }
        }

        private string GetPaymentMethodDisplay(string paymentMethod)
        {
            switch (paymentMethod?.ToLower())
            {
                case "paystack":
                    return "Paystack (Secure Payment Gateway)";
                case "card":
                    return "Credit/Debit Card";
                case "eft":
                    return "EFT/Bank Transfer";
                default:
                    return paymentMethod ?? "Not specified";
            }
        }

        protected void btnViewOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Orders.aspx", true);
        }

        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Shop.aspx");
        }

        protected void btnBookAppointment_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BookAppointment.aspx");
        }

        /// <summary>
        /// Simple method: Verifies payment with Paystack, creates order from cart, saves to Orders and Payments tables, updates inventory
        /// Returns the orderId if successful, 0 if failed
        /// </summary>
        private int CreateOrderAndProcessPayment(string paymentReference)
        {
            try
            {
                // Step 1: Verify payment with Paystack API
                bool paymentVerified = OrderDatabase.VerifyWithPaystackAPI(paymentReference);
                if (!paymentVerified)
                {
                    System.Diagnostics.Debug.WriteLine($"Payment verification failed for reference: {paymentReference}");
                    return 0;
                }
                System.Diagnostics.Debug.WriteLine($"Payment verified successfully: {paymentReference}");

                // Step 2: Get customer and cart
                string custId = Session["Cust_ID"]?.ToString();
                if (string.IsNullOrEmpty(custId))
                {
                    System.Diagnostics.Debug.WriteLine("Error: Customer session expired");
                    return 0;
                }

                int cartId = CartDatabase.GetOrCreateCart(custId);
                var cartItems = CartDatabase.GetCartItems(cartId);
                
                if (cartItems == null || cartItems.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("Error: No items in cart");
                    return 0;
                }

                // Calculate totals
                decimal subtotal = cartItems.Sum(x => x.Subtotal);
                decimal shipping = 150.00m;
                decimal discount = 0;
                
                string promoCode = Session["PromoCode"]?.ToString();
                if (promoCode == "SAVE10")
                {
                    discount = subtotal * 0.10m;
                }
                
                decimal total = subtotal + shipping - discount;

                // Get delivery address
                string deliveryAddress = GetCustomerAddress(custId);
                if (string.IsNullOrEmpty(deliveryAddress))
                {
                    deliveryAddress = "Address not available";
                }

                // Create order object
                var order = new Order
                {
                    CustID = custId,
                    Order_Date = DateTime.Now,
                    Order_Total = total,
                    Order_Status = "Paid",
                    Delivery_Address = deliveryAddress,
                    Payment_Method = "paystack",
                    Payment_Status = "Paid",
                    Order_Number = OrderDatabase.GenerateOrderNumber(),
                    Notes = $"Paystack Reference: {paymentReference}"
                };

                // Single transaction: Create order, payment record, order items, update inventory
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Create order
                            int orderId = OrderDatabase.CreateOrder(order, conn, transaction);
                            System.Diagnostics.Debug.WriteLine($"Order created: {orderId}");

                            // 2. Create payment record
                            int paymentId = OrderDatabase.CreatePaymentRecord(orderId, custId, paymentReference, total, conn, transaction);
                            System.Diagnostics.Debug.WriteLine($"Payment record created: {paymentId}");

                            // 3. Add order items
                            foreach (var cartItem in cartItems)
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
                                OrderDatabase.AddOrderItem(orderItem, conn, transaction);
                            }

                            // 4. Update inventory
                            OrderDatabase.UpdateInventory(orderId, conn, transaction);
                            System.Diagnostics.Debug.WriteLine($"Inventory updated for order {orderId}");

                            // 5. Clear cart
                            CartDatabase.ClearCart(cartId);

                            // Commit everything
                            transaction.Commit();
                            System.Diagnostics.Debug.WriteLine($"Transaction committed successfully - Order ID: {orderId}");

                            // Send email (don't wait for it)
                            order.OrderID = orderId;
                            try
                            {
                                SendOrderConfirmationEmail(orderId, order);
                            }
                            catch (Exception emailEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Email send failed (non-critical): {emailEx.Message}");
                            }
                            
                            // Return the orderId so we can display it
                            return orderId;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateOrderAndProcessPayment Error: {ex.Message}");
                return 0;
            }
        }

        private string GetCustomerAddress(string custId)
        {
            try
            {
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"
                        SELECT Street_Number, Street_Name, Complex_Name, Unit_Number
                        FROM customer 
                        WHERE Cust_ID = @CustID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustID", custId);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string streetNumber = reader["Street_Number"]?.ToString() ?? "";
                                string streetName = reader["Street_Name"]?.ToString() ?? "";
                                string complexName = reader["Complex_Name"]?.ToString() ?? "";
                                string unitNumber = reader["Unit_Number"]?.ToString() ?? "";
                                
                                string address = "";
                                if (!string.IsNullOrEmpty(streetNumber) && !string.IsNullOrEmpty(streetName))
                                {
                                    address = streetNumber + " " + streetName;
                                }
                                else if (!string.IsNullOrEmpty(streetName))
                                {
                                    address = streetName;
                                }
                                
                                if (!string.IsNullOrEmpty(complexName))
                                {
                                    address += (string.IsNullOrEmpty(address) ? "" : ", ") + complexName;
                                }
                                
                                if (!string.IsNullOrEmpty(unitNumber))
                                {
                                    address += (string.IsNullOrEmpty(address) ? "" : ", Unit ") + unitNumber;
                                }
                                
                                return address;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting customer address: {ex.Message}");
            }
            
            return "";
        }

        private void SendOrderConfirmationEmail(int orderId, Order order)
        {
            try
            {
                // Get customer email and name
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                string customerEmail = "";
                string customerName = "";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Customer_Email, Customer_Name, Customer_Surname FROM customer WHERE Cust_ID = @CustID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustID", order.CustID);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customerEmail = reader["Customer_Email"]?.ToString() ?? "";
                                string firstName = reader["Customer_Name"]?.ToString() ?? "";
                                string lastName = reader["Customer_Surname"]?.ToString() ?? "";
                                customerName = $"{firstName} {lastName}".Trim();
                                if (string.IsNullOrEmpty(customerName))
                                {
                                    customerName = "Valued Customer";
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(customerEmail))
                {
                    System.Diagnostics.Debug.WriteLine($"No email found for order {orderId}");
                    return;
                }

                // Get order items
                var orderItems = OrderDatabase.GetOrderItems(orderId);
                if (orderItems == null || orderItems.Count == 0) return;

                string orderItemsList = string.Join("", orderItems.Select(item => 
                    $@"<tr>
                        <td style=""padding: 10px; border-bottom: 1px solid #e0e0e0; color: #333; font-size: 14px;"">{System.Web.HttpUtility.HtmlEncode(item.Product_Name)}</td>
                        <td style=""padding: 10px; border-bottom: 1px solid #e0e0e0; text-align: center; color: #333; font-size: 14px;"">{item.Quantity}</td>
                        <td style=""padding: 10px; border-bottom: 1px solid #e0e0e0; text-align: right; color: #333; font-size: 14px;"">R {item.Subtotal:F2}</td>
                    </tr>"));

                decimal subtotal = orderItems.Sum(x => x.Subtotal);
                decimal shipping = 150.00m;
                string logoBase64 = GetLogoBase64();

                string body = $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#667eea;padding:25px;text-align:center;"">
<img src=""{logoBase64}"" alt=""Emonti Optometrist"" style=""max-width:350px;height:auto;display:block;margin:0 auto 15px;"" />
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Order Confirmation</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {System.Web.HttpUtility.HtmlEncode(customerName)},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;line-height:1.6;"">Thank you for your order! We're preparing your items and will notify you once your order ships.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #667eea;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:5px 0;color:#666;font-size:14px;""><strong>Order Number:</strong></td><td style=""padding:5px 0;color:#333;font-size:14px;text-align:right;"">{order.Order_Number}</td></tr>
<tr><td style=""padding:5px 0;color:#666;font-size:14px;""><strong>Order Date:</strong></td><td style=""padding:5px 0;color:#333;font-size:14px;text-align:right;"">{order.Order_Date:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:5px 0;color:#666;font-size:14px;""><strong>Status:</strong></td><td style=""padding:5px 0;color:#667eea;font-size:14px;text-align:right;font-weight:600;"">{order.Order_Status}</td></tr>
</table></div>
<h2 style=""margin:25px 0 15px 0;color:#333;font-size:18px;font-weight:600;"">Order Items</h2>
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse:collapse;margin-bottom:20px;"">
<thead><tr style=""background-color:#f8f9fa;"">
<th style=""padding:10px;text-align:left;border-bottom:2px solid #e0e0e0;color:#333;font-size:14px;font-weight:600;"">Product</th>
<th style=""padding:10px;text-align:center;border-bottom:2px solid #e0e0e0;color:#333;font-size:14px;font-weight:600;"">Qty</th>
<th style=""padding:10px;text-align:right;border-bottom:2px solid #e0e0e0;color:#333;font-size:14px;font-weight:600;"">Price</th>
</tr></thead>
<tbody>{orderItemsList}</tbody>
</table>
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""margin:20px 0;"">
<tr><td style=""padding:8px 0;color:#666;font-size:15px;text-align:right;"">Subtotal:</td><td style=""padding:8px 0;padding-left:20px;color:#333;font-size:15px;text-align:right;width:120px;"">R {subtotal:F2}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:15px;text-align:right;"">Shipping:</td><td style=""padding:8px 0;padding-left:20px;color:#333;font-size:15px;text-align:right;"">R {shipping:F2}</td></tr>
<tr style=""border-top:2px solid #e0e0e0;""><td style=""padding:12px 0 0 0;color:#333;font-size:18px;font-weight:600;text-align:right;"">Total:</td><td style=""padding:12px 0 0 0;padding-left:20px;color:#667eea;font-size:18px;font-weight:700;text-align:right;"">R {order.Order_Total:F2}</td></tr>
</table>
<div style=""background-color:#f8f9fa;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0 0 8px 0;color:#333;font-size:14px;font-weight:600;"">Shipping Address</p>
<p style=""margin:0;color:#555;font-size:14px;line-height:1.6;"">{System.Web.HttpUtility.HtmlEncode(order.Delivery_Address).Replace("\n", "<br>")}</p>
</div>
<div style=""background-color:#fff3cd;border-left:4px solid #ffc107;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0;color:#856404;font-size:14px;line-height:1.6;""><strong>Processing:</strong> Your order will be processed and shipped within 3-5 business days.</p>
</div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;line-height:1.6;"">Questions? Contact us at <a href=""tel:0764631930"" style=""color:#667eea;text-decoration:none;"">076 463 1930</a> or <a href=""mailto:emontioptom@gmail.com"" style=""color:#667eea;text-decoration:none;"">emontioptom@gmail.com</a></p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

                // Send email
                var smtpSettings = Emonti_Optometrist_Website.SmtpSettings.Load();

                if (string.IsNullOrEmpty(smtpSettings.Username) || string.IsNullOrEmpty(smtpSettings.Password))
                {
                    System.Diagnostics.Debug.WriteLine("SMTP credentials not configured");
                    return;
                }

                using (SmtpClient smtp = new SmtpClient(smtpSettings.Host, smtpSettings.Port))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpSettings.Username, smtpSettings.Password);
                    smtp.EnableSsl = smtpSettings.EnableSsl;
                    smtp.Timeout = 30000;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(string.IsNullOrEmpty(smtpSettings.Email) ? smtpSettings.Username : smtpSettings.Email, smtpSettings.FromName);
                        message.To.Add(customerEmail);
                        message.Subject = $"Order Confirmation - {order.Order_Number} - Emonti Optometrist";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        smtp.Send(message);
                        System.Diagnostics.Debug.WriteLine($"Email sent to: {customerEmail}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private string GetLogoBase64()
        {
            try
            {
                string logoPath = HttpContext.Current.Server.MapPath("~/Images/Logo/Emonti Logo Banner.png");
                byte[] imageBytes = System.IO.File.ReadAllBytes(logoPath);
                string base64 = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading logo: {ex.Message}");
                return "";
            }
        }
    }
}
