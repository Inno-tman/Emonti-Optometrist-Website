using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace Emonti_Optometrist_Website.Models
{
    /// <summary>
    /// FAQ Item model for chatbot functionality
    /// </summary>
    public class FAQItem
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Question { get; set; }
        
        [Required]
        public string Answer { get; set; }
        
        [StringLength(500)]
        public string Keywords { get; set; }
        
        [StringLength(100)]
        public string Category { get; set; }
        
        public int Priority { get; set; } = 2; // 1 = High, 2 = Medium, 3 = Low
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Get keywords as array for easier processing
        /// </summary>
        public string[] GetKeywordsArray()
        {
            if (string.IsNullOrEmpty(Keywords))
                return new string[0];
                
            return Keywords.Split(',')
                          .Select(k => k.Trim().ToLower())
                          .Where(k => !string.IsNullOrEmpty(k))
                          .ToArray();
        }
        
        /// <summary>
        /// Set keywords from array
        /// </summary>
        public void SetKeywordsArray(string[] keywords)
        {
            Keywords = keywords != null ? string.Join(", ", keywords) : string.Empty;
        }
    }

    /// <summary>
    /// Chat conversation log model
    /// </summary>
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

    /// <summary>
    /// User feedback model for chatbot improvement
    /// </summary>
    public class ChatbotFeedback
    {
        public int Id { get; set; }
        
        public int ConversationId { get; set; }
        
        public int? UserRating { get; set; } // 1-5 stars
        
        public bool? WasHelpful { get; set; }
        
        [StringLength(500)]
        public string Comments { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Chatbot configuration model
    /// </summary>
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

    /// <summary>
    /// Database operations for FAQ management
    /// </summary>
    public class FAQDatabase
    {
        private readonly string _connectionString;

        public FAQDatabase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ProductConnection"]?.ConnectionString 
                              ?? ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
        }

        /// <summary>
        /// Get all active FAQ items
        /// </summary>
        public List<FAQItem> GetActiveFAQs()
        {
            var faqs = new List<FAQItem>();
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                // Return default FAQ data if no database connection
                return GetDefaultFAQs();
            }

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        SELECT Id, Question, Answer, Keywords, Category, Priority, IsActive, CreatedDate, UpdatedDate
                        FROM FAQ_Items 
                        WHERE IsActive = 1 
                        ORDER BY Priority ASC, Question";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                faqs.Add(new FAQItem
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    Question = reader["Question"] != DBNull.Value ? reader["Question"].ToString() : string.Empty,
                                    Answer = reader["Answer"] != DBNull.Value ? reader["Answer"].ToString() : string.Empty,
                                    Keywords = reader["Keywords"] != DBNull.Value ? reader["Keywords"].ToString() : string.Empty,
                                    Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : string.Empty,
                                    Priority = reader["Priority"] != DBNull.Value ? Convert.ToInt32(reader["Priority"]) : 2,
                                    IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : true,
                                    CreatedDate = reader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["CreatedDate"]) : DateTime.Now,
                                    UpdatedDate = reader["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["UpdatedDate"]) : DateTime.Now
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error and return default data
                System.Diagnostics.Debug.WriteLine($"FAQ Database Error: {ex.Message}");
                return GetDefaultFAQs();
            }

            return faqs.Any() ? faqs : GetDefaultFAQs();
        }

        /// <summary>
        /// Find best matching FAQ for a user message
        /// </summary>
        public FAQItem FindBestMatch(string userMessage)
        {
            var faqs = GetActiveFAQs();
            var lowerMessage = userMessage.ToLower();
            FAQItem bestMatch = null;
            int maxScore = 0;

            foreach (var faq in faqs)
            {
                int score = 0;
                var keywords = faq.GetKeywordsArray();

                // Check keyword matches
                foreach (var keyword in keywords)
                {
                    if (lowerMessage.Contains(keyword))
                    {
                        score++;
                    }
                }

                // Boost score for exact question match
                if (lowerMessage.Contains(faq.Question.ToLower()))
                {
                    score += 2;
                }

                // Apply priority multiplier
                score *= (4 - faq.Priority);

                if (score > maxScore)
                {
                    maxScore = score;
                    bestMatch = faq;
                }
            }

            return maxScore > 0 ? bestMatch : null;
        }

        /// <summary>
        /// Log conversation
        /// </summary>
        public bool LogConversation(string sessionId, string userMessage, string botResponse, float? confidence = null, int responseTimeMs = 0)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return false;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        INSERT INTO Chat_Conversations (SessionId, UserMessage, BotResponse, ConfidenceScore, ResponseTimeMs, CreatedDate)
                        VALUES (@SessionId, @UserMessage, @BotResponse, @ConfidenceScore, @ResponseTimeMs, @CreatedDate)";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SessionId", sessionId);
                        command.Parameters.AddWithValue("@UserMessage", userMessage);
                        command.Parameters.AddWithValue("@BotResponse", botResponse);
                        command.Parameters.AddWithValue("@ConfidenceScore", confidence.HasValue ? (object)confidence.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@ResponseTimeMs", responseTimeMs);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Log Conversation Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Submit user feedback
        /// </summary>
        public bool SubmitFeedback(int conversationId, int? rating, bool? wasHelpful, string comments = null)
        {
            if (string.IsNullOrEmpty(_connectionString))
                return false;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var query = @"
                        INSERT INTO Chatbot_Feedback (ConversationId, UserRating, WasHelpful, Comments, CreatedDate)
                        VALUES (@ConversationId, @UserRating, @WasHelpful, @Comments, @CreatedDate)";
                    
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ConversationId", conversationId);
                        command.Parameters.AddWithValue("@UserRating", rating.HasValue ? (object)rating.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@WasHelpful", wasHelpful.HasValue ? (object)wasHelpful.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@Comments", string.IsNullOrEmpty(comments) ? DBNull.Value : (object)comments);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        
                        connection.Open();
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Submit Feedback Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get default FAQ data when database is not available
        /// </summary>
        private List<FAQItem> GetDefaultFAQs()
        {
            return new List<FAQItem>
            {
                new FAQItem
                {
                    Id = 1,
                    Question = "How do I book an appointment?",
                    Answer = "You can book an appointment by visiting our \"Book Appointment\" page. Simply select your preferred service, date, and time slot. You'll need to be logged in to your account to complete the booking.",
                    Keywords = "appointment, book, schedule, visit, booking, reserve",
                    Category = "appointments",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 2,
                    Question = "What payment methods do you accept?",
                    Answer = "We accept all major credit cards (Visa, MasterCard, American Express), debit cards, and cash payments. We also work with most medical aid schemes for covered services.",
                    Keywords = "payment, pay, credit card, cash, medical aid, money, cost, price",
                    Category = "payments",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 3,
                    Question = "How long does an eye test take?",
                    Answer = "A comprehensive eye test typically takes between 30-45 minutes. This includes a thorough examination of your eye health, vision assessment, and consultation with our optometrist.",
                    Keywords = "eye test, examination, how long, duration, test time, exam",
                    Category = "services",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 4,
                    Question = "Do you offer home visits?",
                    Answer = "Yes, we offer home visits for elderly patients or those with mobility issues. Please contact us to arrange a home visit appointment.",
                    Keywords = "home visit, mobile, elderly, mobility, house call, visit home",
                    Category = "services",
                    Priority = 2
                },
                new FAQItem
                {
                    Id = 5,
                    Question = "What is your cancellation policy?",
                    Answer = "We require 24 hours notice for appointment cancellations. Late cancellations may incur a fee. You can cancel or reschedule appointments through your profile page or by contacting us directly.",
                    Keywords = "cancel, cancellation, reschedule, policy, change appointment, postpone",
                    Category = "policies",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 6,
                    Question = "How do I know if I need new glasses?",
                    Answer = "Common signs include frequent headaches, eye strain, difficulty reading, or blurry vision. We recommend annual eye tests to monitor your vision health and determine if prescription changes are needed.",
                    Keywords = "glasses, prescription, new glasses, vision, eyewear, spectacles",
                    Category = "services",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 7,
                    Question = "How can I contact you?",
                    Answer = "You can reach us at 076 463 1930 or email emontioptom@gmail.com. We're located at Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247.",
                    Keywords = "contact, phone, email, reach, get in touch, call",
                    Category = "contact",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 8,
                    Question = "What are your opening hours?",
                    Answer = "Our opening hours are: Monday - Friday: 8:00 AM - 5:00 PM, Saturday: 8:00 AM - 2:00 PM, Sunday: Closed.",
                    Keywords = "hours, opening, time, when open, business hours, schedule",
                    Category = "contact",
                    Priority = 1
                },
                new FAQItem
                {
                    Id = 9,
                    Question = "What is your warranty policy?",
                    Answer = "We offer 1-year manufacturer warranty on frames, 1-year scratch warranty on lenses, and free adjustments for 6 months. Normal wear and tear is excluded from coverage.",
                    Keywords = "warranty, guarantee, repair, broken, damaged, fix",
                    Category = "policies",
                    Priority = 2
                },
                new FAQItem
                {
                    Id = 10,
                    Question = "What is your return policy?",
                    Answer = "We offer a 30-day return policy for unused frames, 14-day return policy for accessories. Custom lenses cannot be returned. Items must be in original condition.",
                    Keywords = "return, exchange, refund, bring back, take back",
                    Category = "policies",
                    Priority = 2
                }
            };
        }
    }
}
