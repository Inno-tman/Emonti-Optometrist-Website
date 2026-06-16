using Microsoft.Data.Sqlite;
using System.Text.Json;

namespace EmontiOptometrist.Web.Services;

public class DatabaseInit
{
    private readonly string _connectionString;

    public DatabaseInit(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    public void Initialize()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();

        using var cmd = conn.CreateCommand();

        cmd.CommandText = "PRAGMA journal_mode=WAL";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "PRAGMA foreign_keys=ON";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Products2 (
                Product_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Product_Name TEXT NOT NULL,
                Product_Brand TEXT,
                Product_Category TEXT,
                Product_Price REAL NOT NULL,
                QuantityOnHand INTEGER NOT NULL DEFAULT 0,
                Picture1 TEXT,
                Picture2 TEXT,
                Product_Description TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS customer (
                Cust_ID TEXT PRIMARY KEY,
                Customer_Name TEXT,
                Customer_Surname TEXT,
                Customer_DOB TEXT,
                Customer_Gender TEXT,
                Customer_Email TEXT,
                Customer_Phone TEXT,
                Customer_Address TEXT,
                Medical_Aid TEXT,
                Medical_Aid_Number TEXT,
                Main_Member_Name TEXT,
                Main_Member_Surname TEXT,
                Main_Member_ID TEXT,
                Street_Number TEXT,
                Street_Name TEXT,
                Complex_Name TEXT,
                Unit_Number TEXT,
                City TEXT,
                Province TEXT,
                Postal_Code TEXT,
                Is_Archive INTEGER DEFAULT 0,
                Customer_Password TEXT,
                Cust_FirstName TEXT,
                Cust_LastName TEXT,
                Cust_Email TEXT,
                Cust_Phone TEXT,
                Cust_Address TEXT,
                Last_Login TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Cart (
                Cart_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cust_ID TEXT NOT NULL,
                Status TEXT NOT NULL DEFAULT 'Active',
                Created_At TEXT NOT NULL,
                Updated_At TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS CartItem (
                CartItem_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cart_ID INTEGER NOT NULL,
                Product_ID INTEGER NOT NULL,
                Quantity INTEGER NOT NULL,
                Price REAL NOT NULL,
                FOREIGN KEY (Cart_ID) REFERENCES Cart(Cart_ID),
                FOREIGN KEY (Product_ID) REFERENCES Products2(Product_ID)
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS [Order] (
                OrderID INTEGER PRIMARY KEY AUTOINCREMENT,
                CustID TEXT NOT NULL,
                Order_Date TEXT NOT NULL,
                Order_Total REAL NOT NULL,
                Order_Status TEXT NOT NULL,
                Delivery_Address TEXT,
                Payment_Method TEXT,
                Payment_Status TEXT,
                Payment_Date TEXT,
                Order_Number TEXT,
                Notes TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS OrderItems (
                OrderItemID INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderID INTEGER NOT NULL,
                Product_ID INTEGER NOT NULL,
                Product_Name TEXT,
                Product_Brand TEXT,
                Product_Category TEXT,
                Quantity INTEGER NOT NULL,
                Unit_Price REAL NOT NULL,
                Subtotal REAL NOT NULL,
                FOREIGN KEY (OrderID) REFERENCES [Order](OrderID)
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Wishlist (
                Wishlist_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cust_ID INTEGER NOT NULL,
                Created_At TEXT NOT NULL,
                Updated_At TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS WishlistItem (
                WishlistItem_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cust_ID INTEGER NOT NULL,
                Product_ID INTEGER NOT NULL,
                Added_At TEXT NOT NULL,
                FOREIGN KEY (Product_ID) REFERENCES Products2(Product_ID)
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Appointment (
                Appointment_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cust_ID TEXT NOT NULL,
                Staff_ID TEXT,
                Appointment_Date TEXT NOT NULL,
                AppointmentTimeID TEXT,
                Appoinment_Status TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE UNIQUE INDEX IF NOT EXISTS IX_Appointment_Slot
            ON Appointment(Staff_ID, Appointment_Date, AppointmentTimeID)
            WHERE Appoinment_Status != 'Cancelled'";
        try { cmd.ExecuteNonQuery(); } catch { }

        cmd.CommandText = @"
            CREATE UNIQUE INDEX IF NOT EXISTS IX_Appointment_CustomerDay
            ON Appointment(Cust_ID, Appointment_Date)
            WHERE Appoinment_Status != 'Cancelled'";
        try { cmd.ExecuteNonQuery(); } catch { }

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Staff (
                Staff_ID TEXT PRIMARY KEY,
                Staff_Name TEXT,
                Staff_Surname TEXT,
                Staff_Email TEXT,
                Staff_Password TEXT,
                Staff_Role TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS tblTime (
                TimeID TEXT PRIMARY KEY,
                Timeslot TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS BlockedTimeslots (
                Blocked_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Staff_ID TEXT NOT NULL,
                Blocked_Date TEXT NOT NULL,
                TimeID TEXT NOT NULL,
                Created_At TEXT DEFAULT (datetime('now'))
            )";
        cmd.ExecuteNonQuery();

        try { cmd.CommandText = "ALTER TABLE BlockedTimeslots ADD COLUMN Created_At TEXT DEFAULT (datetime('now'))"; cmd.ExecuteNonQuery(); } catch { }

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Messages (
                Message_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Conversation_ID INTEGER NOT NULL,
                Cust_ID TEXT NOT NULL,
                Staff_ID TEXT,
                Sender_Role TEXT NOT NULL,
                Subject TEXT,
                Message_Body TEXT NOT NULL,
                Created_At TEXT NOT NULL DEFAULT (datetime('now', 'localtime')),
                Is_Read INTEGER DEFAULT 0
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Payments (
                Payment_ID INTEGER PRIMARY KEY AUTOINCREMENT,
                Cust_ID INTEGER,
                Order_ID INTEGER,
                Appointment_ID INTEGER,
                Transaction_Number TEXT,
                Payment_Date TEXT,
                Consultation_Fee REAL,
                Order_Payment REAL,
                Total_Payable REAL,
                Payment_Method TEXT,
                Amount_Received REAL,
                Change_Due REAL,
                Created_Date TEXT,
                Created_By TEXT,
                Payment_Status TEXT,
                Medical_Aid_Amount REAL,
                Patient_Portion_Amount REAL,
                Patient_Payment_Method TEXT,
                Patient_Amount_Received REAL,
                Patient_Change_Due REAL,
                Medical_Aid_Reference TEXT
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS FAQ_Items (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Question TEXT NOT NULL,
                Answer TEXT NOT NULL,
                Keywords TEXT,
                Category TEXT,
                Priority INTEGER NOT NULL DEFAULT 2,
                IsActive INTEGER NOT NULL DEFAULT 1,
                CreatedDate TEXT NOT NULL,
                UpdatedDate TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Chat_Conversations (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                SessionId TEXT NOT NULL,
                UserMessage TEXT NOT NULL,
                BotResponse TEXT NOT NULL,
                ConfidenceScore REAL,
                ResponseTimeMs INTEGER NOT NULL DEFAULT 0,
                CreatedDate TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();

        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Chatbot_Feedback (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ConversationId INTEGER NOT NULL,
                UserRating INTEGER,
                WasHelpful INTEGER,
                Comments TEXT,
                CreatedDate TEXT NOT NULL
            )";
        cmd.ExecuteNonQuery();

        SeedProducts(cmd);

        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM pragma_table_info('customer') WHERE name='Last_Login'";
        var hasLastLogin = (long)cmd.ExecuteScalar() > 0;
        if (!hasLastLogin)
        {
            cmd.CommandText = "ALTER TABLE customer ADD COLUMN Last_Login TEXT";
            cmd.ExecuteNonQuery();
        }

        cmd.Parameters.Clear();
        cmd.CommandText = @"
            UPDATE customer SET Last_Login = DATE('now', '-' || CAST(ABS(RANDOM() % 90) AS TEXT) || ' days', 'localtime')
            WHERE Last_Login IS NULL OR Last_Login = ''";
        cmd.ExecuteNonQuery();

        SeedStaff(cmd);
        SeedFAQ(cmd);
        SeedTimeSlots(cmd);
    }

    private void SeedStaff(SqliteCommand cmd)
    {
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM Staff";
        if ((long)cmd.ExecuteScalar() > 0) return;

        var staff = new[]
        {
            new { Id = "A001", Name = "Admin", Email = "admin@emonti.com", Password = "Admin", Role = "Admin" },
            new { Id = "S001", Name = "Staff", Email = "staff@emonti.com", Password = "Staff", Role = "Staff" }
        };

        foreach (var s in staff)
        {
            cmd.CommandText = "INSERT INTO Staff (Staff_ID, Staff_Name, Staff_Email, Staff_Password, Staff_Role) VALUES (@Id, @Name, @Email, @Password, @Role)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id", s.Id);
            cmd.Parameters.AddWithValue("@Name", s.Name);
            cmd.Parameters.AddWithValue("@Email", s.Email);
            cmd.Parameters.AddWithValue("@Password", s.Password);
            cmd.Parameters.AddWithValue("@Role", s.Role);
            cmd.ExecuteNonQuery();
        }
    }

    private void SeedFAQ(SqliteCommand cmd)
    {
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM FAQ_Items";
        if ((long)cmd.ExecuteScalar() > 0) return;

        var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var faqs = new[]
        {
            new { Q = "What are your opening hours?", A = "We are open Monday to Friday from 8:00 AM to 5:00 PM, and Saturdays from 8:00 AM to 1:00 PM. We are closed on Sundays and public holidays.", Cat = "General", Kw = "hours,open,closing,time,schedule", Pri = 1 },
            new { Q = "Do I need a referral to book an eye exam?", A = "No, you do not need a referral. You can book an eye exam directly with us.", Cat = "Appointments", Kw = "referral,eye exam,appointment,book", Pri = 1 },
            new { Q = "How often should I have my eyes tested?", A = "We recommend having your eyes tested every 1-2 years, or as advised by your optometrist.", Cat = "General", Kw = "often,frequent,test,examination,checkup", Pri = 2 },
            new { Q = "What types of payment do you accept?", A = "We accept cash, debit/credit cards, and most major medical aid schemes.", Cat = "Billing", Kw = "payment,cash,card,medical aid,accept", Pri = 1 },
            new { Q = "Do you accept medical aid?", A = "Yes, we accept most major medical aid schemes. Please bring your medical aid details to your appointment.", Cat = "Billing", Kw = "medical aid,insurance,cover,accept", Pri = 1 },
            new { Q = "Can I buy contact lenses online?", A = "Yes, you can browse and purchase contact lenses through our online shop. You will need a valid prescription.", Cat = "Shop", Kw = "contact lenses,buy,online,purchase,order", Pri = 2 },
            new { Q = "How long does it take to get my glasses?", A = "Standard glasses typically take 7-14 working days. Express options may be available upon request.", Cat = "Products", Kw = "glasses,time,wait,delivery,ready", Pri = 2 },
            new { Q = "What is your return policy?", A = "We accept returns within 14 days of purchase for unused items in original packaging. Prescription lenses are custom-made and cannot be returned.", Cat = "Shop", Kw = "return,refund,exchange,policy", Pri = 2 },
            new { Q = "Do you offer eye tests for children?", A = "Yes, we offer comprehensive eye examinations for children of all ages.", Cat = "Appointments", Kw = "children,kids,child,eye test,examination", Pri = 2 },
            new { Q = "What should I bring to my appointment?", A = "Please bring your ID, medical aid details, current glasses/contact lenses, and a list of any medications you are taking.", Cat = "Appointments", Kw = "bring,need,appointment,prepare", Pri = 1 }
        };

        foreach (var f in faqs)
        {
            cmd.CommandText = "INSERT INTO FAQ_Items (Question, Answer, Keywords, Category, Priority, IsActive, CreatedDate, UpdatedDate) VALUES (@Q, @A, @Kw, @Cat, @Pri, 1, @Now, @Now)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Q", f.Q);
            cmd.Parameters.AddWithValue("@A", f.A);
            cmd.Parameters.AddWithValue("@Kw", f.Kw);
            cmd.Parameters.AddWithValue("@Cat", f.Cat);
            cmd.Parameters.AddWithValue("@Pri", f.Pri);
            cmd.Parameters.AddWithValue("@Now", now);
            cmd.ExecuteNonQuery();
        }
    }

    private void SeedTimeSlots(SqliteCommand cmd)
    {
        cmd.Parameters.Clear();
        cmd.CommandText = "SELECT COUNT(*) FROM tblTime";
        if ((long)cmd.ExecuteScalar() > 0) return;

        var slots = new[] { "08:00", "08:30", "09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00", "14:30", "15:00", "15:30" };

        foreach (var s in slots)
        {
            cmd.CommandText = "INSERT INTO tblTime (TimeID, Timeslot) VALUES (@Id, @Slot)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Id", s.Replace(":", ""));
            cmd.Parameters.AddWithValue("@Slot", s);
            cmd.ExecuteNonQuery();
        }
    }

    private void SeedProducts(SqliteCommand cmd)
    {
        cmd.CommandText = "SELECT COUNT(*) FROM Products2";
        var count = (long)cmd.ExecuteScalar();
        if (count > 0) return;

        var products = new[]
        {
            new { Name = "Transitions Optical Lens", Brand = "Transitions", Category = "Lenses", Price = 450.00m, Stock = 50, Image = "ID_1_Picture1.png" },
            new { Name = "Blue Light Blocking Lens", Brand = "Essilor", Category = "Lenses", Price = 350.00m, Stock = 40, Image = "ID_2_Picture1.png" },
            new { Name = "Polarized Sunglass Lens", Brand = "Maui Jim", Category = "Lenses", Price = 550.00m, Stock = 30, Image = "ID_3_Picture1.png" },
            new { Name = "Progressive Lens", Brand = "Zeiss", Category = "Lenses", Price = 750.00m, Stock = 25, Image = "ID_4_Picture1.png" },
            new { Name = "Anti-Reflective Coating Lens", Brand = "Hoya", Category = "Lenses", Price = 250.00m, Stock = 60, Image = "ID_5_Picture1.png" },
            new { Name = "Photochromic Lens", Brand = "Transitions", Category = "Lenses", Price = 500.00m, Stock = 35, Image = "ID_6_Picture1.png" },
            new { Name = "High-Index Lens", Brand = "Essilor", Category = "Lenses", Price = 600.00m, Stock = 20, Image = "ID_7_Picture1.png" },
            new { Name = "Polycarbonate Lens", Brand = "Shamir", Category = "Lenses", Price = 300.00m, Stock = 45, Image = "ID_8_Picture1.png" },
            new { Name = "Aspheric Lens", Brand = "Zeiss", Category = "Lenses", Price = 400.00m, Stock = 30, Image = "ID_9_Picture1.png" },
            new { Name = "Bifocal Lens", Brand = "Visioneering", Category = "Lenses", Price = 480.00m, Stock = 20, Image = "ID_10_Picture1.png" },
            new { Name = "Ray-Ban Aviator", Brand = "Ray-Ban", Category = "Sunglasses", Price = 250.00m, Stock = 20, Image = "ID_11_Picture1.png" },
            new { Name = "Oakley Holbrook", Brand = "Oakley", Category = "Sunglasses", Price = 200.00m, Stock = 25, Image = "ID_12_Picture1.png" },
            new { Name = "Maui Jim Mavericks", Brand = "Maui Jim", Category = "Sunglasses", Price = 300.00m, Stock = 15, Image = "ID_13_Picture1.png" },
            new { Name = "Polaroid PID 1012", Brand = "Polaroid", Category = "Sunglasses", Price = 180.00m, Stock = 30, Image = "ID_14_Picture1.png" },
            new { Name = "Tom Ford FT0415", Brand = "Tom Ford", Category = "Sunglasses", Price = 400.00m, Stock = 10, Image = "ID_15_Picture1.png" },
            new { Name = "Persol PO0714", Brand = "Persol", Category = "Sunglasses", Price = 350.00m, Stock = 12, Image = "ID_16_Picture1.png" },
            new { Name = "Vogue VO4106S", Brand = "Vogue", Category = "Sunglasses", Price = 150.00m, Stock = 35, Image = "ID_17_Picture1.png" },
            new { Name = "Arnette AN4301", Brand = "Arnette", Category = "Sunglasses", Price = 130.00m, Stock = 40, Image = "ID_18_Picture1.png" },
            new { Name = "Dolce & Gabbana DG2240", Brand = "Dolce & Gabbana", Category = "Sunglasses", Price = 450.00m, Stock = 8, Image = "ID_19_Picture1.png" },
            new { Name = "Prada PR17WS", Brand = "Prada", Category = "Sunglasses", Price = 420.00m, Stock = 10, Image = "ID_20_Picture1.png" },
            new { Name = "Lens Cleaning Kit", Brand = "Zeiss", Category = "Accessories", Price = 15.00m, Stock = 100, Image = "ID_21_Picture1.png" },
            new { Name = "Eyeglass Case Hard Shell", Brand = "Generic", Category = "Accessories", Price = 12.00m, Stock = 80, Image = "ID_22_Picture1.png" },
            new { Name = "Microfiber Cleaning Cloth", Brand = "Generic", Category = "Accessories", Price = 5.00m, Stock = 150, Image = "ID_23_Picture1.png" },
            new { Name = "Sunglasses Clip-On", Brand = "Generic", Category = "Accessories", Price = 25.00m, Stock = 60, Image = "ID_24_Picture1.png" },
            new { Name = "Anti-Fog Spray", Brand = "Hoya", Category = "Accessories", Price = 10.00m, Stock = 90, Image = "ID_25_Picture1.png" },
            new { Name = "Nose Pad Kit", Brand = "Generic", Category = "Accessories", Price = 8.00m, Stock = 120, Image = "ID_26_Picture1.png" },
            new { Name = "Eyeglass Repair Kit", Brand = "Generic", Category = "Accessories", Price = 7.00m, Stock = 75, Image = "ID_27_Picture1.png" },
            new { Name = "Retro Round Frame", Brand = "Warby Parker", Category = "Eyeglass Frames", Price = 145.00m, Stock = 30, Image = "ID_28_Picture1.png" },
            new { Name = "Classic Square Frame", Brand = "Ray-Ban", Category = "Eyeglass Frames", Price = 180.00m, Stock = 25, Image = "ID_29_Picture1.png" },
            new { Name = "Cat-Eye Frame", Brand = "Kate Spade", Category = "Eyeglass Frames", Price = 200.00m, Stock = 20, Image = "ID_30_Picture1.png" },
            new { Name = "Titanium Frame", Brand = "Lindberg", Category = "Eyeglass Frames", Price = 350.00m, Stock = 15, Image = "ID_31_Picture1.png" },
            new { Name = "Half-Rim Frame", Brand = "Oakley", Category = "Eyeglass Frames", Price = 220.00m, Stock = 22, Image = "ID_32_Picture1.png" },
            new { Name = "Full-Rim Acetate Frame", Brand = "Tom Ford", Category = "Eyeglass Frames", Price = 280.00m, Stock = 18, Image = "ID_33_Picture1.png" },
            new { Name = "Flexible Hinge Frame", Brand = "Flexon", Category = "Eyeglass Frames", Price = 160.00m, Stock = 28, Image = "ID_34_Picture1.png" },
            new { Name = "Geometric Frame", Brand = "Dita", Category = "Eyeglass Frames", Price = 380.00m, Stock = 10, Image = "ID_35_Picture1.png" }
        };

        foreach (var p in products)
        {
            cmd.CommandText = @"
                INSERT INTO Products2 (Product_Name, Product_Brand, Product_Category, Product_Price, QuantityOnHand, Picture1)
                VALUES (@Name, @Brand, @Category, @Price, @Stock, @Image)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Name", p.Name);
            cmd.Parameters.AddWithValue("@Brand", p.Brand);
            cmd.Parameters.AddWithValue("@Category", p.Category);
            cmd.Parameters.AddWithValue("@Price", p.Price);
            cmd.Parameters.AddWithValue("@Stock", p.Stock);
            cmd.Parameters.AddWithValue("@Image", p.Image);
            cmd.ExecuteNonQuery();
        }
    }
}
