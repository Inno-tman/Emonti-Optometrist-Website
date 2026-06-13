using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EmontiOptometrist.Web.Data;
using EmontiOptometrist.Web.Services;
using EmontiOptometrist.Web.Models;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<FAQDatabase>();
builder.Services.AddSingleton<AIChatService>();
builder.Services.AddSingleton<ProductDatabase>();
builder.Services.AddSingleton<CartDatabase>();
builder.Services.AddSingleton<WishlistDatabase>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Admin", "Staff" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var adminEmail = "admin@emonti.com";
    var adminPassword = "Admin";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
            await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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
        {
            return fallback.Value;
        }
    }

    return "I'm sorry, I couldn't find a specific answer to your question. Please contact us directly at 076 463 1930 or email emontioptom@gmail.com for assistance. You can also visit our Help page for more information.";
}
