
namespace OMS
{
    partial class Order
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblOrderCustomerID = new System.Windows.Forms.Label();
            this.txtOrderTotal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchCustomer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gridviewOrders = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.custIDDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderTotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderStatusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deliveryAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsOMS = new OMS.dsOMS();
            this.gridviewCustomers = new System.Windows.Forms.DataGridView();
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
            this.btnMakeOrder = new System.Windows.Forms.Button();
            this.taCustomer = new OMS.dsOMSTableAdapters.CustomerTableAdapter();
            this.taOrder = new OMS.dsOMSTableAdapters.OrderTableAdapter();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnMakeOrder);
            this.groupBox1.Location = new System.Drawing.Point(30, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1415, 788);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblOrderCustomerID);
            this.groupBox2.Controls.Add(this.txtOrderTotal);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtSearchCustomer);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.gridviewOrders);
            this.groupBox2.Controls.Add(this.gridviewCustomers);
            this.groupBox2.Location = new System.Drawing.Point(69, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1264, 620);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Customer";
            // 
            // lblOrderCustomerID
            // 
            this.lblOrderCustomerID.AutoSize = true;
            this.lblOrderCustomerID.Location = new System.Drawing.Point(264, 374);
            this.lblOrderCustomerID.Name = "lblOrderCustomerID";
            this.lblOrderCustomerID.Size = new System.Drawing.Size(23, 25);
            this.lblOrderCustomerID.TabIndex = 7;
            this.lblOrderCustomerID.Text = "0";
            this.lblOrderCustomerID.Click += new System.EventHandler(this.lblPurchaseCustomerID_Click);
            // 
            // txtOrderTotal
            // 
            this.txtOrderTotal.Location = new System.Drawing.Point(1015, 368);
            this.txtOrderTotal.Name = "txtOrderTotal";
            this.txtOrderTotal.ReadOnly = true;
            this.txtOrderTotal.Size = new System.Drawing.Size(193, 30);
            this.txtOrderTotal.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(846, 374);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Order Total To Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 374);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Order History For Customer:";
            // 
            // txtSearchCustomer
            // 
            this.txtSearchCustomer.Location = new System.Drawing.Point(369, 75);
            this.txtSearchCustomer.Name = "txtSearchCustomer";
            this.txtSearchCustomer.Size = new System.Drawing.Size(493, 30);
            this.txtSearchCustomer.TabIndex = 3;
            this.txtSearchCustomer.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(469, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(350, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "SEARCH CUSTOMER BY SURNAME";
            // 
            // gridviewOrders
            // 
            this.gridviewOrders.AutoGenerateColumns = false;
            this.gridviewOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridviewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.custIDDataGridViewTextBoxColumn2,
            this.orderDateDataGridViewTextBoxColumn,
            this.orderTotalDataGridViewTextBoxColumn,
            this.orderStatusDataGridViewTextBoxColumn,
            this.deliveryAddressDataGridViewTextBoxColumn});
            this.gridviewOrders.DataSource = this.orderBindingSource;
            this.gridviewOrders.Location = new System.Drawing.Point(44, 427);
            this.gridviewOrders.Name = "gridviewOrders";
            this.gridviewOrders.RowHeadersWidth = 51;
            this.gridviewOrders.Size = new System.Drawing.Size(1164, 167);
            this.gridviewOrders.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "OrderID";
            this.dataGridViewTextBoxColumn1.HeaderText = "OrderID";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // custIDDataGridViewTextBoxColumn2
            // 
            this.custIDDataGridViewTextBoxColumn2.DataPropertyName = "CustID";
            this.custIDDataGridViewTextBoxColumn2.HeaderText = "CustID";
            this.custIDDataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.custIDDataGridViewTextBoxColumn2.Name = "custIDDataGridViewTextBoxColumn2";
            // 
            // orderDateDataGridViewTextBoxColumn
            // 
            this.orderDateDataGridViewTextBoxColumn.DataPropertyName = "Order_Date";
            this.orderDateDataGridViewTextBoxColumn.HeaderText = "Order_Date";
            this.orderDateDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderDateDataGridViewTextBoxColumn.Name = "orderDateDataGridViewTextBoxColumn";
            // 
            // orderTotalDataGridViewTextBoxColumn
            // 
            this.orderTotalDataGridViewTextBoxColumn.DataPropertyName = "Order_Total";
            this.orderTotalDataGridViewTextBoxColumn.HeaderText = "Order_Total";
            this.orderTotalDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderTotalDataGridViewTextBoxColumn.Name = "orderTotalDataGridViewTextBoxColumn";
            // 
            // orderStatusDataGridViewTextBoxColumn
            // 
            this.orderStatusDataGridViewTextBoxColumn.DataPropertyName = "Order_Status";
            this.orderStatusDataGridViewTextBoxColumn.HeaderText = "Order_Status";
            this.orderStatusDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.orderStatusDataGridViewTextBoxColumn.Name = "orderStatusDataGridViewTextBoxColumn";
            // 
            // deliveryAddressDataGridViewTextBoxColumn
            // 
            this.deliveryAddressDataGridViewTextBoxColumn.DataPropertyName = "Delivery_Address";
            this.deliveryAddressDataGridViewTextBoxColumn.HeaderText = "Delivery_Address";
            this.deliveryAddressDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.deliveryAddressDataGridViewTextBoxColumn.Name = "deliveryAddressDataGridViewTextBoxColumn";
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
            // gridviewCustomers
            // 
            this.gridviewCustomers.AutoGenerateColumns = false;
            this.gridviewCustomers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridviewCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewCustomers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.gridviewCustomers.DataSource = this.customerBindingSource;
            this.gridviewCustomers.Location = new System.Drawing.Point(44, 121);
            this.gridviewCustomers.Name = "gridviewCustomers";
            this.gridviewCustomers.RowHeadersWidth = 51;
            this.gridviewCustomers.Size = new System.Drawing.Size(1164, 204);
            this.gridviewCustomers.TabIndex = 1;
            this.gridviewCustomers.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridviewCustomers_RowHeaderMouseClick);
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
            // btnMakeOrder
            // 
            this.btnMakeOrder.Location = new System.Drawing.Point(1072, 692);
            this.btnMakeOrder.Name = "btnMakeOrder";
            this.btnMakeOrder.Size = new System.Drawing.Size(261, 69);
            this.btnMakeOrder.TabIndex = 4;
            this.btnMakeOrder.Text = "MAKE ORDER";
            this.btnMakeOrder.UseVisualStyleBackColor = true;
            this.btnMakeOrder.Click += new System.EventHandler(this.btnMakeOrder_Click);
            // 
            // taCustomer
            // 
            this.taCustomer.ClearBeforeFill = true;
            // 
            // taOrder
            // 
            this.taOrder.ClearBeforeFill = true;
            // 
            // Order
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1469, 805);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Order";
            this.Text = "Order";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Order_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSearchCustomer;
        private System.Windows.Forms.Label label1;
       
        private System.Windows.Forms.DataGridView gridviewOrders;
        private System.Windows.Forms.DataGridView gridviewCustomers;
        private System.Windows.Forms.Button btnMakeOrder;
        private dsOMS dsOMS;
        private dsOMSTableAdapters.CustomerTableAdapter taCustomer;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn orderIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn custIDDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderTotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderStatusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deliveryAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource orderBindingSource;
        private dsOMSTableAdapters.OrderTableAdapter taOrder;
        private System.Windows.Forms.Label lblOrderCustomerID;
        private System.Windows.Forms.TextBox txtOrderTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}