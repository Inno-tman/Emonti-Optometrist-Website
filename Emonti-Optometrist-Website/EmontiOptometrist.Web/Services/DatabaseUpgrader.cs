using System.Data;
using Microsoft.Data.SqlClient;

namespace EmontiOptometrist.Web.Services
{
    public class DatabaseUpgrader
    {
        private readonly string _connectionString;
        private readonly string _contentRootPath;

        public DatabaseUpgrader(IConfiguration configuration, IWebHostEnvironment environment)
        {
            var connStr = configuration.GetConnectionString("ProductConnection");
            if (!string.IsNullOrEmpty(connStr))
            {
                var builder = new SqlConnectionStringBuilder(connStr);
                if (builder.ConnectTimeout > 3) builder.ConnectTimeout = 3;
                _connectionString = builder.ConnectionString;
            }
            else
            {
                _connectionString = "";
            }
            _contentRootPath = environment.ContentRootPath;
        }

        public void Run()
        {
            string sqlScriptPath = Path.Combine(_contentRootPath, "UpgradeDatabase.sql");
            string createSpSql = File.ReadAllText(sqlScriptPath);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SqlCommand(createSpSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new SqlCommand("sp_UpgradeDatabase", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
