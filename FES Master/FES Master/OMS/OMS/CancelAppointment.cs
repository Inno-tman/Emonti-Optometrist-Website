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
    public partial class CancelAppointment : Form
    {
        public CancelAppointment()
        {
            InitializeComponent();
        }

        private void CancelAppointment_Load(object sender, EventArgs e)
        {
            try
            {
                this.taCustomer.Fill(this.dsAppointment.Customer);
                this.taAppointmentDetail.Fill(this.dsAppointment.AppointmentDetail);

                lblSelectedCustomer.Text = "No customer selected";
                lblCustomerID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cancellation form: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.CurrentRow == null || dgvAppointments.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("Please select an appointment to cancel.",
                               "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Based on your screenshot, the columns are:
                // 0: Cust_ID
                // 1: Customer_Name  
                // 2: Customer_Surname
                // 3: Customer_Email
                // 4: Customer_Phone
                // 5: Appointment_Date
                // 6: Timeslot
                // 7: Appointment_ID

                string customerName = lblSelectedCustomer.Text;
                DateTime appointmentDate = Convert.ToDateTime(dgvAppointments.CurrentRow.Cells[5].Value);
                string timeSlot = dgvAppointments.CurrentRow.Cells[6].Value?.ToString() ?? "Unknown";
                int appointmentID = Convert.ToInt32(dgvAppointments.CurrentRow.Cells[7].Value);

                // Confirm cancellation
                string confirmMessage = $"Are you sure you want to CANCEL this appointment?\n\n" +
                                      $"Customer: {customerName}\n" +
                                      $"Date: {appointmentDate.ToShortDateString()}\n" +
                                      $"Time: {timeSlot}";

                DialogResult result = MessageBox.Show(confirmMessage, "Confirm Cancellation",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Delete the appointment
                    taAppointment.DeleteAppointment(appointmentID);

                    MessageBox.Show("Appointment has been successfully cancelled!",
                                   "Cancellation Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the appointments grid
                    RefreshCustomerAppointments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling appointment: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshCustomerAppointments()
        {
            if (!string.IsNullOrEmpty(lblCustomerID.Text) && lblSelectedCustomer.Text != "No customer selected")
            {
                try
                {
                    int customerID = Convert.ToInt32(lblCustomerID.Text);

                    // Extract surname from the selected customer label
                    string customerSurname = "";
                    string[] nameParts = lblSelectedCustomer.Text.Split(' ');
                    if (nameParts.Length > 1)
                    {
                        customerSurname = nameParts[nameParts.Length - 1];
                    }

                    // Clear and reload appointments for this customer
                    dsAppointment.AppointmentDetail.Clear();
                    taAppointmentDetail.FillByCustID(dsAppointment.AppointmentDetail, customerSurname, customerID);

                    if (dsAppointment.AppointmentDetail.Rows.Count == 0)
                    {
                        MessageBox.Show($"{lblSelectedCustomer.Text} has no more appointments.",
                                       "No Appointments", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error refreshing appointments: {ex.Message}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // If no specific customer is selected, show all appointments
                try
                {
                    dsAppointment.AppointmentDetail.Clear();
                    taAppointmentDetail.Fill(dsAppointment.AppointmentDetail);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error refreshing all appointments: {ex.Message}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnShowAllAppointments_Click(object sender, EventArgs e)
        {
            try
            {
                dsAppointment.AppointmentDetail.Clear();
                taAppointmentDetail.Fill(dsAppointment.AppointmentDetail);

                lblSelectedCustomer.Text = "No customer selected";
                lblCustomerID.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading all appointments: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearchCustomer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                taCustomer.FillBySurname(dsAppointment.Customer, txtSearchCustomer.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomers.CurrentRow != null &&
                dgvCustomers.CurrentRow.Cells[0].Value != null)
            {
                try
                {
                    // Use ONLY column indices - no column names
                    int customerID = Convert.ToInt32(dgvCustomers.CurrentRow.Cells[0].Value);
                    string customerName = dgvCustomers.CurrentRow.Cells[1].Value?.ToString() ?? "";
                    string customerSurname = dgvCustomers.CurrentRow.Cells[2].Value?.ToString() ?? "";

                    // Update the selected customer labels
                    lblSelectedCustomer.Text = $"{customerName} {customerSurname}";
                    lblCustomerID.Text = customerID.ToString();

                    // Load appointments for this customer
                    dsAppointment.AppointmentDetail.Clear();
                    taAppointmentDetail.FillByCustID(dsAppointment.AppointmentDetail, customerSurname, customerID);

                    // Check if customer has any appointments
                    if (dsAppointment.AppointmentDetail.Rows.Count == 0)
                    {
                        MessageBox.Show($"{customerName} {customerSurname} has no appointments to cancel.",
                                       "No Appointments", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting customer: {ex.Message}\n\nDebug Info:\nRow Index: {e.RowIndex}\nCells Count: {dgvCustomers.CurrentRow?.Cells.Count}\nFirst Cell Value: {dgvCustomers.CurrentRow?.Cells[0].Value}",
                                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvAppointments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Handle appointment selection
        }

        private void dgvAppointments_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Double-click to cancel appointment
            btnCancel_Click(sender, e);
        }

        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Optional: Handle customer cell content click
        }
    }
}