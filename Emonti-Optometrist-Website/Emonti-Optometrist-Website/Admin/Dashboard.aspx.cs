using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class Dashboard : Page
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

            if (!IsPostBack)
            {
                LoadStats();
                lblAdminName.Text = Session["StaffName"]?.ToString() ?? "Admin";
            }
        }

        private void LoadStats()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Staff", conn))
                    lblStaffCount.Text = cmd.ExecuteScalar().ToString();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Customer", conn))
                    lblCustomerCount.Text = cmd.ExecuteScalar().ToString();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Appointment", conn))
                    lblAppointmentCount.Text = cmd.ExecuteScalar().ToString();
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Products2", conn))
                    lblProductCount.Text = cmd.ExecuteScalar().ToString();
            }
        }
    }
}
