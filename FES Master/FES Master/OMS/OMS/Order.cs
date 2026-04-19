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
    public partial class Order : Form
    {
        decimal OrderTotal = 0;
      


        public Order()
        {
            InitializeComponent();
        }

        private void Order_Load(object sender, EventArgs e)
        {
            taCustomer.Fill(dsOMS.Customer);
            //taOrder.Fill(dsOMS.Customer);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            taCustomer.FillBySurname(dsOMS.Customer, txtSearchCustomer.Text);
        }

        private void btnMakeOrder_Click(object sender, EventArgs e)
        {
            if (gridviewCustomers.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = gridviewCustomers.SelectedRows[0];
                string customerID = selectedRow.Cells[0].Value.ToString();
                string customerName = selectedRow.Cells[1].Value.ToString() + " " + selectedRow.Cells[2].Value.ToString();
                string customerAddress = selectedRow.Cells[7].Value.ToString();
                string orderDate = DateTime.Today.ToShortDateString();

                // Create and configure the AddNewOrder form
                AddNewOrder addOrderForm = new AddNewOrder();

                // Set the label values directly
                addOrderForm.LblCustomerID.Text = customerID;
                addOrderForm.LblCustomerName.Text = customerName;
                addOrderForm.LblDate.Text = orderDate;
                addOrderForm.LblAddress.Text = customerAddress;

                // Subscribe to the form closed event to refresh order history
                addOrderForm.FormClosed += (s, args) => {
                    // Refresh the order history for the selected customer
                    taOrder.FillByCustID(dsOMS.Order, (int)gridviewCustomers.CurrentRow.Cells[0].Value);
                    lblOrderCustomerID.Text = gridviewCustomers.CurrentRow.Cells[0].Value.ToString();

                    // Recalculate order total
                    decimal total = 0;
                    for (int i = 0; i < gridviewOrders.Rows.Count - 1; i++)
                    {
                        total += (decimal)gridviewOrders.Rows[i].Cells[3].Value;
                    }
                    txtOrderTotal.Text = total.ToString("C2");
                };

                // Show the AddNewOrder form
                addOrderForm.Show();
            }
            else if (gridviewCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to proceed.", "No Customer Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Please select only one customer.", "Multiple Customers Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void gridviewCustomers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            taOrder.FillByCustID(dsOMS.Order, (int)gridviewCustomers.CurrentRow.Cells[0].Value);
            lblOrderCustomerID.Text = gridviewCustomers.CurrentRow.Cells[0].Value.ToString();

            decimal OrderTotal = 0;
            decimal total = 0;
            for (int i = 0; i < gridviewOrders.Rows.Count - 1; i++)
            {
                total += (decimal)gridviewOrders.Rows[i].Cells[3].Value;
            }
            txtOrderTotal.Text = total.ToString("C2");
        }

        private void lblPurchaseCustomerID_Click(object sender, EventArgs e)
        {

        }
    }
}
