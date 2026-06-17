using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages.Admin;

public class QueryDbModel : PageModel
{
    private readonly string _connectionString;

    public QueryDbModel(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";
    }

    [BindProperty]
    public string SqlQuery { get; set; } = "";

    public string? ErrorMessage { get; set; }
    public List<string> Columns { get; set; } = new();
    public List<List<string>> Rows { get; set; } = new();
    public int RowCount { get; set; }
    public double ElapsedMs { get; set; }

    public IActionResult OnGet()
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!AuthSession.IsStaffLoggedInCheck(HttpContext))
            return RedirectToPage("/Login");

        if (string.IsNullOrWhiteSpace(SqlQuery))
        {
            ErrorMessage = "Please enter a SQL query.";
            return Page();
        }

        var trimmed = SqlQuery.TrimStart().ToUpperInvariant();
        if (!trimmed.StartsWith("SELECT") && !trimmed.StartsWith("PRAGMA") && !trimmed.StartsWith("WITH"))
        {
            ErrorMessage = "Only SELECT, PRAGMA, and WITH queries are allowed (read-only).";
            return Page();
        }

        try
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = SqlQuery;

            var sw = System.Diagnostics.Stopwatch.StartNew();
            using var reader = cmd.ExecuteReader();
            sw.Stop();
            ElapsedMs = sw.Elapsed.TotalMilliseconds;

            for (int i = 0; i < reader.FieldCount; i++)
                Columns.Add(reader.GetName(i));

            while (reader.Read())
            {
                var row = new List<string>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row.Add(reader.IsDBNull(i) ? "NULL" : reader.GetValue(i).ToString() ?? "");
                Rows.Add(row);
            }

            RowCount = Rows.Count;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }

        return Page();
    }
}
