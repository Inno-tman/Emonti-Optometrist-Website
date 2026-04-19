# FAQ Chatbot Implementation - Refactored

This document describes the refactored FAQ chatbot implementation for the Emonti Optometrist Website.

## Overview

The chatbot has been refactored into a modular, maintainable system with the following improvements:

- **Separated concerns**: CSS, JavaScript, and C# code are in separate files
- **API integration**: Server-side processing with fallback to client-side matching
- **Database support**: Optional database storage for FAQ data and conversation logs
- **Configuration management**: Centralized configuration system
- **Better error handling**: Graceful fallbacks and error recovery
- **Performance optimizations**: Caching, lazy loading, and efficient DOM manipulation

## File Structure

```
├── Content/
│   └── ChatbotStyles.css          # Chatbot-specific CSS styles
├── Scripts/
│   ├── ChatbotConfig.js           # Configuration file
│   └── Chatbot.js                 # Main chatbot functionality
├── Models/
│   └── FAQModels.cs               # C# model classes and database operations
├── ChatbotAPI.ashx                # API endpoint for server-side processing
├── ChatbotDatabaseSchema.sql      # Database schema and setup
└── Site.Master                    # Updated with chatbot integration
```

## Features

### ✅ Implemented Features

1. **Modular Architecture**
   - Separate CSS, JavaScript, and C# files
   - Clean separation of concerns
   - Easy to maintain and extend

2. **API Integration**
   - Server-side FAQ processing via `ChatbotAPI.ashx`
   - Fallback to client-side keyword matching
   - Conversation logging and analytics

3. **Database Support**
   - Optional database storage for FAQ data
   - Conversation history tracking
   - User feedback collection
   - Performance analytics

4. **Configuration Management**
   - Centralized configuration in `ChatbotConfig.js`
   - Runtime configuration updates
   - Data attribute overrides

5. **Enhanced User Experience**
   - Typing indicators
   - Suggestion buttons
   - Smooth animations
   - Mobile responsiveness
   - Accessibility support

6. **Error Handling**
   - Graceful API failure handling
   - Fallback responses
   - Console logging for debugging

## Configuration

### Basic Configuration

The chatbot can be configured through the `ChatbotConfig.js` file:

```javascript
window.ChatbotConfig = {
    // API Configuration
    apiEndpoint: '/ChatbotAPI.ashx',
    
    // UI Configuration
    enableTypingIndicator: true,
    typingDelay: 1000,
    maxMessageLength: 500,
    enableSuggestions: true,
    
    // Behavior Configuration
    enableAnalytics: true,
    maxHistoryLength: 50,
    
    // Customization
    welcomeMessage: "Hello! 👋 I'm your FAQ assistant...",
    fallbackMessage: "I'm sorry, I couldn't find a specific answer...",
    suggestions: ['Book an appointment', 'Payment methods', ...]
};
```

### Data Attribute Configuration

You can also configure the chatbot using data attributes on the container:

```html
<div id="chatbot-container" 
     data-enable-typing="true" 
     data-typing-delay="1000" 
     data-enable-suggestions="true" 
     data-enable-sound="false">
```

## Database Setup

### Option 1: With Database (Recommended)

1. Run the `ChatbotDatabaseSchema.sql` script to create the necessary tables
2. The chatbot will automatically use the database for FAQ data and conversation logging

### Option 2: Without Database

1. The chatbot will use the default FAQ data from the JavaScript file
2. No conversation logging or analytics will be available

## API Endpoints

### Chat Endpoint
```
POST /ChatbotAPI.ashx?method=chat
Content-Type: application/x-www-form-urlencoded

message=How do I book an appointment?
sessionId=chatbot_1234567890_abc123
```

### Get FAQs Endpoint
```
GET /ChatbotAPI.ashx?method=faqs&category=appointments
```

### Feedback Endpoint
```
POST /ChatbotAPI.ashx?method=feedback
Content-Type: application/x-www-form-urlencoded

conversationId=123
rating=5
wasHelpful=true
comments=Very helpful!
```

## Usage

### Basic Usage

The chatbot is automatically initialized when the page loads. No additional setup is required.

### Advanced Usage

```javascript
// Access the chatbot instance
const chatbot = window.chatbot;

// Update configuration
chatbot.updateConfig({
    enableTypingIndicator: false,
    typingDelay: 500
});

// Add new FAQ item
chatbot.addFAQItem({
    question: 'What are your prices?',
    answer: 'Our prices vary depending on the service...',
    keywords: ['price', 'cost', 'fee', 'charge'],
    category: 'pricing',
    priority: 1
});

// Get conversation history
const history = chatbot.getHistory();

// Clear conversation history
chatbot.clearHistory();
```

## Customization

### Styling

Modify `Content/ChatbotStyles.css` to customize the appearance:

```css
.chatbot-container {
    /* Custom positioning */
    bottom: 30px;
    right: 30px;
}

.chatbot-toggle {
    /* Custom colors */
    background: linear-gradient(135deg, #your-color-1, #your-color-2);
}
```

### FAQ Data

Add or modify FAQ items in `Models/FAQModels.cs` or through the database:

```csharp
var faq = new FAQItem
{
    Question = "What are your hours?",
    Answer = "We're open Monday-Friday 9-5...",
    Keywords = "hours, open, time, schedule",
    Category = "contact",
    Priority = 1
};
```

### Responses

Customize responses by modifying the `GetFallbackResponse` method in `ChatbotAPI.ashx` or the `getFallbackResponse` method in `Chatbot.js`.

## Analytics

The chatbot includes built-in analytics tracking:

- Conversation counts
- Response times
- Confidence scores
- User feedback
- Popular questions

Access analytics through the database or by implementing custom tracking in the `trackEvent` method.

## Performance

### Optimizations

1. **Lazy Loading**: FAQ data is loaded only when needed
2. **Caching**: Responses are cached for repeated queries
3. **Efficient DOM**: Minimal DOM manipulation and efficient selectors
4. **Debounced Input**: Prevents excessive API calls during typing

### Monitoring

Monitor performance through:
- Database analytics tables
- Browser developer tools
- Server-side logging
- Custom performance tracking

## Troubleshooting

### Common Issues

1. **Chatbot not appearing**
   - Check that all required files are included
   - Verify HTML structure in Site.Master
   - Check browser console for errors

2. **API not responding**
   - Verify ChatbotAPI.ashx is accessible
   - Check database connection string
   - Review server logs for errors

3. **Styling issues**
   - Ensure ChatbotStyles.css is loaded
   - Check for CSS conflicts
   - Verify responsive design breakpoints

### Debug Mode

Enable debug mode in the configuration:

```javascript
window.ChatbotConfig.advanced.enableDebugMode = true;
```

## Future Enhancements

### Planned Features

1. **AI Integration**
   - OpenAI API integration
   - Azure Cognitive Services
   - Natural language processing

2. **Advanced Analytics**
   - User behavior tracking
   - A/B testing support
   - Performance dashboards

3. **Multi-language Support**
   - Internationalization
   - Language detection
   - Localized responses

4. **Voice Support**
   - Speech-to-text
   - Text-to-speech
   - Voice commands

## Support

For issues or questions:

1. Check the browser console for errors
2. Review the database logs
3. Test with debug mode enabled
4. Contact the development team

## License

This chatbot implementation is part of the Emonti Optometrist Website project.
