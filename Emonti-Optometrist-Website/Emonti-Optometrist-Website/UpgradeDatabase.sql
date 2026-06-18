-- Database Upgrade Script for Emonti Optometrist
-- Creates sp_UpgradeDatabase which tracks and applies schema migrations
-- Run this script once to install the stored procedure, then call:
--     EXEC sp_UpgradeDatabase;
-- The procedure is automatically called on application startup.

CREATE OR ALTER PROCEDURE sp_UpgradeDatabase
AS
BEGIN
    SET NOCOUNT ON;

    -- Ensure SchemaVersion tracking table exists
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'SchemaVersion')
    BEGIN
        CREATE TABLE SchemaVersion (
            Version INT PRIMARY KEY,
            Description NVARCHAR(500) NOT NULL,
            AppliedDate DATETIME DEFAULT GETDATE()
        );
    END

    -- Migration 23: removed (Last_Login and Cust_* alias columns omitted)

    -- Helper: check if migration version already applied
    -- Uses local table variable to track what we need to apply within this session
    DECLARE @v INT;
    -- Count of applied migrations in this run (used by some migrations)
    DECLARE @AppliedCount INT = 0;

    -- Migration 1: customer table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 1)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'customer')
            BEGIN
                CREATE TABLE customer (
                    Cust_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Customer_Name VARCHAR(50) NOT NULL,
                    Customer_Surname VARCHAR(50) NOT NULL,
                    Customer_DOB DATETIME NULL,
                    Customer_Gender VARCHAR(20) NULL,
                    Customer_Email VARCHAR(100) NOT NULL UNIQUE,
                    Customer_Phone VARCHAR(20) NOT NULL,
                    Customer_Address VARCHAR(50) NULL,
                    Medical_Aid VARCHAR(100) NULL,
                    Medical_Aid_Number VARCHAR(50) NULL,
                    Main_Member_Name VARCHAR(50) NULL,
                    Main_Member_Surname VARCHAR(50) NULL,
                    Main_Member_ID VARCHAR(20) NULL,
                    Street_Number VARCHAR(20) NULL,
                    Street_Name VARCHAR(100) NULL,
                    Complex_Name VARCHAR(100) NULL,
                    Unit_Number VARCHAR(20) NULL,
                    City VARCHAR(100) NULL,
                    Province VARCHAR(50) NULL,
                    Postal_Code VARCHAR(10) NULL,
                    Is_Archive BIT DEFAULT 0,
                    Customer_Password VARCHAR(8) NOT NULL
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (1, 'Create customer table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
            RAISERROR('Migration 1 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 22: (removed) Customer_Create_Date changes intentionally omitted to avoid dependency on missing column

    -- Migration 2: Extend Customer_Address column
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 2)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'customer' AND COLUMN_NAME = 'Customer_Address' AND CHARACTER_MAXIMUM_LENGTH = 50)
            BEGIN
                ALTER TABLE customer ALTER COLUMN Customer_Address VARCHAR(500) NULL;
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (2, 'Extend Customer_Address column');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 2 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 3: Products2 table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 3)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Products2')
            BEGIN
                CREATE TABLE Products2 (
                    Product_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Product_Brand VARCHAR(100),
                    Product_Name VARCHAR(200),
                    Product_Description NVARCHAR(MAX),
                    Product_Category VARCHAR(100),
                    Product_Price DECIMAL(10,2),
                    QuantityOnHand INT,
                    Picture1 VARCHAR(255),
                    Picture2 VARCHAR(255)
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (3, 'Create Products2 table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 3 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 4: Cart table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 4)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Cart')
            BEGIN
                CREATE TABLE Cart (
                    Cart_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cust_ID VARCHAR(50) NOT NULL,
                    Status VARCHAR(20) DEFAULT 'Active',
                    Created_At DATETIME DEFAULT GETDATE(),
                    Updated_At DATETIME DEFAULT GETDATE()
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (4, 'Create Cart table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 4 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 5: CartItem table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 5)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CartItem')
            BEGIN
                CREATE TABLE CartItem (
                    CartItem_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cart_ID INT NOT NULL,
                    Product_ID INT NOT NULL,
                    Quantity INT NOT NULL,
                    Price DECIMAL(10,2) NOT NULL
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (5, 'Create CartItem table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 5 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 6: Order table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 6)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Order')
            BEGIN
                CREATE TABLE [Order] (
                    OrderID INT IDENTITY(1,1) PRIMARY KEY,
                    CustID VARCHAR(50) NOT NULL,
                    Order_Date DATETIME DEFAULT GETDATE(),
                    Order_Total DECIMAL(10,2) NOT NULL,
                    Order_Status VARCHAR(20) DEFAULT 'Pending',
                    Delivery_Address NVARCHAR(500),
                    Payment_Method VARCHAR(50),
                    Payment_Status VARCHAR(20) DEFAULT 'pending',
                    Order_Number VARCHAR(50) UNIQUE,
                    Payment_Date DATETIME NULL,
                    Notes NVARCHAR(500)
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (6, 'Create Order table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 6 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 7: OrderItems table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 7)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'OrderItems')
            BEGIN
                CREATE TABLE OrderItems (
                    OrderItemID INT IDENTITY(1,1) PRIMARY KEY,
                    OrderID INT NOT NULL,
                    Product_ID INT NOT NULL,
                    Product_Name NVARCHAR(200),
                    Product_Brand NVARCHAR(100),
                    Product_Category NVARCHAR(100),
                    Quantity INT NOT NULL,
                    Unit_Price DECIMAL(10,2) NOT NULL,
                    Subtotal DECIMAL(10,2) NOT NULL
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (7, 'Create OrderItems table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 7 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 8: Staff table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 8)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Staff')
            BEGIN
                CREATE TABLE Staff (
                    Staff_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Staff_Name VARCHAR(50),
                    Staff_Surname VARCHAR(50),
                    Staff_Role VARCHAR(50),
                    Staff_Password VARCHAR(50),
                    Staff_Email VARCHAR(100)
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (8, 'Create Staff table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 8 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 9: tblTime table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 9)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'tblTime')
            BEGIN
                CREATE TABLE tblTime (
                    TimeID INT PRIMARY KEY,
                    Timeslot VARCHAR(20)
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (9, 'Create tblTime table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 9 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 10: Appointment table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 10)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Appointment')
            BEGIN
                CREATE TABLE Appointment (
                    Appointment_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cust_ID INT NOT NULL,
                    Staff_ID INT NOT NULL,
                    Appointment_Date DATETIME NOT NULL,
                    AppointmentTimeID INT NOT NULL,
                    Appoinment_Status VARCHAR(20) DEFAULT 'Pending'
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (10, 'Create Appointment table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 10 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 11: BlockedTimeslots table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 11)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'BlockedTimeslots')
            BEGIN
                CREATE TABLE BlockedTimeslots (
                    Block_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Staff_ID INT NOT NULL,
                    Blocked_Date DATE NOT NULL,
                    TimeID INT NOT NULL
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (11, 'Create BlockedTimeslots table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 11 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 12: Payments table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 12)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Payments')
            BEGIN
                CREATE TABLE Payments (
                    Payment_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cust_ID INT NOT NULL,
                    Order_ID INT NULL,
                    Appointment_ID INT NULL,
                    Transaction_Number VARCHAR(13) NOT NULL,
                    Payment_Date DATE DEFAULT GETDATE(),
                    Consultation_Fee DECIMAL(10,2) NULL,
                    Order_Payment DECIMAL(10,2) NULL,
                    Total_Payable DECIMAL(10,2) NOT NULL,
                    Payment_Method VARCHAR(11) NOT NULL,
                    Amount_Received DECIMAL(10,2) NOT NULL,
                    Change_Due DECIMAL(10,2) NULL,
                    Created_Date DATETIME2 DEFAULT GETDATE(),
                    Created_By VARCHAR(50) NOT NULL,
                    Payment_Status VARCHAR(20) DEFAULT 'Completed',
                    Medical_Aid_Amount DECIMAL(10,2) NULL,
                    Patient_Portion_Amount DECIMAL(10,2) NULL,
                    Patient_Payment_Method VARCHAR(11) NULL,
                    Patient_Amount_Received DECIMAL(10,2) NULL,
                    Patient_Change_Due DECIMAL(10,2) NULL,
                    Medical_Aid_Reference VARCHAR(50) NULL
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (12, 'Create Payments table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 12 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 13: Wishlist table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 13)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Wishlist')
            BEGIN
                CREATE TABLE Wishlist (
                    Wishlist_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cust_ID INT NOT NULL,
                    Created_At DATETIME DEFAULT GETDATE(),
                    Updated_At DATETIME DEFAULT GETDATE()
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (13, 'Create Wishlist table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 13 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 14: WishlistItem table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 14)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'WishlistItem')
            BEGIN
                CREATE TABLE WishlistItem (
                    WishlistItem_ID INT IDENTITY(1,1) PRIMARY KEY,
                    Cust_ID INT NOT NULL,
                    Product_ID INT NOT NULL,
                    Added_At DATETIME DEFAULT GETDATE()
                );
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (14, 'Create WishlistItem table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 14 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 15: FAQ_Categories table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 15)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FAQ_Categories')
            BEGIN
                CREATE TABLE FAQ_Categories (
                    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
                    CategoryName NVARCHAR(100) NOT NULL,
                    Description NVARCHAR(500),
                    IsActive BIT DEFAULT 1,
                    CreatedDate DATETIME DEFAULT GETDATE(),
                    UpdatedDate DATETIME DEFAULT GETDATE()
                );

                INSERT INTO FAQ_Categories (CategoryName, Description) VALUES
                    ('appointments', 'Questions about booking and managing appointments'),
                    ('payments', 'Questions about payment methods and billing'),
                    ('services', 'Questions about eye care services and procedures'),
                    ('policies', 'Questions about return, warranty, and cancellation policies'),
                    ('contact', 'Questions about contact information and business hours');
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (15, 'Create FAQ_Categories table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 15 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 16: FAQ_Items table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 16)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FAQ_Items')
            BEGIN
                CREATE TABLE FAQ_Items (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Question NVARCHAR(500) NOT NULL,
                    Answer NVARCHAR(MAX) NOT NULL,
                    Keywords NVARCHAR(500),
                    Category NVARCHAR(100),
                    Priority INT DEFAULT 2,
                    IsActive BIT DEFAULT 1,
                    CreatedDate DATETIME DEFAULT GETDATE(),
                    UpdatedDate DATETIME DEFAULT GETDATE()
                );

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
            INSERT INTO SchemaVersion (Version, Description) VALUES (16, 'Create FAQ_Items table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 16 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 17: Chat_Conversations table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 17)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Chat_Conversations')
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

                CREATE INDEX IX_Chat_Conversations_SessionId ON Chat_Conversations(SessionId);
                CREATE INDEX IX_Chat_Conversations_CreatedDate ON Chat_Conversations(CreatedDate);
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (17, 'Create Chat_Conversations table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 17 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 18: Chatbot_Feedback table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 18)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Chatbot_Feedback')
            BEGIN
                CREATE TABLE Chatbot_Feedback (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    ConversationId INT NOT NULL,
                    UserRating INT,
                    WasHelpful BIT,
                    Comments NVARCHAR(500),
                    CreatedDate DATETIME DEFAULT GETDATE()
                );

                CREATE INDEX IX_Chatbot_Feedback_ConversationId ON Chatbot_Feedback(ConversationId);
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (18, 'Create Chatbot_Feedback table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 18 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 19: Chatbot_Analytics table
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 19)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Chatbot_Analytics')
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

                CREATE INDEX IX_Chatbot_Analytics_EventName ON Chatbot_Analytics(EventName);
                CREATE INDEX IX_Chatbot_Analytics_CreatedDate ON Chatbot_Analytics(CreatedDate);
            END
            INSERT INTO SchemaVersion (Version, Description) VALUES (19, 'Create Chatbot_Analytics table');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 19 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 20: Chatbot stored procedures
    IF NOT EXISTS (SELECT * FROM SchemaVersion WHERE Version = 20)
    BEGIN
        BEGIN TRANSACTION;
        BEGIN TRY
            IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_GetFAQStats')
                DROP PROCEDURE sp_GetFAQStats;

            EXEC('
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
            END');

            IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'sp_GetChatbotMetrics')
                DROP PROCEDURE sp_GetChatbotMetrics;

            EXEC('
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
                LEFT JOIN Chat_Conversations c ON c.UserMessage LIKE ''%'' + f.Question + ''%''
                WHERE c.CreatedDate BETWEEN @StartDate AND @EndDate
                GROUP BY f.Question, f.Id
                ORDER BY TimesAsked DESC;
            END');

            INSERT INTO SchemaVersion (Version, Description) VALUES (20, 'Create chatbot stored procedures');
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 20 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Migration 21: Add Appointment_Type column
    IF NOT EXISTS (SELECT 1 FROM SchemaVersion WHERE Version = 21)
    BEGIN
        BEGIN TRY
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Appointment' AND COLUMN_NAME = 'Appointment_Type')
            BEGIN
                ALTER TABLE Appointment ADD Appointment_Type NVARCHAR(100) NULL;
            END

            INSERT INTO SchemaVersion (Version, Description) VALUES (21, 'Add Appointment_Type column to Appointment table');
            SET @AppliedCount = @AppliedCount + 1;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrMsg = ERROR_MESSAGE();
            RAISERROR('Migration 21 failed: %s', 16, 1, @ErrMsg);
            RETURN;
        END CATCH
    END

    -- Return summary of applied migrations
    SELECT Version, Description, AppliedDate
    FROM SchemaVersion
    ORDER BY Version;
END
