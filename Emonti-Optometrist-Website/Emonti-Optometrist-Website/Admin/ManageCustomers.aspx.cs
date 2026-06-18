using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class ManageCustomers : Page
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
            if (!IsPostBack) LoadCustomers(null);
        }

        private void LoadCustomers(string search)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT c.Cust_ID, c.Customer_Name, c.Customer_Surname, c.Customer_Email, c.Customer_Phone,
                                      (SELECT COUNT(*) FROM [Order] o WHERE o.CustID = c.Cust_ID) AS OrderCount
                               FROM customer c";
                if (!string.IsNullOrEmpty(search))
                    sql += " WHERE c.Customer_Name LIKE @Search OR c.Customer_Surname LIKE @Search OR c.Customer_Email LIKE @Search";
                sql += " ORDER BY c.Customer_Surname, c.Customer_Name";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(search))
                        cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                    var dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    // Ensure columns expected by UI exist to avoid binding errors
                    if (!dt.Columns.Contains("OrderCount"))
                        dt.Columns.Add("OrderCount", typeof(int));
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["OrderCount"] == DBNull.Value || r["OrderCount"] == null)
                            r["OrderCount"] = 0;
                    }
                    gvCustomers.DataSource = dt;
                    gvCustomers.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text.Trim());
        }
    }
}
