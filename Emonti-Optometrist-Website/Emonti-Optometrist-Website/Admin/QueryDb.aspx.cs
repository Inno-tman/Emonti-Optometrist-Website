using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class QueryDb : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsStaffLoggedIn"] == null || !(bool)Session["IsStaffLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }
            if (Session["StaffRole"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Staff/Dashboard.aspx");
                return;
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            string sql = txtQuery.Text.Trim();
            if (string.IsNullOrEmpty(sql))
            {
                lblResult.Text = "Please enter a SQL query.";
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Visible = true;
                return;
            }

            string upper = sql.TrimStart().ToUpper();
            if (upper.StartsWith("DROP") || upper.StartsWith("DELETE") || upper.StartsWith("UPDATE") || upper.StartsWith("INSERT") || upper.StartsWith("ALTER") || upper.StartsWith("CREATE") || upper.StartsWith("TRUNCATE") || upper.StartsWith("EXEC") || upper.StartsWith("EXECUTE"))
            {
                lblResult.Text = "Only SELECT queries are allowed.";
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            try
            {
                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        var dt = new DataTable();
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                        gvResults.DataSource = dt;
                        gvResults.DataBind();
                        pnlResults.Visible = true;
                        lblResult.Text = dt.Rows.Count + " row(s) returned.";
                        lblResult.ForeColor = System.Drawing.Color.Green;
                        lblResult.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblResult.Text = "Error: " + ex.Message;
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Visible = true;
                pnlResults.Visible = false;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtQuery.Text = "";
            lblResult.Visible = false;
            pnlResults.Visible = false;
        }
    }
}
