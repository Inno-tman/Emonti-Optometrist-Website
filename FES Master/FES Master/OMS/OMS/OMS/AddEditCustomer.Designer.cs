
namespace OMS
{
    partial class AddEditCustomer
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPostalCode = new System.Windows.Forms.TextBox();
            this.comboProvince = new System.Windows.Forms.ComboBox();
            this.txtStreetNumber = new System.Windows.Forms.TextBox();
            this.txtUnitNumber = new System.Windows.Forms.TextBox();
            this.txtComplexName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtStreetName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.dtpDOB = new System.Windows.Forms.DateTimePicker();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMainID = new System.Windows.Forms.TextBox();
            this.txtMainSurname = new System.Windows.Forms.TextBox();
            this.txtMainName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMedical = new System.Windows.Forms.TextBox();
            this.txtMedicalNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbID = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.customerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsOMS = new OMS.dsOMS();
            this.customerTableAdapter = new OMS.dsOMSTableAdapters.CustomerTableAdapter();
            this.label21 = new System.Windows.Forms.Label();
            this.comboMainMemberQ = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.lbID);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(50, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1400, 760);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label20);
            this.groupBox4.Controls.Add(this.label19);
            this.groupBox4.Controls.Add(this.label18);
            this.groupBox4.Controls.Add(this.txtPostalCode);
            this.groupBox4.Controls.Add(this.comboProvince);
            this.groupBox4.Controls.Add(this.txtStreetNumber);
            this.groupBox4.Controls.Add(this.txtUnitNumber);
            this.groupBox4.Controls.Add(this.txtComplexName);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.txtCity);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.txtStreetName);
            this.groupBox4.Controls.Add(this.txtAddress);
            this.groupBox4.Location = new System.Drawing.Point(38, 416);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1327, 263);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(19, 217);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(94, 16);
            this.label20.TabIndex = 52;
            this.label20.Text = "Street Number";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(19, 103);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(82, 16);
            this.label19.TabIndex = 51;
            this.label19.Text = "Postal Code";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 42);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(61, 16);
            this.label18.TabIndex = 50;
            this.label18.Text = "Province";
            // 
            // txtPostalCode
            // 
            this.txtPostalCode.Location = new System.Drawing.Point(184, 100);
            this.txtPostalCode.Name = "txtPostalCode";
            this.txtPostalCode.Size = new System.Drawing.Size(251, 22);
            this.txtPostalCode.TabIndex = 49;
            this.txtPostalCode.TextChanged += new System.EventHandler(this.txtPostalCode_TextChanged);
            // 
            // comboProvince
            // 
            this.comboProvince.FormattingEnabled = true;
            this.comboProvince.Items.AddRange(new object[] {
            "KwaZulu-Natal",
            "Eastern Cape",
            "Mpumalanga",
            "Free State",
            "Gauteng",
            "North West",
            "Northern Cape",
            "Limpopo",
            "Western Cape"});
            this.comboProvince.Location = new System.Drawing.Point(184, 42);
            this.comboProvince.Name = "comboProvince";
            this.comboProvince.Size = new System.Drawing.Size(254, 24);
            this.comboProvince.TabIndex = 48;
            // 
            // txtStreetNumber
            // 
            this.txtStreetNumber.Location = new System.Drawing.Point(184, 217);
            this.txtStreetNumber.Name = "txtStreetNumber";
            this.txtStreetNumber.Size = new System.Drawing.Size(249, 22);
            this.txtStreetNumber.TabIndex = 47;
            // 
            // txtUnitNumber
            // 
            this.txtUnitNumber.Location = new System.Drawing.Point(661, 162);
            this.txtUnitNumber.Name = "txtUnitNumber";
            this.txtUnitNumber.Size = new System.Drawing.Size(216, 22);
            this.txtUnitNumber.TabIndex = 46;
            // 
            // txtComplexName
            // 
            this.txtComplexName.Location = new System.Drawing.Point(661, 103);
            this.txtComplexName.Name = "txtComplexName";
            this.txtComplexName.Size = new System.Drawing.Size(216, 22);
            this.txtComplexName.TabIndex = 45;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(505, 106);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 16);
            this.label12.TabIndex = 44;
            this.label12.Text = "Complex";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(505, 165);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 16);
            this.label10.TabIndex = 43;
            this.label10.Text = "Unit Number";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(505, 47);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 16);
            this.label14.TabIndex = 41;
            this.label14.Text = "Street Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(18, 162);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 16);
            this.label11.TabIndex = 39;
            this.label11.Text = "Suburb / City";
            // 
            // txtCity
            // 
            this.txtCity.Location = new System.Drawing.Point(184, 159);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(249, 22);
            this.txtCity.TabIndex = 40;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(935, 234);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 16);
            this.label13.TabIndex = 37;
            this.label13.Text = "Short Address";
            // 
            // txtStreetName
            // 
            this.txtStreetName.Location = new System.Drawing.Point(661, 44);
            this.txtStreetName.Name = "txtStreetName";
            this.txtStreetName.Size = new System.Drawing.Size(216, 22);
            this.txtStreetName.TabIndex = 42;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(1089, 228);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.ReadOnly = true;
            this.txtAddress.Size = new System.Drawing.Size(217, 22);
            this.txtAddress.TabIndex = 38;
            this.txtAddress.TextChanged += new System.EventHandler(this.txtAddress_TextChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtName);
            this.groupBox3.Controls.Add(this.txtEmail);
            this.groupBox3.Controls.Add(this.dtpDOB);
            this.groupBox3.Controls.Add(this.cmbGender);
            this.groupBox3.Controls.Add(this.txtSurname);
            this.groupBox3.Controls.Add(this.txtDOB);
            this.groupBox3.Controls.Add(this.txtPhone);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(38, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1327, 148);
            this.groupBox3.TabIndex = 47;
            this.groupBox3.TabStop = false;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(189, 33);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(249, 22);
            this.txtName.TabIndex = 17;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(1089, 91);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(216, 22);
            this.txtEmail.TabIndex = 20;
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // dtpDOB
            // 
            this.dtpDOB.Location = new System.Drawing.Point(420, 86);
            this.dtpDOB.Name = "dtpDOB";
            this.dtpDOB.Size = new System.Drawing.Size(22, 22);
            this.dtpDOB.TabIndex = 43;
            this.dtpDOB.ValueChanged += new System.EventHandler(this.dtpDOB_ValueChanged);
            // 
            // cmbGender
            // 
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "Female",
            "Male"});
            this.cmbGender.Location = new System.Drawing.Point(657, 88);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(217, 24);
            this.cmbGender.TabIndex = 45;
            // 
            // txtSurname
            // 
            this.txtSurname.Location = new System.Drawing.Point(658, 33);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(216, 22);
            this.txtSurname.TabIndex = 21;
            this.txtSurname.TextChanged += new System.EventHandler(this.txtSurname_TextChanged);
            // 
            // txtDOB
            // 
            this.txtDOB.Location = new System.Drawing.Point(189, 85);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.ReadOnly = true;
            this.txtDOB.Size = new System.Drawing.Size(221, 22);
            this.txtDOB.TabIndex = 44;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(1089, 30);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(216, 22);
            this.txtPhone.TabIndex = 22;
            this.txtPhone.TextChanged += new System.EventHandler(this.txtPhone_TextChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "First Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(504, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 24;
            this.label2.Text = "Surname";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 25;
            this.label3.Text = "Date Of Birth";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(504, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 26;
            this.label4.Text = "Gender";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(935, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Email Address";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(935, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 16);
            this.label6.TabIndex = 28;
            this.label6.Text = "Phone Number ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboMainMemberQ);
            this.groupBox2.Controls.Add(this.label21);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtMainID);
            this.groupBox2.Controls.Add(this.txtMainSurname);
            this.groupBox2.Controls.Add(this.txtMainName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtMedical);
            this.groupBox2.Controls.Add(this.txtMedicalNo);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(38, 195);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1327, 206);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(504, 115);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(101, 16);
            this.label17.TabIndex = 38;
            this.label17.Text = "Identity Number";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(935, 45);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(147, 16);
            this.label16.TabIndex = 37;
            this.label16.Text = "Main Member Surname";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(504, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(130, 16);
            this.label15.TabIndex = 36;
            this.label15.Text = "Main Member Name";
            // 
            // txtMainID
            // 
            this.txtMainID.Location = new System.Drawing.Point(658, 109);
            this.txtMainID.Name = "txtMainID";
            this.txtMainID.ReadOnly = true;
            this.txtMainID.Size = new System.Drawing.Size(216, 22);
            this.txtMainID.TabIndex = 35;
            this.txtMainID.TextChanged += new System.EventHandler(this.txtMainID_TextChanged);
            // 
            // txtMainSurname
            // 
            this.txtMainSurname.Location = new System.Drawing.Point(1088, 39);
            this.txtMainSurname.Name = "txtMainSurname";
            this.txtMainSurname.ReadOnly = true;
            this.txtMainSurname.Size = new System.Drawing.Size(217, 22);
            this.txtMainSurname.TabIndex = 34;
            this.txtMainSurname.TextChanged += new System.EventHandler(this.txtMainSurname_TextChanged);
            // 
            // txtMainName
            // 
            this.txtMainName.Location = new System.Drawing.Point(658, 39);
            this.txtMainName.Name = "txtMainName";
            this.txtMainName.ReadOnly = true;
            this.txtMainName.Size = new System.Drawing.Size(216, 22);
            this.txtMainName.TabIndex = 33;
            this.txtMainName.TextChanged += new System.EventHandler(this.txtMainName_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 42);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 16);
            this.label7.TabIndex = 30;
            this.label7.Text = "Medical Aid ";
            // 
            // txtMedical
            // 
            this.txtMedical.Location = new System.Drawing.Point(189, 36);
            this.txtMedical.Name = "txtMedical";
            this.txtMedical.Size = new System.Drawing.Size(250, 22);
            this.txtMedical.TabIndex = 29;
            // 
            // txtMedicalNo
            // 
            this.txtMedicalNo.Location = new System.Drawing.Point(189, 109);
            this.txtMedicalNo.Name = "txtMedicalNo";
            this.txtMedicalNo.Size = new System.Drawing.Size(249, 22);
            this.txtMedicalNo.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 16);
            this.label9.TabIndex = 32;
            this.label9.Text = "Medical Aid Number";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(935, 685);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(191, 56);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "CANCEL";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbID
            // 
            this.lbID.AutoSize = true;
            this.lbID.Location = new System.Drawing.Point(168, 22);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(0, 16);
            this.lbID.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(67, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "Customer ID:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1174, 685);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(191, 56);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbTitle.Location = new System.Drawing.Point(659, 9);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(130, 20);
            this.lbTitle.TabIndex = 1;
            this.lbTitle.Text = "Create Customer";
            this.lbTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // customerBindingSource
            // 
            this.customerBindingSource.DataMember = "Customer";
            this.customerBindingSource.DataSource = this.dsOMS;
            // 
            // dsOMS
            // 
            this.dsOMS.DataSetName = "dsOMS";
            this.dsOMS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // customerTableAdapter
            // 
            this.customerTableAdapter.ClearBeforeFill = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(21, 175);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(249, 16);
            this.label21.TabIndex = 39;
            this.label21.Text = "Are You The Medical Aid Main Member?";
            // 
            // comboMainMemberQ
            // 
            this.comboMainMemberQ.FormattingEnabled = true;
            this.comboMainMemberQ.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboMainMemberQ.Location = new System.Drawing.Point(317, 167);
            this.comboMainMemberQ.Name = "comboMainMemberQ";
            this.comboMainMemberQ.Size = new System.Drawing.Size(121, 24);
            this.comboMainMemberQ.TabIndex = 40;
            this.comboMainMemberQ.SelectedIndexChanged += new System.EventHandler(this.comboMainMemberQ_SelectedIndexChanged);
            // 
            // AddEditCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1492, 804);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.groupBox1);
            this.Name = "AddEditCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Customer";
            this.Load += new System.EventHandler(this.AddEditCustomer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource customerBindingSource;
        private dsOMS dsOMS;
        private dsOMSTableAdapters.CustomerTableAdapter customerTableAdapter;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMedicalNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMedical;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSurname;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.TextBox txtDOB;
        private System.Windows.Forms.DateTimePicker dtpDOB;
        private System.Windows.Forms.TextBox txtStreetName;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtUnitNumber;
        private System.Windows.Forms.TextBox txtComplexName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMainID;
        private System.Windows.Forms.TextBox txtMainSurname;
        private System.Windows.Forms.TextBox txtMainName;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPostalCode;
        private System.Windows.Forms.ComboBox comboProvince;
        private System.Windows.Forms.TextBox txtStreetNumber;
        private System.Windows.Forms.ComboBox comboMainMemberQ;
        private System.Windows.Forms.Label label21;
    }
}