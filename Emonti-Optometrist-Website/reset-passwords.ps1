# Reset passwords in app.db
param(
    [string]$Email,
    [string]$Password = "Password123",
    [ValidateSet("customer","staff","all")]
    [string]$Type = "all"
)

$root = $PSScriptRoot
$db = Join-Path $root "EmontiOptometrist.Web\app.db"
if (!(Test-Path $db)) { Write-Host "app.db not found" -ForegroundColor Red; exit 1 }

$tmp = Join-Path $root "_reset_tool"
Remove-Item -Recurse -Force $tmp -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $tmp -Force | Out-Null

@"
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Data.Sqlite" Version="8.0.26" />
  </ItemGroup>
</Project>
"@ | Set-Content (Join-Path $tmp "tool.csproj") -NoNewline

$emailArg = if ($Email) { '"' + $Email + '"' } else { 'null' }

@"
using Microsoft.Data.Sqlite;

string db = @"$db";
string? email = $emailArg;
string pwd = "$Password";
string type = "$Type";

using var conn = new SqliteConnection("DataSource=" + db);
conn.Open();

static List<string> GetCols(SqliteConnection c, string t)
{
    var r = new List<string>();
    using var cmd = c.CreateCommand();
    cmd.CommandText = "PRAGMA table_info(" + t + ")";
    using var rd = cmd.ExecuteReader();
    while (rd.Read()) r.Add(rd["name"]!.ToString()!);
    return r;
}

string[][] tables = [
    ["customer", "Customer_DOB","Customer_Gender","Customer_Address","Medical_Aid","Medical_Aid_Number","Main_Member_Name","Main_Member_Surname","Main_Member_ID","Province","Is_Archive","Customer_Password"],
    ["Staff", "Staff_Email","Staff_Password","Staff_Role"]
];
string[][] defs = [
    ["TEXT","TEXT","TEXT","TEXT","TEXT","TEXT","TEXT","TEXT","TEXT","INTEGER DEFAULT 0","TEXT"],
    ["TEXT","TEXT","TEXT DEFAULT 'Staff'"]
];

for (int t = 0; t < tables.Length; t++)
{
    var existing = GetCols(conn, tables[t][0]);
    for (int c = 1; c < tables[t].Length; c++)
    {
        if (!existing.Contains(tables[t][c]))
        {
            using var a = conn.CreateCommand();
            a.CommandText = "ALTER TABLE " + tables[t][0] + " ADD COLUMN " + tables[t][c] + " " + defs[t][c - 1];
            a.ExecuteNonQuery();
            Console.WriteLine("+ Added " + tables[t][0] + "." + tables[t][c]);
        }
    }
}

if (type == "customer" || type == "all")
{
    if (email != null)
    {
        using var c = conn.CreateCommand();
        c.CommandText = "UPDATE customer SET Customer_Password=@p WHERE Customer_Email=@e";
        c.Parameters.AddWithValue("@p", pwd); c.Parameters.AddWithValue("@e", email);
        Console.WriteLine((c.ExecuteNonQuery() > 0)
            ? "OK Customer " + email + " -> '" + pwd + "'"
            : "No customer " + email);
    }
    else
    {
        using var c = conn.CreateCommand();
        c.CommandText = "UPDATE customer SET Customer_Password=@p WHERE Customer_Password IS NULL OR Customer_Password=''";
        c.Parameters.AddWithValue("@p", pwd);
        Console.WriteLine("Updated " + c.ExecuteNonQuery() + " customers to '" + pwd + "'");
    }
}

if (type == "staff" || type == "all")
{
    if (email != null)
    {
        using var c = conn.CreateCommand();
        c.CommandText = "UPDATE Staff SET Staff_Password=@p WHERE Staff_Email=@e";
        c.Parameters.AddWithValue("@p", pwd); c.Parameters.AddWithValue("@e", email);
        Console.WriteLine((c.ExecuteNonQuery() > 0)
            ? "OK Staff " + email + " -> '" + pwd + "'"
            : "No staff " + email);
    }
    else
    {
        using var c1 = conn.CreateCommand();
        c1.CommandText = "UPDATE Staff SET Staff_Password='Admin' WHERE Staff_Role='Admin'";
        Console.WriteLine("Reset " + c1.ExecuteNonQuery() + " admin(s) to 'Admin'");
        using var c2 = conn.CreateCommand();
        c2.CommandText = "UPDATE Staff SET Staff_Password='Staff' WHERE Staff_Role='Staff'";
        Console.WriteLine("Reset " + c2.ExecuteNonQuery() + " staff to 'Staff'");
    }
}
"@ | Set-Content (Join-Path $tmp "Program.cs") -NoNewline

Push-Location $tmp
dotnet run 2>&1 | ForEach-Object { $_ }
Pop-Location
Remove-Item -Recurse -Force $tmp -ErrorAction SilentlyContinue
