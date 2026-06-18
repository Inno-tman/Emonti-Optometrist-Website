<%@ WebHandler Language="C#" Class="ResetAdminPwd" %>

using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

public class ResetAdminPwd : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string key = context.Request.QueryString["key"];
        if (key != "reset-now-2024")
        {
            context.Response.Write("Unauthorized");
            context.Response.StatusCode = 403;
            return;
        }

        try
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "UPDATE Staff SET Staff_Password = @Pwd WHERE Staff_Role = @Role";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Pwd", "Admin");
                    cmd.Parameters.AddWithValue("@Role", "Admin");
                    int rows = cmd.ExecuteNonQuery();
                    context.Response.Write("OK - " + rows + " admin password(s) reset to 'Admin'");
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("ERROR: " + ex.Message);
            context.Response.StatusCode = 500;
        }
    }

    public bool IsReusable { get { return false; } }
}
