using System.Text;
using System.Text.Json;

namespace EmontiOptometrist.Web.Services;

public class BrevoEmailService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BrevoEmailService> _logger;

    public BrevoEmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<BrevoEmailService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string? toName, string subject, string htmlContent)
    {
        var apiKey = _configuration["Smtp:Password"] ?? "";
        var fromEmail = _configuration["Smtp:Email"] ?? "";
        var fromName = _configuration["Smtp:FromName"] ?? "Emonti Optometrist";

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail))
        {
            _logger.LogWarning("Brevo API key or sender email not configured, skipping email");
            return false;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("api-key", apiKey);

            var payload = new
            {
                sender = new { email = fromEmail, name = fromName },
                to = new[] { new { email = toEmail, name = toName ?? toEmail } },
                subject,
                htmlContent
            };

            var json = JsonSerializer.Serialize(payload);
            var response = await client.PostAsync("https://api.brevo.com/v3/smtp/email", new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Brevo email sent to {Email}: {Subject}", toEmail, subject);
                return true;
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Brevo API error {Status} sending to {Email}: {Body}", response.StatusCode, toEmail, errorBody);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Brevo API exception sending to {Email}", toEmail);
            return false;
        }
    }

    public async Task<Dictionary<string, object>> TestEmailAsync(string toEmail)
    {
        var result = new Dictionary<string, object>();
        var apiKey = _configuration["Smtp:Password"] ?? "";
        var fromEmail = _configuration["Smtp:Email"] ?? "";

        result["smtpEmail"] = fromEmail;
        result["apiKeyConfigured"] = !string.IsNullOrEmpty(apiKey);

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(fromEmail))
        {
            result["success"] = false;
            result["message"] = "Brevo API key or sender email not configured";
            return result;
        }

        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("api-key", apiKey);
            client.Timeout = TimeSpan.FromSeconds(30);

            var payload = new
            {
                sender = new { email = fromEmail, name = "Emonti Optometrist" },
                to = new[] { new { email = toEmail } },
                subject = $"Test Email from Brevo API - {DateTime.Now:HH:mm:ss}",
                htmlContent = "<h1>Test</h1><p>This is a test email sent via Brevo API.</p>"
            };

            var json = JsonSerializer.Serialize(payload);
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var response = await client.PostAsync("https://api.brevo.com/v3/smtp/email", new StringContent(json, Encoding.UTF8, "application/json"));
            sw.Stop();

            result["statusCode"] = (int)response.StatusCode;
            result["elapsedMs"] = sw.ElapsedMilliseconds;

            if (response.IsSuccessStatusCode)
            {
                result["success"] = true;
                result["message"] = "Email sent successfully via Brevo API";
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                result["success"] = false;
                result["message"] = $"Brevo API error: {errorBody}";
            }
        }
        catch (Exception ex)
        {
            result["success"] = false;
            result["message"] = ex.Message;
        }

        return result;
    }
}
