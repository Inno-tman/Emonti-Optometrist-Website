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
    public partial class MainForm : System.Windows.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void PrepareForm(Form f)
        {
            // start a lopp that iterates through all currently open mdi child forms
            // 'this.MdiChildren' is a collection that contains all child forms currently hosted by this parent from
            foreach (Form c in this.MdiChildren)
                // 'Form c' declares a variable that will old each child form during each iteration of the loop
            {
                c.Close();
                // close currently open child form before opening a new one 
            }
            f.MdiParent = this;
            f.WindowState = FormWindowState.Maximized;
            f.Show();

        }

        private void lOGINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            PrepareForm(login);
        }

        private void hOMEToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void lOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form c in this.MdiChildren)
            {
                c.Close();
            }
            loginMenu.Enabled = true;
            logoutMenu.Enabled = false;
            appointmentMenu.Enabled = false;
            customerMenu.Enabled = false;
            consultationMenu.Enabled = false;
            orderMenu.Enabled = false;
            paymentMenu.Enabled = false;
            reportMenu.Enabled = false;

            Username.ForeColor = Color.Red;
            Username.Text = "Logged Out";

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void aPPOINTMENTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Appointment appoint = new Appointment();
            PrepareForm(appoint);
        }

        private void aDDAPPOINTMENTVIEWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Appointment appoint = new Appointment();
            PrepareForm(appoint);
        }

        private void exitMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aDDORDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            PrepareForm(order);
        }

        private void customerMenu_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer();
            PrepareForm(customer);
        }

        private void aDDCONSULTATIONToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void consultationMenu_Click(object sender, EventArgs e)
        {
            Consultation consultation = new Consultation();
            PrepareForm(consultation);
        }

        private void paymentMenu_Click(object sender, EventArgs e)
        {
            Payment payment = new Payment();
            PrepareForm(payment);
        }

        private void reportMenu_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            PrepareForm(report);
        }

        private void orderMenu_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            PrepareForm(order);
        }
    }
}
