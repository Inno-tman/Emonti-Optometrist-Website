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
            if (!IsPostBack)
                LoadStaff();
        }

        private void LoadStaff()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand("SELECT Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Role FROM Staff ORDER BY Staff_Name", conn))
                {
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvStaff.DataSource = dt;
                        gvStaff.DataBind();
                    }
                }
            }
        }

        protected void gvStaff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteStaff")
            {
                DeleteStaff(e.CommandArgument.ToString());
                LoadStaff();
            }
        }

        private void DeleteStaff(string staffId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand("DELETE FROM Staff WHERE Staff_ID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            ShowMessage("Staff member deleted successfully.", "alert-success");
        }

        protected void btnSaveStaff_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtSurname.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowMessage("Please fill in all fields.", "alert-danger");
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                using (var cmd = new SqlCommand(
                    "INSERT INTO Staff (Staff_Name, Staff_Surname, Staff_Email, Staff_Password, Staff_Role) VALUES (@Name, @Surname, @Email, @Password, @Role)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surname", txtSurname.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            ShowMessage("Staff member added successfully.", "alert-success");
            LoadStaff();
            txtFirstName.Text = "";
            txtSurname.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            ddlRole.SelectedIndex = 0;
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = "alert " + cssClass;
            lblMessage.Visible = true;
        }
    }
}
