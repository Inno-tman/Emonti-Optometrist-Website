
namespace OMS
{
    partial class Appointment
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
            this.components = new System.ComponentModel.Container();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.comboTimeslot = new System.Windows.Forms.ComboBox();
            this.dTTimeslotsAvailableBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsAppointment = new OMS.dsAppointment();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCustID = new System.Windows.Forms.Label();
            this.lblSurname = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch1 = new System.Windows.Forms.TextBox();
            this.dgvCustomer = new System.Windows.Forms.DataGridView();
            this.custIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSurnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerEmailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.monthCalendar2 = new System.Windows.Forms.MonthCalendar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.custIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSurnameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerEmailDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerPhoneDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentDateDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeslotDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentDetailBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.taAppointmentDetail = new OMS.dsAppointmentTableAdapters.AppointmentDetailTableAdapter();
            this.taAvailableTimeslots = new OMS.dsAppointmentTableAdapters.DTTimeslotsAvailableTableAdapter();
            this.taAppointment = new OMS.dsAppointmentTableAdapters.AppointmentTableAdapter();
            this.appointmentDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.taCustomer = new OMS.dsAppointmentTableAdapters.CustomerTableAdapter();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dTTimeslotsAvailableBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl2
            // 
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1545, 830);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage1.Size = new System.Drawing.Size(1537, 799);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox6);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Location = new System.Drawing.Point(1214, 20);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(509, 440);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Appointment";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.comboTimeslot);
            this.groupBox6.Controls.Add(this.button1);
            this.groupBox6.Location = new System.Drawing.Point(23, 137);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(454, 150);
            this.groupBox6.TabIndex = 7;
            this.groupBox6.TabStop = false;
            // 
            // comboTimeslot
            // 
            this.comboTimeslot.DataSource = this.dTTimeslotsAvailableBindingSource;
            this.comboTimeslot.DisplayMember = "Timeslot";
            this.comboTimeslot.FormattingEnabled = true;
            this.comboTimeslot.Location = new System.Drawing.Point(30, 24);
            this.comboTimeslot.Name = "comboTimeslot";
            this.comboTimeslot.Size = new System.Drawing.Size(114, 26);
            this.comboTimeslot.TabIndex = 0;
            this.comboTimeslot.ValueMember = "TimeID";
            this.comboTimeslot.SelectedIndexChanged += new System.EventHandler(this.comboTimeslot_SelectedIndexChanged);
            // 
            // dTTimeslotsAvailableBindingSource
            // 
            this.dTTimeslotsAvailableBindingSource.DataMember = "DTTimeslotsAvailable";
            this.dTTimeslotsAvailableBindingSource.DataSource = this.dsAppointment;
            // 
            // dsAppointment
            // 
            this.dsAppointment.DataSetName = "dsAppointment";
            this.dsAppointment.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.SeaGreen;
            this.button1.Location = new System.Drawing.Point(30, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 67);
            this.button1.TabIndex = 1;
            this.button1.Text = "CONFIRM BOOKING";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(53, 311);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 67);
            this.button2.TabIndex = 6;
            this.button2.Text = "CANCEL BOOKING";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblName);
            this.groupBox3.Controls.Add(this.lblCustID);
            this.groupBox3.Controls.Add(this.lblSurname);
            this.groupBox3.Location = new System.Drawing.Point(23, 36);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(454, 95);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(27, 46);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(51, 18);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "NAME";
            // 
            // lblCustID
            // 
            this.lblCustID.AutoSize = true;
            this.lblCustID.Location = new System.Drawing.Point(310, 46);
            this.lblCustID.Name = "lblCustID";
            this.lblCustID.Size = new System.Drawing.Size(113, 18);
            this.lblCustID.TabIndex = 4;
            this.lblCustID.Text = "CUSTOMER ID";
            // 
            // lblSurname
            // 
            this.lblSurname.AutoSize = true;
            this.lblSurname.Location = new System.Drawing.Point(166, 46);
            this.lblSurname.Name = "lblSurname";
            this.lblSurname.Size = new System.Drawing.Size(83, 18);
            this.lblSurname.TabIndex = 3;
            this.lblSurname.Text = "SURNAME";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.monthCalendar1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSearch1);
            this.groupBox1.Controls.Add(this.dgvCustomer);
            this.groupBox1.Location = new System.Drawing.Point(69, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1123, 440);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Customer";
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(43, 161);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(596, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "SEARCH BY SURNAME";
            // 
            // txtSearch1
            // 
            this.txtSearch1.Location = new System.Drawing.Point(489, 76);
            this.txtSearch1.Name = "txtSearch1";
            this.txtSearch1.Size = new System.Drawing.Size(398, 24);
            this.txtSearch1.TabIndex = 1;
            this.txtSearch1.TextChanged += new System.EventHandler(this.txtSearch1_TextChanged);
            // 
            // dgvCustomer
            // 
            this.dgvCustomer.AutoGenerateColumns = false;
            this.dgvCustomer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomer.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custIDDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.customerSurnameDataGridViewTextBoxColumn,
            this.customerEmailDataGridViewTextBoxColumn,
            this.customerPhoneDataGridViewTextBoxColumn});
            this.dgvCustomer.DataSource = this.customerBindingSource;
            this.dgvCustomer.Location = new System.Drawing.Point(315, 116);
            this.dgvCustomer.Name = "dgvCustomer";
            this.dgvCustomer.RowHeadersWidth = 51;
            this.dgvCustomer.Size = new System.Drawing.Size(740, 297);
            this.dgvCustomer.TabIndex = 0;
            this.dgvCustomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dgvCustomer.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCustomer_RowHeaderMouseDoubleClick);
            // 
            // custIDDataGridViewTextBoxColumn
            // 
            this.custIDDataGridViewTextBoxColumn.DataPropertyName = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn.HeaderText = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.custIDDataGridViewTextBoxColumn.Name = "custIDDataGridViewTextBoxColumn";
            this.custIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            // 
            // customerSurnameDataGridViewTextBoxColumn
            // 
            this.customerSurnameDataGridViewTextBoxColumn.DataPropertyName = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn.HeaderText = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerSurnameDataGridViewTextBoxColumn.Name = "customerSurnameDataGridViewTextBoxColumn";
            // 
            // customerEmailDataGridViewTextBoxColumn
            // 
            this.customerEmailDataGridViewTextBoxColumn.DataPropertyName = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn.HeaderText = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerEmailDataGridViewTextBoxColumn.Name = "customerEmailDataGridViewTextBoxColumn";
            // 
            // customerPhoneDataGridViewTextBoxColumn
            // 
            this.customerPhoneDataGridViewTextBoxColumn.DataPropertyName = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn.HeaderText = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerPhoneDataGridViewTextBoxColumn.Name = "customerPhoneDataGridViewTextBoxColumn";
            // 
            // customerBindingSource
            // 
            this.customerBindingSource.DataMember = "Customer";
            this.customerBindingSource.DataSource = this.dsAppointment;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.GhostWhite;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(59, 490);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1664, 284);
            this.panel1.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.monthCalendar2);
            this.groupBox7.Location = new System.Drawing.Point(24, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(298, 248);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            // 
            // monthCalendar2
            // 
            this.monthCalendar2.Location = new System.Drawing.Point(28, 56);
            this.monthCalendar2.Name = "monthCalendar2";
            this.monthCalendar2.TabIndex = 3;
            this.monthCalendar2.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar2_DateChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custIDDataGridViewTextBoxColumn1,
            this.customerNameDataGridViewTextBoxColumn1,
            this.customerSurnameDataGridViewTextBoxColumn1,
            this.customerEmailDataGridViewTextBoxColumn1,
            this.customerPhoneDataGridViewTextBoxColumn1,
            this.appointmentDateDataGridViewTextBoxColumn1,
            this.timeslotDataGridViewTextBoxColumn1});
            this.dataGridView1.DataSource = this.appointmentDetailBindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(336, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(1264, 240);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // custIDDataGridViewTextBoxColumn1
            // 
            this.custIDDataGridViewTextBoxColumn1.DataPropertyName = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn1.HeaderText = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.custIDDataGridViewTextBoxColumn1.Name = "custIDDataGridViewTextBoxColumn1";
            this.custIDDataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // customerNameDataGridViewTextBoxColumn1
            // 
            this.customerNameDataGridViewTextBoxColumn1.DataPropertyName = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn1.HeaderText = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.customerNameDataGridViewTextBoxColumn1.Name = "customerNameDataGridViewTextBoxColumn1";
            // 
            // customerSurnameDataGridViewTextBoxColumn1
            // 
            this.customerSurnameDataGridViewTextBoxColumn1.DataPropertyName = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn1.HeaderText = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.customerSurnameDataGridViewTextBoxColumn1.Name = "customerSurnameDataGridViewTextBoxColumn1";
            // 
            // customerEmailDataGridViewTextBoxColumn1
            // 
            this.customerEmailDataGridViewTextBoxColumn1.DataPropertyName = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn1.HeaderText = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.customerEmailDataGridViewTextBoxColumn1.Name = "customerEmailDataGridViewTextBoxColumn1";
            // 
            // customerPhoneDataGridViewTextBoxColumn1
            // 
            this.customerPhoneDataGridViewTextBoxColumn1.DataPropertyName = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn1.HeaderText = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.customerPhoneDataGridViewTextBoxColumn1.Name = "customerPhoneDataGridViewTextBoxColumn1";
            // 
            // appointmentDateDataGridViewTextBoxColumn1
            // 
            this.appointmentDateDataGridViewTextBoxColumn1.DataPropertyName = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn1.HeaderText = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.appointmentDateDataGridViewTextBoxColumn1.Name = "appointmentDateDataGridViewTextBoxColumn1";
            // 
            // timeslotDataGridViewTextBoxColumn1
            // 
            this.timeslotDataGridViewTextBoxColumn1.DataPropertyName = "Timeslot";
            this.timeslotDataGridViewTextBoxColumn1.HeaderText = "Timeslot";
            this.timeslotDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.timeslotDataGridViewTextBoxColumn1.Name = "timeslotDataGridViewTextBoxColumn1";
            // 
            // appointmentDetailBindingSource1
            // 
            this.appointmentDetailBindingSource1.DataMember = "AppointmentDetail";
            this.appointmentDetailBindingSource1.DataSource = this.dsAppointment;
            // 
            // taAppointmentDetail
            // 
            this.taAppointmentDetail.ClearBeforeFill = true;
            // 
            // taAvailableTimeslots
            // 
            this.taAvailableTimeslots.ClearBeforeFill = true;
            // 
            // taAppointment
            // 
            this.taAppointment.ClearBeforeFill = true;
            // 
            // appointmentDetailBindingSource
            // 
            this.appointmentDetailBindingSource.DataMember = "AppointmentDetail";
            this.appointmentDetailBindingSource.DataSource = this.dsAppointment;
            // 
            // taCustomer
            // 
            this.taCustomer.ClearBeforeFill = true;
            // 
            // Appointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1545, 830);
            this.Controls.Add(this.tabControl2);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Appointment";
            this.Text = "Appointment";
            this.Load += new System.EventHandler(this.Appointment_Load);
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dTTimeslotsAvailableBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboTimeslot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvCustomer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCustID;
        private System.Windows.Forms.Label lblSurname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch1;
        private dsAppointment dsAppointment;
        private dsAppointmentTableAdapters.AppointmentDetailTableAdapter taAppointmentDetail;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private dsAppointmentTableAdapters.DTTimeslotsAvailableTableAdapter taAvailableTimeslots;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSurnameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerEmailDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPhoneDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn appointmentDateDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeslotDataGridViewTextBoxColumn1;
        private System.Windows.Forms.BindingSource appointmentDetailBindingSource1;
        private System.Windows.Forms.BindingSource dTTimeslotsAvailableBindingSource;
        private dsAppointmentTableAdapters.AppointmentTableAdapter taAppointment;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSurnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerEmailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource appointmentDetailBindingSource;
        private System.Windows.Forms.BindingSource customerBindingSource;
        private dsAppointmentTableAdapters.CustomerTableAdapter taCustomer;
        private System.Windows.Forms.MonthCalendar monthCalendar2;
    }
}