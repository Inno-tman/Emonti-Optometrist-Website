
namespace OMS
{
    partial class Add_Appointment
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ADDcomboAppStaffID = new System.Windows.Forms.ComboBox();
            this.comboAppTimeADD = new System.Windows.Forms.ComboBox();
            this.addDTP = new System.Windows.Forms.DateTimePicker();
            this.txtCustAppID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customerTableAdapter1 = new OMS.dsOMSTableAdapters.CustomerTableAdapter();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ADDcomboAppStaffID);
            this.groupBox1.Controls.Add(this.comboAppTimeADD);
            this.groupBox1.Controls.Add(this.addDTP);
            this.groupBox1.Controls.Add(this.txtCustAppID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(175, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 426);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add Appointment";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 294);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Appointment Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Staff ID";
            // 
            // ADDcomboAppStaffID
            // 
            this.ADDcomboAppStaffID.FormattingEnabled = true;
            this.ADDcomboAppStaffID.Location = new System.Drawing.Point(180, 216);
            this.ADDcomboAppStaffID.Name = "ADDcomboAppStaffID";
            this.ADDcomboAppStaffID.Size = new System.Drawing.Size(162, 21);
            this.ADDcomboAppStaffID.TabIndex = 6;
            // 
            // comboAppTimeADD
            // 
            this.comboAppTimeADD.FormattingEnabled = true;
            this.comboAppTimeADD.Location = new System.Drawing.Point(180, 286);
            this.comboAppTimeADD.Name = "comboAppTimeADD";
            this.comboAppTimeADD.Size = new System.Drawing.Size(162, 21);
            this.comboAppTimeADD.TabIndex = 5;
            // 
            // addDTP
            // 
            this.addDTP.Location = new System.Drawing.Point(113, 55);
            this.addDTP.Name = "addDTP";
            this.addDTP.Size = new System.Drawing.Size(200, 20);
            this.addDTP.TabIndex = 4;
            // 
            // txtCustAppID
            // 
            this.txtCustAppID.Location = new System.Drawing.Point(180, 138);
            this.txtCustAppID.Name = "txtCustAppID";
            this.txtCustAppID.Size = new System.Drawing.Size(162, 20);
            this.txtCustAppID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer ID";
            // 
            // customerTableAdapter1
            // 
            this.customerTableAdapter1.ClearBeforeFill = true;
            // 
            // Add_Appointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "Add_Appointment";
            this.Text = "Add_Appointment";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ADDcomboAppStaffID;
        private System.Windows.Forms.ComboBox comboAppTimeADD;
        private System.Windows.Forms.DateTimePicker addDTP;
        private System.Windows.Forms.TextBox txtCustAppID;
        private System.Windows.Forms.Label label1;
        private dsOMSTableAdapters.CustomerTableAdapter customerTableAdapter1;
    }
}