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
    public partial class Appointment : Form
    {
        public Appointment()
        {
            InitializeComponent();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void ADD_Click(object sender, EventArgs e)
        {
            Add_Appointment appointment = new Add_Appointment();
            appointment.Show();
            //this.Hide();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearch1_TextChanged(object sender, EventArgs e)
        {
            // Change from this:
            // taAppointmentDetail.FillBySurname(dsAppointment.AppointmentDetail, txtSearch1.Text);

            // To this (assuming your customer table adapter is named taCustomer):
            taCustomer.FillBySurname(dsAppointment.Customer, txtSearch1.Text);
        }

        private void Appointment_Load(object sender, EventArgs e)
        {
            try
            {
                // Clear any existing data first
                dsAppointment.AppointmentDetail.Clear();
                dsAppointment.DTTimeslotsAvailable.Clear();
                dsAppointment.Customer.Clear();

                // Load customers for the customer selection grid
                taCustomer.Fill(dsAppointment.Customer);

                // Load appointment details
                taAppointmentDetail.Fill(dsAppointment.AppointmentDetail);

                // Load timeslots and appointments for the initially selected date (today by default)
                DateTime initialDate = monthCalendar1.SelectionRange.Start;
                taAvailableTimeslots.FillByAvailableTimeslots(dsAppointment.DTTimeslotsAvailable, initialDate);
                FilterPastTimeslots(initialDate);

                // Also load appointments for the initial date to show in the appointments grid
                taAppointmentDetail.FillByDate(dsAppointment.AppointmentDetail, monthCalendar2.SelectionRange.Start);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}\n\nPlease check your database connection and data integrity.",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //----------------------------------------------------------------------------------------------------------

        private void FilterPastTimeslots(DateTime selectedDate)
        {
            DateTime now = DateTime.Now;

            if (dsAppointment.DTTimeslotsAvailable is DataTable originalTable)
            {
                DataTable filteredTable = originalTable.Clone();

                foreach (DataRow row in originalTable.Rows)
                {
                    string timeSlotStr = row["Timeslot"].ToString(); // example: "13h30-14h00"

                    // Parse the start time (part before '-'), convert 'h' to ':'
                    string[] parts = timeSlotStr.Split('-');
                    if (parts.Length > 0)
                    {
                        string startTimeStr = parts[0].Trim().Replace("h", ":");

                        if (DateTime.TryParse(startTimeStr, out DateTime startTimeOnly))
                        {
                            // Combine date with start time
                            DateTime slotStartDateTime = selectedDate.Date.Add(startTimeOnly.TimeOfDay);

                            // Keep only slots that start strictly after current time
                            if (slotStartDateTime > now)
                            {
                                filteredTable.ImportRow(row);
                            }
                        }
                    }
                }

                if (filteredTable.Rows.Count > 0)
                {
                    comboTimeslot.DataSource = filteredTable;
                    comboTimeslot.DisplayMember = "Timeslot";
                    comboTimeslot.ValueMember = "TimeID";
                    comboTimeslot.Enabled = true;
                }
                else
                {
                    DataTable noSlotTable = new DataTable();
                    noSlotTable.Columns.Add("Timeslot");
                    noSlotTable.Columns.Add("TimeID");

                    DataRow noSlotRow = noSlotTable.NewRow();
                    noSlotRow["Timeslot"] = "No available time slots";
                    noSlotRow["TimeID"] = -1;
                    noSlotTable.Rows.Add(noSlotRow);

                    comboTimeslot.DataSource = noSlotTable;
                    comboTimeslot.DisplayMember = "Timeslot";
                    comboTimeslot.ValueMember = "TimeID";
                    comboTimeslot.Enabled = false;
                }
            }
        }

        // Calendar 1: For selecting booking date and loading available timeslots
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start.Date;

            try
            {
                // Clear and reload available timeslots for the new date
                dsAppointment.DTTimeslotsAvailable.Clear();
                taAvailableTimeslots.FillByAvailableTimeslots(dsAppointment.DTTimeslotsAvailable, selectedDate);

                // Filter past times if the selected date is today or future
                FilterPastTimeslots(selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating timeslots: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboTimeslot_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvCustomer_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Null checks for safety
            if (dgvCustomer.CurrentRow == null || dgvCustomer.CurrentRow.Cells[0].Value == null)
                return;

            try
            {
                string selectedCustomerID = dgvCustomer.CurrentRow.Cells[0].Value.ToString();

                // Check if this customer already has an appointment on the selected date
                DateTime selectedDate = monthCalendar1.SelectionRange.Start.Date;
                bool hasAppointmentOnSelectedDate = false;

                // Check in dataGridView1 (appointments for the selected date)
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.Cells[0].Value != null &&
                            row.Cells[0].Value.ToString() == selectedCustomerID)
                        {
                            hasAppointmentOnSelectedDate = true;
                            break;
                        }
                    }
                }

                if (hasAppointmentOnSelectedDate)
                {
                    string customerName = dgvCustomer.CurrentRow.Cells[1].Value.ToString();
                    string customerSurname = dgvCustomer.CurrentRow.Cells[2].Value.ToString();

                    MessageBox.Show($"The customer {customerName} {customerSurname} already has an appointment on {selectedDate.ToShortDateString()}.\n\n" +
                                   "Please either:\n" +
                                   "• Edit the existing appointment to change the time slot, or\n" +
                                   "• Choose a different date for the new appointment",
                                   "Customer Already Has Appointment",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);

                    // Don't update the labels - prevent selection
                    return;
                }

                // If no conflict, proceed with customer selection
                lblName.Text = dgvCustomer.CurrentRow.Cells[1].Value.ToString();
                lblSurname.Text = dgvCustomer.CurrentRow.Cells[2].Value.ToString();
                lblCustID.Text = selectedCustomerID;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (monthCalendar1.SelectionRange.Start < DateTime.Today)
            {
                MessageBox.Show("Cannot book prior to today");
                return;
            }
            else if (lblName.Text == "NAME" && (lblSurname.Text == "SURNAME"))
            {
                MessageBox.Show("Please select the customer to book for ");
                return;
            }
            else if (comboTimeslot.SelectedValue == null || (int)comboTimeslot.SelectedValue == -1)
            {
                MessageBox.Show("Please select a valid time slot");
                return;
            }
            else
            {
                // Check if customer already has an appointment on the selected date
                int custId = Convert.ToInt32(lblCustID.Text);
                DateTime selectedDate = monthCalendar1.SelectionRange.Start.Date;
                int selectedTimeSlotId = (int)comboTimeslot.SelectedValue;
                string selectedTimeslotText = comboTimeslot.Text; // Get the display text

                // Check if customer already has an appointment on this date
                bool hasAppointmentOnDate = false;
                foreach (DataRow row in dsAppointment.AppointmentDetail.Rows)
                {
                    if (Convert.ToInt32(row["Cust_ID"]) == custId &&
                        Convert.ToDateTime(row["Appointment_Date"]).Date == selectedDate)
                    {
                        hasAppointmentOnDate = true;
                        break;
                    }
                }

                if (hasAppointmentOnDate)
                {
                    MessageBox.Show($"The customer {lblName.Text} {lblSurname.Text} already has an appointment on {selectedDate.ToShortDateString()}.\n\n" +
                                   "Please either:\n" +
                                   "• Edit the existing appointment to change the time slot, or\n" +
                                   "• Choose a different date for the new appointment",
                                   "Appointment Already Exists",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                    return;
                }

                // Check if the selected timeslot is already booked by someone else
                bool isTimeslotTaken = false;
                foreach (DataRow row in dsAppointment.AppointmentDetail.Rows)
                {
                    try
                    {
                        // Use Timeslot column instead of TimeID since TimeID doesn't exist in AppointmentDetail
                        if (Convert.ToDateTime(row["Appointment_Date"]).Date == selectedDate &&
                            row["Timeslot"].ToString() == selectedTimeslotText)
                        {
                            isTimeslotTaken = true;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error checking timeslot availability: {ex.Message}",
                                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }

                if (isTimeslotTaken)
                {
                    MessageBox.Show("This time slot is no longer available. Please select a different time slot.",
                                   "Time Slot Unavailable",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);

                    // Refresh the timeslots to show current availability
                    RefreshTimeslots();
                    return;
                }

                DialogResult result = MessageBox.Show("Do you want to confirm the appointment booking?", "Appointment Confirm", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    try
                    {
                       
                        string dateString = monthCalendar1.SelectionRange.Start.ToString("yyyy-MM-dd");
                        taAppointment.InsertNewAppointment(custId, null, dateString, selectedTimeSlotId, "Pending");
                        MessageBox.Show("Appointment has been Confirmed");

                        // Refresh the data immediately after booking
                        RefreshAppointmentData();
                        RefreshTimeslots();

                        // Clear customer selection
                        lblName.Text = "NAME";
                        lblSurname.Text = "SURNAME";
                        lblCustID.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error booking appointment: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshTimeslots()
        {
            try
            {
                DateTime selectedDate = monthCalendar1.SelectionRange.Start;

                // Clear and reload timeslots
                dsAppointment.DTTimeslotsAvailable.Clear();
                taAvailableTimeslots.FillByAvailableTimeslots(dsAppointment.DTTimeslotsAvailable, selectedDate);

                // Filter past timeslots
                FilterPastTimeslots(selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing timeslots: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Temporary debug method - you can remove this after finding the correct column names
        private void ShowTableColumns()
        {
            string appointmentColumns = "AppointmentDetail columns:\n";
            foreach (DataColumn column in dsAppointment.AppointmentDetail.Columns)
            {
                appointmentColumns += $"- {column.ColumnName} ({column.DataType.Name})\n";
            }

            string timeslotColumns = "\nTimeslot columns:\n";
            foreach (DataColumn column in dsAppointment.DTTimeslotsAvailable.Columns)
            {
                timeslotColumns += $"- {column.ColumnName} ({column.DataType.Name})\n";
            }

            MessageBox.Show(appointmentColumns + timeslotColumns, "Table Columns", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshAppointmentData()
        {
            try
            {
                // Clear existing data to avoid conflicts
                dsAppointment.AppointmentDetail.Clear();
                dsAppointment.DTTimeslotsAvailable.Clear();

                // Refresh timeslots for booking calendar (monthCalendar1)
                DateTime bookingDate = monthCalendar1.SelectionRange.Start;
                taAvailableTimeslots.FillByAvailableTimeslots(dsAppointment.DTTimeslotsAvailable, bookingDate);
                FilterPastTimeslots(bookingDate);

                // Refresh appointments for viewing calendar (monthCalendar2) 
                DateTime viewingDate = monthCalendar2.SelectionRange.Start;
                taAppointmentDetail.FillByDate(dsAppointment.AppointmentDetail, viewingDate);
            }
            catch (System.Data.ConstraintException ex)
            {
                // Handle constraint violations gracefully
                MessageBox.Show("Appointment created successfully, but there's a data display issue. " +
                               "The appointment is saved in the database.",
                               "Display Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Try loading without constraints so user can still see data
                try
                {
                    dsAppointment.EnforceConstraints = false;
                    dsAppointment.AppointmentDetail.Clear();
                    dsAppointment.DTTimeslotsAvailable.Clear();

                    DateTime bookingDate = monthCalendar1.SelectionRange.Start;
                    DateTime viewingDate = monthCalendar2.SelectionRange.Start;

                    taAvailableTimeslots.FillByAvailableTimeslots(dsAppointment.DTTimeslotsAvailable, bookingDate);
                    FilterPastTimeslots(bookingDate);
                    taAppointmentDetail.FillByDate(dsAppointment.AppointmentDetail, viewingDate);
                }
                catch (Exception innerEx)
                {
                    MessageBox.Show($"Error refreshing display: {innerEx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing appointment data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Use the same form 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Create and show the cancellation form
                CancelAppointment cancelForm = new CancelAppointment();

                // You can choose between these options:

                // Option 1: Show as modal dialog (user must close cancel form before returning to main form)
                cancelForm.ShowDialog();

                // Option 2: Show as regular form (both forms can be open at same time)
                // cancelForm.Show();

                // Option 3: Hide main form and show cancel form
                // this.Hide();
                // cancelForm.ShowDialog();
                // this.Show();

                // After the cancel form closes, refresh the main form data
                RefreshAppointmentData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening cancellation form: {ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Calendar 2: For viewing existing appointments on selected date
        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start.Date;

            try
            {
                // Load appointments for the selected date to display in the appointments grid
                taAppointmentDetail.FillByDate(dsAppointment.AppointmentDetail, selectedDate);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}