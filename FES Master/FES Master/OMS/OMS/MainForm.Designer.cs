
namespace OMS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Home = new System.Windows.Forms.ToolStripMenuItem();
            this.loginMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.customerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.appointmentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.consultationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.orderMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.paymentMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.reportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.Username = new System.Windows.Forms.ToolStripTextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.AliceBlue;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Home,
            this.customerMenu,
            this.appointmentMenu,
            this.consultationMenu,
            this.orderMenu,
            this.paymentMenu,
            this.reportMenu,
            this.Username});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1699, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Home
            // 
            this.Home.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Home.BackgroundImage")));
            this.Home.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginMenu,
            this.logoutMenu,
            this.exitMenu});
            this.Home.Image = ((System.Drawing.Image)(resources.GetObject("Home.Image")));
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(103, 32);
            this.Home.Text = "HOME";
            this.Home.Click += new System.EventHandler(this.hOMEToolStripMenuItem_Click);
            // 
            // loginMenu
            // 
            this.loginMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginMenu.Image = ((System.Drawing.Image)(resources.GetObject("loginMenu.Image")));
            this.loginMenu.Name = "loginMenu";
            this.loginMenu.Size = new System.Drawing.Size(162, 28);
            this.loginMenu.Text = "LOGIN";
            this.loginMenu.Click += new System.EventHandler(this.lOGINToolStripMenuItem_Click);
            // 
            // logoutMenu
            // 
            this.logoutMenu.Enabled = false;
            this.logoutMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logoutMenu.Image = ((System.Drawing.Image)(resources.GetObject("logoutMenu.Image")));
            this.logoutMenu.Name = "logoutMenu";
            this.logoutMenu.Size = new System.Drawing.Size(162, 28);
            this.logoutMenu.Text = "LOGOUT";
            this.logoutMenu.Click += new System.EventHandler(this.lOGOUTToolStripMenuItem_Click);
            // 
            // exitMenu
            // 
            this.exitMenu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exitMenu.Image = ((System.Drawing.Image)(resources.GetObject("exitMenu.Image")));
            this.exitMenu.Name = "exitMenu";
            this.exitMenu.Size = new System.Drawing.Size(162, 28);
            this.exitMenu.Text = "EXIT";
            this.exitMenu.Click += new System.EventHandler(this.exitMenu_Click);
            // 
            // customerMenu
            // 
            this.customerMenu.Enabled = false;
            this.customerMenu.Image = ((System.Drawing.Image)(resources.GetObject("customerMenu.Image")));
            this.customerMenu.Name = "customerMenu";
            this.customerMenu.Size = new System.Drawing.Size(147, 32);
            this.customerMenu.Text = "CUSTOMER";
            this.customerMenu.Click += new System.EventHandler(this.customerMenu_Click);
            // 
            // appointmentMenu
            // 
            this.appointmentMenu.Enabled = false;
            this.appointmentMenu.Image = ((System.Drawing.Image)(resources.GetObject("appointmentMenu.Image")));
            this.appointmentMenu.Name = "appointmentMenu";
            this.appointmentMenu.Size = new System.Drawing.Size(179, 32);
            this.appointmentMenu.Text = "APPOINTMENT";
            this.appointmentMenu.Click += new System.EventHandler(this.aPPOINTMENTToolStripMenuItem_Click);
            // 
            // consultationMenu
            // 
            this.consultationMenu.Enabled = false;
            this.consultationMenu.Image = ((System.Drawing.Image)(resources.GetObject("consultationMenu.Image")));
            this.consultationMenu.Name = "consultationMenu";
            this.consultationMenu.Size = new System.Drawing.Size(186, 32);
            this.consultationMenu.Text = "CONSULTATION";
            this.consultationMenu.Click += new System.EventHandler(this.consultationMenu_Click);
            // 
            // orderMenu
            // 
            this.orderMenu.Enabled = false;
            this.orderMenu.Image = ((System.Drawing.Image)(resources.GetObject("orderMenu.Image")));
            this.orderMenu.Name = "orderMenu";
            this.orderMenu.Size = new System.Drawing.Size(109, 32);
            this.orderMenu.Text = "ORDER";
            this.orderMenu.Click += new System.EventHandler(this.orderMenu_Click);
            // 
            // paymentMenu
            // 
            this.paymentMenu.Enabled = false;
            this.paymentMenu.Image = ((System.Drawing.Image)(resources.GetObject("paymentMenu.Image")));
            this.paymentMenu.Name = "paymentMenu";
            this.paymentMenu.Size = new System.Drawing.Size(132, 32);
            this.paymentMenu.Text = "PAYMENT";
            this.paymentMenu.Click += new System.EventHandler(this.paymentMenu_Click);
            // 
            // reportMenu
            // 
            this.reportMenu.Enabled = false;
            this.reportMenu.Image = ((System.Drawing.Image)(resources.GetObject("reportMenu.Image")));
            this.reportMenu.Name = "reportMenu";
            this.reportMenu.Size = new System.Drawing.Size(115, 32);
            this.reportMenu.Text = "REPORT";
            this.reportMenu.Click += new System.EventHandler(this.reportMenu_Click);
            // 
            // Username
            // 
            this.Username.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Username.ForeColor = System.Drawing.Color.Red;
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(332, 32);
            this.Username.Text = "Logged Out";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1699, 768);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Home;
        private System.Windows.Forms.ToolStripMenuItem loginMenu;
        private System.Windows.Forms.ToolStripMenuItem logoutMenu;
        private System.Windows.Forms.ToolStripMenuItem appointmentMenu;
        private System.Windows.Forms.ToolStripMenuItem consultationMenu;
        private System.Windows.Forms.ToolStripMenuItem orderMenu;
        private System.Windows.Forms.ToolStripMenuItem paymentMenu;
        private System.Windows.Forms.ToolStripMenuItem reportMenu;
        private System.Windows.Forms.ToolStripMenuItem customerMenu;
        private System.Windows.Forms.ToolStripTextBox Username;
        private System.Windows.Forms.ToolStripMenuItem exitMenu;
    }
}

