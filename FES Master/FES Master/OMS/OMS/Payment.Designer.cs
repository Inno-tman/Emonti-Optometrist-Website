
namespace OMS
{
    partial class Payment
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.orderIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custIDDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderTotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deliveryAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsOMS = new OMS.dsOMS();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvAppointments = new System.Windows.Forms.DataGridView();
            this.appointmentIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Appoinment_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.appointmentBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsAppointment = new OMS.dsAppointment();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCustID = new System.Windows.Forms.TextBox();
            this.txtCustName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtOrderID = new System.Windows.Forms.TextBox();
            this.txtAppointID = new System.Windows.Forms.TextBox();
            this.txtTotalPayable = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtOrderTotal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtConsultation = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkOrderPayment = new System.Windows.Forms.CheckBox();
            this.checkConsultation = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.grpboxMedicalAid = new System.Windows.Forms.GroupBox();
            this.txtPatientPortion = new System.Windows.Forms.TextBox();
            this.txtPatientAmountReceived = new System.Windows.Forms.TextBox();
            this.txtPatientChange = new System.Windows.Forms.TextBox();
            this.txtMedicalAidRef = new System.Windows.Forms.TextBox();
            this.txtMedicalAidAmount = new System.Windows.Forms.TextBox();
            this.radPatientEFT = new System.Windows.Forms.RadioButton();
            this.radPatientCard = new System.Windows.Forms.RadioButton();
            this.radPatientCash = new System.Windows.Forms.RadioButton();
            this.lblMedicalAidRef = new System.Windows.Forms.Label();
            this.lblPatientChange = new System.Windows.Forms.Label();
            this.lblPatientAmountReceived = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblPatientPortion = new System.Windows.Forms.Label();
            this.lblMedicalAidAmount = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtChangeDue = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnExact = new System.Windows.Forms.Button();
            this.txtAmountRec = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.radMedAid = new System.Windows.Forms.RadioButton();
            this.radCard = new System.Windows.Forms.RadioButton();
            this.radEFT = new System.Windows.Forms.RadioButton();
            this.radCash = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtPaymentDate = new System.Windows.Forms.TextBox();
            this.txtTransactionNo = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.taCustomer = new OMS.dsOMSTableAdapters.CustomerTableAdapter();
            this.taAppointment = new OMS.dsOMSTableAdapters.AppointmentTableAdapter();
            this.taOrder = new OMS.dsOMSTableAdapters.OrderTableAdapter();
            this.taPayments = new OMS.dsOMSTableAdapters.PaymentsTableAdapter();
            this.taAppointment2 = new OMS.dsAppointmentTableAdapters.AppointmentTableAdapter();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grpboxMedicalAid.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvOrders);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dgvAppointments);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.dgvCustomers);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(24, 16);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(614, 864);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customer Selection";
            // 
            // dgvOrders
            // 
            this.dgvOrders.AutoGenerateColumns = false;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderIDDataGridViewTextBoxColumn,
            this.custIDDataGridViewTextBoxColumn2,
            this.orderDateDataGridViewTextBoxColumn,
            this.orderTotalDataGridViewTextBoxColumn,
            this.orderStatusDataGridViewTextBoxColumn,
            this.deliveryAddressDataGridViewTextBoxColumn});
            this.dgvOrders.DataSource = this.orderBindingSource;
            this.dgvOrders.Location = new System.Drawing.Point(28, 605);
            this.dgvOrders.Margin = new System.Windows.Forms.Padding(4);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.RowHeadersWidth = 51;
            this.dgvOrders.Size = new System.Drawing.Size(536, 129);
            this.dgvOrders.TabIndex = 8;
            this.dgvOrders.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOrders_RowHeaderMouseDoubleClick);
            this.dgvOrders.SelectionChanged += new System.EventHandler(this.dgvOrders_SelectionChanged);
            // 
            // orderIDDataGridViewTextBoxColumn
            // 
            this.orderIDDataGridViewTextBoxColumn.DataPropertyName = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.HeaderText = "OrderID";
            this.orderIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderIDDataGridViewTextBoxColumn.Name = "orderIDDataGridViewTextBoxColumn";
            this.orderIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.orderIDDataGridViewTextBoxColumn.Width = 125;
            // 
            // custIDDataGridViewTextBoxColumn2
            // 
            this.custIDDataGridViewTextBoxColumn2.DataPropertyName = "CustID";
            this.custIDDataGridViewTextBoxColumn2.HeaderText = "CustID";
            this.custIDDataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.custIDDataGridViewTextBoxColumn2.Name = "custIDDataGridViewTextBoxColumn2";
            this.custIDDataGridViewTextBoxColumn2.Width = 125;
            // 
            // orderDateDataGridViewTextBoxColumn
            // 
            this.orderDateDataGridViewTextBoxColumn.DataPropertyName = "Order_Date";
            this.orderDateDataGridViewTextBoxColumn.HeaderText = "Order_Date";
            this.orderDateDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderDateDataGridViewTextBoxColumn.Name = "orderDateDataGridViewTextBoxColumn";
            this.orderDateDataGridViewTextBoxColumn.Width = 125;
            // 
            // orderTotalDataGridViewTextBoxColumn
            // 
            this.orderTotalDataGridViewTextBoxColumn.DataPropertyName = "Order_Total";
            this.orderTotalDataGridViewTextBoxColumn.HeaderText = "Order_Total";
            this.orderTotalDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderTotalDataGridViewTextBoxColumn.Name = "orderTotalDataGridViewTextBoxColumn";
            this.orderTotalDataGridViewTextBoxColumn.Width = 125;
            // 
            // orderStatusDataGridViewTextBoxColumn
            // 
            this.orderStatusDataGridViewTextBoxColumn.DataPropertyName = "Order_Status";
            this.orderStatusDataGridViewTextBoxColumn.HeaderText = "Order_Status";
            this.orderStatusDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderStatusDataGridViewTextBoxColumn.Name = "orderStatusDataGridViewTextBoxColumn";
            this.orderStatusDataGridViewTextBoxColumn.Width = 125;
            // 
            // deliveryAddressDataGridViewTextBoxColumn
            // 
            this.deliveryAddressDataGridViewTextBoxColumn.DataPropertyName = "Delivery_Address";
            this.deliveryAddressDataGridViewTextBoxColumn.HeaderText = "Delivery_Address";
            this.deliveryAddressDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.deliveryAddressDataGridViewTextBoxColumn.Name = "deliveryAddressDataGridViewTextBoxColumn";
            this.deliveryAddressDataGridViewTextBoxColumn.Width = 125;
            // 
            // orderBindingSource
            // 
            this.orderBindingSource.DataMember = "Order";
            this.orderBindingSource.DataSource = this.dsOMS;
            // 
            // dsOMS
            // 
            this.dsOMS.DataSetName = "dsOMS";
            this.dsOMS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 581);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Pending Orders:";
            // 
            // dgvAppointments
            // 
            this.dgvAppointments.AutoGenerateColumns = false;
            this.dgvAppointments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAppointments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAppointments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.appointmentIDDataGridViewTextBoxColumn,
            this.appointmentDateDataGridViewTextBoxColumn,
            this.Appoinment_Status});
            this.dgvAppointments.DataSource = this.appointmentBindingSource;
            this.dgvAppointments.Location = new System.Drawing.Point(28, 438);
            this.dgvAppointments.Margin = new System.Windows.Forms.Padding(4);
            this.dgvAppointments.Name = "dgvAppointments";
            this.dgvAppointments.RowHeadersWidth = 51;
            this.dgvAppointments.Size = new System.Drawing.Size(540, 129);
            this.dgvAppointments.TabIndex = 6;
            this.dgvAppointments.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAppointments_RowHeaderMouseDoubleClick);
            this.dgvAppointments.SelectionChanged += new System.EventHandler(this.dgvAppointments_SelectionChanged);
            // 
            // appointmentIDDataGridViewTextBoxColumn
            // 
            this.appointmentIDDataGridViewTextBoxColumn.DataPropertyName = "Appointment_ID";
            this.appointmentIDDataGridViewTextBoxColumn.HeaderText = "Appointment_ID";
            this.appointmentIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.appointmentIDDataGridViewTextBoxColumn.Name = "appointmentIDDataGridViewTextBoxColumn";
            this.appointmentIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // appointmentDateDataGridViewTextBoxColumn
            // 
            this.appointmentDateDataGridViewTextBoxColumn.DataPropertyName = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn.HeaderText = "Appointment_Date";
            this.appointmentDateDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.appointmentDateDataGridViewTextBoxColumn.Name = "appointmentDateDataGridViewTextBoxColumn";
            // 
            // Appoinment_Status
            // 
            this.Appoinment_Status.DataPropertyName = "Appoinment_Status";
            this.Appoinment_Status.HeaderText = "Status";
            this.Appoinment_Status.MinimumWidth = 6;
            this.Appoinment_Status.Name = "Appoinment_Status";
            // 
            // appointmentBindingSource
            // 
            this.appointmentBindingSource.DataMember = "Appointment";
            this.appointmentBindingSource.DataSource = this.dsAppointment;
            // 
            // dsAppointment
            // 
            this.dsAppointment.DataSetName = "dsAppointment";
            this.dsAppointment.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 413);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Appointments:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCustID);
            this.groupBox2.Controls.Add(this.txtCustName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(32, 277);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(544, 123);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Customer";
            // 
            // txtCustID
            // 
            this.txtCustID.Location = new System.Drawing.Point(144, 68);
            this.txtCustID.Name = "txtCustID";
            this.txtCustID.ReadOnly = true;
            this.txtCustID.Size = new System.Drawing.Size(77, 26);
            this.txtCustID.TabIndex = 3;
            // 
            // txtCustName
            // 
            this.txtCustName.Location = new System.Drawing.Point(144, 34);
            this.txtCustName.Name = "txtCustName";
            this.txtCustName.ReadOnly = true;
            this.txtCustName.Size = new System.Drawing.Size(144, 26);
            this.txtCustName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 74);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Customer ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Name:";
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.AutoGenerateColumns = false;
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
            this.dgvCustomers.Location = new System.Drawing.Point(29, 130);
            this.dgvCustomers.Margin = new System.Windows.Forms.Padding(4);
            this.dgvCustomers.Name = "dgvCustomers";
            this.dgvCustomers.RowHeadersWidth = 51;
            this.dgvCustomers.Size = new System.Drawing.Size(544, 133);
            this.dgvCustomers.TabIndex = 3;
            this.dgvCustomers.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCustomers_RowHeaderMouseDoubleClick);
            // 
            // custIDDataGridViewTextBoxColumn
            // 
            this.custIDDataGridViewTextBoxColumn.DataPropertyName = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn.HeaderText = "Cust_ID";
            this.custIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.custIDDataGridViewTextBoxColumn.Name = "custIDDataGridViewTextBoxColumn";
            this.custIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.custIDDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerNameDataGridViewTextBoxColumn
            // 
            this.customerNameDataGridViewTextBoxColumn.DataPropertyName = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn.HeaderText = "Customer_Name";
            this.customerNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerNameDataGridViewTextBoxColumn.Name = "customerNameDataGridViewTextBoxColumn";
            this.customerNameDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerSurnameDataGridViewTextBoxColumn
            // 
            this.customerSurnameDataGridViewTextBoxColumn.DataPropertyName = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn.HeaderText = "Customer_Surname";
            this.customerSurnameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerSurnameDataGridViewTextBoxColumn.Name = "customerSurnameDataGridViewTextBoxColumn";
            this.customerSurnameDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerDOBDataGridViewTextBoxColumn
            // 
            this.customerDOBDataGridViewTextBoxColumn.DataPropertyName = "Customer_DOB";
            this.customerDOBDataGridViewTextBoxColumn.HeaderText = "Customer_DOB";
            this.customerDOBDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerDOBDataGridViewTextBoxColumn.Name = "customerDOBDataGridViewTextBoxColumn";
            this.customerDOBDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerGenderDataGridViewTextBoxColumn
            // 
            this.customerGenderDataGridViewTextBoxColumn.DataPropertyName = "Customer_Gender";
            this.customerGenderDataGridViewTextBoxColumn.HeaderText = "Customer_Gender";
            this.customerGenderDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerGenderDataGridViewTextBoxColumn.Name = "customerGenderDataGridViewTextBoxColumn";
            this.customerGenderDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerEmailDataGridViewTextBoxColumn
            // 
            this.customerEmailDataGridViewTextBoxColumn.DataPropertyName = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn.HeaderText = "Customer_Email";
            this.customerEmailDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerEmailDataGridViewTextBoxColumn.Name = "customerEmailDataGridViewTextBoxColumn";
            this.customerEmailDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerPhoneDataGridViewTextBoxColumn
            // 
            this.customerPhoneDataGridViewTextBoxColumn.DataPropertyName = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn.HeaderText = "Customer_Phone";
            this.customerPhoneDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerPhoneDataGridViewTextBoxColumn.Name = "customerPhoneDataGridViewTextBoxColumn";
            this.customerPhoneDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerAddressDataGridViewTextBoxColumn
            // 
            this.customerAddressDataGridViewTextBoxColumn.DataPropertyName = "Customer_Address";
            this.customerAddressDataGridViewTextBoxColumn.HeaderText = "Customer_Address";
            this.customerAddressDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.customerAddressDataGridViewTextBoxColumn.Name = "customerAddressDataGridViewTextBoxColumn";
            this.customerAddressDataGridViewTextBoxColumn.Width = 125;
            // 
            // medicalAidDataGridViewTextBoxColumn
            // 
            this.medicalAidDataGridViewTextBoxColumn.DataPropertyName = "Medical_Aid";
            this.medicalAidDataGridViewTextBoxColumn.HeaderText = "Medical_Aid";
            this.medicalAidDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.medicalAidDataGridViewTextBoxColumn.Name = "medicalAidDataGridViewTextBoxColumn";
            this.medicalAidDataGridViewTextBoxColumn.Width = 125;
            // 
            // medicalAidNumberDataGridViewTextBoxColumn
            // 
            this.medicalAidNumberDataGridViewTextBoxColumn.DataPropertyName = "Medical_Aid_Number";
            this.medicalAidNumberDataGridViewTextBoxColumn.HeaderText = "Medical_Aid_Number";
            this.medicalAidNumberDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.medicalAidNumberDataGridViewTextBoxColumn.Name = "medicalAidNumberDataGridViewTextBoxColumn";
            this.medicalAidNumberDataGridViewTextBoxColumn.Width = 125;
            // 
            // customerBindingSource
            // 
            this.customerBindingSource.DataMember = "Customer";
            this.customerBindingSource.DataSource = this.dsOMS;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Customers:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(28, 66);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(292, 26);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search Customer By Surname:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtOrderID);
            this.groupBox3.Controls.Add(this.txtAppointID);
            this.groupBox3.Controls.Add(this.txtTotalPayable);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtOrderTotal);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.txtConsultation);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.checkOrderPayment);
            this.groupBox3.Controls.Add(this.checkConsultation);
            this.groupBox3.Location = new System.Drawing.Point(665, 22);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(295, 561);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Payments Items";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Order ID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Appointment ID:";
            // 
            // txtOrderID
            // 
            this.txtOrderID.Location = new System.Drawing.Point(169, 189);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.ReadOnly = true;
            this.txtOrderID.Size = new System.Drawing.Size(100, 26);
            this.txtOrderID.TabIndex = 10;
            // 
            // txtAppointID
            // 
            this.txtAppointID.Location = new System.Drawing.Point(169, 148);
            this.txtAppointID.Name = "txtAppointID";
            this.txtAppointID.ReadOnly = true;
            this.txtAppointID.Size = new System.Drawing.Size(100, 26);
            this.txtAppointID.TabIndex = 9;
            // 
            // txtTotalPayable
            // 
            this.txtTotalPayable.Location = new System.Drawing.Point(33, 498);
            this.txtTotalPayable.Margin = new System.Windows.Forms.Padding(4);
            this.txtTotalPayable.Name = "txtTotalPayable";
            this.txtTotalPayable.ReadOnly = true;
            this.txtTotalPayable.Size = new System.Drawing.Size(188, 26);
            this.txtTotalPayable.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 452);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(115, 20);
            this.label11.TabIndex = 7;
            this.label11.Text = "Total Payable:";
            // 
            // txtOrderTotal
            // 
            this.txtOrderTotal.Location = new System.Drawing.Point(33, 393);
            this.txtOrderTotal.Margin = new System.Windows.Forms.Padding(4);
            this.txtOrderTotal.Name = "txtOrderTotal";
            this.txtOrderTotal.ReadOnly = true;
            this.txtOrderTotal.Size = new System.Drawing.Size(189, 26);
            this.txtOrderTotal.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(30, 352);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 20);
            this.label10.TabIndex = 5;
            this.label10.Text = "Order Total:";
            // 
            // txtConsultation
            // 
            this.txtConsultation.Location = new System.Drawing.Point(33, 302);
            this.txtConsultation.Margin = new System.Windows.Forms.Padding(4);
            this.txtConsultation.Name = "txtConsultation";
            this.txtConsultation.ReadOnly = true;
            this.txtConsultation.Size = new System.Drawing.Size(192, 26);
            this.txtConsultation.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(30, 256);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 20);
            this.label9.TabIndex = 3;
            this.label9.Text = "Consultation Fee:";
            // 
            // checkOrderPayment
            // 
            this.checkOrderPayment.AutoSize = true;
            this.checkOrderPayment.Location = new System.Drawing.Point(36, 100);
            this.checkOrderPayment.Margin = new System.Windows.Forms.Padding(4);
            this.checkOrderPayment.Name = "checkOrderPayment";
            this.checkOrderPayment.Size = new System.Drawing.Size(144, 24);
            this.checkOrderPayment.TabIndex = 2;
            this.checkOrderPayment.Text = "Order Payment\r\n";
            this.checkOrderPayment.UseVisualStyleBackColor = true;
            this.checkOrderPayment.CheckedChanged += new System.EventHandler(this.checkOrderPayment_CheckedChanged);
            // 
            // checkConsultation
            // 
            this.checkConsultation.AutoSize = true;
            this.checkConsultation.Location = new System.Drawing.Point(36, 47);
            this.checkConsultation.Margin = new System.Windows.Forms.Padding(4);
            this.checkConsultation.Name = "checkConsultation";
            this.checkConsultation.Size = new System.Drawing.Size(157, 24);
            this.checkConsultation.TabIndex = 0;
            this.checkConsultation.Text = "Consultation Fee";
            this.checkConsultation.UseVisualStyleBackColor = true;
            this.checkConsultation.CheckedChanged += new System.EventHandler(this.checkConsultation_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.grpboxMedicalAid);
            this.groupBox4.Controls.Add(this.btnClose);
            this.groupBox4.Controls.Add(this.btnClear);
            this.groupBox4.Controls.Add(this.btnProcess);
            this.groupBox4.Controls.Add(this.txtChangeDue);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.btnExact);
            this.groupBox4.Controls.Add(this.txtAmountRec);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.radMedAid);
            this.groupBox4.Controls.Add(this.radCard);
            this.groupBox4.Controls.Add(this.radEFT);
            this.groupBox4.Controls.Add(this.radCash);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(985, 22);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(617, 862);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Payment Processing";
            this.groupBox4.Enter += new System.EventHandler(this.groupBox4_Enter);
            // 
            // grpboxMedicalAid
            // 
            this.grpboxMedicalAid.Controls.Add(this.txtPatientPortion);
            this.grpboxMedicalAid.Controls.Add(this.txtPatientAmountReceived);
            this.grpboxMedicalAid.Controls.Add(this.txtPatientChange);
            this.grpboxMedicalAid.Controls.Add(this.txtMedicalAidRef);
            this.grpboxMedicalAid.Controls.Add(this.txtMedicalAidAmount);
            this.grpboxMedicalAid.Controls.Add(this.radPatientEFT);
            this.grpboxMedicalAid.Controls.Add(this.radPatientCard);
            this.grpboxMedicalAid.Controls.Add(this.radPatientCash);
            this.grpboxMedicalAid.Controls.Add(this.lblMedicalAidRef);
            this.grpboxMedicalAid.Controls.Add(this.lblPatientChange);
            this.grpboxMedicalAid.Controls.Add(this.lblPatientAmountReceived);
            this.grpboxMedicalAid.Controls.Add(this.label18);
            this.grpboxMedicalAid.Controls.Add(this.lblPatientPortion);
            this.grpboxMedicalAid.Controls.Add(this.lblMedicalAidAmount);
            this.grpboxMedicalAid.Location = new System.Drawing.Point(31, 352);
            this.grpboxMedicalAid.Name = "grpboxMedicalAid";
            this.grpboxMedicalAid.Size = new System.Drawing.Size(567, 224);
            this.grpboxMedicalAid.TabIndex = 18;
            this.grpboxMedicalAid.TabStop = false;
            // 
            // txtPatientPortion
            // 
            this.txtPatientPortion.Location = new System.Drawing.Point(410, 16);
            this.txtPatientPortion.Name = "txtPatientPortion";
            this.txtPatientPortion.ReadOnly = true;
            this.txtPatientPortion.Size = new System.Drawing.Size(100, 26);
            this.txtPatientPortion.TabIndex = 13;
            // 
            // txtPatientAmountReceived
            // 
            this.txtPatientAmountReceived.Location = new System.Drawing.Point(164, 132);
            this.txtPatientAmountReceived.Name = "txtPatientAmountReceived";
            this.txtPatientAmountReceived.Size = new System.Drawing.Size(100, 26);
            this.txtPatientAmountReceived.TabIndex = 12;
            this.txtPatientAmountReceived.TextChanged += new System.EventHandler(this.txtPatientAmountReceived_TextChanged_1);
            // 
            // txtPatientChange
            // 
            this.txtPatientChange.Location = new System.Drawing.Point(411, 129);
            this.txtPatientChange.Name = "txtPatientChange";
            this.txtPatientChange.ReadOnly = true;
            this.txtPatientChange.Size = new System.Drawing.Size(100, 26);
            this.txtPatientChange.TabIndex = 11;
            this.txtPatientChange.TextChanged += new System.EventHandler(this.txtPatientChange_TextChanged);
            // 
            // txtMedicalAidRef
            // 
            this.txtMedicalAidRef.Location = new System.Drawing.Point(164, 183);
            this.txtMedicalAidRef.Name = "txtMedicalAidRef";
            this.txtMedicalAidRef.Size = new System.Drawing.Size(100, 26);
            this.txtMedicalAidRef.TabIndex = 10;
            // 
            // txtMedicalAidAmount
            // 
            this.txtMedicalAidAmount.Location = new System.Drawing.Point(156, 19);
            this.txtMedicalAidAmount.Name = "txtMedicalAidAmount";
            this.txtMedicalAidAmount.Size = new System.Drawing.Size(100, 26);
            this.txtMedicalAidAmount.TabIndex = 9;
            this.txtMedicalAidAmount.TextChanged += new System.EventHandler(this.txtMedicalAidAmount_TextChanged_1);
            // 
            // radPatientEFT
            // 
            this.radPatientEFT.AutoSize = true;
            this.radPatientEFT.Location = new System.Drawing.Point(209, 100);
            this.radPatientEFT.Name = "radPatientEFT";
            this.radPatientEFT.Size = new System.Drawing.Size(61, 24);
            this.radPatientEFT.TabIndex = 8;
            this.radPatientEFT.TabStop = true;
            this.radPatientEFT.Text = "EFT";
            this.radPatientEFT.UseVisualStyleBackColor = true;
            this.radPatientEFT.CheckedChanged += new System.EventHandler(this.radPatientEFT_CheckedChanged_1);
            // 
            // radPatientCard
            // 
            this.radPatientCard.AutoSize = true;
            this.radPatientCard.Location = new System.Drawing.Point(116, 100);
            this.radPatientCard.Name = "radPatientCard";
            this.radPatientCard.Size = new System.Drawing.Size(78, 24);
            this.radPatientCard.TabIndex = 7;
            this.radPatientCard.TabStop = true;
            this.radPatientCard.Text = "CARD";
            this.radPatientCard.UseVisualStyleBackColor = true;
            this.radPatientCard.CheckedChanged += new System.EventHandler(this.radPatientCard_CheckedChanged_1);
            // 
            // radPatientCash
            // 
            this.radPatientCash.AutoSize = true;
            this.radPatientCash.Location = new System.Drawing.Point(19, 100);
            this.radPatientCash.Name = "radPatientCash";
            this.radPatientCash.Size = new System.Drawing.Size(77, 24);
            this.radPatientCash.TabIndex = 6;
            this.radPatientCash.TabStop = true;
            this.radPatientCash.Text = "CASH";
            this.radPatientCash.UseVisualStyleBackColor = true;
            this.radPatientCash.CheckedChanged += new System.EventHandler(this.radPatientCash_CheckedChanged_1);
            // 
            // lblMedicalAidRef
            // 
            this.lblMedicalAidRef.AutoSize = true;
            this.lblMedicalAidRef.Location = new System.Drawing.Point(6, 189);
            this.lblMedicalAidRef.Name = "lblMedicalAidRef";
            this.lblMedicalAidRef.Size = new System.Drawing.Size(152, 20);
            this.lblMedicalAidRef.TabIndex = 5;
            this.lblMedicalAidRef.Text = "Med Aid Reference\r\n";
            // 
            // lblPatientChange
            // 
            this.lblPatientChange.AutoSize = true;
            this.lblPatientChange.Location = new System.Drawing.Point(275, 132);
            this.lblPatientChange.Name = "lblPatientChange";
            this.lblPatientChange.Size = new System.Drawing.Size(102, 20);
            this.lblPatientChange.TabIndex = 4;
            this.lblPatientChange.Text = "Change Due";
            // 
            // lblPatientAmountReceived
            // 
            this.lblPatientAmountReceived.AutoSize = true;
            this.lblPatientAmountReceived.Location = new System.Drawing.Point(13, 132);
            this.lblPatientAmountReceived.Name = "lblPatientAmountReceived";
            this.lblPatientAmountReceived.Size = new System.Drawing.Size(140, 20);
            this.lblPatientAmountReceived.TabIndex = 3;
            this.lblPatientAmountReceived.Text = "Amount Received";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 70);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(134, 20);
            this.label18.TabIndex = 2;
            this.label18.Text = "Payment Method";
            // 
            // lblPatientPortion
            // 
            this.lblPatientPortion.AutoSize = true;
            this.lblPatientPortion.Location = new System.Drawing.Point(275, 25);
            this.lblPatientPortion.Name = "lblPatientPortion";
            this.lblPatientPortion.Size = new System.Drawing.Size(130, 20);
            this.lblPatientPortion.TabIndex = 1;
            this.lblPatientPortion.Text = "Payable Amount";
            // 
            // lblMedicalAidAmount
            // 
            this.lblMedicalAidAmount.AutoSize = true;
            this.lblMedicalAidAmount.Location = new System.Drawing.Point(6, 22);
            this.lblMedicalAidAmount.Name = "lblMedicalAidAmount";
            this.lblMedicalAidAmount.Size = new System.Drawing.Size(145, 20);
            this.lblMedicalAidAmount.TabIndex = 0;
            this.lblMedicalAidAmount.Text = "Claimable Amount";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(138, 586);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 56);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "CLOSE";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(33, 586);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 56);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "CLEAR";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(358, 585);
            this.btnProcess.Margin = new System.Windows.Forms.Padding(4);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(200, 59);
            this.btnProcess.TabIndex = 15;
            this.btnProcess.Text = "PROCESS PAYMENT";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // txtChangeDue
            // 
            this.txtChangeDue.Location = new System.Drawing.Point(240, 314);
            this.txtChangeDue.Margin = new System.Windows.Forms.Padding(4);
            this.txtChangeDue.Name = "txtChangeDue";
            this.txtChangeDue.ReadOnly = true;
            this.txtChangeDue.Size = new System.Drawing.Size(132, 26);
            this.txtChangeDue.TabIndex = 14;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(29, 320);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(107, 20);
            this.label19.TabIndex = 13;
            this.label19.Text = "Change Due:";
            // 
            // btnExact
            // 
            this.btnExact.Location = new System.Drawing.Point(408, 280);
            this.btnExact.Margin = new System.Windows.Forms.Padding(4);
            this.btnExact.Name = "btnExact";
            this.btnExact.Size = new System.Drawing.Size(100, 60);
            this.btnExact.TabIndex = 10;
            this.btnExact.Text = "EXACT\r\n";
            this.btnExact.UseVisualStyleBackColor = true;
            this.btnExact.Click += new System.EventHandler(this.btnExact_Click);
            // 
            // txtAmountRec
            // 
            this.txtAmountRec.Location = new System.Drawing.Point(31, 280);
            this.txtAmountRec.Margin = new System.Windows.Forms.Padding(4);
            this.txtAmountRec.Name = "txtAmountRec";
            this.txtAmountRec.Size = new System.Drawing.Size(341, 26);
            this.txtAmountRec.TabIndex = 7;
            this.txtAmountRec.TextChanged += new System.EventHandler(this.txtAmountRec_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(29, 256);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(145, 20);
            this.label17.TabIndex = 6;
            this.label17.Text = "Amount Received:";
            // 
            // radMedAid
            // 
            this.radMedAid.AutoSize = true;
            this.radMedAid.Location = new System.Drawing.Point(408, 212);
            this.radMedAid.Margin = new System.Windows.Forms.Padding(4);
            this.radMedAid.Name = "radMedAid";
            this.radMedAid.Size = new System.Drawing.Size(138, 24);
            this.radMedAid.TabIndex = 5;
            this.radMedAid.TabStop = true;
            this.radMedAid.Text = "MEDICAL AID";
            this.radMedAid.UseVisualStyleBackColor = true;
            this.radMedAid.CheckedChanged += new System.EventHandler(this.radMedAid_CheckedChanged);
            // 
            // radCard
            // 
            this.radCard.AutoSize = true;
            this.radCard.Location = new System.Drawing.Point(177, 212);
            this.radCard.Margin = new System.Windows.Forms.Padding(4);
            this.radCard.Name = "radCard";
            this.radCard.Size = new System.Drawing.Size(78, 24);
            this.radCard.TabIndex = 4;
            this.radCard.TabStop = true;
            this.radCard.Text = "CARD";
            this.radCard.UseVisualStyleBackColor = true;
            this.radCard.CheckedChanged += new System.EventHandler(this.radCard_CheckedChanged);
            // 
            // radEFT
            // 
            this.radEFT.AutoSize = true;
            this.radEFT.Location = new System.Drawing.Point(296, 212);
            this.radEFT.Margin = new System.Windows.Forms.Padding(4);
            this.radEFT.Name = "radEFT";
            this.radEFT.Size = new System.Drawing.Size(61, 24);
            this.radEFT.TabIndex = 3;
            this.radEFT.TabStop = true;
            this.radEFT.Text = "EFT";
            this.radEFT.UseVisualStyleBackColor = true;
            this.radEFT.CheckedChanged += new System.EventHandler(this.radEFT_CheckedChanged);
            // 
            // radCash
            // 
            this.radCash.AutoSize = true;
            this.radCash.Location = new System.Drawing.Point(33, 212);
            this.radCash.Margin = new System.Windows.Forms.Padding(4);
            this.radCash.Name = "radCash";
            this.radCash.Size = new System.Drawing.Size(77, 24);
            this.radCash.TabIndex = 2;
            this.radCash.TabStop = true;
            this.radCash.Text = "CASH\r\n";
            this.radCash.UseVisualStyleBackColor = true;
            this.radCash.CheckedChanged += new System.EventHandler(this.radCash_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(35, 178);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(139, 20);
            this.label16.TabIndex = 1;
            this.label16.Text = "Payment Method:";
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Honeydew;
            this.groupBox5.Controls.Add(this.txtPaymentDate);
            this.groupBox5.Controls.Add(this.txtTransactionNo);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.label12);
            this.groupBox5.Location = new System.Drawing.Point(32, 37);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(542, 123);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Transaction Information";
            // 
            // txtPaymentDate
            // 
            this.txtPaymentDate.Location = new System.Drawing.Point(228, 82);
            this.txtPaymentDate.Name = "txtPaymentDate";
            this.txtPaymentDate.ReadOnly = true;
            this.txtPaymentDate.Size = new System.Drawing.Size(178, 26);
            this.txtPaymentDate.TabIndex = 5;
            // 
            // txtTransactionNo
            // 
            this.txtTransactionNo.Location = new System.Drawing.Point(228, 36);
            this.txtTransactionNo.Name = "txtTransactionNo";
            this.txtTransactionNo.ReadOnly = true;
            this.txtTransactionNo.Size = new System.Drawing.Size(178, 26);
            this.txtTransactionNo.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(25, 88);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 20);
            this.label14.TabIndex = 2;
            this.label14.Text = "Payment Date:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(25, 42);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(73, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "Number:\r\n";
            // 
            // taCustomer
            // 
            this.taCustomer.ClearBeforeFill = true;
            // 
            // taAppointment
            // 
            this.taAppointment.ClearBeforeFill = true;
            // 
            // taOrder
            // 
            this.taOrder.ClearBeforeFill = true;
            // 
            // taPayments
            // 
            this.taPayments.ClearBeforeFill = true;
            // 
            // taAppointment2
            // 
            this.taAppointment2.ClearBeforeFill = true;
            // 
            // Payment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1630, 893);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Payment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Payment";
            this.Load += new System.EventHandler(this.Payment_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAppointments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAppointment)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.grpboxMedicalAid.ResumeLayout(false);
            this.grpboxMedicalAid.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dgvAppointments;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtCustID;
        private System.Windows.Forms.TextBox txtCustName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvCustomers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtTotalPayable;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtOrderTotal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtConsultation;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtChangeDue;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button btnExact;
        private System.Windows.Forms.TextBox txtAmountRec;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.RadioButton radMedAid;
        private System.Windows.Forms.RadioButton radCard;
        private System.Windows.Forms.RadioButton radEFT;
        private System.Windows.Forms.RadioButton radCash;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtPaymentDate;
        private System.Windows.Forms.TextBox txtTransactionNo;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
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
        private dsOMS dsOMS;
        private dsOMSTableAdapters.CustomerTableAdapter taCustomer;
        private dsOMSTableAdapters.AppointmentTableAdapter taAppointment;
        private dsOMSTableAdapters.OrderTableAdapter taOrder;
        private dsAppointment dsAppointment;
        private dsAppointmentTableAdapters.AppointmentTableAdapter taAppointment2;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderTotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource orderBindingSource;
        private System.Windows.Forms.CheckBox checkOrderPayment;
        private System.Windows.Forms.CheckBox checkConsultation;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOrderID;
        private System.Windows.Forms.TextBox txtAppointID;
        private dsOMSTableAdapters.PaymentsTableAdapter taPayments;
        private System.Windows.Forms.GroupBox grpboxMedicalAid;
        private System.Windows.Forms.TextBox txtPatientPortion;
        private System.Windows.Forms.TextBox txtPatientAmountReceived;
        private System.Windows.Forms.TextBox txtPatientChange;
        private System.Windows.Forms.TextBox txtMedicalAidRef;
        private System.Windows.Forms.TextBox txtMedicalAidAmount;
        private System.Windows.Forms.RadioButton radPatientEFT;
        private System.Windows.Forms.RadioButton radPatientCard;
        private System.Windows.Forms.RadioButton radPatientCash;
        private System.Windows.Forms.Label lblMedicalAidRef;
        private System.Windows.Forms.Label lblPatientChange;
        private System.Windows.Forms.Label lblPatientAmountReceived;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblPatientPortion;
        private System.Windows.Forms.Label lblMedicalAidAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn appointmentIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn appointmentDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource appointmentBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Appoinment_Status;
    }
}