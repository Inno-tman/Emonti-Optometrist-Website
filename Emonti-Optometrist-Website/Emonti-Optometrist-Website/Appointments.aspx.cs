using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class Appointments : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadAppointments();
            }
        }

        private void LoadAppointments()
        {
            try
            {
                string customerId = Session["Cust_ID"]?.ToString();
                if (string.IsNullOrEmpty(customerId))
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                // Debug: Log customer ID
                System.Diagnostics.Debug.WriteLine($"Loading appointments for Customer ID: {customerId}");
                
                // Debug: Test basic connection and query
                TestDatabaseConnection(customerId);

                // Load real appointments from database
                List<AppointmentInfo> appointments = GetCustomerAppointments(customerId);
                
                // Debug: Log appointment count
                System.Diagnostics.Debug.WriteLine($"Found {appointments.Count} appointments for customer {customerId}");
                
                if (appointments.Count > 0)
                {
                    rptAppointments.DataSource = appointments;
                    rptAppointments.DataBind();
                    pnlAppointments.Visible = true;
                    pnlNoAppointments.Visible = false;
                }
                else
                {
                    pnlAppointments.Visible = false;
                    pnlNoAppointments.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Handle error
                System.Diagnostics.Debug.WriteLine($"Error in LoadAppointments: {ex.Message}");
                pnlAppointments.Visible = false;
                pnlNoAppointments.Visible = true;
            }
        }

        private List<AppointmentInfo> GetCustomerAppointments(string customerId)
        {
            List<AppointmentInfo> appointments = new List<AppointmentInfo>();
            
            try
            {
                // Simple approach: Just get basic appointment data first
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string basicQuery = @"
                        SELECT a.Appointment_ID, a.Appointment_Date, a.Appoinment_Status, a.Staff_ID, a.AppointmentTimeID
                        FROM Appointment a
                        WHERE a.Cust_ID = @CustomerId
                        ORDER BY a.Appointment_Date DESC";

                    using (SqlCommand cmd = new SqlCommand(basicQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                System.Diagnostics.Debug.WriteLine($"Found appointment {count}: ID={reader["Appointment_ID"]}, Date={reader["Appointment_Date"]}, Status={reader["Appoinment_Status"]}");
                                
                                // Get staff name separately
                                string staffName = GetStaffName(reader["Staff_ID"]?.ToString());
                                
                                // Get time slot separately  
                                string timeSlot = GetTimeSlot(reader["AppointmentTimeID"]?.ToString());
                                
                                // Get payment info separately
                                var paymentInfo = GetPaymentInfo(reader["Appointment_ID"].ToString());
                                
                                var appointment = new AppointmentInfo
                                {
                                    AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                                    AppointmentDate = Convert.ToDateTime(reader["Appointment_Date"]),
                                    ServiceType = "",
                                    Duration = 0,
                                    DoctorName = staffName,
                                    Status = reader["Appoinment_Status"].ToString(),
                                    Notes = timeSlot,
                                    HasPayment = paymentInfo.HasPayment,
                                    ConsultationFee = paymentInfo.ConsultationFee,
                                    TotalPayable = paymentInfo.TotalPayable,
                                    PaymentStatus = paymentInfo.PaymentStatus,
                                    PaymentMethod = paymentInfo.PaymentMethod,
                                    MedicalAidAmount = 0,
                                    PatientPortionAmount = 0,
                                    IsUpcoming = Convert.ToDateTime(reader["Appointment_Date"]) >= DateTime.Today
                                };
                                
                                // Calculate appointment type
                                appointment.AppointmentType = CalculateAppointmentType(appointment);
                                
                                appointments.Add(appointment);
                            }
                            
                            System.Diagnostics.Debug.WriteLine($"Basic query found {count} appointments for customer {customerId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading appointments: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            return appointments;
        }
        
        
        
        private string CalculateAppointmentType(AppointmentInfo appointment)
        {
            if (appointment.Status == "Cancelled")
                return "Cancelled";
            else if (appointment.AppointmentDate < DateTime.Today)
            {
                if (appointment.HasPayment)
                    return "Completed";
                else
                    return "Missed";
            }
            else
                return "Upcoming";
        }
        
        private string GetStaffName(string staffId)
        {
            if (string.IsNullOrEmpty(staffId))
                return ""; // Don't show anything if no staff assigned
                
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Staff_Name, Staff_Surname FROM Staff WHERE Staff_ID = @StaffId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return $"{reader["Staff_Name"]} {reader["Staff_Surname"]}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting staff name: {ex.Message}");
            }
            
            return ""; // Don't show anything if not found in database
        }
        
        private string GetTimeSlot(string timeId)
        {
            if (string.IsNullOrEmpty(timeId))
                return ""; // Don't show anything if no time assigned
                
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Timeslot FROM tblTime WHERE TimeID = @TimeId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return reader["Timeslot"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting time slot: {ex.Message}");
            }
            
            return ""; // Don't show anything if not found in database
        }
        
        private (bool HasPayment, decimal ConsultationFee, decimal TotalPayable, string PaymentStatus, string PaymentMethod) GetPaymentInfo(string appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Payment_ID, Consultation_Fee, Total_Payable, Payment_Status, Payment_Method FROM Payments WHERE Appointment_ID = @AppointmentId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return (
                                    reader["Payment_ID"] != DBNull.Value,
                                    reader["Consultation_Fee"] != DBNull.Value ? Convert.ToDecimal(reader["Consultation_Fee"]) : 0,
                                    reader["Total_Payable"] != DBNull.Value ? Convert.ToDecimal(reader["Total_Payable"]) : 0,
                                    reader["Payment_Status"]?.ToString() ?? "Not Paid",
                                    reader["Payment_Method"]?.ToString() ?? ""
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting payment info: {ex.Message}");
            }
            
            return (false, 0, 0, "Not Paid", "");
        }
        
        private void TestDatabaseConnection(string customerId)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"=== DATABASE DEBUGGING START ===");
                System.Diagnostics.Debug.WriteLine($"Connection String: {connectionString}");
                System.Diagnostics.Debug.WriteLine($"Customer ID from session: {customerId}");
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    System.Diagnostics.Debug.WriteLine("Database connection successful");
                    
                    // Test 1: Check if customer exists in Appointment table
                    string testQuery1 = "SELECT COUNT(*) FROM Appointment WHERE Cust_ID = @CustomerId";
                    using (SqlCommand cmd = new SqlCommand(testQuery1, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        int count = (int)cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine($"Test 1 - Appointments for customer {customerId}: {count}");
                    }
                    
                    // Test 2: Check all appointments in the table
                    string testQuery2 = "SELECT TOP 5 Cust_ID, Appointment_ID, Appointment_Date FROM Appointment ORDER BY Appointment_ID DESC";
                    using (SqlCommand cmd = new SqlCommand(testQuery2, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            System.Diagnostics.Debug.WriteLine("Test 2 - Recent appointments in database:");
                            int totalCount = 0;
                            while (reader.Read())
                            {
                                totalCount++;
                                System.Diagnostics.Debug.WriteLine($"  {totalCount}. Cust_ID: {reader["Cust_ID"]}, Appointment_ID: {reader["Appointment_ID"]}, Date: {reader["Appointment_Date"]}");
                            }
                            System.Diagnostics.Debug.WriteLine($"Total appointments found: {totalCount}");
                        }
                    }
                    
                    // Test 3: Check if customer 6 specifically exists
                    string testQuery3 = "SELECT Cust_ID, Appointment_ID, Appointment_Date, Appoinment_Status FROM Appointment WHERE Cust_ID = 6";
                    using (SqlCommand cmd = new SqlCommand(testQuery3, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            System.Diagnostics.Debug.WriteLine("Test 3 - All appointments for customer 6:");
                            int count = 0;
                            while (reader.Read())
                            {
                                count++;
                                System.Diagnostics.Debug.WriteLine($"  Appointment {count}: ID={reader["Appointment_ID"]}, Date={reader["Appointment_Date"]}, Status={reader["Appoinment_Status"]}");
                            }
                            if (count == 0)
                            {
                                System.Diagnostics.Debug.WriteLine("  No appointments found for customer 6");
                            }
                        }
                    }
                    
                    // Test 4: Check all customers with appointments
                    string testQuery4 = "SELECT DISTINCT Cust_ID, COUNT(*) as AppointmentCount FROM Appointment GROUP BY Cust_ID ORDER BY Cust_ID";
                    using (SqlCommand cmd = new SqlCommand(testQuery4, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            System.Diagnostics.Debug.WriteLine("Test 4 - All customers with appointments:");
                            while (reader.Read())
                            {
                                System.Diagnostics.Debug.WriteLine($"  Customer {reader["Cust_ID"]}: {reader["AppointmentCount"]} appointments");
                            }
                        }
                    }
                    
                    // Test 5: Check session data
                    System.Diagnostics.Debug.WriteLine("Test 5 - Session data:");
                    System.Diagnostics.Debug.WriteLine($"  Session['Cust_ID']: {Session["Cust_ID"]}");
                    System.Diagnostics.Debug.WriteLine($"  Session['IsLoggedIn']: {Session["IsLoggedIn"]}");
                    System.Diagnostics.Debug.WriteLine($"  Session['UserEmail']: {Session["UserEmail"]}");
                }
                
                System.Diagnostics.Debug.WriteLine($"=== DATABASE DEBUGGING END ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection test failed: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            // Implement filter functionality
            LoadAppointments(); // For now, just reload all appointments
        }

        protected void rptAppointments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string commandName = e.CommandName;
            string appointmentId = e.CommandArgument.ToString();

            switch (commandName)
            {
                case "CancelAppointment":
                    // Cancel the appointment
                    CancelAppointment(appointmentId);
                    break;
                case "RebookAppointment":
                    // Redirect to booking page with pre-filled details
                    Response.Redirect($"~/BookAppointment.aspx?rebook={appointmentId}");
                    break;
            }
        }

        protected string GetAppointmentCssClass(object appointmentType)
        {
            string type = appointmentType?.ToString() ?? "";
            switch (type.ToLower())
            {
                case "missed":
                    return "missed";
                case "upcoming":
                    return "upcoming";
                case "completed":
                    return "completed";
                case "cancelled":
                    return "cancelled";
                default:
                    return "";
            }
        }

        private void CancelAppointment(string appointmentId)
        {
            try
            {
                // First, check if appointment can be cancelled
                if (!CanCancelAppointment(appointmentId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "CancelError", 
                        "alert('This appointment cannot be cancelled. Please contact us for assistance.');", true);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE Appointment 
                        SET Appoinment_Status = 'Cancelled' 
                        WHERE Appointment_ID = @AppointmentId 
                        AND Cust_ID = @CustomerId
                        AND Appoinment_Status IN ('Pending', 'Scheduled')
                        AND Appointment_Date > GETDATE()";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@CustomerId", Session["Cust_ID"]);
                        conn.Open();
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected > 0)
                        {
                            // Send cancellation email
                            SendCancellationEmail(appointmentId);
                            
                            ScriptManager.RegisterStartupScript(this, GetType(), "CancelSuccess", 
                                "alert('Appointment cancelled successfully! A confirmation email has been sent.');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "CancelError", 
                                "alert('Failed to cancel appointment. It may have already been cancelled or is too close to the appointment time.');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "CancelError", 
                    "alert('An error occurred while cancelling the appointment.');", true);
                System.Diagnostics.Debug.WriteLine($"Error cancelling appointment: {ex.Message}");
            }
            
            LoadAppointments(); // Reload appointments to reflect changes
        }

        private bool CanCancelAppointment(string appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT Appointment_Date, Appoinment_Status
                        FROM Appointment 
                        WHERE Appointment_ID = @AppointmentId 
                        AND Cust_ID = @CustomerId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@CustomerId", Session["Cust_ID"]);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime appointmentDate = Convert.ToDateTime(reader["Appointment_Date"]);
                                string status = reader["Appoinment_Status"].ToString();
                                
                                // Can cancel if:
                                // 1. Status is Pending or Scheduled
                                // 2. Appointment is in the future
                                // 3. At least 2 hours before appointment (optional business rule)
                                return (status == "Pending" || status == "Scheduled") && 
                                       appointmentDate > DateTime.Now.AddHours(2);
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            
            return false;
        }

        private void SendCancellationEmail(string appointmentId)
        {
            try
            {
                // Get appointment details
                var appointmentDetails = GetAppointmentDetails(appointmentId);
                if (appointmentDetails == null) return;

                string customerEmail = Session["UserEmail"]?.ToString();
                if (string.IsNullOrEmpty(customerEmail)) return;

                // Get customer name from session
                string firstName = Session["FirstName"]?.ToString() ?? "";
                string lastName = Session["LastName"]?.ToString() ?? "";
                string customerName = $"{firstName} {lastName}".Trim();
                if (string.IsNullOrEmpty(customerName))
                {
                    customerName = "Valued Customer";
                }

                string logoBase64 = GetLogoBase64();

                string body = $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#6c757d;padding:25px;text-align:center;"">
<img src=""{logoBase64}"" alt=""Emonti Optometrist"" style=""max-width:350px;height:auto;display:block;margin:0 auto 15px;"" />
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Appointment Cancelled</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {System.Web.HttpUtility.HtmlEncode(customerName)},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;line-height:1.6;"">This email confirms that your appointment has been successfully cancelled.</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #6c757d;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Optometrist:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;text-align:right;"">{System.Web.HttpUtility.HtmlEncode(appointmentDetails.DoctorName)}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;text-align:right;"">{appointmentDetails.AppointmentDate:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;text-align:right;"">{System.Web.HttpUtility.HtmlEncode(appointmentDetails.Notes)}</td></tr>
</table></div>
<div style=""background-color:#e7f3ff;border-left:4px solid #2196F3;padding:15px;margin:20px 0;border-radius:4px;text-align:center;"">
<p style=""margin:0 0 8px 0;color:#1976D2;font-size:15px;font-weight:600;"">Need to reschedule?</p>
<p style=""margin:0;color:#1565C0;font-size:14px;line-height:1.6;"">We'd be happy to help you book a new appointment. Call us at <a href=""tel:0764631930"" style=""color:#1976D2;text-decoration:underline;font-weight:600;"">076 463 1930</a> or book online.</p>
</div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;line-height:1.6;"">Thank you for choosing Emonti Optometrist. We hope to serve you in the future!</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

                // Get SMTP configuration from web.config
                string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpEmail = System.Configuration.ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
                string smtpFromName = System.Configuration.ConfigurationManager.AppSettings["SmtpFromName"] ?? "Emonti Optometrist";
                bool enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");

                if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
                {
                    System.Diagnostics.Debug.WriteLine("SMTP credentials not configured");
                    return;
                }

                using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                    smtp.EnableSsl = enableSsl;
                    smtp.Timeout = 30000;

                    using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
                    {
                        message.From = new System.Net.Mail.MailAddress(smtpEmail, smtpFromName);
                        message.To.Add(customerEmail);
                        message.Subject = "Appointment Cancellation Confirmation - Emonti Optometrist";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending cancellation email: {ex.Message}");
            }
        }

        private AppointmentInfo GetAppointmentDetails(string appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT a.Appointment_ID, a.Appointment_Date, 
                               s.Staff_Name, s.Staff_Surname, t.Timeslot
                        FROM Appointment a
                        INNER JOIN Staff s ON a.Staff_ID = s.Staff_ID
                        INNER JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                        WHERE a.Appointment_ID = @AppointmentId 
                        AND a.Cust_ID = @CustomerId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@CustomerId", Session["Cust_ID"]);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new AppointmentInfo
                                {
                                    AppointmentId = Convert.ToInt32(reader["Appointment_ID"]),
                                    AppointmentDate = Convert.ToDateTime(reader["Appointment_Date"]),
                                    DoctorName = $"{reader["Staff_Name"]} {reader["Staff_Surname"]}",
                                    Notes = $"Time: {reader["Timeslot"]}"
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting appointment details: {ex.Message}");
            }
            
            return null;
        }

        protected string GetPaymentStatusDisplay(object hasPayment, object paymentStatus, object totalPayable)
        {
            try
            {
                bool hasPaymentValue = Convert.ToBoolean(hasPayment);
                
                if (!hasPaymentValue)
                {
                    return "Not Paid";
                }
                
                // Get payment status from database
                string status = paymentStatus?.ToString() ?? "Paid";
                
                // Get total payable amount
                decimal totalAmount = 0;
                if (totalPayable != null && totalPayable != DBNull.Value)
                {
                    totalAmount = Convert.ToDecimal(totalPayable);
                }
                
                // Format the display
                if (totalAmount > 0)
                {
                    return $"{status} - R{totalAmount:F2}";
                }
                else
                {
                    return status;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error formatting payment status: {ex.Message}");
                return "Not Paid";
            }
        }

        protected void btnBookAppointment_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AppointmentStart.aspx");
        }

        protected void btnBookFirstAppointment_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AppointmentStart.aspx");
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

    // Helper class for appointment data
    public class AppointmentInfo
    {
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string ServiceType { get; set; }
        public int Duration { get; set; }
        public string DoctorName { get; set; }
        public string Status { get; set; }
        public string AppointmentType { get; set; } // Missed, Upcoming, Completed, Cancelled
        public string Notes { get; set; }
        public bool HasPayment { get; set; }
        public decimal ConsultationFee { get; set; }
        public decimal TotalPayable { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public decimal MedicalAidAmount { get; set; }
        public decimal PatientPortionAmount { get; set; }
        public bool IsUpcoming { get; set; }
    }
}
