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
        // Public properties to receive data from the Order form
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OrderDate { get; set; }
        decimal OrderTotal = 0;
        public AddNewOrder()
        {
            InitializeComponent();
        }

        private void AddNewOrder_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dsOMS1.Product' table. You can move, or remove it, as needed.
            this.productTableAdapter1.Fill(this.dsOMS1.Product);
            // Display the received data in the labels
            lbl8CustomerID.Text = CustomerID;
            lbl9CustomerNam.Text = CustomerName;
            lbl10date.Text = OrderDate;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            productTableAdapter1.FillByName1(dsOMS1.Product, textBox3.Text);
        }

        private void gridviewordeproduct_RowDividerDoubleClick(object sender, DataGridViewRowDividerDoubleClickEventArgs e)
        {

        }

        private void btnAddtoinvoice_Click(object sender, EventArgs e)
        {

            try
            {
                // Reset order total
                OrderTotal = 0;
                // Check if a valid row is selected
                if (gridviewordeproduct.CurrentRow != null && gridviewordeproduct.CurrentRow.Cells[0].Value != null)
                {
                    // Get the Product_ID from the selected row (assuming it's in column 0)
                    int productID = Convert.ToInt32(gridviewordeproduct.CurrentRow.Cells[0].Value);

                    // Get the quantity value from the selected row
                    var selectedQuantity = gridviewordeproduct.CurrentRow.Cells[6].Value;

                    // Check if quantity is selected
                    if (selectedQuantity == null || selectedQuantity.ToString().Trim() == "")
                    {
                        MessageBox.Show("Please select a quantity before adding to invoice.", "No Quantity Selected",
                                       MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return; // Exit the method without adding to invoice
                    }

                    // Add the product to the invoice
                    dsAddOrderTableAdapter.FillByID(dsAddOrder1._DsAddOrder, productID);

                    // Set the quantity value in the newly added row
                    if (gridviewinvioce.Rows.Count > 0 && selectedQuantity != null)
                    {
                        int lastRowIndex = gridviewinvioce.Rows.Count - 2; // -2 because last row is usually empty
                        if (lastRowIndex >= 0)
                        {
                            gridviewinvioce.Rows[lastRowIndex].Cells[6].Value = selectedQuantity;
                        }
                    }

                    // Calculate the order total from the gridviewinvoice

                    for (int i = 0; i < gridviewinvioce.Rows.Count - 1; i++)
                    {
                        if (gridviewinvioce.Rows[i].Cells[5].Value != null && gridviewinvioce.Rows[i].Cells[6].Value != null)
                        {
                            decimal price = Convert.ToDecimal(gridviewinvioce.Rows[i].Cells[5].Value);
                            int qty = Convert.ToInt32(gridviewinvioce.Rows[i].Cells[6].Value);
                            OrderTotal += (price * qty);
                        }
                    }
                    // Update the Order Total display
                    txtOrderTotal.Text = OrderTotal.ToString("C");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading product details: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void btnToAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string customerID = lbl8CustomerID.Text;
                string orderDateText = lbl10date.Text;
                string orderTotalText = txtOrderTotal.Text;
                string orderStatus = "pending";

                decimal orderTotal = decimal.Parse(orderTotalText.Replace("R", ""));
                DateTime orderDate = DateTime.Parse(orderDateText);

            
                dsOMSTableAdapters.OrderTableAdapter orderAdapter = new dsOMSTableAdapters.OrderTableAdapter();
                orderAdapter.InsertQuery1(Convert.ToInt32(customerID), orderDate, orderTotal, orderStatus);
                orderAdapter.Fill(this.dsOMS1.Order);

                MessageBox.Show("Order added successfully!", "Success",
                               MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding order: " + ex.Message, "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCleareInvoice_Click(object sender, EventArgs e)
        {

        }
    }
    }
        
