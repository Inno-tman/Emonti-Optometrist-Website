using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
            if (!IsPostBack) LoadStats();
        }

        private void LoadStats()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Order] WHERE CAST(Order_Date AS DATE) = CAST(GETDATE() AS DATE)", conn))
                    lblOrdersToday.Text = cmd.ExecuteScalar().ToString();

                using (var cmd = new SqlCommand("SELECT ISNULL(SUM(Order_Total), 0) FROM [Order]", conn))
                    lblTotalRevenue.Text = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("N2");

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Order] WHERE Order_Status IN ('Pending', 'Processing')", conn))
                    lblPendingOrders.Text = cmd.ExecuteScalar().ToString();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Products2", conn))
                    lblTotalProducts.Text = cmd.ExecuteScalar().ToString();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Staff", conn))
                    lblTotalStaff.Text = cmd.ExecuteScalar().ToString();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM Appointment WHERE CAST(Appointment_Date AS DATE) = CAST(GETDATE() AS DATE)", conn))
                    lblTodayAppointments.Text = cmd.ExecuteScalar().ToString();

                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM customer WHERE Customer_Create_Date IS NOT NULL AND CAST(Customer_Create_Date AS DATE) >= DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)", conn))
                    lblNewCustomers.Text = cmd.ExecuteScalar().ToString();

                var orders = new DataTable();
                using (var cmd = new SqlCommand(@"
                    SELECT o.OrderID, o.CustID, o.Order_Date, o.Order_Total, o.Order_Status,
                           c.Customer_Name, c.Customer_Surname
                    FROM [Order] o
                    LEFT JOIN customer c ON o.CustID = c.Cust_ID
                    ORDER BY o.Order_Date DESC", conn))
                {
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(orders);
                    }
                }
                var dt = new DataTable();
                dt.Columns.Add("CustomerName", typeof(string));
                dt.Columns.Add("OrderDate", typeof(DateTime));
                dt.Columns.Add("Total", typeof(decimal));
                dt.Columns.Add("Status", typeof(string));
                int count = 0;
                foreach (DataRow row in orders.Rows)
                {
                    if (count >= 5) break;
                    dt.Rows.Add(
                        row["Customer_Name"]?.ToString() + " " + row["Customer_Surname"]?.ToString(),
                        Convert.ToDateTime(row["Order_Date"]),
                        Convert.ToDecimal(row["Order_Total"]),
                        row["Order_Status"]?.ToString()
                    );
                    count++;
                }
                gvRecentOrders.DataSource = dt;
                gvRecentOrders.DataBind();
            }
        }
    }
}
