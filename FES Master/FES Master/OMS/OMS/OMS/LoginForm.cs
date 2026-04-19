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
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "Emonti" && txtPassword.Text == "OMS2025")
            {
                MessageBox.Show("Access Granted!");
                Form frm = (Form)Application.OpenForms["MainForm"];

                //Find the menu strip control 'menustrip1' on the main form and cast it to the menustrp type
                MenuStrip ms = (MenuStrip)frm.Controls["menuStrip1"];
                //These lines enable six menu items that were previously disabled
                ms.Items["appointmentMenu"].Enabled = true;
                ms.Items["customerMenu"].Enabled = true;
                ms.Items["consultationMenu"].Enabled = true;
                ms.Items["orderMenu"].Enabled = true;
                ms.Items["paymentMenu"].Enabled = true;
                ms.Items["reportMenu"].Enabled = true;

                // Now access the Home menu item
                ToolStripMenuItem ti = (ToolStripMenuItem)ms.Items["Home"];
                ti.DropDownItems["logoutMenu"].Enabled = true;
                ti.DropDownItems["loginMenu"].Enabled = false;

                // Now access the textbox on the main memu
                ToolStripTextBox tuser = (ToolStripTextBox)ms.Items["Username"];
                // Update the textbox to show the current user
                tuser.ForeColor = Color.Green;
                tuser.Text = "Logged In as " + txtUsername.Text;

                // Close the login form
                this.Close();
            }
            else
            {
                MessageBox.Show("Username or Password has been entered incorrectly! Please try again.");
                txtUsername.Clear();
                txtPassword.Clear();
            }
        }
    }
}
