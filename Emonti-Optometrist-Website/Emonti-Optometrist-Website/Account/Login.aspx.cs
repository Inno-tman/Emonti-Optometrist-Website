using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Emonti_Optometrist_Website.Models;
using System.Net.Mail;
using System.Text;
using System.Security.Cryptography;

namespace Emonti_Optometrist_Website.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterHyperLink.NavigateUrl = "Register";
                // Enable this once you have account confirmation enabled for password reset functionality
                //ForgotPasswordHyperLink.NavigateUrl = "Forgot";

                var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
                }

               
            }
        }

        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes = 5;

        protected void LogIn(object sender, EventArgs e)
        {
            string email = Email.Text.Trim();
            var failedKey = $"FailedAttempts_{email.ToLower()}";
            var lockoutKey = $"LockoutEnd_{email.ToLower()}";

            var lockoutEnd = Session[lockoutKey] as DateTime?;
            if (lockoutEnd.HasValue && lockoutEnd.Value > DateTime.Now)
            {
                FailureText.Text = $"Account is locked. Try again after {lockoutEnd.Value:HH:mm}.";
                ErrorMessage.Visible = true;
                return;
            }
            if (lockoutEnd.HasValue)
            {
                Session.Remove(failedKey);
                Session.Remove(lockoutKey);
            }

            string connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["ProductConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Include Customer_Name and Customer_Surname so we can greet the user after login
                string query = "SELECT Cust_ID, Customer_Email, Customer_Name, Customer_Surname FROM customer WHERE Customer_Email = @Email AND Customer_Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", Email.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", Password.Text.Trim());

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Clear failed attempts on successful login
                        Session.Remove(failedKey);
                        Session.Remove(lockoutKey);

                        // Store customer data in session
                        string custId = reader["Cust_ID"].ToString();
                        Session["Cust_ID"] = custId;
                        Session["UserEmail"] = reader["Customer_Email"];

                        // Store first and last name for greeting
                        Session["FirstName"] = reader["Customer_Name"]?.ToString() ?? "";
                        Session["LastName"] = reader["Customer_Surname"]?.ToString() ?? "";

                        Session["IsLoggedIn"] = true;

                        // Merge session cart with user's database cart
                        try
                        {
                            CartDatabase.MergeSessionCartWithUserCart(Session.SessionID, custId);
                        }
                        catch (Exception ex)
                        {
                            // Log error but don't prevent login
                            System.Diagnostics.Debug.WriteLine($"Error merging cart on login: {ex.Message}");
                        }

                        // Redirect
                        string returnUrl = Request.QueryString["ReturnUrl"];
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            Response.Redirect(HttpUtility.UrlDecode(returnUrl));
                        }
                        else
                        {
                            Response.Redirect("~/Default.aspx");
                        }
                    }
                    else
                    {
                        // No customer match - check staff table
                        reader.Close();

                        try
                        {
                            string staffQuery = "SELECT Staff_ID, Staff_Name, Staff_Role, Staff_Password, Staff_Email FROM Staff WHERE Staff_Email = @Email AND Staff_Password = @Password";
                            using (SqlCommand staffCmd = new SqlCommand(staffQuery, conn))
                            {
                                staffCmd.Parameters.AddWithValue("@Email", Email.Text.Trim());
                                staffCmd.Parameters.AddWithValue("@Password", Password.Text.Trim());
 
                                using (SqlDataReader staffReader = staffCmd.ExecuteReader())
                                {
                                    // existing staff login handling (unchanged)
                                    if (staffReader.Read())
                                    {
                                        // Clear failed attempts on successful staff login
                                        Session.Remove(failedKey);
                                        Session.Remove(lockoutKey);

                                        // handle staff login
                                        Session["IsStaffLoggedIn"] = true;
                                        Session["Staff_ID"] = staffReader["Staff_ID"].ToString();
                                        Session["StaffName"] = staffReader["Staff_Name"]?.ToString() ?? "";

                                        Response.Redirect("~/Staff/Dashboard.aspx");
                                    }
                                    else
                                    {
                                        // Track failed attempts
                                        var attempts = Session[failedKey] as int? ?? 0;
                                        attempts++;
                                        Session[failedKey] = attempts;

                                        if (attempts >= MaxFailedAttempts)
                                        {
                                            Session[lockoutKey] = DateTime.Now.AddMinutes(LockoutMinutes);
                                            FailureText.Text = $"Too many failed attempts. Account locked for {LockoutMinutes} minutes.";
                                        }
                                        else
                                        {
                                            FailureText.Text = $"Invalid login attempt. {MaxFailedAttempts - attempts} attempt(s) remaining.";
                                        }
                                        ErrorMessage.Visible = true;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            FailureText.Text = "An error occurred during login.";
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                            ErrorMessage.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnSendResetCode_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide previous messages
                ForgotPasswordErrorMessage.Visible = false;
                ForgotPasswordSuccessMessage.Visible = false;

                string email = txtForgotPasswordEmail.Text.Trim();

                if (string.IsNullOrEmpty(email))
                {
                    ForgotPasswordFailureText.Text = "Please enter your email address.";
                    ForgotPasswordErrorMessage.Visible = true;
                    updForgotPassword.Update();
                    return;
                }

                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                // Check if email exists in database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Cust_ID, Customer_Email, Customer_Name FROM customer WHERE Customer_Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Generate reset code
                                string resetCode = GenerateResetCode();
                                string customerName = reader["Customer_Name"]?.ToString() ?? "Valued Customer";

                                // Store reset code in session with expiration (15 minutes)
                                Session["PasswordResetCode"] = resetCode;
                                Session["PasswordResetEmail"] = email;
                                Session["PasswordResetExpiry"] = DateTime.Now.AddMinutes(15);

                                // Send reset code email
                                SendPasswordResetEmail(email, customerName, resetCode);

                                // Show success message
                                ForgotPasswordSuccessText.Text = $"A password reset code has been sent to {email}. Please check your email and enter the code below.";
                                ForgotPasswordSuccessMessage.Visible = true;
                                
                                // Pre-fill email in reset password modal and show it
                                txtResetPasswordEmail.Text = email;
                                ScriptManager.RegisterStartupScript(this, GetType(), "openResetModal", 
                                    "setTimeout(function() { openResetPasswordModal(); }, 500);", true);
                            }
                            else
                            {
                                // Don't reveal if email exists or not for security
                                ForgotPasswordSuccessText.Text = "If an account with that email exists, a password reset code has been sent. Please check your email.";
                                ForgotPasswordSuccessMessage.Visible = true;
                            }
                        }
                    }
                }

                updForgotPassword.Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in btnSendResetCode_Click: {ex.Message}");
                ForgotPasswordFailureText.Text = "An error occurred. Please try again later.";
                ForgotPasswordErrorMessage.Visible = true;
                updForgotPassword.Update();
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide previous messages
                ResetPasswordErrorMessage.Visible = false;
                ResetPasswordSuccessMessage.Visible = false;

                string email = txtResetPasswordEmail.Text.Trim();
                string resetCode = txtResetCode.Text.Trim();
                string newPassword = txtNewPassword.Text.Trim();
                string confirmPassword = txtConfirmNewPassword.Text.Trim();

                // Validate inputs
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(resetCode) || 
                    string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
                {
                    ResetPasswordFailureText.Text = "Please fill in all fields.";
                    ResetPasswordErrorMessage.Visible = true;
                    updResetPassword.Update();
                    return;
                }

                if (newPassword != confirmPassword)
                {
                    ResetPasswordFailureText.Text = "Passwords do not match.";
                    ResetPasswordErrorMessage.Visible = true;
                    updResetPassword.Update();
                    return;
                }

                // Verify reset code
                string storedCode = Session["PasswordResetCode"]?.ToString();
                string storedEmail = Session["PasswordResetEmail"]?.ToString();
                DateTime? expiry = Session["PasswordResetExpiry"] as DateTime?;

                if (string.IsNullOrEmpty(storedCode) || storedEmail != email || 
                    !expiry.HasValue || DateTime.Now > expiry.Value)
                {
                    ResetPasswordFailureText.Text = "Invalid or expired reset code. Please request a new code.";
                    ResetPasswordErrorMessage.Visible = true;
                    updResetPassword.Update();
                    return;
                }

                if (storedCode != resetCode)
                {
                    ResetPasswordFailureText.Text = "Invalid reset code. Please check and try again.";
                    ResetPasswordErrorMessage.Visible = true;
                    updResetPassword.Update();
                    return;
                }

                // Update password in database
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE customer SET Customer_Password = @Password WHERE Customer_Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Clear reset code from session
                            Session.Remove("PasswordResetCode");
                            Session.Remove("PasswordResetEmail");
                            Session.Remove("PasswordResetExpiry");

                            ResetPasswordSuccessText.Text = "Your password has been reset successfully. You can now login with your new password.";
                            ResetPasswordSuccessMessage.Visible = true;

                            // Clear form
                            txtResetPasswordEmail.Text = "";
                            txtResetCode.Text = "";
                            txtNewPassword.Text = "";
                            txtConfirmNewPassword.Text = "";

                            // Close modal after 3 seconds and redirect to login
                            ScriptManager.RegisterStartupScript(this, GetType(), "closeResetModal", 
                                "setTimeout(function() { closeResetPasswordModal(); }, 3000);", true);
                        }
                        else
                        {
                            ResetPasswordFailureText.Text = "Failed to reset password. Please try again.";
                            ResetPasswordErrorMessage.Visible = true;
                        }
                    }
                }

                updResetPassword.Update();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in btnResetPassword_Click: {ex.Message}");
                ResetPasswordFailureText.Text = "An error occurred. Please try again later.";
                ResetPasswordErrorMessage.Visible = true;
                updResetPassword.Update();
            }
        }

        private string GenerateResetCode()
        {
            // Generate a 6-digit random code
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void SendPasswordResetEmail(string email, string customerName, string resetCode)
        {
            try
            {
                // Get SMTP configuration from web.config
                string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpEmail = System.Configuration.ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
                string smtpFromName = System.Configuration.ConfigurationManager.AppSettings["SmtpFromName"] ?? "Emonti Optometrist";
                bool enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");

                // Validate SMTP credentials are configured
                if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: SMTP credentials not configured in web.config");
                    throw new Exception("Email service not configured");
                }

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
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Password Reset</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {System.Web.HttpUtility.HtmlEncode(customerName)},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;line-height:1.6;"">We received a request to reset your password. Use the code below to complete your password reset.</p>
<div style=""background:linear-gradient(135deg,#f8f9fa 0%,#e9ecef 100%);border:2px dashed #667eea;padding:30px;margin:25px 0;text-align:center;border-radius:8px;"">
<p style=""margin:0 0 10px 0;color:#666;font-size:13px;font-weight:600;text-transform:uppercase;letter-spacing:1px;"">Your Reset Code</p>
<p style=""margin:0;color:#667eea;font-size:36px;font-weight:700;letter-spacing:8px;font-family:'Courier New',monospace;"">{resetCode}</p>
</div>
<div style=""background-color:#fff3cd;border-left:4px solid #ffc107;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0 0 8px 0;color:#856404;font-size:14px;font-weight:600;"">Important:</p>
<ul style=""margin:0;padding-left:20px;color:#856404;font-size:13px;line-height:1.8;"">
<li>This code expires in <strong>15 minutes</strong></li>
<li>Do not share this code with anyone</li>
<li>If you didn't request this, please ignore this email</li>
</ul>
</div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;line-height:1.6;"">Questions? Contact us at <a href=""mailto:emontioptom@gmail.com"" style=""color:#667eea;text-decoration:none;"">emontioptom@gmail.com</a> or <a href=""tel:0764631930"" style=""color:#667eea;text-decoration:none;"">076 463 1930</a>.</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                    smtp.EnableSsl = enableSsl;
                    smtp.Timeout = 30000; // 30 seconds timeout

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpEmail, smtpFromName);
                        message.To.Add(email);
                        message.Subject = "Password Reset Code - Emonti Optometrist";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        smtp.Send(message);
                        System.Diagnostics.Debug.WriteLine($"Password reset email sent to: {email}");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                System.Diagnostics.Debug.WriteLine($"SMTP Error sending password reset email: {smtpEx.Message}");
                System.Diagnostics.Debug.WriteLine($"SMTP Status Code: {smtpEx.StatusCode}");
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending password reset email: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
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