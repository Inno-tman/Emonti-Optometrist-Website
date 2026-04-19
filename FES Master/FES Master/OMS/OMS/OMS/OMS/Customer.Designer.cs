
namespace OMS
{
    partial class Customer
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
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
            this.dsOMS = new OMS.dsOMS();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.customerTableAdapter = new OMS.dsOMSTableAdapters.CustomerTableAdapter();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.dataGridView3);
            this.tabPage2.Controls.Add(this.textBox6);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1071, 603);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "View Customers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Location = new System.Drawing.Point(143, 428);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(781, 100);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(475, 38);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(263, 40);
            this.button3.TabIndex = 7;
            this.button3.Text = "DELETE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(32, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(251, 40);
            this.button2.TabIndex = 6;
            this.button2.Text = "UPDATE";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // dataGridView3
            // 
            this.dataGridView3.AutoGenerateColumns = false;
            this.dataGridView3.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dataGridView3.DataSource = this.customerBindingSource;
            this.dataGridView3.Location = new System.Drawing.Point(25, 171);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowHeadersWidth = 51;
            this.dataGridView3.Size = new System.Drawing.Size(1021, 213);
            this.dataGridView3.TabIndex = 5;
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
            this.customerBindingSource.DataSource = this.dsOMS;
            // 
            // dsOMS
            // 
            this.dsOMS.DataSetName = "dsOMS";
            this.dsOMS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(343, 111);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(353, 29);
            this.textBox6.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(405, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(216, 24);
            this.label8.TabIndex = 3;
            this.label8.Text = "SEARCH BY SURNAME";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1071, 603);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Add Customer";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.textBox9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBox7);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Location = new System.Drawing.Point(43, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 514);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(731, 421);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(205, 68);
            this.button1.TabIndex = 16;
            this.button1.Text = "RECORD";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(401, 421);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(183, 24);
            this.label9.TabIndex = 15;
            this.label9.Text = "Medical Aid Number";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(401, 460);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(247, 29);
            this.textBox9.TabIndex = 14;
            this.textBox9.TextChanged += new System.EventHandler(this.textBox9_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 421);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 24);
            this.label7.TabIndex = 13;
            this.label7.Text = "Medical Aid ";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(25, 460);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(247, 29);
            this.textBox8.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(401, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(145, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "Phone Number ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 291);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 24);
            this.label5.TabIndex = 10;
            this.label5.Text = "Email Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(401, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 24);
            this.label4.TabIndex = 9;
            this.label4.Text = "Gender";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Date Of Birth";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "Surname";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "First Name";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(405, 330);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(224, 29);
            this.textBox7.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 90);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(247, 29);
            this.textBox1.TabIndex = 0;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(405, 90);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(224, 29);
            this.textBox5.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(29, 203);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(247, 29);
            this.textBox2.TabIndex = 1;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(29, 330);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(247, 29);
            this.textBox4.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(405, 203);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(224, 29);
            this.textBox3.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1079, 640);
            this.tabControl1.TabIndex = 0;
            // 
            // customerTableAdapter
            // 
            this.customerTableAdapter.ClearBeforeFill = true;
            // 
            // Customer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 640);
            this.Controls.Add(this.tabControl1);
            this.Name = "Customer";
            this.Text = "Customer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Customer_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private dsOMS dsOMS;
        private System.Windows.Forms.BindingSource customerBindingSource;
        private dsOMSTableAdapters.CustomerTableAdapter customerTableAdapter;
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
    }
}