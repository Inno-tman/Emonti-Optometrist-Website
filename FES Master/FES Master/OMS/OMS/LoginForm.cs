using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMS
{
    public partial class LoginForm : Form
    {
        // Dataset reference
        private dsOMS dsOMS;
        private dsOMSTableAdapters.StaffTableAdapter staffTableAdapter;

        public LoginForm()
        {
            InitializeComponent();
            // Initialize dataset and table adapter
            dsOMS = new dsOMS();
            staffTableAdapter = new dsOMSTableAdapters.StaffTableAdapter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both username and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Fill the Staff table from database
                taStaff.Fill(dsOMS.Staff);

                // First, check if username exists
                var usernameQuery = from staff in dsOMS.Staff.AsEnumerable()
                                    where staff.Field<string>("Staff_Name").Equals(txtUsername.Text, StringComparison.OrdinalIgnoreCase)
                                    select new
                                    {
                                        StaffID = staff.Field<int>("Staff_ID"),
                                        StaffName = staff.Field<string>("Staff_Name"),
                                        StaffSurname = staff.Field<string>("Staff_Surname"),
                                        StaffRole = staff.Field<string>("Staff_Role"),
                                        StaffEmail = staff.Field<string>("Staff_Email"),
                                        StaffPhone = staff.Field<string>("Staff_Phone"),
                                        StaffPassword = staff.Field<string>("Staff_Password")
                                    };

                var userRecord = usernameQuery.FirstOrDefault();

                if (userRecord == null)
                {
                    // Username doesn't exist
                    MessageBox.Show("Username not found. Please check your username and try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsername.Clear();
                    txtPassword.Clear();
                    txtUsername.Focus();
                    return;
                }

                // Username exists, now check password
                if (userRecord.StaffPassword != txtPassword.Text)
                {
                    // Username is correct but password is wrong
                    MessageBox.Show("Password is incorrect. Please check your password and try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus(); // Keep focus on password field since username is correct
                    return;
                }

                // Both username and password are correct
                MessageBox.Show($"Welcome {userRecord.StaffName} {userRecord.StaffSurname}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Store user information for session
                CurrentUser.StaffID = userRecord.StaffID;
                CurrentUser.StaffName = userRecord.StaffName;
                CurrentUser.FullName = $"{userRecord.StaffName} {userRecord.StaffSurname}";
                CurrentUser.Role = userRecord.StaffRole;
                CurrentUser.Email = userRecord.StaffEmail;
                CurrentUser.Phone = userRecord.StaffPhone;

                // Access the main form and enable menu items
                Form frm = (Form)Application.OpenForms["MainForm"];
                if (frm != null)
                {
                    MenuStrip ms = (MenuStrip)frm.Controls["menuStrip1"];

                    // Enable menu items based on user role
                    EnableMenuItemsBasedOnRole(ms, userRecord.StaffRole);

                    // Update Home menu
                    ToolStripMenuItem ti = (ToolStripMenuItem)ms.Items["Home"];
                    ti.DropDownItems["logoutMenu"].Enabled = true;
                    ti.DropDownItems["loginMenu"].Enabled = false;

                    // Update username display
                    ToolStripTextBox tuser = (ToolStripTextBox)ms.Items["Username"];
                    tuser.ForeColor = Color.Green;
                    tuser.Text = $"Logged in as {userRecord.StaffName} {userRecord.StaffSurname} ({userRecord.StaffRole})";

                    string staffName = "Sam Wilson"; // ← Check

                    //  Report reportForm = new Report();
                    //  reportForm.LoggedInStaffName = staffName;
                    //   reportForm.Show();
                    // this.Hide();
                }

                // Close the login form
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableMenuItemsBasedOnRole(MenuStrip ms, string role)
        {
            // Default - enable all menus
            ms.Items["appointmentMenu"].Enabled = true;
            ms.Items["customerMenu"].Enabled = true;
            ms.Items["consultationMenu"].Enabled = true;
            ms.Items["orderMenu"].Enabled = true;
            ms.Items["paymentMenu"].Enabled = true;
            ms.Items["reportMenu"].Enabled = true;

            // Role-based access control based staff data
            switch (role?.ToLower())
            {
                case "administrator":
                    // Administrator has access to everything (all menus remain enabled)
                    //For Now
                    ms.Items["consultationMenu"].Enabled = false;
                    break;

                case "optometrist":
                    // For now, giving them full access except reports
                    ms.Items["reportMenu"].Enabled = false;
                    break;

                default:
                    // Default case - enable all functionality
                    // You can modify this based on other roles you might add
                    break;
            }
        }
    }

    // Helper class to store current user session information
    public static class CurrentUser
    {
        public static int StaffID { get; set; }
        public static string StaffName { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }
        public static string Email { get; set; }
        public static string Phone { get; set; }

        public static void ClearSession()
        {
            StaffID = 0;
            StaffName = string.Empty;
            FullName = string.Empty;
            Role = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
        }
    }
}