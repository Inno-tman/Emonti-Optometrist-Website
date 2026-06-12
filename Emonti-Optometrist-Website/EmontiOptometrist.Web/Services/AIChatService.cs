using System.Text;
using System.Text.Json;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Services
{
    public class AIChatService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly IHttpClientFactory _httpClientFactory;

        public AIChatService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiKey = configuration["GroqApiKey"] ?? "";
            _model = configuration["GroqModel"] ?? "llama-3.3-70b-versatile";
            _httpClientFactory = httpClientFactory;
        }

        public bool IsConfigured => !string.IsNullOrEmpty(_apiKey);

        public async Task<string?> GetAIResponse(string userMessage, List<FAQItem> faqContext, string? conversationHistory = null)
        {
            if (!IsConfigured)
                return null;

            try
            {
                string systemPrompt = BuildSystemPrompt(faqContext);
                var messages = BuildMessages(systemPrompt, userMessage, conversationHistory);

                var requestBody = new
                {
                    model = _model,
                    messages = messages,
                    max_tokens = 500,
                    temperature = 0.3
                };

                string jsonRequest = JsonSerializer.Serialize(requestBody);
                string jsonResponse = await CallGroqAsync(jsonRequest);

                return ExtractResponse(jsonResponse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI Chat Error: {ex.Message}");
                return null;
            }
        }

        public string? GetAIResponseSync(string userMessage, List<FAQItem> faqContext, string? conversationHistory = null)
        {
            return GetAIResponse(userMessage, faqContext, conversationHistory).GetAwaiter().GetResult();
        }

        private static string BuildSystemPrompt(List<FAQItem> faqContext)
        {
            var sb = new StringBuilder();
            sb.AppendLine("You are a friendly and helpful AI assistant for Emonti Optometrist, an eye care practice in East London, South Africa.");
            sb.AppendLine();
            sb.AppendLine("Business Information:");
            sb.AppendLine("- Name: Emonti Optometrist");
            sb.AppendLine("- Address: Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247, South Africa");
            sb.AppendLine("- Phone: 076 463 1930");
            sb.AppendLine("- Email: emontioptom@gmail.com");
            sb.AppendLine("- Hours: Mon-Fri 8:00 AM - 5:00 PM, Sat 8:00 AM - 2:00 PM, Sun Closed");
            sb.AppendLine();
            sb.AppendLine("Your role is to answer questions based on the FAQ knowledge base below. Be conversational, concise, and accurate.");
            sb.AppendLine("If asked about pricing, direct them to call the practice or visit the website for current pricing.");
            sb.AppendLine("If you don't know the answer, suggest they call 076 463 1930 or email emontioptom@gmail.com.");
            sb.AppendLine();
            sb.AppendLine("FAQ Knowledge Base:");

            foreach (var faq in faqContext)
            {
                sb.AppendLine($"Q: {faq.Question}");
                sb.AppendLine($"A: {faq.Answer}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static List<Dictionary<string, string>> BuildMessages(string systemPrompt, string userMessage, string? conversationHistory)
        {
            var messages = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string> { { "role", "system" }, { "content", systemPrompt } }
            };

            if (!string.IsNullOrEmpty(conversationHistory))
            {
                messages.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", conversationHistory } });
            }

            messages.Add(new Dictionary<string, string> { { "role", "user" }, { "content", userMessage } });

            return messages;
        }

        private async Task<string> CallGroqAsync(string jsonRequest)
        {
            var url = "https://api.groq.com/openai/v1/chat/completions";
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static string? ExtractResponse(string jsonResponse)
        {
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            if (root.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
            {
                var firstChoice = choices[0];
                if (firstChoice.TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var content))
                {
                    return content.GetString()?.Trim();
                }
            }

            return null;
        }
    }
}
