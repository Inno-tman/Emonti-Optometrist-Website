using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class ManageOrders : Page
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
            if (!IsPostBack) LoadOrders(null);
        }

        private void LoadOrders(string statusFilter)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = @"SELECT o.OrderID, o.CustID, o.Order_Date, o.Order_Total, o.Order_Status,
                                      c.Customer_Name + ' ' + c.Customer_Surname AS CustomerName
                               FROM [Order] o
                               LEFT JOIN customer c ON o.CustID = c.Cust_ID";
                if (!string.IsNullOrEmpty(statusFilter))
                    sql += " WHERE o.Order_Status = @Status";
                sql += " ORDER BY o.Order_Date DESC";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(statusFilter))
                        cmd.Parameters.AddWithValue("@Status", statusFilter);
                    var dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    gvOrders.DataSource = dt;
                    gvOrders.DataBind();
                }
            }
        }

        protected void ddlStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filter = ddlStatusFilter.SelectedValue;
            if (filter == "") filter = null;
            LoadOrders(filter);
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            int orderId = Convert.ToInt32(gvOrders.DataKeys[row.RowIndex].Value);
            string newStatus = ddl.SelectedValue;

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE [Order] SET Order_Status = @Status WHERE OrderID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Status", newStatus);
                    cmd.Parameters.AddWithValue("@Id", orderId);
                    cmd.ExecuteNonQuery();
                }
            }

            string filter = ddlStatusFilter.SelectedValue;
            if (filter == "") filter = null;
            LoadOrders(filter);
        }
    }
}
