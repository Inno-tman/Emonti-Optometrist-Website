using System;
using System.ComponentModel.DataAnnotations;

namespace EmontiOptometrist.Web.Models
{
    public class ChatConversation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string SessionId { get; set; }

        [Required]
        [StringLength(500)]
        public string UserMessage { get; set; }

        [Required]
        public string BotResponse { get; set; }

        public float? ConfidenceScore { get; set; }

        public int ResponseTimeMs { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsResolved { get; set; } = false;
    }

    public class ChatbotFeedback
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public int? UserRating { get; set; }

        public bool? WasHelpful { get; set; }

        [StringLength(500)]
        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    public class ChatbotConfig
    {
        public bool EnableTypingIndicator { get; set; } = true;

        public int TypingDelay { get; set; } = 1000;

        public int MaxMessageLength { get; set; } = 500;

        public bool EnableSuggestions { get; set; } = true;

        public bool EnableSound { get; set; } = false;

        public string ApiEndpoint { get; set; }

        public bool EnableAnalytics { get; set; } = true;

        public int MaxHistoryLength { get; set; } = 50;
    }
}
