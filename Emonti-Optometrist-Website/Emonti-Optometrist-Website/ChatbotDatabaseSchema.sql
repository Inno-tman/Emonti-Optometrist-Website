-- Chatbot Database Schema for Emonti Optometrist Website
-- Run this script to create the necessary tables for the chatbot functionality

-- Create FAQ Categories table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FAQ_Categories' AND xtype='U')
BEGIN
    CREATE TABLE FAQ_Categories (
        CategoryID INT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME DEFAULT GETDATE()
    );
    
    -- Insert default categories
    INSERT INTO FAQ_Categories (CategoryName, Description) VALUES
    ('appointments', 'Questions about booking and managing appointments'),
    ('payments', 'Questions about payment methods and billing'),
    ('services', 'Questions about eye care services and procedures'),
    ('policies', 'Questions about return, warranty, and cancellation policies'),
    ('contact', 'Questions about contact information and business hours');
END

-- Create FAQ Items table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='FAQ_Items' AND xtype='U')
BEGIN
    CREATE TABLE FAQ_Items (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Question NVARCHAR(500) NOT NULL,
        Answer NVARCHAR(MAX) NOT NULL,
        Keywords NVARCHAR(500),
        Category NVARCHAR(100),
        Priority INT DEFAULT 2, -- 1 = High, 2 = Medium, 3 = Low
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME DEFAULT GETDATE()
    );
    
    -- Insert default FAQ items
    INSERT INTO FAQ_Items (Question, Answer, Keywords, Category, Priority) VALUES
    ('How do I book an appointment?', 
     'You can book an appointment by visiting our "Book Appointment" page. Simply select your preferred service, date, and time slot. You''ll need to be logged in to your account to complete the booking.',
     'appointment, book, schedule, visit, booking, reserve',
     'appointments', 1),
    
    ('What payment methods do you accept?',
     'We accept all major credit cards (Visa, MasterCard, American Express), debit cards, and cash payments. We also work with most medical aid schemes for covered services.',
     'payment, pay, credit card, cash, medical aid, money, cost, price',
     'payments', 1),
    
    ('How long does an eye test take?',
     'A comprehensive eye test typically takes between 30-45 minutes. This includes a thorough examination of your eye health, vision assessment, and consultation with our optometrist.',
     'eye test, examination, how long, duration, test time, exam',
     'services', 1),
    
    ('Do you offer home visits?',
     'Yes, we offer home visits for elderly patients or those with mobility issues. Please contact us to arrange a home visit appointment.',
     'home visit, mobile, elderly, mobility, house call, visit home',
     'services', 2),
    
    ('What is your cancellation policy?',
     'We require 24 hours notice for appointment cancellations. Late cancellations may incur a fee. You can cancel or reschedule appointments through your profile page or by contacting us directly.',
     'cancel, cancellation, reschedule, policy, change appointment, postpone',
     'policies', 1),
    
    ('How do I know if I need new glasses?',
     'Common signs include frequent headaches, eye strain, difficulty reading, or blurry vision. We recommend annual eye tests to monitor your vision health and determine if prescription changes are needed.',
     'glasses, prescription, new glasses, vision, eyewear, spectacles',
     'services', 1),
    
    ('How can I contact you?',
     'You can reach us at 076 463 1930 or email emontioptom@gmail.com. We''re located at Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247.',
     'contact, phone, email, reach, get in touch, call',
     'contact', 1),
    
    ('What are your opening hours?',
     'Our opening hours are: Monday - Friday: 8:00 AM - 5:00 PM, Saturday: 8:00 AM - 2:00 PM, Sunday: Closed.',
     'hours, opening, time, when open, business hours, schedule',
     'contact', 1),
    
    ('What is your warranty policy?',
     'We offer 1-year manufacturer warranty on frames, 1-year scratch warranty on lenses, and free adjustments for 6 months. Normal wear and tear is excluded from coverage.',
     'warranty, guarantee, repair, broken, damaged, fix',
     'policies', 2),
    
    ('What is your return policy?',
     'We offer a 30-day return policy for unused frames, 14-day return policy for accessories. Custom lenses cannot be returned. Items must be in original condition.',
     'return, exchange, refund, bring back, take back',
     'policies', 2);
END

-- Create Chat Conversations table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Chat_Conversations' AND xtype='U')
BEGIN
    CREATE TABLE Chat_Conversations (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        SessionId NVARCHAR(100) NOT NULL,
        UserMessage NVARCHAR(500) NOT NULL,
        BotResponse NVARCHAR(MAX) NOT NULL,
        ConfidenceScore FLOAT,
        ResponseTimeMs INT DEFAULT 0,
        CreatedDate DATETIME DEFAULT GETDATE(),
        IsResolved BIT DEFAULT 0
    );
    
    -- Create index for better performance
    CREATE INDEX IX_Chat_Conversations_SessionId ON Chat_Conversations(SessionId);
    CREATE INDEX IX_Chat_Conversations_CreatedDate ON Chat_Conversations(CreatedDate);
END

-- Create Chatbot Feedback table
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Chatbot_Feedback' AND xtype='U')
BEGIN
    CREATE TABLE Chatbot_Feedback (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ConversationId INT NOT NULL,
        UserRating INT, -- 1-5 stars
        WasHelpful BIT,
        Comments NVARCHAR(500),
        CreatedDate DATETIME DEFAULT GETDATE(),
        FOREIGN KEY (ConversationId) REFERENCES Chat_Conversations(Id)
    );
    
    -- Create index for better performance
    CREATE INDEX IX_Chatbot_Feedback_ConversationId ON Chatbot_Feedback(ConversationId);
END

-- Create Chatbot Analytics table (optional)
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Chatbot_Analytics' AND xtype='U')
BEGIN
    CREATE TABLE Chatbot_Analytics (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        EventName NVARCHAR(100) NOT NULL,
        SessionId NVARCHAR(100),
        EventData NVARCHAR(MAX),
        UserAgent NVARCHAR(500),
        IPAddress NVARCHAR(45),
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    
    -- Create index for better performance
    CREATE INDEX IX_Chatbot_Analytics_EventName ON Chatbot_Analytics(EventName);
    CREATE INDEX IX_Chatbot_Analytics_CreatedDate ON Chatbot_Analytics(CreatedDate);
END

-- Create stored procedure for getting FAQ statistics
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_GetFAQStats')
    DROP PROCEDURE sp_GetFAQStats;
GO

CREATE PROCEDURE sp_GetFAQStats
AS
BEGIN
    SELECT 
        f.Category,
        COUNT(f.Id) as TotalFAQs,
        COUNT(CASE WHEN f.IsActive = 1 THEN 1 END) as ActiveFAQs,
        AVG(CAST(c.ResponseTimeMs AS FLOAT)) as AvgResponseTime,
        COUNT(c.Id) as TotalConversations
    FROM FAQ_Items f
    LEFT JOIN Chat_Conversations c ON 1=1
    GROUP BY f.Category
    ORDER BY TotalFAQs DESC;
END
GO

-- Create stored procedure for getting chatbot performance metrics
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_GetChatbotMetrics')
    DROP PROCEDURE sp_GetChatbotMetrics;
GO

CREATE PROCEDURE sp_GetChatbotMetrics
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET @StartDate = ISNULL(@StartDate, DATEADD(day, -30, GETDATE()));
    SET @EndDate = ISNULL(@EndDate, GETDATE());
    
    SELECT 
        COUNT(*) as TotalConversations,
        COUNT(DISTINCT SessionId) as UniqueSessions,
        AVG(CAST(ResponseTimeMs AS FLOAT)) as AvgResponseTime,
        AVG(CAST(ConfidenceScore AS FLOAT)) as AvgConfidence,
        COUNT(CASE WHEN IsResolved = 1 THEN 1 END) as ResolvedConversations
    FROM Chat_Conversations
    WHERE CreatedDate BETWEEN @StartDate AND @EndDate;
    
    SELECT 
        f.Question,
        COUNT(c.Id) as TimesAsked,
        AVG(CAST(c.ConfidenceScore AS FLOAT)) as AvgConfidence
    FROM FAQ_Items f
    LEFT JOIN Chat_Conversations c ON c.UserMessage LIKE '%' + f.Question + '%'
    WHERE c.CreatedDate BETWEEN @StartDate AND @EndDate
    GROUP BY f.Question, f.Id
    ORDER BY TimesAsked DESC;
END
GO

PRINT 'Chatbot database schema created successfully!';
PRINT 'Tables created: FAQ_Categories, FAQ_Items, Chat_Conversations, Chatbot_Feedback, Chatbot_Analytics';
PRINT 'Stored procedures created: sp_GetFAQStats, sp_GetChatbotMetrics';
