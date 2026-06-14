using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

public class MigrateModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MigrateModel> _logger;

    public string? LogOutput { get; set; }

    public MigrateModel(IConfiguration configuration, ILogger<MigrateModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!AuthSession.IsAdmin(HttpContext))
            return RedirectToPage("/Login");

        var log = new List<string>();
        string sqliteConn = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
        string sqlServerConn = _configuration["LegacyConnection"] ?? "Data Source=146.230.177.46;Initial Catalog=WstGrp5;User ID=WstGrp5;Password=87ad5;Encrypt=False";

        try
        {
            using var sqlite = new SqliteConnection(sqliteConn);
            sqlite.Open();

            using var sqlServer = new SqlConnection(sqlServerConn);
            sqlServer.Open();
            log.Add("Connected to SQL Server successfully.");

            MigrateTable(sqlServer, sqlite, "customer", "Cust_ID", log);
            MigrateTable(sqlServer, sqlite, "Staff", "Staff_ID", log);
            MigrateProducts(sqlServer, sqlite, log);
            MigrateTable(sqlServer, sqlite, "Cart", "Cart_ID", log);
            MigrateTable(sqlServer, sqlite, "CartItem", "CartItem_ID", log);
            MigrateTable(sqlServer, sqlite, "[Order]", "OrderID", log);
            MigrateTable(sqlServer, sqlite, "OrderItems", "OrderItemID", log);
            MigrateTable(sqlServer, sqlite, "Wishlist", "Wishlist_ID", log);
            MigrateTable(sqlServer, sqlite, "WishlistItem", "WishlistItem_ID", log);
            MigrateTable(sqlServer, sqlite, "Appointment", "Appointment_ID", log);
            MigrateTable(sqlServer, sqlite, "tblTime", "TimeID", log);
            MigrateTable(sqlServer, sqlite, "Payments", "Payment_ID", log);
            MigrateTable(sqlServer, sqlite, "FAQ_Items", "Id", log);
            MigrateTable(sqlServer, sqlite, "Chat_Conversations", "Id", log);
            MigrateTable(sqlServer, sqlite, "Chatbot_Feedback", "Id", log);

            log.Add("");
            log.Add("Migration completed successfully.");
        }
        catch (Exception ex)
        {
            log.Add($"ERROR: {ex.Message}");
            _logger.LogError(ex, "Migration failed");
        }

        LogOutput = string.Join("\n", log);
        return Page();
    }

    private void MigrateTable(SqlConnection src, SqliteConnection dst, string table, string pkColumn, List<string> log)
    {
        try
        {
            var srcCmd = src.CreateCommand();
            srcCmd.CommandText = $"SELECT * FROM {table}";
            using var reader = srcCmd.ExecuteReader();

            var schema = reader.GetColumnSchema();
            var columns = schema.Select(c => c.ColumnName).ToArray();
            if (columns.Length == 0) { log.Add($"{table}: no columns"); return; }

            var dstCmd = dst.CreateCommand();
            dstCmd.CommandText = $"SELECT COUNT(*) FROM {table}";
            var existing = (long)dstCmd.ExecuteScalar();
            if (existing > 0)
            {
                log.Add($"{table}: SKIPPED ({existing} records already exist)");
                return;
            }

            int count = 0;
            while (reader.Read())
            {
                var names = string.Join(", ", columns.Select(c => $"[{c}]"));
                var vals = string.Join(", ", columns.Select(c => $"@{c}"));
                dstCmd.CommandText = $"INSERT INTO {table} ({names}) VALUES ({vals})";
                dstCmd.Parameters.Clear();

                foreach (var col in columns)
                {
                    var val = reader[col];
                    dstCmd.Parameters.AddWithValue($"@{col}", val == DBNull.Value ? DBNull.Value : val);
                }
                dstCmd.ExecuteNonQuery();
                count++;
            }

            log.Add($"{table}: {count} rows migrated");
        }
        catch (Exception ex)
        {
            log.Add($"{table}: ERROR - {ex.Message}");
        }
    }

    private void MigrateProducts(SqlConnection src, SqliteConnection dst, List<string> log)
    {
        try
        {
            var srcCmd = src.CreateCommand();
            srcCmd.CommandText = "SELECT * FROM Products2";
            using var reader = srcCmd.ExecuteReader();
            var schema = reader.GetColumnSchema();
            var columns = schema.Select(c => c.ColumnName).ToArray();

            var dstCmd = dst.CreateCommand();
            dstCmd.CommandText = "SELECT COUNT(*) FROM Products2";
            if ((long)dstCmd.ExecuteScalar() > 0)
            {
                log.Add("Products2: SKIPPED (records already exist)");
                return;
            }

            int count = 0;
            while (reader.Read())
            {
                var names = string.Join(", ", columns.Select(c => $"[{c}]"));
                var vals = string.Join(", ", columns.Select(c => $"@{c}"));
                dstCmd.CommandText = $"INSERT INTO Products2 ({names}) VALUES ({vals})";
                dstCmd.Parameters.Clear();
                foreach (var col in columns)
                {
                    var val = reader[col];
                    dstCmd.Parameters.AddWithValue($"@{col}", val == DBNull.Value ? DBNull.Value : val);
                }
                dstCmd.ExecuteNonQuery();
                count++;
            }
            log.Add($"Products2: {count} rows migrated");
        }
        catch (Exception ex)
        {
            log.Add($"Products2: ERROR - {ex.Message}");
        }
    }
}