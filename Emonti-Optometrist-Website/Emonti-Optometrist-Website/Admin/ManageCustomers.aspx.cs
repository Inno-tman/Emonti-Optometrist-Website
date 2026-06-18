using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            if (!IsPostBack)
                LoadCustomers();
        }

        private void LoadCustomers(string search = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                string query = "SELECT Cust_ID, Customer_Name, Customer_Surname, Customer_Email, Customer_Phone, Customer_Address, Customer_Gender, Customer_DOB FROM Customer";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query += " WHERE Customer_Name LIKE @Search OR Customer_Surname LIKE @Search OR Customer_Email LIKE @Search OR Customer_Phone LIKE @Search";
                }
                query += " ORDER BY Customer_Surname, Customer_Name";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(search))
                        cmd.Parameters.AddWithValue("@Search", "%" + search.Trim() + "%");
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvCustomers.DataSource = dt;
                        gvCustomers.DataBind();
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCustomers.PageIndex = e.NewPageIndex;
            LoadCustomers(txtSearch.Text);
        }

        protected void gvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;

                TableRow detailRow = new TableRow();
                detailRow.CssClass = "detail-row";
                TableCell detailCell = new TableCell();
                detailCell.ColumnSpan = e.Row.Cells.Count;

                StringBuilder sb = new StringBuilder();
                sb.Append("<div class=\"detail-content\">");
                sb.AppendFormat("<p><strong>Gender:</strong> {0}</p>", row["Customer_Gender"] ?? "N/A");
                sb.AppendFormat("<p><strong>Date of Birth:</strong> {0}</p>", row["Customer_DOB"] != DBNull.Value ? Convert.ToDateTime(row["Customer_DOB"]).ToString("dd MMM yyyy") : "N/A");
                sb.AppendFormat("<p><strong>Address:</strong> {0}</p>", row["Customer_Address"] ?? "N/A");
                sb.Append("</div>");

                detailCell.Text = sb.ToString();
                detailRow.Cells.Add(detailCell);

                if (e.Row.Parent is Table table)
                {
                    int index = e.Row.RowIndex + 1;
                    if (index < table.Rows.Count)
                        table.Rows.AddAt(index, detailRow);
                    else
                        table.Rows.Add(detailRow);
                }
            }
        }
    }
}
