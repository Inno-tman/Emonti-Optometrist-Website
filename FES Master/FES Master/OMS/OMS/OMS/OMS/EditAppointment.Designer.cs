
namespace OMS
{
    partial class EditAppointment
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
            this.comboEditTime = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EDITdtp = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboEditTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.EDITdtp);
            this.groupBox1.Location = new System.Drawing.Point(235, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 425);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Edit Appointment";
            // 
            // comboEditTime
            // 
            this.comboEditTime.FormattingEnabled = true;
            this.comboEditTime.Location = new System.Drawing.Point(151, 160);
            this.comboEditTime.Name = "comboEditTime";
            this.comboEditTime.Size = new System.Drawing.Size(121, 21);
            this.comboEditTime.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Appointment Time";
            // 
            // EDITdtp
            // 
            this.EDITdtp.Location = new System.Drawing.Point(28, 78);
            this.EDITdtp.Name = "EDITdtp";
            this.EDITdtp.Size = new System.Drawing.Size(200, 20);
            this.EDITdtp.TabIndex = 0;
            // 
            // EditAppointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox1);
            this.Name = "EditAppointment";
            this.Text = "EditAppointment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboEditTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker EDITdtp;
    }
}