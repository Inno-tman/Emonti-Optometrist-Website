using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Models;
using System.Net.Mail;

namespace EmontiOptometrist.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string ForgotPasswordError { get; set; }
        public string ForgotPasswordSuccess { get; set; }
        public string ResetPasswordError { get; set; }
        public string ResetPasswordSuccess { get; set; }
        public string ResetEmail { get; set; }
        public bool ShowForgotPasswordModal { get; set; }
        public bool ShowResetPasswordModal { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            returnUrl ??= Url.Content("~/");
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                if (result.IsLockedOut)
                    return RedirectToPage("./Lockout");
                else
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSendResetCode(string forgotPasswordEmail)
        {
            if (string.IsNullOrEmpty(forgotPasswordEmail))
            {
                ForgotPasswordError = "Please enter your email address.";
                ShowForgotPasswordModal = true;
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordEmail);
            if (user == null)
            {
                ForgotPasswordSuccess = "If an account with that email exists, a password reset code has been sent. Please check your email.";
                ShowForgotPasswordModal = true;
                return Page();
            }

            try
            {
                string resetCode = GenerateResetCode();
                HttpContext.Session.SetString("PasswordResetCode", resetCode);
                HttpContext.Session.SetString("PasswordResetEmail", forgotPasswordEmail);
                HttpContext.Session.SetString("PasswordResetExpiry", DateTime.Now.AddMinutes(15).ToString("O"));

                string customerName = user.FullName ?? "Valued Customer";
                await SendResetEmail(forgotPasswordEmail, customerName, resetCode);

                ResetEmail = forgotPasswordEmail;
                ForgotPasswordSuccess = $"A password reset code has been sent to {forgotPasswordEmail}. Please check your email and enter the code below.";
                ShowResetPasswordModal = true;
                ShowForgotPasswordModal = false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending reset code to {Email}", forgotPasswordEmail);
                ForgotPasswordError = "An error occurred while sending the reset code. Please try again later.";
                ShowForgotPasswordModal = true;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResetPassword(string resetEmail, string resetCode, string newPassword, string confirmNewPassword)
        {
            if (string.IsNullOrEmpty(resetEmail) || string.IsNullOrEmpty(resetCode) ||
                string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmNewPassword))
            {
                ResetPasswordError = "Please fill in all fields.";
                ShowResetPasswordModal = true;
                return Page();
            }

            if (newPassword != confirmNewPassword)
            {
                ResetPasswordError = "Passwords do not match.";
                ShowResetPasswordModal = true;
                return Page();
            }

            string storedCode = HttpContext.Session.GetString("PasswordResetCode");
            string storedEmail = HttpContext.Session.GetString("PasswordResetEmail");
            string expiryStr = HttpContext.Session.GetString("PasswordResetExpiry");

            if (string.IsNullOrEmpty(storedCode) || storedEmail != resetEmail ||
                string.IsNullOrEmpty(expiryStr) || DateTime.Parse(expiryStr) < DateTime.Now)
            {
                ResetPasswordError = "Invalid or expired reset code. Please request a new code.";
                ShowResetPasswordModal = true;
                return Page();
            }

            if (storedCode != resetCode)
            {
                ResetPasswordError = "Invalid reset code. Please check and try again.";
                ShowResetPasswordModal = true;
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(resetEmail);
            if (user == null)
            {
                ResetPasswordError = "User not found.";
                ShowResetPasswordModal = true;
                return Page();
            }

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                ResetPasswordError = "Failed to reset password. Please try again.";
                ShowResetPasswordModal = true;
                return Page();
            }

            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (!addResult.Succeeded)
            {
                ResetPasswordError = "Failed to reset password. Please try again.";
                ShowResetPasswordModal = true;
                return Page();
            }

            HttpContext.Session.Remove("PasswordResetCode");
            HttpContext.Session.Remove("PasswordResetEmail");
            HttpContext.Session.Remove("PasswordResetExpiry");

            ResetPasswordSuccess = "Your password has been reset successfully. You can now login with your new password.";
            SuccessMessage = "Your password has been reset successfully. You can now login with your new password.";
            ShowResetPasswordModal = false;
            return Page();
        }

        private string GenerateResetCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task SendResetEmail(string email, string customerName, string resetCode)
        {
            string smtpHost = _configuration["SmtpHost"] ?? "smtp.gmail.com";
            int smtpPort = int.Parse(_configuration["SmtpPort"] ?? "587");
            string smtpEmail = _configuration["SmtpEmail"];
            string smtpPassword = _configuration["SmtpPassword"];
            string smtpFromName = _configuration["SmtpFromName"] ?? "Emonti Optometrist";
            bool enableSsl = bool.Parse(_configuration["SmtpEnableSsl"] ?? "true");

            if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP credentials not configured. Reset code for {Email}: {Code}", email, resetCode);
                return;
            }

            string body = $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#667eea;padding:25px;text-align:center;"">
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Password Reset</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {System.Net.WebUtility.HtmlEncode(customerName)},</p>
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

            using var smtp = new SmtpClient(smtpHost, smtpPort);
            smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
            smtp.EnableSsl = enableSsl;
            smtp.Timeout = 30000;

            using var message = new MailMessage();
            message.From = new MailAddress(smtpEmail, smtpFromName);
            message.To.Add(email);
            message.Subject = "Password Reset Code - Emonti Optometrist";
            message.Body = body;
            message.IsBodyHtml = true;

            await smtp.SendMailAsync(message);
        }
    }
}
