using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace Emonti_Optometrist_Website.Models
{
    public class AIChatService
    {
        private static readonly string _apiKey;
        private readonly string _model;

        static AIChatService()
        {
            _apiKey = ConfigurationManager.AppSettings["GroqApiKey"] ?? "";
        }

        public AIChatService()
        {
            _model = ConfigurationManager.AppSettings["GroqModel"] ?? "llama-3.3-70b-versatile";
        }

        public bool IsConfigured => !string.IsNullOrEmpty(_apiKey);

        public string GetAIResponse(string userMessage, List<FAQItem> faqContext, string conversationHistory = null)
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

                var jsonSerializer = new JavaScriptSerializer();
                string jsonRequest = jsonSerializer.Serialize(requestBody);
                string jsonResponse = CallGroq(jsonRequest);

                return ExtractResponse(jsonResponse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"AI Chat Error: {ex.Message}");
                return null;
            }
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

        private static List<Dictionary<string, string>> BuildMessages(string systemPrompt, string userMessage, string conversationHistory)
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

        private static string CallGroq(string jsonRequest)
        {
            var url = "https://api.groq.com/openai/v1/chat/completions";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", $"Bearer {_apiKey}");

            byte[] data = Encoding.UTF8.GetBytes(jsonRequest);
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        private static string ExtractResponse(string jsonResponse)
        {
            var serializer = new JavaScriptSerializer();
            var root = serializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (root != null && root.ContainsKey("choices") && root["choices"] is ArrayList choices && choices.Count > 0)
            {
                var firstChoice = choices[0] as Dictionary<string, object>;
                if (firstChoice != null && firstChoice.ContainsKey("message") && firstChoice["message"] is Dictionary<string, object> message)
                {
                    if (message.ContainsKey("content") && message["content"] is string content)
                    {
                        return content.Trim();
                    }
                }
            }

            return null;
        }
    }
}
