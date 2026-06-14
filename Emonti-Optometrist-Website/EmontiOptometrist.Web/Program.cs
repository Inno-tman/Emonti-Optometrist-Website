using EmontiOptometrist.Web.Services;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
// On Azure App Service, use persistent path so DB survives redeploys
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME")))
{
    var home = Environment.GetEnvironmentVariable("HOME") ?? "/home";
    connectionString = $"DataSource={home}/site/wwwroot/app.db;Cache=Shared";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;
}

builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<DatabaseInit>();
builder.Services.AddSingleton<FAQDatabase>();
builder.Services.AddSingleton<AIChatService>();
builder.Services.AddSingleton<ProductDatabase>();
builder.Services.AddSingleton<CartDatabase>();
builder.Services.AddSingleton<WishlistDatabase>();
builder.Services.AddSingleton<OrderDatabase>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInit = scope.ServiceProvider.GetRequiredService<DatabaseInit>();
    dbInit.Initialize();

    SeedAdminAndStaff(connectionString);
}

if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.MapPost("/api/chatbot/chat", async (HttpContext http) =>
{
    try
    {
        var form = await http.Request.ReadFormAsync();
        var userMessage = form["message"].FirstOrDefault();
        var sessionId = form["sessionId"].FirstOrDefault() ?? Guid.NewGuid().ToString();

        if (string.IsNullOrEmpty(userMessage))
        {
            return Results.Json(new { success = false, message = "Message is required" });
        }

        var faqDb = http.RequestServices.GetRequiredService<FAQDatabase>();
        var aiService = http.RequestServices.GetRequiredService<AIChatService>();

        var bestMatch = faqDb.FindBestMatch(userMessage);
        string botResponse;
        bool aiPowered = false;

        if (bestMatch != null)
        {
            botResponse = bestMatch.Answer;
        }
        else if (aiService.IsConfigured)
        {
            var faqs = faqDb.GetActiveFAQs();
            var aiResponse = await aiService.GetAIResponse(userMessage, faqs);
            if (!string.IsNullOrEmpty(aiResponse))
            {
                botResponse = aiResponse;
                aiPowered = true;
            }
            else
            {
                botResponse = GetFallbackResponse(userMessage.ToLower());
            }
        }
        else
        {
            botResponse = GetFallbackResponse(userMessage.ToLower());
        }

        faqDb.LogConversation(sessionId, userMessage, botResponse, aiPowered ? 0.9f : 0.2f, 0);

        return Results.Json(new
        {
            success = true,
            message = botResponse,
            aiPowered = aiPowered,
            sessionId = sessionId
        });
    }
    catch (Exception ex)
    {
        return Results.Json(new { success = false, message = "Internal server error", error = ex.Message });
    }
});

app.Run();

static void SeedAdminAndStaff(string connectionString)
{
    using var conn = new SqliteConnection(connectionString);
    conn.Open();

    // Seed admin user in Staff table
    using var checkAdmin = conn.CreateCommand();
    checkAdmin.CommandText = "SELECT COUNT(*) FROM Staff WHERE Staff_Role = 'Admin'";
    if ((long)checkAdmin.ExecuteScalar()! > 0) return;

    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
        INSERT OR IGNORE INTO Staff (Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Password, Staff_Role)
        VALUES (@id, @name, @surname, @email, @password, @role)";
    cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
    cmd.Parameters.AddWithValue("@name", "Admin");
    cmd.Parameters.AddWithValue("@surname", "User");
    cmd.Parameters.AddWithValue("@email", "admin@emonti.com");
    cmd.Parameters.AddWithValue("@password", "Admin");
    cmd.Parameters.AddWithValue("@role", "Admin");
    cmd.ExecuteNonQuery();

    // Seed staff user
    cmd.Parameters.Clear();
    cmd.CommandText = @"
        INSERT OR IGNORE INTO Staff (Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Password, Staff_Role)
        VALUES (@id, @name, @surname, @email, @password, @role)";
    cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
    cmd.Parameters.AddWithValue("@name", "Staff");
    cmd.Parameters.AddWithValue("@surname", "User");
    cmd.Parameters.AddWithValue("@email", "staff@emonti.com");
    cmd.Parameters.AddWithValue("@password", "Staff");
    cmd.Parameters.AddWithValue("@role", "Staff");
    cmd.ExecuteNonQuery();
}

static string GetFallbackResponse(string message)
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
            return fallback.Value;
    }

    return "I'm sorry, I couldn't find a specific answer to your question. Please contact us directly at 076 463 1930 or email emontioptom@gmail.com for assistance. You can also visit our Help page for more information.";
}
