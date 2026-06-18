using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class ManageStaff : Page
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
            if (!IsPostBack) LoadStaff();
        }

        private void LoadStaff()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT Staff_ID, Staff_Name, Staff_Email, Staff_Role, Can_Mark_Attendance FROM Staff ORDER BY Staff_Name", conn))
                {
                    var dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    gvStaff.DataSource = dt;
                    gvStaff.DataBind();
                }
            }
        }

        protected void btnAddStaff_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;
            bool canMark = chkAttendance.Checked;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                lblAddError.Text = "Name and email are required.";
                lblAddError.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                using (var check = new SqlCommand("SELECT COUNT(*) FROM Staff WHERE Staff_Email = @Email", conn))
                {
                    check.Parameters.AddWithValue("@Email", email);
                    int exists = (int)check.ExecuteScalar();
                    if (exists > 0)
                    {
                        lblAddError.Text = "A staff member with this email already exists.";
                        lblAddError.Visible = true;
                        return;
                    }
                }

                using (var cmd = new SqlCommand(@"INSERT INTO Staff (Staff_Name, Staff_Email, Staff_Password, Staff_Role, Can_Mark_Attendance) VALUES (@Name, @Email, @Password, @Role, @CanMark)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@CanMark", canMark);
                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("ManageStaff.aspx");
        }

        protected void btnPromote_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int staffId = Convert.ToInt32(btn.CommandArgument);
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("UPDATE Staff SET Staff_Role = 'Admin' WHERE Staff_ID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("ManageStaff.aspx");
        }

        protected void gvStaff_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int staffId = Convert.ToInt32(gvStaff.DataKeys[e.RowIndex].Value);
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string currentRole = null;
                using (var getRole = new SqlCommand("SELECT Staff_Role FROM Staff WHERE Staff_ID = @Id", conn))
                {
                    getRole.Parameters.AddWithValue("@Id", staffId);
                    currentRole = getRole.ExecuteScalar()?.ToString();
                }
                if (currentRole == "Admin")
                {
                    pnlMessage.Visible = true;
                    pnlMessage.CssClass = "alert alert-danger";
                    pnlMessage.Controls.Clear();
                    pnlMessage.Controls.Add(new LiteralControl("Cannot delete another admin."));
                    LoadStaff();
                    return;
                }
                using (var cmd = new SqlCommand("DELETE FROM Staff WHERE Staff_ID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("ManageStaff.aspx");
        }
    }
}
