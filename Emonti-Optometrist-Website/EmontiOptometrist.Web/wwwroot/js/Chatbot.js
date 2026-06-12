/**
 * FAQ Chatbot for Emonti Optometrist Website
 * Refactored version with improved structure and maintainability
 */

class FAQChatbot {
    constructor(config = {}) {
        // Merge with global configuration
        const globalConfig = window.ChatbotConfig || {};
        this.config = {
            ...globalConfig,
            ...config
        };

        // FAQ data - extracted from About, Contact, and Help pages
        this.faqData = [
            {
                id: 1,
                keywords: ['appointment', 'book', 'schedule', 'visit', 'booking', 'reserve'],
                question: 'How do I book an appointment?',
                answer: 'You can book an appointment by visiting our "Book Appointment" page. Simply select your preferred service, date, and time slot. You\'ll need to be logged in to your account to complete the booking. Online booking is available 24/7, or you can call us during business hours at 076 463 1930.',
                category: 'appointments',
                priority: 1
            },
            {
                id: 2,
                keywords: ['payment', 'pay', 'credit card', 'cash', 'medical aid', 'money', 'cost', 'price'],
                question: 'What payment methods do you accept?',
                answer: 'We accept all major credit cards (Visa, MasterCard, American Express), debit cards, and cash payments. We also work with most medical aid schemes for covered services. We submit medical aid claims on your behalf, though coverage varies by scheme and gap payments may apply.',
                category: 'payments',
                priority: 1
            },
            {
                id: 3,
                keywords: ['eye test', 'examination', 'how long', 'duration', 'test time', 'exam'],
                question: 'How long does an eye test take?',
                answer: 'A comprehensive eye test typically takes between 30-45 minutes. This includes a thorough examination of your eye health, vision assessment, and consultation with our optometrist.',
                category: 'services',
                priority: 1
            },
            {
                id: 4,
                keywords: ['home visit', 'mobile', 'elderly', 'mobility', 'house call', 'visit home'],
                question: 'Do you offer home visits?',
                answer: 'Yes, we offer home visits for elderly patients or those with mobility issues. Please contact us at 076 463 1930 or email emontioptom@gmail.com to arrange a home visit appointment.',
                category: 'services',
                priority: 2
            },
            {
                id: 5,
                keywords: ['cancel', 'cancellation', 'reschedule', 'policy', 'change appointment', 'postpone'],
                question: 'What is your cancellation policy?',
                answer: 'We require 24 hours notice for appointment cancellations. Late cancellations may incur a fee. You can cancel or reschedule appointments through your profile page or by contacting us directly at 076 463 1930.',
                category: 'policies',
                priority: 1
            },
            {
                id: 6,
                keywords: ['glasses', 'prescription', 'new glasses', 'vision', 'eyewear', 'spectacles'],
                question: 'How do I know if I need new glasses?',
                answer: 'Common signs include frequent headaches, eye strain, difficulty reading, or blurry vision. We recommend annual eye tests to monitor your vision health and determine if prescription changes are needed.',
                category: 'services',
                priority: 1
            },
            {
                id: 7,
                keywords: ['contact', 'phone', 'email', 'reach', 'get in touch', 'call', 'address', 'location'],
                question: 'How can I contact you?',
                answer: 'You can reach us at 076 463 1930 or email emontioptom@gmail.com. We\'re located at Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247, South Africa. We are conveniently located in the heart of East London, easily accessible by public transport and with ample parking available.',
                category: 'contact',
                priority: 1
            },
            {
                id: 8,
                keywords: ['hours', 'opening', 'time', 'when open', 'business hours', 'schedule', 'closed'],
                question: 'What are your opening hours?',
                answer: 'Our regular hours are: Monday - Friday: 8:00 AM - 5:00 PM, Saturday: 8:00 AM - 2:00 PM, Sunday: Closed. Online booking is available 24/7. Phone bookings can be made during business hours. Walk-ins are subject to availability.',
                category: 'contact',
                priority: 1
            },
            {
                id: 9,
                keywords: ['warranty', 'guarantee', 'repair', 'broken', 'damaged', 'fix', 'adjustment'],
                question: 'What is your warranty policy?',
                answer: 'We offer 1-year manufacturer warranty on frames, 1-year scratch warranty on lenses, and free adjustments for 6 months. Normal wear and tear is excluded from coverage.',
                category: 'policies',
                priority: 2
            },
            {
                id: 10,
                keywords: ['return', 'exchange', 'refund', 'bring back', 'take back'],
                question: 'What is your return policy?',
                answer: 'We offer a 30-day return policy for unused frames, 14-day return policy for accessories. Custom lenses cannot be returned. Items must be in original condition.',
                category: 'policies',
                priority: 2
            },
            {
                id: 11,
                keywords: ['emergency', 'urgent', 'immediate', 'emergency care'],
                question: 'Do you offer emergency eye care?',
                answer: 'Yes, for urgent eye care, call us immediately at 076 463 1930 or visit our emergency service.',
                category: 'services',
                priority: 1
            },
            {
                id: 12,
                keywords: ['about', 'story', 'history', 'founded', 'when started', 'established'],
                question: 'Tell me about Emonti Optometrist',
                answer: 'Emonti Optometrist was founded in 2023 and has been serving the East London community with dedication and expertise. We combine cutting-edge technology with years of experience to deliver accurate diagnoses and effective treatments. Our mission is to provide exceptional eye care services that enhance the quality of life for our patients.',
                category: 'about',
                priority: 2
            },
            {
                id: 13,
                keywords: ['team', 'staff', 'optometrist', 'doctor', 'who works', 'practitioners'],
                question: 'Who is on your team?',
                answer: 'Our team includes Alex Brown (Senior Optometrist specializing in comprehensive eye exams and contact lens fittings), Jane Smith (Optometrist specializing in pediatric optometry), and Sam Wilson (Practice Manager ensuring smooth operations and excellent customer service).',
                category: 'about',
                priority: 2
            },
            {
                id: 14,
                keywords: ['values', 'mission', 'what you believe', 'principles'],
                question: 'What are your core values?',
                answer: 'Our core values are: Compassionate Care - treating every patient with kindness and respect; Excellence - maintaining the highest standards using latest technology; Community Focus - serving our local community; and Innovation - continuously embracing new technologies and techniques.',
                category: 'about',
                priority: 2
            },
            {
                id: 15,
                keywords: ['privacy', 'data', 'confidential', 'information', 'personal'],
                question: 'How do you protect my privacy?',
                answer: 'Your personal information is kept confidential. We do not share data with third parties. Medical records are protected by law, and you can request data deletion anytime.',
                category: 'policies',
                priority: 2
            },
            {
                id: 16,
                keywords: ['medical aid', 'scheme', 'claim', 'insurance', 'coverage'],
                question: 'How do medical aid claims work?',
                answer: 'We submit medical aid claims on your behalf. Coverage varies by medical aid scheme, and gap payments may apply. Contact us at 076 463 1930 for specific scheme information.',
                category: 'payments',
                priority: 2
            },
            {
                id: 17,
                keywords: ['walk in', 'walk-in', 'without appointment', 'drop in'],
                question: 'Can I walk in without an appointment?',
                answer: 'Walk-ins are subject to availability. We recommend booking an appointment in advance to ensure you get the time slot that works best for you. You can book online 24/7 or call us during business hours.',
                category: 'appointments',
                priority: 2
            }
        ];

        // State management
        this.isTyping = false;
        this.isOpen = false;
        this.messageHistory = [];
        this.sessionId = this.generateSessionId();

        // DOM elements
        this.elements = {};

        // Initialize
        this.init();
    }

    /**
     * Initialize the chatbot
     */
    init() {
        this.createElements();
        this.bindEvents();
        this.showWelcomeMessage();
        this.loadConfiguration();
        
        // Initialize scroll indicator after a short delay
        setTimeout(() => {
            this.updateScrollIndicator();
        }, 500);
    }

    /**
     * Create and cache DOM elements
     */
    createElements() {
        this.elements = {
            container: document.getElementById('chatbot-container'),
            toggle: document.getElementById('chatbot-toggle'),
            window: document.getElementById('chatbot-window'),
            close: document.getElementById('chatbot-close'),
            messages: document.getElementById('chatbot-messages'),
            input: document.getElementById('chatbot-input'),
            send: document.getElementById('chatbot-send')
        };

        if (!this.validateElements()) {
            console.error('Chatbot: Required elements not found. Please check HTML structure.');
            return false;
        }

        return true;
    }

    /**
     * Validate that all required elements exist
     */
    validateElements() {
        const required = ['container', 'toggle', 'window', 'close', 'messages', 'input', 'send'];
        return required.every(key => this.elements[key] !== null);
    }

    /**
     * Bind event listeners
     */
    bindEvents() {
        // Toggle chatbot
        this.elements.toggle.addEventListener('click', () => this.toggleWindow());

        // Close chatbot
        this.elements.close.addEventListener('click', () => this.closeWindow());

        // Send message
        this.elements.send.addEventListener('click', () => this.sendMessage());

        // Input events
        this.elements.input.addEventListener('keypress', (e) => {
            if (e.key === 'Enter' && !e.shiftKey) {
                e.preventDefault();
                this.sendMessage();
            }
        });

        this.elements.input.addEventListener('input', () => {
            const hasValue = this.elements.input.value.trim().length > 0;
            this.elements.send.disabled = !hasValue;
            
            // Add input animation
            if (hasValue && this.elements.send.disabled === false) {
                this.elements.send.style.transform = 'scale(1.05)';
                setTimeout(() => {
                    this.elements.send.style.transform = '';
                }, 200);
            }
        });
        
        // Add scroll listener for scroll indicator
        this.elements.messages.addEventListener('scroll', () => {
            this.updateScrollIndicator();
        });

        // Close on outside click
        document.addEventListener('click', (e) => {
            if (this.isOpen && 
                !this.isTyping &&
                !this.elements.window.contains(e.target) && 
                !this.elements.toggle.contains(e.target)) {
                this.closeWindow();
            }
        });

        // Keyboard shortcuts
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.closeWindow();
            }
        });
    }

    /**
     * Load configuration from data attributes or localStorage
     */
    loadConfiguration() {
        // Load from data attributes
        const container = this.elements.container;
        if (container) {
            // Helper to check if value is explicitly false ('false' or '0')
            const isExplicitlyFalse = (value) => {
                if (!value) return false;
                const lower = value.toLowerCase();
                return lower === 'false' || lower === '0';
            };
            
            // Helper to check if value is explicitly true ('true' or '1')
            const isExplicitlyTrue = (value) => {
                if (!value) return false;
                const lower = value.toLowerCase();
                return lower === 'true' || lower === '1';
            };
            
            const dataConfig = {
                // Default to true unless explicitly 'false' or '0' (preserves original behavior)
                enableTypingIndicator: !isExplicitlyFalse(container.dataset.enableTyping),
                typingDelay: parseInt(container.dataset.typingDelay) || this.config.typingDelay,
                // Default to true unless explicitly 'false' or '0' (preserves original behavior)
                enableSuggestions: !isExplicitlyFalse(container.dataset.enableSuggestions),
                // Only true if explicitly 'true' or '1' (preserves original behavior)
                enableSound: isExplicitlyTrue(container.dataset.enableSound)
            };
            this.config = { ...this.config, ...dataConfig };
        }

        // Load from localStorage
        const savedConfig = localStorage.getItem('chatbot-config');
        if (savedConfig) {
            try {
                const parsed = JSON.parse(savedConfig);
                this.config = { ...this.config, ...parsed };
            } catch (e) {
                console.warn('Chatbot: Invalid saved configuration');
            }
        }
    }

    /**
     * Save configuration to localStorage
     */
    saveConfiguration() {
        localStorage.setItem('chatbot-config', JSON.stringify(this.config));
    }

    /**
     * Toggle chatbot window
     */
    toggleWindow() {
        if (this.isOpen) {
            this.closeWindow();
        } else {
            this.openWindow();
        }
    }

    /**
     * Open chatbot window
     */
    openWindow() {
        this.elements.window.style.display = 'flex';
        this.elements.window.classList.remove('closing');
        this.isOpen = true;
        
        // Trigger animation
        requestAnimationFrame(() => {
            this.elements.window.style.opacity = '0';
            this.elements.window.style.transform = 'translateY(30px) scale(0.95)';
            requestAnimationFrame(() => {
                this.elements.window.style.transition = 'opacity 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275), transform 0.4s cubic-bezier(0.175, 0.885, 0.32, 1.275)';
                this.elements.window.style.opacity = '1';
                this.elements.window.style.transform = 'translateY(0) scale(1)';
            });
        });
        
        this.elements.input.focus();
        this.elements.send.disabled = !this.elements.input.value.trim();
        
        // Track analytics
        this.trackEvent('chatbot_opened');
    }

    /**
     * Close chatbot window
     */
    closeWindow() {
        this.elements.window.classList.add('closing');
        
        setTimeout(() => {
            this.elements.window.style.display = 'none';
            this.elements.window.classList.remove('closing');
        }, 300);
        
        this.isOpen = false;
        
        // Track analytics
        this.trackEvent('chatbot_closed');
    }

    /**
     * Show welcome message and suggestions
     */
    showWelcomeMessage() {
        const welcomeMessage = this.config.welcomeMessage || 'Hello! 👋 I\'m your FAQ assistant. How can I help you today?';
        this.addMessage('bot', welcomeMessage);
        
        if (this.config.enableSuggestions) {
            this.showSuggestions();
        }
    }

    /**
     * Send user message
     */
    async sendMessage() {
        const message = this.elements.input.value.trim();
        if (!message || this.isTyping) return;

        // Add loading state
        this.elements.container.classList.add('chatbot-loading');

        // Add user message
        this.addMessage('user', message);
        this.messageHistory.push({ type: 'user', content: message, timestamp: new Date() });

        // Clear input with animation
        this.elements.input.style.transform = 'scale(0.98)';
        setTimeout(() => {
            this.elements.input.value = '';
            this.elements.input.style.transform = '';
        }, 100);
        
        this.elements.send.disabled = true;
        
        // Animate send button
        this.elements.send.style.transform = 'scale(0.9) rotate(-10deg)';
        setTimeout(() => {
            this.elements.send.style.transform = '';
        }, 200);

        // Show typing indicator
        if (this.config.enableTypingIndicator) {
            setTimeout(() => {
                this.showTypingIndicator();
            }, 300);
        }

        try {
            // Get response with delay for better UX
            const delay = this.config.typingDelay || 1000;
            const [result] = await Promise.all([
                this.getResponse(message),
                new Promise(resolve => setTimeout(resolve, delay))
            ]);
            
            const botText = typeof result === 'object' ? result.message : result;
            const aiPowered = typeof result === 'object' ? result.aiPowered : false;
            
            // Hide typing indicator
            this.hideTypingIndicator();
            
            // Add bot response with slight delay
            setTimeout(() => {
                this.addMessage('bot', botText, aiPowered);
                this.messageHistory.push({ type: 'bot', content: botText, timestamp: new Date() });
                
                // Remove loading state
                this.elements.container.classList.remove('chatbot-loading');
            }, 200);

        } catch (error) {
            console.error('Chatbot error:', error);
            this.hideTypingIndicator();
            this.elements.container.classList.remove('chatbot-loading');
            
            setTimeout(() => {
                this.addMessage('bot', 'Sorry, I encountered an error. Please try again.');
            }, 200);
        }

        this.elements.input.focus();
    }

    /**
     * Get response for user message
     */
    async getResponse(message) {
        if (this.config.apiEndpoint) {
            try {
                return await this.getAPIResponse(message);
            } catch (error) {
                if (this.config.advanced?.enableConsoleLogging) {
                    console.warn('API request failed, falling back to local matching:', error);
                }
            }
        }

        return { message: this.findLocalAnswer(message), aiPowered: false };
    }

    /**
     * Get response from API
     */
    async getAPIResponse(message) {
        const response = await fetch(this.config.apiEndpoint, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: new URLSearchParams({
                method: 'chat',
                message: message,
                sessionId: this.sessionId
            })
        });

        if (!response.ok) {
            throw new Error(`API request failed: ${response.status}`);
        }

        const data = await response.json();
        
        if (!data.success) {
            throw new Error(data.message || 'API request failed');
        }
        
        return {
            message: data.message || 'I\'m sorry, I couldn\'t process your request.',
            aiPowered: data.aiPowered || false
        };
    }

    /**
     * Find answer using local keyword matching
     */
    findLocalAnswer(message) {
        const lowerMessage = message.toLowerCase();
        let bestMatch = null;
        let maxScore = 0;

        // Score each FAQ item
        for (const faq of this.faqData) {
            let score = 0;
            
            // Check keyword matches
            for (const keyword of faq.keywords) {
                if (lowerMessage.includes(keyword)) {
                    score += 1;
                }
            }

            // Boost score for exact question match
            if (lowerMessage.includes(faq.question.toLowerCase())) {
                score += 2;
            }

            // Apply priority multiplier
            score *= (3 - faq.priority) / 2;

            if (score > maxScore) {
                maxScore = score;
                bestMatch = faq;
            }
        }

        // Return best match if score is high enough
        if (bestMatch && maxScore > 0) {
            return bestMatch.answer;
        }

        // Fallback responses
        return this.getFallbackResponse(lowerMessage);
    }

    /**
     * Get fallback response for unmatched queries
     */
    getFallbackResponse(message) {
        const fallbacks = {
            greeting: ['hello', 'hi', 'hey', 'good morning', 'good afternoon', 'good evening'],
            thanks: ['thank', 'thanks', 'appreciate'],
            goodbye: ['bye', 'goodbye', 'see you', 'farewell'],
            help: ['help', 'what can you do', 'commands', 'options']
        };

        for (const [type, keywords] of Object.entries(fallbacks)) {
            if (keywords.some(keyword => message.includes(keyword))) {
                return this.getFallbackMessage(type);
            }
        }

        return this.config.fallbackMessage || 'I\'m sorry, I couldn\'t find a specific answer to your question. Please contact us directly at 076 463 1930 or email emontioptom@gmail.com for assistance. You can also visit our Help page for more information.';
    }

    /**
     * Get specific fallback message
     */
    getFallbackMessage(type) {
        const messages = {
            greeting: 'Hello! How can I assist you today?',
            thanks: 'You\'re welcome! Is there anything else I can help you with?',
            goodbye: 'Goodbye! Feel free to come back anytime if you have more questions.',
            help: 'I can help you with questions about appointments, payments, services, policies, and contact information. What would you like to know?'
        };

        return messages[type] || messages.help;
    }

    /**
     * Add message to chat
     */
    addMessage(type, text, aiPowered) {
        const messageDiv = document.createElement('div');
        messageDiv.className = `chatbot-message ${type}`;
        
        const timestamp = new Date();
        const timeString = timestamp.toLocaleTimeString('en-US', { 
            hour: 'numeric', 
            minute: '2-digit',
            hour12: true 
        });
        
        const textSpan = document.createElement('span');
        textSpan.textContent = text;
        messageDiv.appendChild(textSpan);
        
        if (type === 'bot' && aiPowered) {
            const aiBadge = document.createElement('span');
            aiBadge.className = 'chatbot-ai-badge';
            aiBadge.textContent = 'AI';
            messageDiv.appendChild(aiBadge);
        }
        
        const timestampSpan = document.createElement('span');
        timestampSpan.className = 'chatbot-message-timestamp';
        timestampSpan.textContent = timeString;
        messageDiv.appendChild(timestampSpan);
        
        this.elements.messages.appendChild(messageDiv);
        this.scrollToBottom();
    }

    /**
     * Show typing indicator
     */
    showTypingIndicator() {
        if (this.isTyping) return;
        
        this.isTyping = true;
        const typingDiv = document.createElement('div');
        typingDiv.className = 'chatbot-message bot typing';
        typingDiv.innerHTML = `
            <span>Bot is typing</span>
            <div class="typing-dots">
                <span></span>
                <span></span>
                <span></span>
            </div>
        `;
        typingDiv.id = 'typing-indicator';
        
        // Animate in
        typingDiv.style.opacity = '0';
        typingDiv.style.transform = 'translateY(15px) scale(0.95)';
        this.elements.messages.appendChild(typingDiv);
        
        requestAnimationFrame(() => {
            typingDiv.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
            typingDiv.style.opacity = '1';
            typingDiv.style.transform = 'translateY(0) scale(1)';
        });
        
        this.scrollToBottom();
    }

    /**
     * Hide typing indicator
     */
    hideTypingIndicator() {
        this.isTyping = false;
        const typingDiv = document.getElementById('typing-indicator');
        if (typingDiv && typingDiv.parentNode) {
            typingDiv.style.transition = 'opacity 0.2s ease, transform 0.2s ease';
            typingDiv.style.opacity = '0';
            typingDiv.style.transform = 'translateY(-10px) scale(0.9)';
            
            setTimeout(() => {
                if (typingDiv.parentNode) {
                    typingDiv.remove();
                }
            }, 200);
        }
    }

    /**
     * Show suggestion buttons
     */
    showSuggestions() {
        const suggestions = this.config.suggestions || [
            'Book an appointment',
            'Contact information',
            'Opening hours',
            'Payment methods',
            'Eye test duration',
            'Warranty policy',
            'Return policy',
            'Emergency care'
        ];

        const suggestionsDiv = document.createElement('div');
        suggestionsDiv.className = 'chatbot-suggestions';
        suggestionsDiv.style.opacity = '0';
        suggestionsDiv.style.transform = 'translateY(10px)';

        suggestions.forEach((suggestion, index) => {
            const button = document.createElement('button');
            button.className = 'chatbot-suggestion';
            const span = document.createElement('span');
            span.textContent = suggestion;
            button.appendChild(span);
            
            button.style.opacity = '0';
            button.style.transform = 'translateY(10px)';
            
            button.addEventListener('click', () => {
                // Add click animation
                button.style.transform = 'scale(0.95)';
                setTimeout(() => {
                    button.style.transform = '';
                }, 150);
                
                this.elements.input.value = suggestion;
                this.sendMessage();
            });
            
            suggestionsDiv.appendChild(button);
            
            // Stagger animation
            setTimeout(() => {
                button.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                button.style.opacity = '1';
                button.style.transform = 'translateY(0)';
            }, index * 50);
        });

        this.elements.messages.appendChild(suggestionsDiv);
        
        // Animate container
        requestAnimationFrame(() => {
            suggestionsDiv.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
            suggestionsDiv.style.opacity = '1';
            suggestionsDiv.style.transform = 'translateY(0)';
        });
        
        this.scrollToBottom();
    }

    /**
     * Scroll to bottom of messages
     */
    scrollToBottom(smooth = true) {
        if (smooth) {
            this.elements.messages.scrollTo({
                top: this.elements.messages.scrollHeight,
                behavior: 'smooth'
            });
        } else {
            this.elements.messages.scrollTop = this.elements.messages.scrollHeight;
        }
        
        // Hide scroll indicator if at bottom
        this.updateScrollIndicator();
    }
    
    /**
     * Update scroll indicator visibility
     */
    updateScrollIndicator() {
        const messages = this.elements.messages;
        const isAtBottom = messages.scrollHeight - messages.scrollTop <= messages.clientHeight + 50;
        
        let indicator = messages.querySelector('.chatbot-scroll-indicator');
        if (!indicator) {
            indicator = document.createElement('div');
            indicator.className = 'chatbot-scroll-indicator';
            indicator.innerHTML = '<i class="fas fa-chevron-down"></i>';
            indicator.addEventListener('click', () => this.scrollToBottom());
            messages.appendChild(indicator);
        }
        
        if (isAtBottom) {
            indicator.classList.remove('visible');
        } else {
            indicator.classList.add('visible');
        }
    }

    /**
     * Generate unique session ID
     */
    generateSessionId() {
        return 'chatbot_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    }

    /**
     * Track analytics events
     */
    trackEvent(eventName, data = {}) {
        // You can integrate with Google Analytics, Mixpanel, etc.
        if (typeof gtag !== 'undefined') {
            gtag('event', eventName, {
                event_category: 'chatbot',
                ...data
            });
        }
        
        // Console log for debugging
        console.log(`Chatbot Event: ${eventName}`, data);
    }

    /**
     * Update configuration
     */
    updateConfig(newConfig) {
        this.config = { ...this.config, ...newConfig };
        this.saveConfiguration();
    }

    /**
     * Add new FAQ item
     */
    addFAQItem(faqItem) {
        this.faqData.push({
            id: Date.now(),
            priority: 2,
            ...faqItem
        });
    }

    /**
     * Get conversation history
     */
    getHistory() {
        return this.messageHistory;
    }

    /**
     * Clear conversation history
     */
    clearHistory() {
        this.messageHistory = [];
        this.elements.messages.innerHTML = '';
        this.showWelcomeMessage();
    }

    /**
     * Destroy chatbot instance
     */
    destroy() {
        // Remove event listeners
        document.removeEventListener('click', this.handleOutsideClick);
        document.removeEventListener('keydown', this.handleKeydown);
        
        // Clear DOM
        if (this.elements.container) {
            this.elements.container.remove();
        }
    }
}

// Auto-initialize when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    // Check if chatbot elements exist
    if (document.getElementById('chatbot-container')) {
        window.chatbot = new FAQChatbot();
    }
});

// Export for module systems
if (typeof module !== 'undefined' && module.exports) {
    module.exports = FAQChatbot;
}
