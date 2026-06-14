using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; }

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
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        string connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";

        using var conn = new SqliteConnection(connStr);
        conn.Open();

        // Try customer login
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Cust_ID, Customer_Name, Customer_Surname, Customer_Email
            FROM customer
            WHERE Customer_Email = @Email AND Customer_Password = @Password
              AND (Is_Archive IS NULL OR Is_Archive = 0)";
        cmd.Parameters.AddWithValue("@Email", Input.Email.Trim());
        cmd.Parameters.AddWithValue("@Password", Input.Password.Trim());

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            var custId = reader["Cust_ID"].ToString();
            var firstName = reader["Customer_Name"]?.ToString() ?? "";
            var lastName = reader["Customer_Surname"]?.ToString() ?? "";
            var email = reader["Customer_Email"]?.ToString() ?? "";

            AuthSession.SignInCustomer(HttpContext, custId, email, firstName, lastName);
            _logger.LogInformation("Customer {Email} logged in.", email);

            reader.Close();

            using var updateCmd = conn.CreateCommand();
            updateCmd.CommandText = "UPDATE customer SET Last_Login = datetime('now', 'localtime') WHERE Cust_ID = @CustId";
            updateCmd.Parameters.AddWithValue("@CustId", custId);
            updateCmd.ExecuteNonQuery();

            return RedirectToPage("/Index");
        }

        reader.Close();

        // Try staff login
        cmd.Parameters.Clear();
        cmd.CommandText = @"
            SELECT Staff_ID, Staff_Name, Staff_Email, Staff_Role
            FROM Staff
            WHERE Staff_Email = @Email AND Staff_Password = @Password";
        cmd.Parameters.AddWithValue("@Email", Input.Email.Trim());
        cmd.Parameters.AddWithValue("@Password", Input.Password);

        using var staffReader = cmd.ExecuteReader();
        if (staffReader.Read())
        {
            var staffId = staffReader["Staff_ID"].ToString();
            var staffName = staffReader["Staff_Name"]?.ToString() ?? "";
            var role = staffReader["Staff_Role"]?.ToString() ?? "Staff";

            AuthSession.SignInStaff(HttpContext, staffId, staffName, role);
            _logger.LogInformation("Staff {Email} logged in as {Role}.", Input.Email, role);

            if (role == "Admin")
                return RedirectToPage("/Admin/Dashboard");
            return RedirectToPage("/Staff/Dashboard");
        }

        ErrorMessage = "Invalid login attempt.";
        return Page();
    }

    public IActionResult OnPostSendResetCode(string forgotPasswordEmail)
    {
        if (string.IsNullOrEmpty(forgotPasswordEmail))
        {
            ForgotPasswordError = "Please enter your email address.";
            ShowForgotPasswordModal = true;
            return Page();
        }

        string connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
        using var conn = new SqliteConnection(connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Cust_ID FROM customer WHERE Customer_Email = @Email AND (Is_Archive IS NULL OR Is_Archive = 0)";
        cmd.Parameters.AddWithValue("@Email", forgotPasswordEmail);
        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
        {
            ForgotPasswordSuccess = "If an account with that email exists, a password reset code has been sent. Please check your email.";
            ShowForgotPasswordModal = true;
            return Page();
        }

        string resetCode = new Random().Next(100000, 999999).ToString();
        HttpContext.Session.SetString("PasswordResetCode", resetCode);
        HttpContext.Session.SetString("PasswordResetEmail", forgotPasswordEmail);
        HttpContext.Session.SetString("PasswordResetExpiry", DateTime.Now.AddMinutes(15).ToString("O"));

        ResetEmail = forgotPasswordEmail;
        ForgotPasswordSuccess = $"A password reset code has been sent to {forgotPasswordEmail}.";
        ShowResetPasswordModal = true;
        ShowForgotPasswordModal = false;

        string smtpEmail = _configuration["SmtpEmail"];
        string smtpPassword = _configuration["SmtpPassword"];
        if (!string.IsNullOrEmpty(smtpEmail) && !string.IsNullOrEmpty(smtpPassword))
        {
            try
            {
                using var smtp = new System.Net.Mail.SmtpClient(
                    _configuration["SmtpHost"] ?? "smtp.gmail.com",
                    int.Parse(_configuration["SmtpPort"] ?? "587"));
                smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                smtp.EnableSsl = bool.Parse(_configuration["SmtpEnableSsl"] ?? "true");
                using var msg = new System.Net.Mail.MailMessage(smtpEmail, forgotPasswordEmail,
                    "Password Reset Code - Emonti Optometrist",
                    $"Your reset code is: {resetCode}");
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send reset email");
            }
        }

        return Page();
    }

    public IActionResult OnPostResetPassword(string resetEmail, string resetCode, string newPassword, string confirmNewPassword)
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

        string connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
        using var conn = new SqliteConnection(connStr);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "UPDATE customer SET Customer_Password = @Password WHERE Customer_Email = @Email";
        cmd.Parameters.AddWithValue("@Password", newPassword);
        cmd.Parameters.AddWithValue("@Email", resetEmail);
        cmd.ExecuteNonQuery();

        HttpContext.Session.Remove("PasswordResetCode");
        HttpContext.Session.Remove("PasswordResetEmail");
        HttpContext.Session.Remove("PasswordResetExpiry");

        ResetPasswordSuccess = "Your password has been reset successfully.";
        SuccessMessage = "Your password has been reset successfully. You can now login.";
        ShowResetPasswordModal = false;
        return Page();
    }
}
