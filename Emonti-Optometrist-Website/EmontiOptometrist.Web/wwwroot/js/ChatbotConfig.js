/**
 * Chatbot Configuration
 * Centralized configuration for the FAQ chatbot
 */

window.ChatbotConfig = {
    // API Configuration
    apiEndpoint: '/ChatbotAPI.ashx',
    
    // UI Configuration
    enableTypingIndicator: true,
    typingDelay: 1000,
    maxMessageLength: 500,
    enableSuggestions: true,
    enableSound: false,
    
    // Behavior Configuration
    enableAnalytics: true,
    maxHistoryLength: 50,
    autoOpenDelay: 0, // 0 = disabled, milliseconds to auto-open after page load
    
    // Fallback Configuration
    fallbackMessage: "I'm sorry, I couldn't find a specific answer to your question. Please contact us directly at 076 463 1930 or email emontioptom@gmail.com for assistance. You can also visit our Help page for more information.",
    
    // Welcome Configuration
    welcomeMessage: "Hello! 👋 I'm your FAQ assistant. How can I help you today?",
    
    // Suggestion Configuration
    suggestions: [
        'Book an appointment',
        'Contact information',
        'Opening hours',
        'Payment methods',
        'Eye test duration',
        'Warranty policy',
        'Return policy',
        'Emergency care'
    ],
    
    // Analytics Configuration
    analytics: {
        enabled: true,
        trackEvents: true,
        trackConversations: true,
        trackFeedback: true
    },
    
    // Theme Configuration
    theme: {
        primaryColor: '#667eea',
        secondaryColor: '#764ba2',
        borderRadius: '15px',
        shadow: '0 8px 25px rgba(0,0,0,0.15)'
    },
    
    // Localization
    localization: {
        language: 'en',
        messages: {
            typing: 'Bot is typing',
            send: 'Send',
            close: 'Close',
            placeholder: 'Ask a question...',
            error: 'Sorry, I encountered an error. Please try again.',
            noResponse: 'I didn\'t understand that. Could you please rephrase your question?'
        }
    },
    
    // Advanced Configuration
    advanced: {
        enableDebugMode: false,
        enableConsoleLogging: true,
        enablePerformanceTracking: false,
        enableA11y: true,
        enableKeyboardNavigation: true
    }
};

// Override configuration from server-side if needed
if (window.ChatbotServerConfig) {
    Object.assign(window.ChatbotConfig, window.ChatbotServerConfig);
}

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = window.ChatbotConfig;
}
