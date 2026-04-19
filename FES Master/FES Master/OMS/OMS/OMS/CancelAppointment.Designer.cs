
namespace OMS
{
    partial class CancelAppointment
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchCustomer = new System.Windows.Forms.TextBox();
            this.dgvCustomers = new System.Windows.Forms.DataGridView();
            this.custIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSurnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerDOBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerGenderDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerEmailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerPhoneDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.medicalAidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.medicalAidNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsAppointment = new OMS.dsAppointment();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCustomerID = new System.Windows.Forms.Label();
            this.lblSelectedCustomer = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.custIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerSurnameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerEmailDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerPhoneDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeslotDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnShowAllAppointments = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.taCustomer = new OMS.dsAppointmentTableAdapters.CustomerTableAdapter();
            this.taAppointmentDetail = new OMS.dsAppointmentTableAdapters.AppointmentDetailTableAdapter();
            this.taAppointment = new OMS.dsAppointmentTableAdapters.AppointmentTableAdapter();
            this.taAppointmentDetail1 = new OMS.dsAppointmentTableAdapters.AppointmentDetailTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(569, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search Customer";
            // 
            // txtSearchCustomer
            // 
            this.txtSearchCustomer.Location = new System.Drawing.Point(444, 66);
            this.txtSearchCustomer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearchCustomer.Name = "txtSearchCustomer";
            this.txtSearchCustomer.Size = new System.Drawing.Size(379, 24);
            this.txtSearchCustomer.TabIndex = 1;
            this.txtSearchCustomer.TextChanged += new System.EventHandler(this.txtSearchCustomer_TextChanged);
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.AutoGenerateColumns = false;
            this.dgvCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCustomers.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custIDDataGridViewTextBoxColumn,
            this.customerNameDataGridViewTextBoxColumn,
            this.customerSurnameDataGridViewTextBoxColumn,
            this.customerDOBDataGridViewTextBoxColumn,
            this.customerGenderDataGridViewTextBoxColumn,
            this.customerEmailDataGridViewTextBoxColumn,
            this.customerPhoneDataGridViewTextBoxColumn,
            this.customerAddressDataGridViewTextBoxColumn,
            this.medicalAidDataGridViewTextBoxColumn,
            this.medicalAidNumberDataGridViewTextBoxColumn});
            this.dgvCustomers.DataSource = this.customerBindingSource;
            this.dgvCustomers.Location = new System.Drawing.Point(81, 108);
            this.dgvCustomers.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.RowHeadersWidth = 51;
            this.dgvCustomers.Size = new System.Drawing.Size(1123, 93);
            this.dgvCustomers.TabIndex = 2;
            this.dgvCustomers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomers_CellClick);
            this.dgvCustomers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomers_CellContentClick);
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
            // customerDOBDataGridViewTextBoxColumn
            // 
            this.customerDOBDataGridViewTextBoxColumn.DataPropertyName = "Customer_DOB";
            this.customerDOBDataGridViewTextBoxColumn.HeaderText = "Customer_DOB";
            this.customerDOBDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerDOBDataGridViewTextBoxColumn.Name = "customerDOBDataGridViewTextBoxColumn";
            // 
            // customerGenderDataGridViewTextBoxColumn
            // 
            this.customerGenderDataGridViewTextBoxColumn.DataPropertyName = "Customer_Gender";
            this.customerGenderDataGridViewTextBoxColumn.HeaderText = "Customer_Gender";
            this.customerGenderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerGenderDataGridViewTextBoxColumn.Name = "customerGenderDataGridViewTextBoxColumn";
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
            // customerAddressDataGridViewTextBoxColumn
            // 
            this.customerAddressDataGridViewTextBoxColumn.DataPropertyName = "Customer_Address";
            this.customerAddressDataGridViewTextBoxColumn.HeaderText = "Customer_Address";
            this.customerAddressDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerAddressDataGridViewTextBoxColumn.Name = "customerAddressDataGridViewTextBoxColumn";
            // 
            // medicalAidDataGridViewTextBoxColumn
            // 
            this.medicalAidDataGridViewTextBoxColumn.DataPropertyName = "Medical_Aid";
            this.medicalAidDataGridViewTextBoxColumn.HeaderText = "Medical_Aid";
            this.medicalAidDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.medicalAidDataGridViewTextBoxColumn.Name = "medicalAidDataGridViewTextBoxColumn";
            // 
            // medicalAidNumberDataGridViewTextBoxColumn
            // 
            this.medicalAidNumberDataGridViewTextBoxColumn.DataPropertyName = "Medical_Aid_Number";
            this.medicalAidNumberDataGridViewTextBoxColumn.HeaderText = "Medical_Aid_Number";
            this.medicalAidNumberDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.medicalAidNumberDataGridViewTextBoxColumn.Name = "medicalAidNumberDataGridViewTextBoxColumn";
            // 
            // customerBindingSource
            // 
            this.customerBindingSource.DataMember = "Customer";
            this.customerBindingSource.DataSource = this.dsAppointment;
            // 
            // dsAppointment
            // 
            this.dsAppointment.DataSetName = "dsAppointment";
            this.dsAppointment.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox1.Controls.Add(this.dgvCustomers);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSearchCustomer);
            this.groupBox1.Location = new System.Drawing.Point(40, 17);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1272, 239);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupBox2.Controls.Add(this.lblCustomerID);
            this.groupBox2.Controls.Add(this.lblSelectedCustomer);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(81, 25);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(1123, 89);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // lblCustomerID
            // 
            this.lblCustomerID.AutoSize = true;
            this.lblCustomerID.Location = new System.Drawing.Point(836, 43);
            this.lblCustomerID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCustomerID.Name = "lblCustomerID";
            this.lblCustomerID.Size = new System.Drawing.Size(92, 18);
            this.lblCustomerID.TabIndex = 2;
            this.lblCustomerID.Text = "Customer ID";
            // 
            // lblSelectedCustomer
            // 
            this.lblSelectedCustomer.AutoSize = true;
            this.lblSelectedCustomer.Location = new System.Drawing.Point(429, 43);
            this.lblSelectedCustomer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSelectedCustomer.Name = "lblSelectedCustomer";
            this.lblSelectedCustomer.Size = new System.Drawing.Size(48, 18);
            this.lblSelectedCustomer.TabIndex = 1;
            this.lblSelectedCustomer.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 18);
            this.label2.TabIndex = 0;
            this.label2.Text = "Selected Customer:";
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AutoGenerateColumns = false;
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.custIDDataGridViewTextBoxColumn1,
            this.customerNameDataGridViewTextBoxColumn1,
            this.customerSurnameDataGridViewTextBoxColumn1,
            this.customerEmailDataGridViewTextBoxColumn1,
            this.customerPhoneDataGridViewTextBoxColumn1,
            this.appointmentDateDataGridViewTextBoxColumn,
            this.timeslotDataGridViewTextBoxColumn,
            this.appointmentIDDataGridViewTextBoxColumn});
            this.dgvAppointments.DataSource = this.appointmentDetailBindingSource;
            this.dgvAppointments.Location = new System.Drawing.Point(81, 122);
            this.dgvAppointments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.RowHeadersWidth = 51;
            this.dgvAppointments.Size = new System.Drawing.Size(1123, 125);
            this.dgvAppointments.TabIndex = 5;
            this.dgvAppointments.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAppointments_CellClick);
            this.dgvAppointments.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAppointments_RowHeaderMouseDoubleClick);
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
            // appointmentDateDataGridViewTextBoxColumn
            // 
            this.appointmentDateDataGridViewTextBoxColumn.DataPropertyName = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn.HeaderText = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.appointmentDateDataGridViewTextBoxColumn.Name = "appointmentDateDataGridViewTextBoxColumn";
            // 
            // timeslotDataGridViewTextBoxColumn
            // 
            this.timeslotDataGridViewTextBoxColumn.DataPropertyName = "Timeslot";
            this.timeslotDataGridViewTextBoxColumn.HeaderText = "Timeslot";
            this.timeslotDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.timeslotDataGridViewTextBoxColumn.Name = "timeslotDataGridViewTextBoxColumn";
            // 
            // appointmentIDDataGridViewTextBoxColumn
            // 
            this.appointmentIDDataGridViewTextBoxColumn.DataPropertyName = "Appointment_ID";
            this.appointmentIDDataGridViewTextBoxColumn.HeaderText = "Appointment_ID";
            this.appointmentIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.appointmentIDDataGridViewTextBoxColumn.Name = "appointmentIDDataGridViewTextBoxColumn";
            this.appointmentIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // appointmentDetailBindingSource
            // 
            this.appointmentDetailBindingSource.DataMember = "AppointmentDetail";
            this.appointmentDetailBindingSource.DataSource = this.dsAppointment;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnShowAllAppointments);
            this.groupBox3.Controls.Add(this.btnCancel);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.dgvAppointments);
            this.groupBox3.Location = new System.Drawing.Point(40, 264);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(1272, 374);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1070, 292);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(134, 65);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnShowAllAppointments
            // 
            this.btnShowAllAppointments.Location = new System.Drawing.Point(766, 292);
            this.btnShowAllAppointments.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnShowAllAppointments.Name = "btnShowAllAppointments";
            this.btnShowAllAppointments.Size = new System.Drawing.Size(243, 65);
            this.btnShowAllAppointments.TabIndex = 7;
            this.btnShowAllAppointments.Text = "DISPLAY ALL";
            this.btnShowAllAppointments.UseVisualStyleBackColor = true;
            this.btnShowAllAppointments.Click += new System.EventHandler(this.btnShowAllAppointments_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Red;
            this.btnCancel.Location = new System.Drawing.Point(475, 292);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(243, 65);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // taCustomer
            // 
            this.taCustomer.ClearBeforeFill = true;
            // 
            // taAppointmentDetail
            // 
            this.taAppointmentDetail.ClearBeforeFill = true;
            // 
            // taAppointment
            // 
            this.taAppointment.ClearBeforeFill = true;
            // 
            // taAppointmentDetail1
            // 
            this.taAppointmentDetail1.ClearBeforeFill = true;
            // 
            // CancelAppointment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1361, 673);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CancelAppointment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CancelAppointment";
            this.Load += new System.EventHandler(this.CancelAppointment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentDetailBindingSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchCustomer;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCustomerID;
        private System.Windows.Forms.Label lblSelectedCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnShowAllAppointments;
        private System.Windows.Forms.Button btnCancel;
        private dsAppointment dsAppointment;
        private dsAppointmentTableAdapters.CustomerTableAdapter taCustomer;
        private dsAppointmentTableAdapters.AppointmentDetailTableAdapter taAppointmentDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSurnameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerDOBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerGenderDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerEmailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPhoneDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn medicalAidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn medicalAidNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource customerBindingSource;
        private dsAppointmentTableAdapters.AppointmentTableAdapter taAppointment;
        private dsAppointmentTableAdapters.AppointmentDetailTableAdapter taAppointmentDetail1;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerSurnameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerEmailDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPhoneDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn appointmentDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeslotDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn appointmentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource appointmentDetailBindingSource;
    }
}