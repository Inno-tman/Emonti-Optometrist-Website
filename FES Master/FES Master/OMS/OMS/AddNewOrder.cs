using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMS
{
    public partial class AddNewOrder : Form
    {
        decimal OrderTotal = 0;
        decimal total = 0;

        // Labels Accessible for the Order Form
        public Label LblCustomerID => lblCustomerID;
        public Label LblCustomerName => lblCustomerName;
        public Label LblDate => lblDate;
        public Label LblAddress => lblAddress;

        // Dictionary to store quantity on hand for validation
        private Dictionary<int, int> productStockLookup = new Dictionary<int, int>();

        public AddNewOrder()
        {
            InitializeComponent();
        }

        private void AddNewOrder_Load(object sender, EventArgs e)
        {
            // Add DataError handler to prevent format exceptions
            gridviewProducts.DataError += GridviewProducts_DataError;

            // Fill the products data first
            try
            {
                taProducts2.Fill1(dsOMS.Products2);

                // Setup products grid AFTER data is loaded
                SetupProductsQuantityColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        // Handle DataGridView data errors gracefully
        private void GridviewProducts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Check if the error is in the Quantity column
            if (gridviewProducts.Columns[e.ColumnIndex].Name == "Quantity")
            {
                // Set a default value and suppress the error
                gridviewProducts.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                e.ThrowException = false;
                e.Cancel = false;
            }
        }

        private void SetupProductsQuantityColumn()
        {
            try
            {
                // Find the quantity column
                DataGridViewComboBoxColumn quantityCol = gridviewProducts.Columns["Quantity"] as DataGridViewComboBoxColumn;

                if (quantityCol != null)
                {
                    // CRITICAL: Completely disconnect from data binding
                    quantityCol.DataPropertyName = null;
                    quantityCol.DataSource = null;
                    quantityCol.ValueMember = null;
                    quantityCol.DisplayMember = null;

                    // Clear any existing items
                    quantityCol.Items.Clear();

                    // Add quantity options (1-10)
                    for (int i = 1; i <= 10; i++)
                    {
                        quantityCol.Items.Add(i);
                    }

                    // Set proper display properties
                    quantityCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    quantityCol.FlatStyle = FlatStyle.Standard;
                    quantityCol.ReadOnly = false;
                    quantityCol.DefaultCellStyle.NullValue = 1;
                    quantityCol.DefaultCellStyle.DataSourceNullValue = 1;
                    quantityCol.ValueType = typeof(int);

                    // Set default values after column is properly configured
                    SetDefaultQuantityValues();
                }
                else
                {
                    MessageBox.Show("Quantity column not found or is not a ComboBox column");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error setting up quantity column: " + ex.Message);
            }
        }

        private void SetDefaultQuantityValues()
        {
            try
            {
                // Use BeginInvoke to ensure this happens after the UI is ready
                this.BeginInvoke(new Action(() =>
                {
                    foreach (DataGridViewRow row in gridviewProducts.Rows)
                    {
                        if (!row.IsNewRow && row.Cells["Quantity"] != null && row.Cells["Quantity"].OwningColumn is DataGridViewComboBoxColumn)
                        {
                            try
                            {
                                row.Cells["Quantity"].Value = 1;
                            }
                            catch (Exception cellEx)
                            {
                                // Log the error but don't show message box for each cell
                                System.Diagnostics.Debug.WriteLine($"Error setting cell value: {cellEx.Message}");
                            }
                        }
                    }

                    // Refresh the grid to ensure changes are applied
                    gridviewProducts.Refresh();
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error setting default values: " + ex.Message);
            }
        }

        // Alternative method - if the above still doesn't work, try removing the quantity column entirely and recreating it
        private void RecreateQuantityColumn()
        {
            try
            {
                // Remove existing quantity column if it exists
                if (gridviewProducts.Columns.Contains("Quantity"))
                {
                    int columnIndex = gridviewProducts.Columns["Quantity"].Index;
                    gridviewProducts.Columns.RemoveAt(columnIndex);

                    // Create a new ComboBox column
                    DataGridViewComboBoxColumn newQuantityCol = new DataGridViewComboBoxColumn();
                    newQuantityCol.Name = "Quantity";
                    newQuantityCol.HeaderText = "Select Quantity";
                    newQuantityCol.Width = 120;

                    // Add items
                    for (int i = 1; i <= 10; i++)
                    {
                        newQuantityCol.Items.Add(i);
                    }

                    // Configure the column
                    newQuantityCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    newQuantityCol.FlatStyle = FlatStyle.Standard;
                    newQuantityCol.DefaultCellStyle.NullValue = 1;
                    newQuantityCol.ValueType = typeof(int);

                    // Insert at the original position
                    gridviewProducts.Columns.Insert(columnIndex, newQuantityCol);

                    // Set default values
                    SetDefaultQuantityValues();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error recreating quantity column: " + ex.Message);
            }
        }

        private void lblDate_Click(object sender, EventArgs e)
        {

        }

        private void txtSearchCategory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                taProducts2.FillByCategory(dsOMS.Products2, txtSearchCategory.Text);

                // Reset quantity values after filtering - use BeginInvoke to avoid timing issues
                this.BeginInvoke(new Action(() => SetDefaultQuantityValues()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering products: " + ex.Message);
            }
        }

        private void ResetQuantityComboboxes()
        {
            // Set default quantity to 1 for all visible rows
            foreach (DataGridViewRow row in gridviewProducts.Rows)
            {
                if (!row.IsNewRow && row.Cells["Quantity"] != null)
                {
                    row.Cells["Quantity"].Value = 1;
                }
            }
        }

        private void gridviewProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (gridviewProducts.CurrentRow == null) return;

                // Get selected quantity from combobox
                int selectedQuantity = 1; // default
                if (gridviewProducts.CurrentRow.Cells["Quantity"].Value != null)
                {
                    if (!int.TryParse(gridviewProducts.CurrentRow.Cells["Quantity"].Value.ToString(), out selectedQuantity))
                    {
                        selectedQuantity = 1; // fallback to 1 if parsing fails
                    }
                }

                // Check if product has stock available
                if (Convert.ToInt32(gridviewProducts.CurrentRow.Cells[6].Value) >= selectedQuantity)
                {
                    // Add the product to cart with selected quantity
                    AddProductToCart(selectedQuantity);
                }
                else
                {
                    MessageBox.Show($"Insufficient stock. Available: {gridviewProducts.CurrentRow.Cells[6].Value}, Requested: {selectedQuantity}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product to cart: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProductToCart(int quantity)
        {
            try
            {
                // Get product details
                int productId = Convert.ToInt32(gridviewProducts.CurrentRow.Cells[0].Value);
                string productName = gridviewProducts.CurrentRow.Cells[2].Value.ToString();
                decimal price = Convert.ToDecimal(gridviewProducts.CurrentRow.Cells[5].Value);
                int quantityOnHand = Convert.ToInt32(gridviewProducts.CurrentRow.Cells[6].Value);

                // Store/update stock information for this product
                productStockLookup[productId] = quantityOnHand;

                // Check if product already exists in cart
                bool productExists = false;
                DataRow existingRow = null;

                foreach (DataRow row in dsOMS.Cart.Rows)
                {
                    if (Convert.ToInt32(row[0]) == productId)
                    {
                        productExists = true;
                        existingRow = row;
                        break;
                    }
                }

                if (productExists)
                {
                    // Update existing product quantity
                    int currentQuantity = Convert.ToInt32(existingRow[3]);
                    int newQuantity = currentQuantity + quantity;

                    // Check if new total quantity exceeds stock
                    if (newQuantity > quantityOnHand)
                    {
                        MessageBox.Show($"Cannot add {quantity} more. Total would be {newQuantity} but only {quantityOnHand} available.",
                                       "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Update the existing row
                    existingRow[3] = newQuantity;
                    existingRow[4] = price * newQuantity;
                }
                else
                {
                    // Add new row to cart
                    DataRow dr = dsOMS.Cart.NewRow();
                    dr[0] = productId; // Product_ID
                    dr[1] = productName; // Product_Name
                    dr[2] = price; // Product_Price
                    dr[3] = quantity; // Quantity
                    dr[4] = price * quantity; // Subtotal
                    dsOMS.Cart.Rows.Add(dr);
                }

                // Refresh the cart grid and update total
                gridviewCart.DataSource = dsOMS.Cart;
                GetCartTotal();

                // Reset the quantity combobox to 1
                if (gridviewProducts.CurrentRow.Cells["Quantity"] != null)
                {
                    gridviewProducts.CurrentRow.Cells["Quantity"].Value = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product to cart: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetCartTotal()
        {
            decimal total = 0;

            try
            {
                // Calculate from the DataSet to ensure we get the most up-to-date values
                foreach (DataRow row in dsOMS.Cart.Rows)
                {
                    if (row.RowState != DataRowState.Deleted && row[4] != null && row[4] != DBNull.Value)
                    {
                        total += Convert.ToDecimal(row[4]);
                    }
                }

                // Update the total textbox with proper formatting
                txtCartTotal.Text = total.ToString("C2");

                // Also update the OrderTotal variable if you're using it elsewhere
                OrderTotal = total;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error calculating cart total: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCartTotal.Text = "R0.00";
            }
        }

        private void btnAddToInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridviewProducts.CurrentRow == null)
                {
                    MessageBox.Show("Please select a product first.", "No Product Selected",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get selected quantity from combobox
                int selectedQuantity = 1; // default
                if (gridviewProducts.CurrentRow.Cells["Quantity"].Value != null)
                {
                    if (!int.TryParse(gridviewProducts.CurrentRow.Cells["Quantity"].Value.ToString(), out selectedQuantity))
                    {
                        selectedQuantity = 1; // fallback to 1 if parsing fails
                    }
                }

                // Check if product has stock available
                if (Convert.ToInt32(gridviewProducts.CurrentRow.Cells[6].Value) >= selectedQuantity)
                {
                    // Add the product to cart with selected quantity
                    AddProductToCart(selectedQuantity);
                }
                else
                {
                    MessageBox.Show($"Insufficient stock. Available: {gridviewProducts.CurrentRow.Cells[6].Value}, Requested: {selectedQuantity}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product to cart: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Remove the cart quantity change handlers since cart no longer has combobox
        private void gridviewCart_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            // This method can be removed or simplified since cart no longer has editable quantity
        }

        private void gridviewCart_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // This method can be removed since cart no longer has combobox
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This method can be removed since cart no longer has combobox
        }

        private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // This method can be removed since cart no longer has combobox
        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridviewCart.Rows.Count <= 1) // Only header row
                {
                    MessageBox.Show("Cart is empty. Please add products before placing order.", "Empty Cart",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate all quantities before placing order
                bool validOrder = true;
                string errorMessage = "";

                for (int i = 0; i < gridviewCart.Rows.Count - 1; i++)
                {
                    int quantity = Convert.ToInt32(gridviewCart.Rows[i].Cells[3].Value);
                    int productId = Convert.ToInt32(gridviewCart.Rows[i].Cells[0].Value);
                    string productName = gridviewCart.Rows[i].Cells[1].Value.ToString();

                    // Get quantity on hand from lookup dictionary
                    int quantityOnHand = productStockLookup.ContainsKey(productId) ? productStockLookup[productId] : 999;

                    if (quantity > quantityOnHand)
                    {
                        validOrder = false;
                        errorMessage += $"Product '{productName}': Ordered quantity ({quantity}) exceeds stock ({quantityOnHand})\n";
                    }
                }

                if (!validOrder)
                {
                    MessageBox.Show("Order cannot be placed due to stock issues:\n\n" + errorMessage,
                                   "Stock Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string orderStatus = "Pending";

                // Calculate the actual cart total from DataSet
                decimal cartTotal = 0;
                foreach (DataRow row in dsOMS.Cart.Rows)
                {
                    if (row[4] != null && row[4] != DBNull.Value)
                    {
                        cartTotal += Convert.ToDecimal(row[4]);
                    }
                }

                // Insert order with correct total
                taOrder.InsertNewOrder(Convert.ToInt32(lblCustomerID.Text), DateTime.Now, cartTotal, orderStatus, lblAddress.Text);

                // Update stock levels for all products in the cart
                UpdateProductStock();

                MessageBox.Show("Order Placed Successfully!\nStock levels have been updated.", "Success",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear the cart after successful order placement
                dsOMS.Cart.Clear();
                gridviewCart.DataSource = dsOMS.Cart;
                txtCartTotal.Text = "R0.00";

                // Clear the stock lookup dictionary
                productStockLookup.Clear();

                // Refresh products grid to show updated stock
                taProducts2.Fill1(dsOMS.Products2);
                SetDefaultQuantityValues();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Order HAS NOT Been Placed. Error: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridviewCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // Remove item from cart
                if (gridviewCart.CurrentRow != null && gridviewCart.CurrentRow.Index < gridviewCart.Rows.Count - 1)
                {
                    dsOMS.Cart.Rows.RemoveAt(gridviewCart.CurrentRow.Index);
                    gridviewCart.DataSource = dsOMS.Cart;
                    GetCartTotal();
                }
                else
                {
                    MessageBox.Show("There is no item selected to remove", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing item from cart: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProductStock()
        {
            try
            {
                // Update stock for each product in the cart
                foreach (DataRow cartRow in dsOMS.Cart.Rows)
                {
                    int productId = Convert.ToInt32(cartRow[0]);
                    int orderedQuantity = Convert.ToInt32(cartRow[3]);

                    // Get current stock from lookup
                    if (productStockLookup.ContainsKey(productId))
                    {
                        int currentStock = productStockLookup[productId];
                        int newStock = currentStock - orderedQuantity;

                        // Ensure stock doesn't go below 0
                        if (newStock < 0) newStock = 0;

                        try
                        {
                            // Find the product row in the dataset
                            var productRow = dsOMS.Products2.FindByProduct_ID(productId);
                            if (productRow != null)
                            {
                                // Update the quantity on hand
                                productRow.QuantityOnHand = newStock;

                                // Update the database using the TableAdapter
                                int result = taProducts2.Update(dsOMS.Products2);

                                if (result > 0)
                                {
                                    // Update our lookup dictionary
                                    productStockLookup[productId] = newStock;
                                }
                                else
                                {
                                    MessageBox.Show($"Warning: Could not update stock for Product ID {productId}",
                                                  "Stock Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch (Exception updateEx)
                        {
                            MessageBox.Show($"Error updating stock for Product ID {productId}: {updateEx.Message}",
                                          "Stock Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Warning: Order was placed but stock levels may not have been updated properly.\nError: " + ex.Message,
                               "Stock Update Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear whole cart
                dsOMS.Cart.Clear();
                gridviewCart.DataSource = dsOMS.Cart;
                txtCartTotal.Text = "R0.00";

                // Clear the stock lookup dictionary as well
                productStockLookup.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing cart: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}