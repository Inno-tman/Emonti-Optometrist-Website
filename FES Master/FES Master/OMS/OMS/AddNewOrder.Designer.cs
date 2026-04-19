
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddToInvoice = new System.Windows.Forms.Button();
            this.btnClearCart = new System.Windows.Forms.Button();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.gridviewCart = new System.Windows.Forms.DataGridView();
            this.dsOMS = new OMS.dsOMS();
            this.txtCartTotal = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gridviewProducts = new System.Windows.Forms.DataGridView();
            this.txtSearchCategory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.lblCustomerID = new System.Windows.Forms.Label();
            this.taOrder = new OMS.dsOMSTableAdapters.OrderTableAdapter();
            this.taProducts2 = new OMS.dsOMSTableAdapters.Products2TableAdapter();
            this.products2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsOMS1 = new OMS.dsOMS();
            this.cartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productPriceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subtotalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productBrandDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productCategoryDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityOnHandDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewCart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewProducts)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cartBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.btnAddToInvoice);
            this.groupBox1.Controls.Add(this.btnClearCart);
            this.groupBox1.Controls.Add(this.btnPlaceOrder);
            this.groupBox1.Controls.Add(this.gridviewCart);
            this.groupBox1.Controls.Add(this.txtCartTotal);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.gridviewProducts);
            this.groupBox1.Controls.Add(this.txtSearchCategory);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(44, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1312, 780);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(71, 695);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(182, 59);
            this.btnRemove.TabIndex = 10;
            this.btnRemove.Text = "REMOVE FROM CART";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddToInvoice
            // 
            this.btnAddToInvoice.Location = new System.Drawing.Point(983, 403);
            this.btnAddToInvoice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnAddToInvoice.Name = "btnAddToInvoice";
            this.btnAddToInvoice.Size = new System.Drawing.Size(258, 59);
            this.btnAddToInvoice.TabIndex = 9;
            this.btnAddToInvoice.Text = "ADD TO CART";
            this.btnAddToInvoice.UseVisualStyleBackColor = true;
            this.btnAddToInvoice.Click += new System.EventHandler(this.btnAddToInvoice_Click);
            // 
            // btnClearCart
            // 
            this.btnClearCart.Location = new System.Drawing.Point(293, 695);
            this.btnClearCart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnClearCart.Name = "btnClearCart";
            this.btnClearCart.Size = new System.Drawing.Size(182, 59);
            this.btnClearCart.TabIndex = 8;
            this.btnClearCart.Text = "CLEAR CART";
            this.btnClearCart.UseVisualStyleBackColor = true;
            this.btnClearCart.Click += new System.EventHandler(this.btnClearCart_Click);
            // 
            // btnPlaceOrder
            // 
            this.btnPlaceOrder.Location = new System.Drawing.Point(983, 687);
            this.btnPlaceOrder.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(258, 67);
            this.btnPlaceOrder.TabIndex = 7;
            this.btnPlaceOrder.Text = "PLACE ORDER";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            this.btnPlaceOrder.Click += new System.EventHandler(this.btnPlaceOrder_Click);
            // 
            // gridviewCart
            // 
            this.gridviewCart.AutoGenerateColumns = false;
            this.gridviewCart.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridviewCart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewCart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productIDDataGridViewTextBoxColumn1,
            this.productNameDataGridViewTextBoxColumn1,
            this.productPriceDataGridViewTextBoxColumn1,
            this.quantityDataGridViewTextBoxColumn,
            this.subtotalDataGridViewTextBoxColumn});
            this.gridviewCart.DataSource = this.cartBindingSource;
            this.gridviewCart.Location = new System.Drawing.Point(71, 501);
            this.gridviewCart.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridviewCart.Name = "gridviewCart";
            this.gridviewCart.RowHeadersWidth = 51;
            this.gridviewCart.Size = new System.Drawing.Size(1170, 157);
            this.gridviewCart.TabIndex = 6;
            this.gridviewCart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridviewCart_CellContentClick);
            this.gridviewCart.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridviewCart_CellValueChanged_1);
            // 
            // dsOMS
            // 
            this.dsOMS.DataSetName = "dsOMS";
            this.dsOMS.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // txtCartTotal
            // 
            this.txtCartTotal.Location = new System.Drawing.Point(278, 436);
            this.txtCartTotal.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtCartTotal.Name = "txtCartTotal";
            this.txtCartTotal.ReadOnly = true;
            this.txtCartTotal.Size = new System.Drawing.Size(196, 30);
            this.txtCartTotal.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 442);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "CART TOTAL:";
            // 
            // gridviewProducts
            // 
            this.gridviewProducts.AutoGenerateColumns = false;
            this.gridviewProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridviewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridviewProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.productIDDataGridViewTextBoxColumn,
            this.productBrandDataGridViewTextBoxColumn,
            this.productNameDataGridViewTextBoxColumn,
            this.productDescriptionDataGridViewTextBoxColumn,
            this.productCategoryDataGridViewTextBoxColumn,
            this.productPriceDataGridViewTextBoxColumn,
            this.quantityOnHandDataGridViewTextBoxColumn,
            this.Quantity});
            this.gridviewProducts.DataSource = this.products2BindingSource;
            this.gridviewProducts.Location = new System.Drawing.Point(71, 212);
            this.gridviewProducts.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridviewProducts.Name = "gridviewProducts";
            this.gridviewProducts.RowHeadersWidth = 51;
            this.gridviewProducts.Size = new System.Drawing.Size(1170, 155);
            this.gridviewProducts.TabIndex = 3;
            this.gridviewProducts.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridviewProducts_RowHeaderMouseClick);
            // 
            // txtSearchCategory
            // 
            this.txtSearchCategory.Location = new System.Drawing.Point(367, 166);
            this.txtSearchCategory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearchCategory.Name = "txtSearchCategory";
            this.txtSearchCategory.Size = new System.Drawing.Size(579, 30);
            this.txtSearchCategory.TabIndex = 2;
            this.txtSearchCategory.TextChanged += new System.EventHandler(this.txtSearchCategory_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(557, 132);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "SEARCH BY CATEGORY";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.AliceBlue;
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.lblAddress);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblDate);
            this.groupBox2.Controls.Add(this.lblCustomerName);
            this.groupBox2.Controls.Add(this.lblCustomerID);
            this.groupBox2.Location = new System.Drawing.Point(71, 18);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1170, 89);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(338, 40);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 25);
            this.label6.TabIndex = 7;
            this.label6.Text = "Address:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(464, 40);
            this.lblAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(23, 25);
            this.lblAddress.TabIndex = 6;
            this.lblAddress.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 25);
            this.label5.TabIndex = 5;
            this.label5.Text = "Name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(713, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(966, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "DATE:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(1084, 40);
            this.lblDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(23, 25);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "0";
            this.lblDate.Click += new System.EventHandler(this.lblDate_Click);
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Location = new System.Drawing.Point(118, 40);
            this.lblCustomerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(23, 25);
            this.lblCustomerName.TabIndex = 1;
            this.lblCustomerName.Text = "0";
            // 
            // lblCustomerID
            // 
            this.lblCustomerID.AutoSize = true;
            this.lblCustomerID.Location = new System.Drawing.Point(783, 40);
            this.lblCustomerID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCustomerID.Name = "lblCustomerID";
            this.lblCustomerID.Size = new System.Drawing.Size(23, 25);
            this.lblCustomerID.TabIndex = 0;
            this.lblCustomerID.Text = "0";
            // 
            // taOrder
            // 
            this.taOrder.ClearBeforeFill = true;
            // 
            // taProducts2
            // 
            this.taProducts2.ClearBeforeFill = true;
            // 
            // products2BindingSource
            // 
            this.products2BindingSource.DataMember = "Products2";
            this.products2BindingSource.DataSource = this.dsOMS;
            // 
            // dsOMS1
            // 
            this.dsOMS1.DataSetName = "dsOMS";
            this.dsOMS1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cartBindingSource
            // 
            this.cartBindingSource.DataMember = "Cart";
            this.cartBindingSource.DataSource = this.dsOMS1;
            // 
            // productIDDataGridViewTextBoxColumn1
            // 
            this.productIDDataGridViewTextBoxColumn1.DataPropertyName = "Product_ID";
            this.productIDDataGridViewTextBoxColumn1.HeaderText = "Product_ID";
            this.productIDDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.productIDDataGridViewTextBoxColumn1.Name = "productIDDataGridViewTextBoxColumn1";
            // 
            // productNameDataGridViewTextBoxColumn1
            // 
            this.productNameDataGridViewTextBoxColumn1.DataPropertyName = "Product_Name";
            this.productNameDataGridViewTextBoxColumn1.HeaderText = "Product_Name";
            this.productNameDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.productNameDataGridViewTextBoxColumn1.Name = "productNameDataGridViewTextBoxColumn1";
            // 
            // productPriceDataGridViewTextBoxColumn1
            // 
            this.productPriceDataGridViewTextBoxColumn1.DataPropertyName = "Product_Price";
            this.productPriceDataGridViewTextBoxColumn1.HeaderText = "Product_Price";
            this.productPriceDataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.productPriceDataGridViewTextBoxColumn1.Name = "productPriceDataGridViewTextBoxColumn1";
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Quantity";
            this.quantityDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            // 
            // subtotalDataGridViewTextBoxColumn
            // 
            this.subtotalDataGridViewTextBoxColumn.DataPropertyName = "Subtotal";
            this.subtotalDataGridViewTextBoxColumn.HeaderText = "Subtotal";
            this.subtotalDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.subtotalDataGridViewTextBoxColumn.Name = "subtotalDataGridViewTextBoxColumn";
            // 
            // productIDDataGridViewTextBoxColumn
            // 
            this.productIDDataGridViewTextBoxColumn.DataPropertyName = "Product_ID";
            this.productIDDataGridViewTextBoxColumn.HeaderText = "Product_ID";
            this.productIDDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productIDDataGridViewTextBoxColumn.Name = "productIDDataGridViewTextBoxColumn";
            this.productIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // productBrandDataGridViewTextBoxColumn
            // 
            this.productBrandDataGridViewTextBoxColumn.DataPropertyName = "Product_Brand";
            this.productBrandDataGridViewTextBoxColumn.HeaderText = "Product_Brand";
            this.productBrandDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productBrandDataGridViewTextBoxColumn.Name = "productBrandDataGridViewTextBoxColumn";
            // 
            // productNameDataGridViewTextBoxColumn
            // 
            this.productNameDataGridViewTextBoxColumn.DataPropertyName = "Product_Name";
            this.productNameDataGridViewTextBoxColumn.HeaderText = "Product_Name";
            this.productNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productNameDataGridViewTextBoxColumn.Name = "productNameDataGridViewTextBoxColumn";
            // 
            // productDescriptionDataGridViewTextBoxColumn
            // 
            this.productDescriptionDataGridViewTextBoxColumn.DataPropertyName = "Product_Description";
            this.productDescriptionDataGridViewTextBoxColumn.HeaderText = "Product_Description";
            this.productDescriptionDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productDescriptionDataGridViewTextBoxColumn.Name = "productDescriptionDataGridViewTextBoxColumn";
            // 
            // productCategoryDataGridViewTextBoxColumn
            // 
            this.productCategoryDataGridViewTextBoxColumn.DataPropertyName = "Product_Category";
            this.productCategoryDataGridViewTextBoxColumn.HeaderText = "Product_Category";
            this.productCategoryDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productCategoryDataGridViewTextBoxColumn.Name = "productCategoryDataGridViewTextBoxColumn";
            // 
            // productPriceDataGridViewTextBoxColumn
            // 
            this.productPriceDataGridViewTextBoxColumn.DataPropertyName = "Product_Price";
            this.productPriceDataGridViewTextBoxColumn.HeaderText = "Product_Price";
            this.productPriceDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.productPriceDataGridViewTextBoxColumn.Name = "productPriceDataGridViewTextBoxColumn";
            // 
            // quantityOnHandDataGridViewTextBoxColumn
            // 
            this.quantityOnHandDataGridViewTextBoxColumn.DataPropertyName = "QuantityOnHand";
            this.quantityOnHandDataGridViewTextBoxColumn.HeaderText = "QuantityOnHand";
            this.quantityOnHandDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.quantityOnHandDataGridViewTextBoxColumn.Name = "quantityOnHandDataGridViewTextBoxColumn";
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Product_ID";
            this.Quantity.HeaderText = "Select Quantity";
            this.Quantity.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.Quantity.MinimumWidth = 6;
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // AddNewOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1408, 808);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AddNewOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddNewOrder";
            this.Load += new System.EventHandler(this.AddNewOrder_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewCart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridviewProducts)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsOMS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cartBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gridviewProducts;
        private System.Windows.Forms.TextBox txtSearchCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAddToInvoice;
        private System.Windows.Forms.Button btnClearCart;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.DataGridView gridviewCart;
        private System.Windows.Forms.TextBox txtCartTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Label lblCustomerID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private dsOMS dsOMS;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRemove;
        private dsOMSTableAdapters.OrderTableAdapter taOrder;
        private dsOMSTableAdapters.Products2TableAdapter taProducts2;
        private System.Windows.Forms.BindingSource products2BindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productPriceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subtotalDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource cartBindingSource;
        private dsOMS dsOMS1;
        private System.Windows.Forms.DataGridViewTextBoxColumn productIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productBrandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productCategoryDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityOnHandDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn Quantity;
    }
}