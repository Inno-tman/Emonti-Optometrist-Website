
namespace OMS
{
    partial class AddNewOrder
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddtoinvoice = new System.Windows.Forms.Button();
            this.btnCleareInvoice = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbl10date = new System.Windows.Forms.Label();
            this.lbl9CustomerNam = new System.Windows.Forms.Label();
            this.lbl8CustomerID = new System.Windows.Forms.Label();
            this.llblCustID = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOrderTotal = new System.Windows.Forms.TextBox();
            this.btnToAdd = new System.Windows.Forms.Button();
            this.gridviewinvioce = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dsAddOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsAddOrder1 = new OMS.DsAddOrder();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.gridviewordeproduct = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dsOMS1 = new OMS.dsOMS();
            this.productTableAdapter1 = new OMS.dsOMSTableAdapters.ProductTableAdapter();
            this.dsAddOrderTableAdapter = new OMS.DsAddOrderTableAdapters.DsAddOrderTableAdapter();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewinvioce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAddOrderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAddOrder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewordeproduct)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAddtoinvoice);
            this.groupBox2.Controls.Add(this.btnCleareInvoice);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtOrderTotal);
            this.groupBox2.Controls.Add(this.btnToAdd);
            this.groupBox2.Controls.Add(this.gridviewinvioce);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.gridviewordeproduct);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1285, 672);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // btnAddtoinvoice
            // 
            this.btnAddtoinvoice.Location = new System.Drawing.Point(200, 368);
            this.btnAddtoinvoice.Name = "btnAddtoinvoice";
            this.btnAddtoinvoice.Size = new System.Drawing.Size(150, 40);
            this.btnAddtoinvoice.TabIndex = 13;
            this.btnAddtoinvoice.Text = "Add To Invoice";
            this.btnAddtoinvoice.UseVisualStyleBackColor = true;
            this.btnAddtoinvoice.Click += new System.EventHandler(this.btnAddtoinvoice_Click);
            // 
            // btnCleareInvoice
            // 
            this.btnCleareInvoice.Location = new System.Drawing.Point(938, 607);
            this.btnCleareInvoice.Name = "btnCleareInvoice";
            this.btnCleareInvoice.Size = new System.Drawing.Size(150, 40);
            this.btnCleareInvoice.TabIndex = 12;
            this.btnCleareInvoice.Text = "Clear Invoice";
            this.btnCleareInvoice.UseVisualStyleBackColor = true;
            this.btnCleareInvoice.Click += new System.EventHandler(this.btnCleareInvoice_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.Info;
            this.groupBox4.Controls.Add(this.lbl10date);
            this.groupBox4.Controls.Add(this.lbl9CustomerNam);
            this.groupBox4.Controls.Add(this.lbl8CustomerID);
            this.groupBox4.Controls.Add(this.llblCustID);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(107, 15);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(981, 89);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            // 
            // lbl10date
            // 
            this.lbl10date.AutoSize = true;
            this.lbl10date.Location = new System.Drawing.Point(781, 36);
            this.lbl10date.Name = "lbl10date";
            this.lbl10date.Size = new System.Drawing.Size(0, 13);
            this.lbl10date.TabIndex = 18;
            // 
            // lbl9CustomerNam
            // 
            this.lbl9CustomerNam.AutoSize = true;
            this.lbl9CustomerNam.Location = new System.Drawing.Point(413, 36);
            this.lbl9CustomerNam.Name = "lbl9CustomerNam";
            this.lbl9CustomerNam.Size = new System.Drawing.Size(0, 13);
            this.lbl9CustomerNam.TabIndex = 17;
            // 
            // lbl8CustomerID
            // 
            this.lbl8CustomerID.AutoSize = true;
            this.lbl8CustomerID.Location = new System.Drawing.Point(90, 36);
            this.lbl8CustomerID.Name = "lbl8CustomerID";
            this.lbl8CustomerID.Size = new System.Drawing.Size(0, 13);
            this.lbl8CustomerID.TabIndex = 16;
            // 
            // llblCustID
            // 
            this.llblCustID.AutoSize = true;
            this.llblCustID.Location = new System.Drawing.Point(101, 36);
            this.llblCustID.Name = "llblCustID";
            this.llblCustID.Size = new System.Drawing.Size(0, 13);
            this.llblCustID.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(718, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Date: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(294, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Customer Name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Customer ID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(796, 373);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Order Total: ";
            // 
            // txtOrderTotal
            // 
            this.txtOrderTotal.Location = new System.Drawing.Point(917, 368);
            this.txtOrderTotal.Name = "txtOrderTotal";
            this.txtOrderTotal.Size = new System.Drawing.Size(180, 20);
            this.txtOrderTotal.TabIndex = 9;
            // 
            // btnToAdd
            // 
            this.btnToAdd.Location = new System.Drawing.Point(759, 607);
            this.btnToAdd.Name = "btnToAdd";
            this.btnToAdd.Size = new System.Drawing.Size(150, 40);
            this.btnToAdd.TabIndex = 8;
            this.btnToAdd.Text = "Add To Orders";
            this.btnToAdd.UseVisualStyleBackColor = true;
            this.btnToAdd.Click += new System.EventHandler(this.btnToAdd_Click);
            // 
            // gridviewinvioce
            // 
            this.gridviewinvioce.AutoGenerateColumns = false;
            this.gridviewinvioce.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewinvioce.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.Quantit});
            this.gridviewinvioce.DataSource = this.dsAddOrderBindingSource;
            this.gridviewinvioce.Location = new System.Drawing.Point(200, 428);
            this.gridviewinvioce.Name = "gridviewinvioce";
            this.gridviewinvioce.RowHeadersWidth = 51;
            this.gridviewinvioce.RowTemplate.Height = 24;
            this.gridviewinvioce.Size = new System.Drawing.Size(897, 150);
            this.gridviewinvioce.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "Product_ID";
            this.dataGridViewTextBoxColumn7.HeaderText = "Product_ID";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "Product_Brand";
            this.dataGridViewTextBoxColumn8.HeaderText = "Product_Brand";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Product_Name";
            this.dataGridViewTextBoxColumn9.HeaderText = "Product_Name";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "Product_Description";
            this.dataGridViewTextBoxColumn10.HeaderText = "Product_Description";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "Product_Category";
            this.dataGridViewTextBoxColumn11.HeaderText = "Product_Category";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "Product_Price";
            this.dataGridViewTextBoxColumn12.HeaderText = "Product_Price";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            // 
            // Quantit
            // 
            this.Quantit.HeaderText = "Quantity";
            this.Quantit.Name = "Quantit";
            this.Quantit.ReadOnly = true;
            // 
            // dsAddOrderBindingSource
            // 
            this.dsAddOrderBindingSource.DataMember = "DsAddOrder";
            this.dsAddOrderBindingSource.DataSource = this.dsAddOrder1;
            // 
            // dsAddOrder1
            // 
            this.dsAddOrder1.DataSetName = "DsAddOrder";
            this.dsAddOrder1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(466, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Search Product By Name";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(423, 144);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(319, 20);
            this.textBox3.TabIndex = 5;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // gridviewordeproduct
            // 
            this.gridviewordeproduct.AutoGenerateColumns = false;
            this.gridviewordeproduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewordeproduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.Quantity});
            this.gridviewordeproduct.DataSource = this.bindingSource1;
            this.gridviewordeproduct.Location = new System.Drawing.Point(191, 184);
            this.gridviewordeproduct.Name = "gridviewordeproduct";
            this.gridviewordeproduct.RowHeadersWidth = 51;
            this.gridviewordeproduct.RowTemplate.Height = 24;
            this.gridviewordeproduct.Size = new System.Drawing.Size(897, 150);
            this.gridviewordeproduct.TabIndex = 4;
            this.gridviewordeproduct.RowDividerDoubleClick += new System.Windows.Forms.DataGridViewRowDividerDoubleClickEventHandler(this.gridviewordeproduct_RowDividerDoubleClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Product_ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Product_ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Product_Brand";
            this.dataGridViewTextBoxColumn2.HeaderText = "Product_Brand";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Product_Name";
            this.dataGridViewTextBoxColumn3.HeaderText = "Product_Name";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Product_Description";
            this.dataGridViewTextBoxColumn4.HeaderText = "Product_Description";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "Product_Category";
            this.dataGridViewTextBoxColumn5.HeaderText = "Product_Category";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Product_Price";
            this.dataGridViewTextBoxColumn6.HeaderText = "Product_Price";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // Quantity
            // 
            this.Quantity.HeaderText = "Quantity";
            this.Quantity.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.Quantity.Name = "Quantity";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "Product";
            this.bindingSource1.DataSource = this.dsOMS1;
            // 
            // dsOMS1
            // 
            this.dsOMS1.DataSetName = "dsOMS";
            this.dsOMS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productTableAdapter1
            // 
            this.productTableAdapter1.ClearBeforeFill = true;
            // 
            // dsAddOrderTableAdapter
            // 
            this.dsAddOrderTableAdapter.ClearBeforeFill = true;
            // 
            // AddNewOrder
            // 
            this.ClientSize = new System.Drawing.Size(1322, 714);
            this.Controls.Add(this.groupBox2);
            this.Name = "AddNewOrder";
            this.Load += new System.EventHandler(this.AddNewOrder_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewinvioce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAddOrderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsAddOrder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewordeproduct)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCustID;
        private System.Windows.Forms.Label lblCustName;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button AddOrder;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private dsOMS dsOMS;
        private System.Windows.Forms.BindingSource productBindingSource;
        private dsOMSTableAdapters.ProductTableAdapter productTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBrandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productCategoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridView dataGridView1;
       // private DsAddOrderTableAdapters.AddProductTableAdapter TAaddProduct;
        private DsAddOrder dsAddOrder;
        private System.Windows.Forms.BindingSource addProductBindingSource;
        //private DsAddOrderTableAdapters.TaAddProduct taAddProduct1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBrandDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDescriptionDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productCategoryDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productPriceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView gridviewinvioce;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView gridviewordeproduct;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOrderTotal;
        private System.Windows.Forms.Button btnToAdd;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl8CustomerID;
        private System.Windows.Forms.Label llblCustID;
        private System.Windows.Forms.Label lbl10date;
        private System.Windows.Forms.Label lbl9CustomerNam;
        private System.Windows.Forms.Button btnCleareInvoice;
        private dsOMS dsOMS1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private dsOMSTableAdapters.ProductTableAdapter productTableAdapter1;
        private DsAddOrder dsAddOrder1;
        private System.Windows.Forms.BindingSource dsAddOrderBindingSource;
        private DsAddOrderTableAdapters.DsAddOrderTableAdapter dsAddOrderTableAdapter;
        private System.Windows.Forms.Button btnAddtoinvoice;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewComboBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantit;
    }
}