<%@ WebHandler Language="C#" Class="ChatbotAPI" %>

using Emonti_Optometrist_Website.Models;
using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Linq;

public class ChatbotAPI : IHttpHandler
{
    private readonly FAQDatabase _faqDatabase;
    private readonly AIChatService _aiService;
    private readonly JavaScriptSerializer _jsonSerializer;

    public ChatbotAPI()
    {
        _faqDatabase = new FAQDatabase();
        _aiService = new AIChatService();
        _jsonSerializer = new JavaScriptSerializer();
    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";

        try
        {
            var method = context.Request.QueryString["method"] ?? context.Request.Form["method"];

            switch (method?.ToLower())
            {
                case "chat":
                    HandleChatRequest(context);
                    break;
                case "faqs":
                    HandleGetFAQs(context);
                    break;
                case "feedback":
                    HandleFeedback(context);
                    break;
                case "config":
                    HandleConfig(context);
                    break;
                default:
                    HandleChatRequest(context);
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.Write(_jsonSerializer.Serialize(new
            {
                success = false,
                message = "Internal server error",
                error = ex.Message
            }));
        }
    }

    private void HandleChatRequest(HttpContext context)
    {
        var request = GetRequestData(context);
        var userMessage = request["message"]?.ToString();

        if (string.IsNullOrEmpty(userMessage))
        {
            context.Response.Write(_jsonSerializer.Serialize(new { success = false, message = "Message is required" }));
            return;
        }

        var bestMatch = _faqDatabase.FindBestMatch(userMessage);
        context.Response.Write(_jsonSerializer.Serialize(new { success = true, message = bestMatch?.Answer ?? GetFallbackResponse(userMessage.ToLower()) }));
    }

    private void HandleGetFAQs(HttpContext context)
    {
        var faqs = _faqDatabase.GetActiveFAQs();
        var category = context.Request.QueryString["category"];

        if (!string.IsNullOrEmpty(category))
        {
            faqs = faqs.Where(f => f.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        context.Response.Write(_jsonSerializer.Serialize(new
        {
            success = true,
            faqs = faqs.Select(f => new
            {
                id = f.Id,
                question = f.Question,
                answer = f.Answer,
                category = f.Category,
                priority = f.Priority
            })
        }));
    }

    private void HandleFeedback(HttpContext context)
    {
        var request = GetRequestData(context);
        var conversationId = Convert.ToInt32(request["conversationId"]);
        var rating = request["rating"] != null ? Convert.ToInt32(request["rating"]) : (int?)null;
        var wasHelpful = request["wasHelpful"] != null ? Convert.ToBoolean(request["wasHelpful"]) : (bool?)null;
        var comments = request["comments"]?.ToString();

        var success = _faqDatabase.SubmitFeedback(conversationId, rating, wasHelpful, comments);

        context.Response.Write(_jsonSerializer.Serialize(new
        {
            success = success,
            message = success ? "Feedback submitted successfully" : "Failed to submit feedback"
        }));
    }

    private void HandleConfig(HttpContext context)
    {
        var config = new ChatbotConfig();

        context.Response.Write(_jsonSerializer.Serialize(new
        {
            success = true,
            config = new
            {
                enableTypingIndicator = config.EnableTypingIndicator,
                typingDelay = config.TypingDelay,
                maxMessageLength = config.MaxMessageLength,
                enableSuggestions = config.EnableSuggestions,
                enableSound = config.EnableSound,
                enableAnalytics = config.EnableAnalytics
            }
        }));
    }

    private Dictionary<string, object> GetRequestData(HttpContext context)
    {
        var data = new Dictionary<string, object>();

        foreach (string key in context.Request.Form.AllKeys)
        {
            data[key] = context.Request.Form[key];
        }

        foreach (string key in context.Request.QueryString.AllKeys)
        {
            if (key != null && !data.ContainsKey(key))
            {
                data[key] = context.Request.QueryString[key];
            }
        }

        return data;
    }

    private string GetFallbackResponse(string message)
    {
        var fallbacks = new Dictionary<string, string>
        {
            { "hello", "Hello! How can I assist you today?" },
            { "hi", "Hello! How can I assist you today?" },
            { "hey", "Hello! How can I assist you today?" },
            { "thank", "You're welcome! Is there anything else I can help you with?" },
            { "thanks", "You're welcome! Is there anything else I can help you with?" },
            { "bye", "Goodbye! Feel free to come back anytime if you have more questions." },
            { "goodbye", "Goodbye! Feel free to come back anytime if you have more questions." },
            { "help", "I can help you with questions about appointments, payments, services, policies, and contact information. What would you like to know?" }
        };

        foreach (var fallback in fallbacks)
        {
            if (message.Contains(fallback.Key))
            {
                return fallback.Value;
            }
        }

        return "I'm sorry, I couldn't find a specific answer to your question. Please contact us directly at 076 463 1930 or email emontioptom@gmail.com for assistance. You can also visit our Help page for more information.";
    }

    public bool IsReusable
    {
        get { return true; }
    }
}
