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
                using (var cmd = new SqlCommand("SELECT Staff_ID, Staff_Name, Staff_Surname, Staff_Email, Staff_Role FROM Staff ORDER BY Staff_Name", conn))
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
            string mode = Request.Form["__VIEWSTATE"] != null ? (Request.Form["ctl00$MainContent$hiddenMode"] ?? "add") : "add";
            string staffIdStr = Request.Form["ctl00$MainContent$hiddenStaffId"] ?? "0";
            int staffId = 0;
            int.TryParse(staffIdStr, out staffId);

            // Handle different operations based on mode
            if (mode == "delete")
            {
                DeleteStaff(staffId);
                return;
            }
            else if (mode == "promote")
            {
                PromoteStaff(staffId);
                return;
            }

            // Handle Add or Edit
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = ddlRole.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(email))
            {
                lblAddError.Text = "Name, surname and email are required.";
                lblAddError.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;

            if (mode == "add")
            {
                AddNewStaff(connStr, name, surname, email, password, role);
            }
            else if (mode == "edit")
            {
                UpdateStaff(connStr, staffId, name, surname, email, password, role);
            }
        }

        private void AddNewStaff(string connStr, string name, string surname, string email, string password, string role)
        {
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

                using (var cmd = new SqlCommand(@"INSERT INTO Staff (Staff_Name, Staff_Surname, Staff_Email, Staff_Password, Staff_Role) VALUES (@Name, @Surname, @Email, @Password, @Role)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadStaff();
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "closeModal();", true);
        }

        private void UpdateStaff(string connStr, int staffId, string name, string surname, string email, string password, string role)
        {
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check if email is already taken by another user
                using (var check = new SqlCommand("SELECT COUNT(*) FROM Staff WHERE Staff_Email = @Email AND Staff_ID != @Id", conn))
                {
                    check.Parameters.AddWithValue("@Email", email);
                    check.Parameters.AddWithValue("@Id", staffId);
                    int exists = (int)check.ExecuteScalar();
                    if (exists > 0)
                    {
                        lblAddError.Text = "A staff member with this email already exists.";
                        lblAddError.Visible = true;
                        return;
                    }
                }

                // Update staff information
                string updateQuery = string.IsNullOrEmpty(password) || password == "Staff123"
                    ? "UPDATE Staff SET Staff_Name = @Name, Staff_Surname = @Surname, Staff_Email = @Email, Staff_Role = @Role WHERE Staff_ID = @Id"
                    : "UPDATE Staff SET Staff_Name = @Name, Staff_Surname = @Surname, Staff_Email = @Email, Staff_Password = @Password, Staff_Role = @Role WHERE Staff_ID = @Id";

                using (var cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@Email", email);
                    if (!string.IsNullOrEmpty(password) && password != "Staff123")
                    {
                        cmd.Parameters.AddWithValue("@Password", password);
                    }
                    cmd.Parameters.AddWithValue("@Role", role);
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadStaff();
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "closeModal();", true);
        }

        private void DeleteStaff(int staffId)
        {
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
                    lblAddError.Text = "Cannot delete another admin.";
                    lblAddError.Visible = true;
                    return;
                }
                using (var cmd = new SqlCommand("DELETE FROM Staff WHERE Staff_ID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", staffId);
                    cmd.ExecuteNonQuery();
                }
            }

            LoadStaff();
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "closeModal();", true);
        }

        private void PromoteStaff(int staffId)
        {
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

            LoadStaff();
            ScriptManager.RegisterStartupScript(this, GetType(), "CloseModal", "closeModal();", true);
        }
    }
}
