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

                // First show current admin record
                string show = "SELECT Staff_ID, Staff_Name, Staff_Email, Staff_Role FROM Staff WHERE Staff_Role = 'Admin'";
                using (var sc = new SqlCommand(show, conn))
                {
                    using (var r = sc.ExecuteReader())
                    {
                        context.Response.Write("Current admin:\n");
                        while (r.Read())
                        {
                            context.Response.Write("  ID=" + r["Staff_ID"] + " Name=" + r["Staff_Name"] + " Email=" + r["Staff_Email"] + " Role=" + r["Staff_Role"] + "\n");
                        }
                    }
                }

                // Update email and password
                string upd = "UPDATE Staff SET Staff_Email = @email, Staff_Password = @pwd WHERE Staff_Role = @role";
                using (var u = new SqlCommand(upd, conn))
                {
                    u.Parameters.AddWithValue("@email", "admin@emonti.com");
                    u.Parameters.AddWithValue("@pwd", "Admin");
                    u.Parameters.AddWithValue("@role", "Admin");
                    int rows = u.ExecuteNonQuery();
                    context.Response.Write("\nUpdated " + rows + " admin record(s) - email=admin@emonti.com password=Admin\n");
                }

                // Verify
                using (var sc = new SqlCommand(show, conn))
                {
                    using (var r = sc.ExecuteReader())
                    {
                        context.Response.Write("After update:\n");
                        while (r.Read())
                        {
                            context.Response.Write("  ID=" + r["Staff_ID"] + " Name=" + r["Staff_Name"] + " Email=" + r["Staff_Email"] + " Role=" + r["Staff_Role"] + "\n");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("ERROR: " + ex.Message);
        }
    }

    public bool IsReusable { get { return false; } }
}
