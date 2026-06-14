using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;

namespace Emonti_Optometrist_Website
{
    public static class DatabaseUpgrader
    {
        private static string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        public static void Run()
        {
            string sqlScriptPath = HostingEnvironment.MapPath("~/UpgradeDatabase.sql");
            string createSpSql = File.ReadAllText(sqlScriptPath);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(createSpSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand("sp_UpgradeDatabase", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
