using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Printing;

namespace OMS
{
    public partial class Payment : Form
    {
        public Payment()
        {
            InitializeComponent();
        }
        private void PrintPaymentSlip()
        {
            try
            {
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += PrintDoc_PrintPage;

                // Set paper size to receipt size (80mm width)
                PaperSize receiptSize = new PaperSize("Receipt", 315, 600); // 80mm x 152mm in hundredths of an inch
                printDoc.DefaultPageSettings.PaperSize = receiptSize;
                printDoc.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);

                printDoc.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing slip: {ex.Message}", "Print Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Professional fonts
            Font companyFont = new Font("Arial", 14, FontStyle.Bold);
            Font titleFont = new Font("Arial", 12, FontStyle.Bold);
            Font headerFont = new Font("Arial", 10, FontStyle.Bold);
            Font normalFont = new Font("Arial", 9);
            Font smallFont = new Font("Arial", 8);
            Font tinyFont = new Font("Arial", 7);

            Brush blackBrush = Brushes.Black;
            Brush grayBrush = Brushes.Gray;
            Pen linePen = new Pen(Color.Black, 1);
            Pen dottedPen = new Pen(Color.Gray, 1);
            dottedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            float yPos = 20;
            float leftMargin = 20;
            float rightMargin = 295; // Based on 315 width - 20 margin
            float centerX = (rightMargin - leftMargin) / 2 + leftMargin;

            // ==================== HEADER SECTION ====================
            // Company Name - Centered
            SizeF companySize = g.MeasureString("EMONTI OPTOMETRIST", companyFont);
            g.DrawString("EMONTI OPTOMETRIST", companyFont, blackBrush,
                centerX - companySize.Width / 2, yPos);
            yPos += 25;

            // Professional tagline
            string tagline = "Excellence in Eye Care";
            SizeF taglineSize = g.MeasureString(tagline, smallFont);
            g.DrawString(tagline, smallFont, grayBrush,
                centerX - taglineSize.Width / 2, yPos);
            yPos += 20;

            // Contact info (centered)
            string[] contactInfo = {
        "Tel: (012) 345-6789",
        "Email: emontioptometrist@gmail.com",
        "www.emontioptometrist.co.za"
    };

            foreach (string info in contactInfo)
            {
                SizeF infoSize = g.MeasureString(info, tinyFont);
                g.DrawString(info, tinyFont, grayBrush,
                    centerX - infoSize.Width / 2, yPos);
                yPos += 12;
            }

            yPos += 10;

            // Horizontal line
            g.DrawLine(linePen, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;

            // ==================== RECEIPT HEADER ====================
            // Receipt title - Centered
            string receiptTitle = "PAYMENT RECEIPT";
            SizeF titleSize = g.MeasureString(receiptTitle, titleFont);
            g.DrawString(receiptTitle, titleFont, blackBrush,
                centerX - titleSize.Width / 2, yPos);
            yPos += 25;

            // Transaction details in two columns
            g.DrawString("Receipt No#:", normalFont, blackBrush, leftMargin, yPos);
            g.DrawString(txtTransactionNo.Text, normalFont, blackBrush, leftMargin + 70, yPos);
            yPos += 18;

            g.DrawString("Date:", normalFont, blackBrush, leftMargin, yPos);
            g.DrawString(txtPaymentDate.Text, normalFont, blackBrush, leftMargin + 70, yPos);
            yPos += 18;

            g.DrawString("Time:", normalFont, blackBrush, leftMargin, yPos);
            g.DrawString(DateTime.Now.ToString("HH:mm"), normalFont, blackBrush, leftMargin + 70, yPos);
            yPos += 18;

            g.DrawString("Customer:", normalFont, blackBrush, leftMargin, yPos);
            yPos += 15;
            g.DrawString(txtCustName.Text, headerFont, blackBrush, leftMargin + 10, yPos);
            yPos += 25;

            // Dotted line separator
            g.DrawLine(dottedPen, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;

            // ==================== SERVICES SECTION ====================
            g.DrawString("SERVICES/PRODUCT", headerFont, blackBrush, leftMargin, yPos);
            yPos += 20;

            decimal totalAmount = 0;

            if (checkConsultation.Checked)
            {
                decimal consultationFee = 500.00m; // Use fixed consultation fee
                                                   // OR alternatively: decimal consultationFee = SafeParseDecimal(txtConsultation.Text);
                                                   // if (consultationFee == 0) consultationFee = 500.00m; // fallback to standard fee

                totalAmount += consultationFee;

                g.DrawString("Eye Consultation", normalFont, blackBrush, leftMargin, yPos);
                string consultationPrice = "R " + consultationFee.ToString("F2");
                SizeF priceSize = g.MeasureString(consultationPrice, normalFont);
                g.DrawString(consultationPrice, normalFont, blackBrush,
                    rightMargin - priceSize.Width, yPos);
                yPos += 18;
            }

            if (checkOrderPayment.Checked)
            {
                decimal orderAmount = SafeParseDecimal(txtOrderTotal.Text);
                totalAmount += orderAmount;

                g.DrawString("Product Payment", normalFont, blackBrush, leftMargin, yPos);
                string orderPrice = "R " + orderAmount.ToString("F2");
                SizeF priceSize = g.MeasureString(orderPrice, normalFont);
                g.DrawString(orderPrice, normalFont, blackBrush,
                    rightMargin - priceSize.Width, yPos);
                yPos += 18;

                if (!string.IsNullOrEmpty(txtOrderID.Text))
                {
                    g.DrawString($"  Order ID: {txtOrderID.Text}", smallFont, grayBrush, leftMargin + 10, yPos);
                    yPos += 15;
                }
            }

            yPos += 10;

            // Total line
            g.DrawLine(linePen, leftMargin, yPos, rightMargin, yPos);
            yPos += 10;

            g.DrawString("TOTAL", headerFont, blackBrush, leftMargin, yPos);
            string totalPrice = "R " + SafeParseDecimal(txtTotalPayable.Text).ToString("F2");
            SizeF totalSize = g.MeasureString(totalPrice, headerFont);
            g.DrawString(totalPrice, headerFont, blackBrush,
                rightMargin - totalSize.Width, yPos);
            yPos += 25;

            // Double line for total
            g.DrawLine(linePen, leftMargin, yPos, rightMargin, yPos);
            g.DrawLine(linePen, leftMargin, yPos + 2, rightMargin, yPos + 2);
            yPos += 15;

            // ==================== PAYMENT SECTION ====================
            g.DrawString("PAYMENT DETAILS", headerFont, blackBrush, leftMargin, yPos);
            yPos += 20;

            string paymentMethod = GetSelectedPaymentMethod();
            g.DrawString("Method:", normalFont, blackBrush, leftMargin, yPos);

            string methodDisplay = paymentMethod;
            if (paymentMethod == "MEDICAL_AID") methodDisplay = "Medical Aid";

            g.DrawString(methodDisplay, normalFont, blackBrush, leftMargin + 60, yPos);
            yPos += 18;

            if (radMedAid.Checked)
            {
                // Medical Aid Payment Details
                decimal medAidAmount = SafeParseDecimal(txtMedicalAidAmount.Text);
                decimal patientPortion = SafeParseDecimal(txtPatientPortion.Text);

                g.DrawString("Medical Aid:", normalFont, blackBrush, leftMargin, yPos);
                string medAidPrice = "R " + medAidAmount.ToString("F2");
                SizeF medAidSize = g.MeasureString(medAidPrice, normalFont);
                g.DrawString(medAidPrice, normalFont, blackBrush,
                    rightMargin - medAidSize.Width, yPos);
                yPos += 18;

                if (patientPortion > 0)
                {
                    g.DrawString("Customer Portion:", normalFont, blackBrush, leftMargin, yPos);
                    string patientPrice = "R " + patientPortion.ToString("F2");
                    SizeF patientSize = g.MeasureString(patientPrice, normalFont);
                    g.DrawString(patientPrice, normalFont, blackBrush,
                        rightMargin - patientSize.Width, yPos);
                    yPos += 18;

                    string patientMethod = GetSelectedPatientPaymentMethod();
                    if (!string.IsNullOrEmpty(patientMethod))
                    {
                        g.DrawString($"  Paid by: {patientMethod}", smallFont, grayBrush, leftMargin + 10, yPos);
                        yPos += 15;
                    }
                }

                if (!string.IsNullOrEmpty(txtMedicalAidRef.Text))
                {
                    g.DrawString($"Reference: {txtMedicalAidRef.Text}", smallFont, grayBrush, leftMargin, yPos);
                    yPos += 15;
                }
            }
            else
            {
                // Regular Payment Details
                decimal amountReceived = SafeParseDecimal(txtAmountRec.Text);
                decimal changeDue = SafeParseDecimal(txtChangeDue.Text);

                g.DrawString("Amount Paid:", normalFont, blackBrush, leftMargin, yPos);
                string paidPrice = "R " + amountReceived.ToString("F2");
                SizeF paidSize = g.MeasureString(paidPrice, normalFont);
                g.DrawString(paidPrice, normalFont, blackBrush,
                    rightMargin - paidSize.Width, yPos);
                yPos += 18;

                if (changeDue > 0)
                {
                    g.DrawString("Change:", normalFont, blackBrush, leftMargin, yPos);
                    string changePrice = "R " + changeDue.ToString("F2");
                    SizeF changeSize = g.MeasureString(changePrice, normalFont);
                    g.DrawString(changePrice, normalFont, blackBrush,
                        rightMargin - changeSize.Width, yPos);
                    yPos += 18;
                }
            }

            yPos += 15;

            // Dotted line separator
            g.DrawLine(dottedPen, leftMargin, yPos, rightMargin, yPos);
            yPos += 15;

            // ==================== FOOTER SECTION ====================
            // Status
            string status = radMedAid.Checked ? "PROCESSING" : "PAID";
            g.DrawString($"Status: {status}", headerFont, blackBrush, leftMargin, yPos);
            yPos += 25;

            // Thank you message - centered
            string thankYou = "Thank you for choosing";
            SizeF thankYouSize = g.MeasureString(thankYou, normalFont);
            g.DrawString(thankYou, normalFont, blackBrush,
                centerX - thankYouSize.Width / 2, yPos);
            yPos += 15;

            string companyName = "Emonti Optometrist";
            SizeF companyNameSize = g.MeasureString(companyName, headerFont);
            g.DrawString(companyName, headerFont, blackBrush,
                centerX - companyNameSize.Width / 2, yPos);
            yPos += 25;

            // Professional closing
            string[] closingLines = {
        "Your vision is our priority",
        "Keep this receipt for your records",
        "",
        $"Printed: {DateTime.Now:dd/MM/yyyy HH:mm:ss}"
    };

            foreach (string line in closingLines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    SizeF lineSize = g.MeasureString(line, tinyFont);
                    g.DrawString(line, tinyFont, grayBrush,
                        centerX - lineSize.Width / 2, yPos);
                }
                yPos += 12;
            }

            // Cleanup
            companyFont.Dispose();
            titleFont.Dispose();
            headerFont.Dispose();
            normalFont.Dispose();
            smallFont.Dispose();
            tinyFont.Dispose();
            linePen.Dispose();
            dottedPen.Dispose();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            // Load initial data
            taCustomer.Fill(dsOMS.Customer);
            this.taAppointment2.Fill(this.dsAppointment.Appointment);

            taAppointment2.Update(dsAppointment.Appointment);

            // Orders will be loaded when customer is selected

            // Initialize payment date to today
            txtPaymentDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

            // Generate transaction number
            txtTransactionNo.Text = GenerateTransactionNumber();

            // Initialize Medical Aid groupbox as hidden
            grpboxMedicalAid.Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            taCustomer.FillBySurname(dsOMS.Customer, txtSearch.Text);
        }

        private void checkConsultation_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkConsultation.Checked)
                {
                    // Check if customer is selected and has appointments
                    if (dgvAppointments.CurrentRow != null && dgvAppointments.CurrentRow.Index >= 0)
                    {
                        // Extract Appointment_ID from selected appointment
                        int appointmentID = Convert.ToInt32(dgvAppointments.CurrentRow.Cells["appointmentIDDataGridViewTextBoxColumn"].Value);
                        txtAppointID.Text = appointmentID.ToString();

                        // Set standard consultation fee of R500
                        txtConsultation.Text = "500.00";
                    }
                    else
                    {
                        MessageBox.Show("Please select an appointment first.", "No Appointment Selected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        checkConsultation.Checked = false;
                        return;
                    }
                }
                else
                {
                    // Clear consultation fields when unchecked
                    txtAppointID.Clear();
                    txtConsultation.Clear();
                }

                // Always call CalculateTotalPayable after setting or clearing values
                CalculateTotalPayable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing consultation: {ex.Message}", "Processing Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkConsultation.Checked = false;
            }
        }

        private void checkOrderPayment_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkOrderPayment.Checked)
                {
                    // Check if customer is selected and has pending orders
                    if (dgvOrders.CurrentRow != null && dgvOrders.CurrentRow.Index >= 0)
                    {
                        // Extract Order_ID and Order_Total from selected order
                        int orderID = Convert.ToInt32(dgvOrders.CurrentRow.Cells["orderIDDataGridViewTextBoxColumn"].Value);
                        decimal orderTotal = Convert.ToDecimal(dgvOrders.CurrentRow.Cells["orderTotalDataGridViewTextBoxColumn"].Value);

                        txtOrderID.Text = orderID.ToString();
                        txtOrderTotal.Text = orderTotal.ToString("F2");
                    }
                    else
                    {
                        MessageBox.Show("Please select a pending order first.", "No Order Selected",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        checkOrderPayment.Checked = false;
                        return;
                    }
                }
                else
                {
                    // Clear order fields when unchecked
                    txtOrderID.Clear();
                    txtOrderTotal.Clear();
                }

                // Recalculate total payable amount
                CalculateTotalPayable();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing order payment: {ex.Message}", "Processing Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkOrderPayment.Checked = false;
            }
        }

        private void CalculateTotalPayable()
        {
            decimal total = 0;

            // Add consultation fee if checked
            if (checkConsultation.Checked)
            {
                total += 500;
            }

            // Add order total if checked
            if (checkOrderPayment.Checked)
            {
                decimal orderAmount = 0;
                if (decimal.TryParse(txtOrderTotal.Text, out orderAmount))
                {
                    total += orderAmount;
                }
            }

            txtTotalPayable.Text = total.ToString();

            // Recalculate patient portion when total changes
            if (radMedAid.Checked)
            {
                CalculatePatientPortion();
            }
        }

        private void ClearPaymentItems()
        {
            // Uncheck payment checkboxes
            checkConsultation.Checked = false;
            checkOrderPayment.Checked = false;

            // Clear all payment-related fields
            txtAppointID.Clear();
            txtOrderID.Clear();
            txtConsultation.Clear();
            txtOrderTotal.Clear();
            txtTotalPayable.Clear();

            // Clear medical aid fields
            ClearMedicalAidFields();
        }

        private void ClearMedicalAidFields()
        {
            txtMedicalAidAmount.Clear();
            txtPatientPortion.Clear();
            txtPatientAmountReceived.Clear();
            txtPatientChange.Clear();
            txtMedicalAidRef.Clear();

            // Uncheck patient payment method radio buttons
            radPatientCash.Checked = false;
            radPatientCard.Checked = false;
            radPatientEFT.Checked = false;

            // Re-enable patient payment method radio buttons
            radPatientCash.Enabled = true;
            radPatientCard.Enabled = true;
            radPatientEFT.Enabled = true;

            // Disable patient amount received field
            txtPatientAmountReceived.Enabled = false;
        }

        private string GenerateTransactionNumber()
        {
            // Generate 13-digit transaction number using timestamp
            // Format: yyMMddHHmmssfff truncated to 13 digits
            DateTime now = DateTime.Now;

            // Use 2-digit year + month + day + hour + minute + second + first 3 digits of millisecond
            string fullTimeStamp = now.ToString("yyMMddHHmmssffff");

            // Take only the first 13 digits
            return fullTimeStamp.Substring(0, 13);
        }

        private void dgvCustomers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dgvCustomers.Rows[e.RowIndex] != null)
                {
                    // Get the double-clicked row
                    DataGridViewRow selectedRow = dgvCustomers.Rows[e.RowIndex];

                    // Extract customer info and display
                    int custID = Convert.ToInt32(selectedRow.Cells["custIDDataGridViewTextBoxColumn"].Value);
                    string custName = selectedRow.Cells["customerNameDataGridViewTextBoxColumn"].Value.ToString();
                    string custSurname = selectedRow.Cells["customerSurnameDataGridViewTextBoxColumn"].Value.ToString();

                    txtCustID.Text = custID.ToString();
                    txtCustName.Text = $"{custName} {custSurname}";

                    // Load appointments and orders for selected customer
                    LoadCustomerAppointments(custID);
                    LoadCustomerPendingOrders(custID);

                    // Clear previous payment selections
                    ClearPaymentItems();

                    // Optional: Show confirmation
                    MessageBox.Show($"Customer {custName} {custSurname} selected.", "Customer Selected",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting customer: {ex.Message}", "Selection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerAppointments(int custID)
        {
            try
            {
                // Load pending appointments for the selected customer using table adapter
                taAppointment2.FillByPendingAppointment(dsAppointment.Appointment, custID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Data Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCustomerPendingOrders(int custID)
        {
            try
            {
                // Load pending orders for the selected customer using table adapter
                taOrder.FillByPendingCustID(dsOMS.Order, custID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Data Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAppointments_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Implementation can be added if needed
        }

        private void dgvOrders_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Implementation can be added if needed
        }

        private void dgvAppointments_SelectionChanged(object sender, EventArgs e)
        {
            // If consultation is already checked and appointment selection changes, update appointment ID
            if (checkConsultation.Checked && dgvAppointments.CurrentRow != null)
            {
                try
                {
                    int appointmentID = Convert.ToInt32(dgvAppointments.CurrentRow.Cells["appointmentIDDataGridViewTextBoxColumn"].Value);
                    txtAppointID.Text = appointmentID.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating appointment ID: {ex.Message}", "Update Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            // If order payment is already checked and order selection changes, update order details
            if (checkOrderPayment.Checked && dgvOrders.CurrentRow != null)
            {
                try
                {
                    int orderID = Convert.ToInt32(dgvOrders.CurrentRow.Cells["orderIDDataGridViewTextBoxColumn"].Value);
                    decimal orderTotal = Convert.ToDecimal(dgvOrders.CurrentRow.Cells["orderTotalDataGridViewTextBoxColumn"].Value);

                    txtOrderID.Text = orderID.ToString();
                    txtOrderTotal.Text = orderTotal.ToString("F2", CultureInfo.InvariantCulture);

                    // Recalculate total when order selection changes
                    CalculateTotalPayable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating order details: {ex.Message}", "Update Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnExact_Click(object sender, EventArgs e)
        {
            try
            {
                // Set amount received to total payable for exact payment
                if (!string.IsNullOrEmpty(txtTotalPayable.Text))
                {
                    if (radMedAid.Checked)
                    {
                        // For medical aid, set exact payment for patient portion
                        if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                        {
                            txtPatientAmountReceived.Text = txtPatientPortion.Text;
                            txtPatientChange.Text = "0.00";
                        }
                        else
                        {
                            MessageBox.Show("Please enter medical aid amount first.", "Medical Aid Amount Required",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // For regular payments
                        txtAmountRec.Text = txtTotalPayable.Text;
                        txtChangeDue.Text = "0.00";
                    }
                }
                else
                {
                    MessageBox.Show("Please calculate total payable first.", "No Amount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting exact amount: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAmountRec_TextChanged(object sender, EventArgs e)
        {
            // Calculate change due when amount received changes (for cash payments)
            if (radCash.Checked)
            {
                CalculateChangeDue();
            }
        }

        private void CalculateChangeDue()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtAmountRec.Text) && !string.IsNullOrEmpty(txtTotalPayable.Text))
                {
                    decimal amountReceived = 0;
                    decimal totalPayable = 0;

                    // Simple decimal parsing without culture formatting
                    if (!decimal.TryParse(txtAmountRec.Text, out amountReceived))
                    {
                        txtChangeDue.Text = "0.00";
                        return;
                    }

                    if (!decimal.TryParse(txtTotalPayable.Text, out totalPayable))
                    {
                        txtChangeDue.Text = "0.00";
                        return;
                    }

                    decimal changeDue = amountReceived - totalPayable;

                    // Display change with simple formatting
                    txtChangeDue.Text = changeDue.ToString("0.00");

                    // Visual feedback for insufficient payment
                    if (changeDue < 0)
                    {
                        // Make change due field red to indicate insufficient payment
                        txtChangeDue.BackColor = Color.LightCoral;
                        txtChangeDue.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        // Reset to normal colors for sufficient payment
                        txtChangeDue.BackColor = SystemColors.Window;
                        txtChangeDue.ForeColor = SystemColors.WindowText;
                    }
                }
                else
                {
                    txtChangeDue.Text = "0.00";
                    txtChangeDue.BackColor = SystemColors.Window;
                    txtChangeDue.ForeColor = SystemColors.WindowText;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating change: {ex.Message}", "Calculation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtChangeDue.Text = "0.00";
            }
        }

        private void radCash_CheckedChanged(object sender, EventArgs e)
        {
            if (radCash.Checked)
            {
                // Enable amount received field for cash payments
                txtAmountRec.Enabled = true;
                txtAmountRec.Clear();
                txtChangeDue.Clear();
                ResetFieldColors(); // Add this line

                // Hide medical aid groupbox
                grpboxMedicalAid.Visible = false;
                ClearMedicalAidFields();
            }
        }
        private void ResetFieldColors()
        {
            txtChangeDue.BackColor = SystemColors.Window;
            txtChangeDue.ForeColor = SystemColors.WindowText;
        }

        private void radCard_CheckedChanged(object sender, EventArgs e)
        {
            if (radCard.Checked)
            {
                // For card payments, set exact amount automatically
                SetExactPayment();

                // Hide medical aid groupbox
                grpboxMedicalAid.Visible = false;
                ClearMedicalAidFields();
            }
        }

        private void radEFT_CheckedChanged(object sender, EventArgs e)
        {
            if (radEFT.Checked)
            {
                // For EFT payments, set exact amount automatically
                SetExactPayment();

                // Hide medical aid groupbox
                grpboxMedicalAid.Visible = false;
                ClearMedicalAidFields();
            }
        }

        private void radMedAid_CheckedChanged(object sender, EventArgs e)
        {
            if (radMedAid.Checked)
            {
                // Show medical aid groupbox for split payment
                grpboxMedicalAid.Visible = true;

                // Clear regular payment fields
                txtAmountRec.Clear();
                txtChangeDue.Clear();
                txtAmountRec.Enabled = false;

                // Clear medical aid fields for fresh start
                ClearMedicalAidFields();

                // Focus on medical aid amount field for immediate entry
                txtMedicalAidAmount.Focus();
            }
            else
            {
                // Hide medical aid groupbox
                grpboxMedicalAid.Visible = false;
                ClearMedicalAidFields();

                // Re-enable regular amount received field
                txtAmountRec.Enabled = true;
            }
        }

        private void SetExactPayment()
        {
            try
            {
                // Disable amount received field for non-cash payments
                txtAmountRec.Enabled = false;

                if (!string.IsNullOrEmpty(txtTotalPayable.Text))
                {
                    txtAmountRec.Text = txtTotalPayable.Text;
                    txtChangeDue.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting exact payment: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // New Medical Aid Event Handlers
        private void txtMedicalAidAmount_TextChanged(object sender, EventArgs e)
        {
            CalculatePatientPortion();
        }

        private void CalculatePatientPortion()
        {
            try
            {
                decimal totalPayable = SafeParseDecimal(txtTotalPayable.Text);
                decimal medicalAidAmount = SafeParseDecimal(txtMedicalAidAmount.Text);

                if (totalPayable > 0)
                {
                    decimal patientPortion = totalPayable - medicalAidAmount;

                    if (patientPortion < 0) patientPortion = 0;

                    txtPatientPortion.Text = patientPortion.ToString("F2");

                    if (patientPortion > 0)
                    {
                        radPatientCash.Enabled = true;
                        radPatientCard.Enabled = true;
                        radPatientEFT.Enabled = true;
                    }
                    else
                    {
                        radPatientCash.Enabled = false;
                        radPatientCard.Enabled = false;
                        radPatientEFT.Enabled = false;
                        txtPatientAmountReceived.Clear();
                        txtPatientChange.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating patient portion: {ex.Message}", "Calculation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radPatientCash_CheckedChanged(object sender, EventArgs e)
        {
            if (radPatientCash.Checked)
            {
                // Enable patient amount received field for cash payments
                txtPatientAmountReceived.Enabled = true;
                txtPatientAmountReceived.Clear();
                txtPatientAmountReceived.Focus(); // Focus on the field for immediate entry
                txtPatientChange.Clear();
            }
        }

        private void radPatientCard_CheckedChanged(object sender, EventArgs e)
        {
            if (radPatientCard.Checked)
            {
                // For card payments, set exact amount automatically
                SetExactPatientPayment();
            }
        }

        private void radPatientEFT_CheckedChanged(object sender, EventArgs e)
        {
            if (radPatientEFT.Checked)
            {
                // For EFT payments, set exact amount automatically
                SetExactPatientPayment();
            }
        }

        private void SetExactPatientPayment()
        {
            try
            {
                // For card/EFT payments, make field read-only but visible
                txtPatientAmountReceived.ReadOnly = true;
                txtPatientAmountReceived.Enabled = true;

                if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                {
                    txtPatientAmountReceived.Text = txtPatientPortion.Text;
                    txtPatientChange.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting exact patient payment: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPatientAmountReceived_TextChanged(object sender, EventArgs e)
        {
            // Calculate patient change due when amount received changes (for cash payments)
            if (radPatientCash.Checked)
            {
                CalculatePatientChangeDue();
            }
        }

        private void CalculatePatientChangeDue()
        {
            try
            {
                // Simple parsing - no culture stuff
                decimal patientAmountReceived = 0;
                decimal patientPortion = 0;

                if (!string.IsNullOrEmpty(txtPatientAmountReceived.Text))
                {
                    decimal.TryParse(txtPatientAmountReceived.Text, out patientAmountReceived);
                }

                if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                {
                    decimal.TryParse(txtPatientPortion.Text, out patientPortion);
                }

                // Calculate change
                decimal patientChangeDue = patientAmountReceived - patientPortion;

                // Display change (can be negative for underpayment)
                txtPatientChange.Text = patientChangeDue.ToString("0.00");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating patient change: {ex.Message}", "Calculation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPatientChange.Text = "0.00";
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (!ValidatePaymentData())
                    return;

                // Get payment method
                string paymentMethod = GetSelectedPaymentMethod();
                if (string.IsNullOrEmpty(paymentMethod))
                {
                    MessageBox.Show("Please select a payment method.", "Payment Method Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Process the payment
                ProcessPayment(paymentMethod);

                MessageBox.Show("Payment processed successfully!", "Payment Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear form after successful payment and print payment slip 
                PrintPaymentSlip();
                UpdateAppointments();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", "Processing Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateAppointments()
        {


            if (dgvAppointments.CurrentRow != null)
            {

                string item = dgvAppointments.CurrentRow.Cells[0].Value?.ToString();

                foreach (DataRow row in dsAppointment.Appointment)
                {
                    if (row[0].ToString() == item)
                    {
                        row[5] = "Paid";

                    }


                }
            }
            taAppointment2.Update(dsAppointment.Appointment);
            taAppointment2.FillByPendingAppointment(dsAppointment.Appointment, Convert.ToInt32(txtCustID.Text));
        }


        private bool ValidatePaymentData()
        {
            // Check if customer is selected
            if (string.IsNullOrEmpty(txtCustID.Text))
            {
                MessageBox.Show("Please select a customer.", "Customer Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if at least one payment item is selected
            if (!checkConsultation.Checked && !checkOrderPayment.Checked)
            {
                MessageBox.Show("Please select at least one payment item (Consultation or Order).",
                    "Payment Item Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if total payable is valid
            decimal totalPayable;
            if (string.IsNullOrEmpty(txtTotalPayable.Text) ||
                !decimal.TryParse(txtTotalPayable.Text, NumberStyles.Number,
                CultureInfo.InvariantCulture, out totalPayable) || totalPayable <= 0)
            {
                MessageBox.Show("Total payable amount must be greater than zero.", "Invalid Amount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Special validation for Medical Aid payments
            if (radMedAid.Checked)
            {
                return ValidateMedicalAidPayment(totalPayable);
            }
            else
            {
                // Regular payment validation
                return ValidateRegularPayment(totalPayable);
            }
        }

        private bool ValidateMedicalAidPayment(decimal totalPayable)
        {
            // Validate medical aid amount
            decimal medicalAidAmount = SafeParseDecimal(txtMedicalAidAmount.Text);
            if (medicalAidAmount <= 0)
            {
                MessageBox.Show("Please enter a valid medical aid amount.", "Invalid Medical Aid Amount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check if medical aid amount exceeds total
            if (medicalAidAmount > totalPayable)
            {
                MessageBox.Show("Medical aid amount cannot exceed total payable.", "Invalid Medical Aid Amount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate medical aid reference
            if (string.IsNullOrWhiteSpace(txtMedicalAidRef.Text))
            {
                MessageBox.Show("Please enter a medical aid reference number.", "Medical Aid Reference Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Calculate patient portion directly
            decimal patientPortion = totalPayable - medicalAidAmount;

            // Only validate patient payment if there's actually a patient portion to pay
            if (patientPortion > 0.01m) // Using 0.01 to handle small rounding differences
            {
                // Check if patient payment method is selected
                if (!radPatientCash.Checked && !radPatientCard.Checked && !radPatientEFT.Checked)
                {
                    MessageBox.Show("Please select a payment method for the patient portion.", "Patient Payment Method Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Get patient amount received using SafeParseDecimal
                decimal patientAmountReceived = SafeParseDecimal(txtPatientAmountReceived.Text);
                if (patientAmountReceived <= 0)
                {
                    MessageBox.Show("Patient amount received must be greater than zero.", "Invalid Patient Amount",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Simple comparison for cash payments
                if (radPatientCash.Checked && Math.Round(patientAmountReceived, 2) < Math.Round(patientPortion, 2))
                {
                    MessageBox.Show($"Patient amount received (R{patientAmountReceived:F2}) is less than required (R{patientPortion:F2}).", "Insufficient Patient Payment",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            // If patientPortion is 0 or very small, no patient payment validation needed - medical aid covers everything

            return true;
        }

        private bool ValidateRegularPayment(decimal totalPayable)
        {
            // Check if amount received is valid
            decimal amountReceived;
            if (string.IsNullOrEmpty(txtAmountRec.Text) ||
                !decimal.TryParse(txtAmountRec.Text, NumberStyles.Number,
                CultureInfo.InvariantCulture, out amountReceived) || amountReceived <= 0)
            {
                MessageBox.Show("Amount received must be greater than zero.", "Invalid Amount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // For cash payments, check if sufficient amount is received
            if (radCash.Checked && amountReceived < totalPayable)
            {
                MessageBox.Show("Amount received is insufficient.", "Insufficient Payment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private string GetSelectedPaymentMethod()
        {
            if (radCash.Checked) return "CASH";
            if (radCard.Checked) return "CARD";
            if (radEFT.Checked) return "EFT";
            if (radMedAid.Checked) return "MEDICAL_AID";
            return string.Empty;
        }

        private string GetSelectedPatientPaymentMethod()
        {
            if (radPatientCash.Checked) return "CASH";
            if (radPatientCard.Checked) return "CARD";
            if (radPatientEFT.Checked) return "EFT";
            return string.Empty;
        }

        private void ProcessPayment(string paymentMethod)
        {
            // Create new payment record
            DataRow newPayment = dsOMS.Payments.NewRow();

            // Set payment data with safe parsing
            newPayment["Cust_ID"] = SafeParseInt(txtCustID.Text);

            // Set Appointment_ID if consultation is paid
            if (checkConsultation.Checked && !string.IsNullOrEmpty(txtAppointID.Text))
            {
                newPayment["Appointment_ID"] = SafeParseInt(txtAppointID.Text);
            }
            else
            {
                newPayment["Appointment_ID"] = DBNull.Value;
            }

            // Set Order_ID if order is paid
            if (checkOrderPayment.Checked && !string.IsNullOrEmpty(txtOrderID.Text))
            {
                newPayment["Order_ID"] = SafeParseInt(txtOrderID.Text);
            }
            else
            {
                newPayment["Order_ID"] = DBNull.Value;
            }

            newPayment["Transaction_Number"] = txtTransactionNo.Text ?? "";

            // Parse date safely
            DateTime paymentDate;
            if (DateTime.TryParseExact(txtPaymentDate.Text, "yyyy/MM/dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out paymentDate))
            {
                newPayment["Payment_Date"] = paymentDate;
            }
            else
            {
                newPayment["Payment_Date"] = DateTime.Now.Date;
            }

            // Set fee amounts with safe parsing
            if (checkConsultation.Checked && !string.IsNullOrEmpty(txtConsultation.Text))
            {
                newPayment["Consultation_Fee"] = SafeParseDecimal(txtConsultation.Text);
            }
            else
            {
                newPayment["Consultation_Fee"] = DBNull.Value;
            }

            if (checkOrderPayment.Checked && !string.IsNullOrEmpty(txtOrderTotal.Text))
            {
                newPayment["Order_Payment"] = SafeParseDecimal(txtOrderTotal.Text);
            }
            else
            {
                newPayment["Order_Payment"] = DBNull.Value;
            }

            newPayment["Total_Payable"] = SafeParseDecimal(txtTotalPayable.Text);
            newPayment["Payment_Method"] = paymentMethod;

            // Set Payment_Status based on payment method
            if (paymentMethod == "MEDICAL_AID")
            {
                newPayment["Payment_Status"] = "Processing";

                // Set Medical Aid specific fields
                decimal medicalAidAmount = SafeParseDecimal(txtMedicalAidAmount.Text);
                decimal patientPortion = SafeParseDecimal(txtPatientPortion.Text);

                newPayment["Medical_Aid_Amount"] = medicalAidAmount;
                newPayment["Patient_Portion_Amount"] = patientPortion;

                // Check if medical aid covers the full amount
                if (patientPortion <= 0.01m) // Medical aid covers everything
                {
                    newPayment["Patient_Payment_Method"] = "MEDICAL AID";
                    newPayment["Patient_Amount_Received"] = 0;
                    newPayment["Patient_Change_Due"] = DBNull.Value;
                }
                else // There's a patient portion to pay
                {
                    newPayment["Patient_Payment_Method"] = GetSelectedPatientPaymentMethod();
                    newPayment["Patient_Amount_Received"] = SafeParseDecimal(txtPatientAmountReceived.Text);

                    decimal patientChangeDue = SafeParseDecimal(txtPatientChange.Text);
                    newPayment["Patient_Change_Due"] = patientChangeDue > 0 ? (object)patientChangeDue : DBNull.Value;
                }

                newPayment["Medical_Aid_Reference"] = txtMedicalAidRef.Text ?? "";

                // For medical aid, set amount received to total payable (full amount)
                newPayment["Amount_Received"] = SafeParseDecimal(txtTotalPayable.Text);
                newPayment["Change_Due"] = DBNull.Value;
            }
            else
            {
                // THIS IS THE MISSING CODE FOR REGULAR PAYMENTS!
                newPayment["Payment_Status"] = "Paid";

                // Set regular payment fields
                newPayment["Amount_Received"] = SafeParseDecimal(txtAmountRec.Text);

                // Calculate and set change due
                decimal changeDue = SafeParseDecimal(txtChangeDue.Text);
                newPayment["Change_Due"] = changeDue > 0 ? (object)changeDue : DBNull.Value;

                // Set Medical Aid fields to null for regular payments
                newPayment["Medical_Aid_Amount"] = DBNull.Value;
                newPayment["Patient_Portion_Amount"] = DBNull.Value;
                newPayment["Patient_Payment_Method"] = DBNull.Value;
                newPayment["Patient_Amount_Received"] = DBNull.Value;
                newPayment["Patient_Change_Due"] = DBNull.Value;
                newPayment["Medical_Aid_Reference"] = DBNull.Value;
            }

            newPayment["Created_Date"] = DateTime.Now.Date;
            newPayment["Created_By"] = Environment.UserName; // or get from current user session

            // Add the row to dataset
            dsOMS.Payments.Rows.Add(newPayment);

            // Update the database
            taPayments.Update(dsOMS.Payments);

            // Update order status if order payment was made
            if (checkOrderPayment.Checked && !string.IsNullOrEmpty(txtOrderID.Text))
            {
                string orderStatus = paymentMethod == "MEDICAL_AID" ? "Processing" : "Paid";
                UpdateOrderStatus(SafeParseInt(txtOrderID.Text), orderStatus);
            }
        }

        // Helper methods for safe parsing
        private int SafeParseInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : 0;
        }

        private decimal SafeParseDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0m;

            decimal result;
            if (decimal.TryParse(value, out result))
                return result;

            return 0m;
        }

        private void UpdateOrderStatus(int orderID, string status)
        {
            try
            {
                // Find the order in the dataset and update status
                DataRow[] orderRows = dsOMS.Order.Select($"OrderID = {orderID}");

                if (orderRows.Length > 0)
                {
                    orderRows[0]["Order_Status"] = status;
                    taOrder.Update(dsOMS.Order);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Update Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            try
            {
                // Clear customer selection
                txtCustID.Clear();
                txtCustName.Clear();
                txtSearch.Clear();

                // Clear payment items
                ClearPaymentItems();

                // Clear payment processing fields
                txtAmountRec.Clear();
                txtChangeDue.Clear();

                // Uncheck all radio buttons
                radCash.Checked = false;
                radCard.Checked = false;
                radEFT.Checked = false;
                radMedAid.Checked = false;

                // Hide medical aid groupbox
                grpboxMedicalAid.Visible = false;

                // Generate new transaction number and reset date
                txtTransactionNo.Text = GenerateTransactionNumber();
                txtPaymentDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

                // Enable amount received field
                txtAmountRec.Enabled = true;

                // FIXED: Clear data grids safely - just reload empty data
                try
                {
                    // Clear appointments by loading empty dataset
                    dsAppointment.Appointment.Clear();

                    // Clear orders by loading empty dataset  
                    dsOMS.Order.Clear();
                }
                catch (Exception dgvEx)
                {
                    // Silently handle DataGridView clearing errors - not critical
                    System.Diagnostics.Debug.WriteLine($"DataGridView clear warning: {dgvEx.Message}");
                }

                // Reload all customers
                taCustomer.Fill(dsOMS.Customer);

                // Remove this message box or make it optional
                // MessageBox.Show("Form cleared successfully.", "Form Cleared",
                //     MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Log the error but don't show annoying message to user
                System.Diagnostics.Debug.WriteLine($"ClearForm error: {ex.Message}");

                // Only show critical errors that prevent form from working
                if (ex is OutOfMemoryException || ex is StackOverflowException)
                {
                    MessageBox.Show($"Critical error clearing form: {ex.Message}", "Critical Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // For all other errors, silently continue - form will still be mostly cleared
            }
        }

        private void radPatientCash_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radPatientCash.Checked)
            {
                // Enable patient amount received field for cash payments
                txtPatientAmountReceived.Enabled = true;
                txtPatientAmountReceived.ReadOnly = false;

                // AUTO-FILL with patient portion amount
                if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                {
                    txtPatientAmountReceived.Text = txtPatientPortion.Text;
                    txtPatientChange.Text = "0.00";
                }

                txtPatientAmountReceived.Focus();
            }
        }

        private void txtMedicalAidAmount_TextChanged_1(object sender, EventArgs e)
        {
            // VALIDATE: Medical aid amount cannot exceed total payable
            decimal totalPayable = SafeParseDecimal(txtTotalPayable.Text);
            decimal medicalAidAmount = SafeParseDecimal(txtMedicalAidAmount.Text);

            if (medicalAidAmount > totalPayable && totalPayable > 0)
            {
                MessageBox.Show("Medical aid amount cannot exceed total payable.", "Invalid Amount",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMedicalAidAmount.Text = totalPayable.ToString("F2");
                return;
            }

            CalculatePatientPortion();
        }

        private void radPatientCard_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radPatientCard.Checked)
            {
                // For card payments, set exact amount automatically
                txtPatientAmountReceived.ReadOnly = true;
                txtPatientAmountReceived.Enabled = true;

                if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                {
                    txtPatientAmountReceived.Text = txtPatientPortion.Text;
                    txtPatientChange.Text = "0.00";
                }
            }
        }

        private void radPatientEFT_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radPatientEFT.Checked)
            {
                // For EFT payments, set exact amount automatically
                txtPatientAmountReceived.ReadOnly = true;
                txtPatientAmountReceived.Enabled = true;

                if (!string.IsNullOrEmpty(txtPatientPortion.Text))
                {
                    txtPatientAmountReceived.Text = txtPatientPortion.Text;
                    txtPatientChange.Text = "0.00";
                }
            }
        }

        private void txtPatientChange_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPatientAmountReceived_TextChanged_1(object sender, EventArgs e)
        {
            // Calculate patient change due when amount received changes (for cash payments)
            if (radPatientCash.Checked)
            {
                CalculatePatientChangeDue();
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}