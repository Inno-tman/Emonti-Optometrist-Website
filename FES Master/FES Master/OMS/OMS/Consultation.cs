using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace OMS
{
    public partial class Consultation : Form
    {
        public Consultation()
        {
            InitializeComponent();

            // Wire up the new event handlers
            txtRightSphere.Leave += txtRightSphere_Leave;
            txtLeftSphere.Leave += txtLeftSphere_Leave;
            txtRightCylinder.Leave += txtRightCylinder_Leave;
            txtLeftCylinder.Leave += txtLeftCylinder_Leave;
            vaRightEye.TextChanged += vaRightEye_TextChanged;
            vaLeftEye.TextChanged += vaLeftEye_TextChanged;
        }

        private void Consultation_Load(object sender, EventArgs e)
        {
            //taConsultations.Fill(dsOMS.Consultations);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            taCustomers.FillBySurname(dsOMS.Customer, txtSearch.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Basic non-null validations
            if (string.IsNullOrWhiteSpace(txtCustID.Text))
            {
                MessageBox.Show("Customer must be selected.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtChiefComplaint.Text))
            {
                MessageBox.Show("Chief complaint is required.");
                return;
            }

            if (cmbNextVisit.SelectedItem == null)
            {
                MessageBox.Show("Please select next visit.");
                return;
            }
            if (cmbOptometrist.SelectedItem == null)
            {
                MessageBox.Show("Please select the Optometrist facilitating this session");
                return;
            }

            // Validate visual acuity before saving
            if (!string.IsNullOrWhiteSpace(vaRightEye.Text) && !IsValidVisualAcuity(vaRightEye.Text))
            {
                MessageBox.Show("Please enter valid right eye visual acuity (format: number/20, e.g., 20/20, 15/20).");
                vaRightEye.Focus();
                return;
            }

            if (!string.IsNullOrWhiteSpace(vaLeftEye.Text) && !IsValidVisualAcuity(vaLeftEye.Text))
            {
                MessageBox.Show("Please enter valid left eye visual acuity (format: number/20, e.g., 20/20, 15/20).");
                vaLeftEye.Focus();
                return;
            }

            try
            {
                // Parse and validate all values strictly with detailed error messages
                int custId = int.Parse(txtCustID.Text);

                string chiefComplaint = txtChiefComplaint.Text.Trim();
                string vaRight = vaRightEye.Text.Trim();
                string vaLeft = vaLeftEye.Text.Trim();

                // Validate and parse decimal values (now supports positive/negative with signs)
                decimal? rSphere = ValidateAndParseDecimal(txtRightSphere.Text, "Right Sphere");
                decimal? rCylinder = ValidateAndParseDecimal(txtRightCylinder.Text, "Right Cylinder");
                int? rAxis = ValidateAndParseInt(txtRightAxis.Text, "Right Axis", 0, 180);

                decimal? lSphere = ValidateAndParseDecimal(txtLeftSphere.Text, "Left Sphere");
                decimal? lCylinder = ValidateAndParseDecimal(txtLeftCylinder.Text, "Left Cylinder");
                int? lAxis = ValidateAndParseInt(txtLeftAxis.Text, "Left Axis", 0, 180);

                decimal? addReading = ValidateAndParseDecimal(txtAddReading.Text, "Add Reading");
                int? pressureRight = ValidateAndParseInt(txtRightEyePressure.Text, "Right Eye Pressure", 0, 50);
                int? pressureLeft = ValidateAndParseInt(txtLeftEyePressure.Text, "Left Eye Pressure", 0, 50);

                string healthAssessment = cmbOverallAssessment.SelectedItem?.ToString();
                string healthNotes = txtHealthNotes.Text.Trim();
                string treatmentAdvice = txtTreatmentAdvice.Text.Trim();
                string nextVisit = cmbNextVisit.SelectedItem.ToString();
                string createdBy = cmbOptometrist.SelectedItem.ToString();

                // Method 1: Using TableAdapter Insert method (recommended)
                taConsultations.Insert(
                    custId,
                    DateTime.Now,
                    chiefComplaint,
                    string.IsNullOrWhiteSpace(vaRight) ? null : vaRight,
                    string.IsNullOrWhiteSpace(vaLeft) ? null : vaLeft,
                    rSphere,
                    rCylinder,
                    rAxis,
                    lSphere,
                    lCylinder,
                    lAxis,
                    addReading,
                    pressureRight,
                    pressureLeft,
                    string.IsNullOrWhiteSpace(healthAssessment) ? null : healthAssessment,
                    string.IsNullOrWhiteSpace(healthNotes) ? null : healthNotes,
                    string.IsNullOrWhiteSpace(treatmentAdvice) ? null : treatmentAdvice,
                    nextVisit,
                    string.IsNullOrWhiteSpace(createdBy) ? null : createdBy,
                    DateTime.Now
                );

                MessageBox.Show("Consultation saved successfully.");

                // Refresh the data
                taConsultations.Fill(dsOMS.Consultations);
                ClearForm();

                // taCustomers.Fill(dsOMS.Customer);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid data format: {ex.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Value out of range: {ex.Message}", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving consultation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Updated validation method for sphere and cylinder values (accepts positive/negative decimals with signs)
        private decimal? ValidateAndParseDecimal(string input, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            // Allow explicit positive signs
            string cleanInput = input.Trim();

            if (!decimal.TryParse(cleanInput, out decimal result))
            {
                throw new FormatException($"{fieldName} must be a valid decimal number (e.g., +2.50, -1.75, 0.25). Current value: '{input}'");
            }

            // Optional: Add range validation for typical sphere/cylinder values
            if (fieldName.Contains("Sphere") && (result < -20 || result > 20))
            {
                throw new ArgumentOutOfRangeException($"{fieldName} should typically be between -20.00 and +20.00. Current value: {result}");
            }

            if (fieldName.Contains("Cylinder") && (result < -10 || result > 0))
            {
                throw new ArgumentOutOfRangeException($"{fieldName} should typically be between -10.00 and 0.00. Current value: {result}");
            }

            return result;
        }

        private int? ValidateAndParseInt(string input, string fieldName, int? minValue = null, int? maxValue = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (!int.TryParse(input, out int result))
            {
                throw new FormatException($"{fieldName} must be a valid integer. Current value: '{input}'");
            }

            // Range validation for specific fields
            if (minValue.HasValue && result < minValue.Value)
            {
                throw new ArgumentOutOfRangeException($"{fieldName} must be greater than or equal to {minValue.Value}. Current value: {result}");
            }

            if (maxValue.HasValue && result > maxValue.Value)
            {
                throw new ArgumentOutOfRangeException($"{fieldName} must be less than or equal to {maxValue.Value}. Current value: {result}");
            }
            return result;
        }

        // Method to validate visual acuity (format: number/20 where denominator must be 20)
        public bool IsValidVisualAcuity(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return true; // Allow empty input

            int index = userInput.IndexOf('/');

            // Check if '/' exists and is not at the beginning or end
            if (index <= 0 || index >= userInput.Length - 1)
                return false;

            // Extract parts safely
            string leftStr = userInput.Substring(0, index).Trim();
            string rightStr = userInput.Substring(index + 1).Trim();

            // Try parsing both parts
            if (!int.TryParse(leftStr, out int leftNum) || !int.TryParse(rightStr, out int rightNum))
                return false;

            // First number should be positive and not more than 20
            if (leftNum <= 0 || leftNum > 20)
                return false;

            // Second number (denominator) must always be 20
            if (rightNum != 20)
                return false;

            return true;
        }

        // Method to get the numerator (left number) from visual acuity string
        private int? GetVisualAcuityNumerator(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return null;

            int index = userInput.IndexOf('/');
            if (index <= 0)
                return null;

            string leftStr = userInput.Substring(0, index).Trim();
            if (int.TryParse(leftStr, out int leftNum))
                return leftNum;

            return null;
        }

        // Method to set textbox border color based on visual acuity value
        private void SetVisualAcuityBorderColor(TextBox textBox, string input)
        {
            // Reset to default first
            textBox.BorderStyle = BorderStyle.Fixed3D;

            if (string.IsNullOrWhiteSpace(input))
            {
                // Reset to default border for empty input
                return;
            }

            if (!IsValidVisualAcuity(input))
            {
                // Keep red text color for invalid format, but don't change border
                return;
            }

            int? numerator = GetVisualAcuityNumerator(input);
            if (!numerator.HasValue)
                return;

            // Create a custom paint event to draw colored border
            // Since TextBox doesn't have a direct border color property, we'll use the Paint event
            // But for simpler implementation, we'll change the BackColor slightly to indicate the status

            if (numerator.Value < 10)
            {
                // Red border indication - use light red background tint
                textBox.BackColor = Color.FromArgb(255, 240, 240); // Very light red
            }
            else if (numerator.Value >= 10 && numerator.Value <= 17)
            {
                // Orange border indication - use light orange background tint
                textBox.BackColor = Color.FromArgb(255, 248, 230); // Very light orange
            }
            else if (numerator.Value >= 18 && numerator.Value <= 20)
            {
                // Green border indication - use light green background tint
                textBox.BackColor = Color.FromArgb(240, 255, 240); // Very light green
            }
        }

        // Alternative method using Panel wrapper for true border coloring
        private void SetTextBoxBorderColor(TextBox textBox, Color borderColor)
        {
            // This method assumes each TextBox is wrapped in a Panel for border coloring
            // You would need to modify your form design to implement this properly

            if (textBox.Parent is Panel panel)
            {
                panel.BackColor = borderColor;
                panel.Padding = new Padding(2);
                textBox.Dock = DockStyle.Fill;
            }
        }

        // Method to show immediate error for visual acuity with border coloring
        private void ShowVisualAcuityError(TextBox textBox, string eyeSide)
        {
            string input = textBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                textBox.ForeColor = Color.Black;
                textBox.BackColor = SystemColors.Window; // Reset to default
                return;
            }

            if (!IsValidVisualAcuity(input))
            {
                textBox.ForeColor = Color.Red;
                textBox.BackColor = SystemColors.Window; // Keep default background for invalid input

                // Show immediate error message
                string errorMsg = $"Invalid {eyeSide} eye visual acuity format.\n" +
                                 "Required format: number/20 (e.g., 20/20, 15/20, 10/20)\n" +
                                 "First number must be between 1 and 20, second number must be 20.";

                // Use a timer to show the error briefly to avoid constant popups
                ToolTip toolTip = new ToolTip();
                toolTip.Show(errorMsg, textBox, 0, -30, 3000); // Show for 3 seconds
            }
            else
            {
                textBox.ForeColor = Color.Black;
                // Set border color based on visual acuity value
                SetVisualAcuityBorderColor(textBox, input);
            }
        }

        // Helper method for immediate sphere/cylinder validation
        private void ValidateSphereOrCylinder(TextBox textBox, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Black;
                return;
            }

            try
            {
                ValidateAndParseDecimal(textBox.Text, fieldName);
                textBox.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                textBox.ForeColor = Color.Red;

                ToolTip toolTip = new ToolTip();
                toolTip.Show(ex.Message, textBox, 0, -30, 3000);
            }
        }

        // Updated event handlers for immediate validation with border coloring
        private void vaRightEye_TextChanged(object sender, EventArgs e)
        {
           // ShowVisualAcuityError(vaRightEye, "Right");
        }

        private void vaLeftEye_TextChanged(object sender, EventArgs e)
        {
           // ShowVisualAcuityError(vaLeftEye, "Left");
        }

        // New event handlers for sphere and cylinder validation
        private void txtRightSphere_Leave(object sender, EventArgs e)
        {
           // ValidateSphereOrCylinder(txtRightSphere, "Right Sphere");
        }

        private void txtLeftSphere_Leave(object sender, EventArgs e)
        {
           // ValidateSphereOrCylinder(txtLeftSphere, "Left Sphere");
        }

        private void txtRightCylinder_Leave(object sender, EventArgs e)
        {
            //ValidateSphereOrCylinder(txtRightCylinder, "Right Cylinder");
        }

        private void txtLeftCylinder_Leave(object sender, EventArgs e)
        {
            //ValidateSphereOrCylinder(txtLeftCylinder, "Left Cylinder");
        }

        // Keep original methods for backward compatibility if needed elsewhere
        private decimal? TryParseDecimal(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (decimal.TryParse(input, out decimal result))
                return result;
            return null;
        }

        private int? TryParseInt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (int.TryParse(input, out int result))
                return result;
            return null;
        }

        private void ClearForm()
        {
            // Basic Info
            txtCustID.Clear();
            txtCustomerName.Clear();
            txtDateToday.Text = DateTime.Now.ToString("yyyy-MM-dd");

            // Chief Complaint
            txtChiefComplaint.Clear();

            // Visual Acuity
            vaRightEye.Clear();
            vaLeftEye.Clear();

            // Visual Correction Data
            txtRightSphere.Clear();
            txtRightCylinder.Clear();
            txtRightAxis.Clear();
            txtLeftSphere.Clear();
            txtLeftCylinder.Clear();
            txtLeftAxis.Clear();
            txtAddReading.Clear();

            // Eye Pressure
            txtRightEyePressure.Clear();
            txtLeftEyePressure.Clear();

            // Eye Health
            cmbOverallAssessment.SelectedIndex = -1;
            txtHealthNotes.Clear();

            // Recommendations
            txtTreatmentAdvice.Clear();
            cmbNextVisit.SelectedIndex = -1;

            // Metadata
            cmbOptometrist.SelectedIndex = -1;

            // Enable all controls for new entry
            SetFormEditMode(true);

            // Reset text colors and background colors to default
            vaRightEye.ForeColor = Color.Black;
            vaLeftEye.ForeColor = Color.Black;
            txtRightSphere.ForeColor = Color.Black;
            txtLeftSphere.ForeColor = Color.Black;
            txtRightCylinder.ForeColor = Color.Black;
            txtLeftCylinder.ForeColor = Color.Black;

            // Reset visual acuity textbox background colors
            vaRightEye.BackColor = SystemColors.Window;
            vaLeftEye.BackColor = SystemColors.Window;

            // Reset DataGridViews if needed
            //dgvConsultations.DataSource = null;
            // dgvCustomers.DataSource = null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Validate that we have consultation data to print
            if (string.IsNullOrWhiteSpace(txtCustID.Text))
            {
                MessageBox.Show("No consultation data to print. Please select a consultation first.",
                               "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Create and configure PrintDocument
                PrintDocument printDoc = new PrintDocument();
                printDoc.PrintPage += PrintDoc_PrintPage;
                printDoc.DocumentName = $"Eye Consultation Report - {txtCustomerName.Text}";

                // Show print preview dialog
                PrintPreviewDialog previewDialog = new PrintPreviewDialog();
                previewDialog.Document = printDoc;
                previewDialog.Size = new Size(900, 700);
                previewDialog.StartPosition = FormStartPosition.CenterParent;

                if (previewDialog.ShowDialog(this) == DialogResult.OK)
                {
                    printDoc.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing consultation report: {ex.Message}",
                               "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Define fonts
            Font titleFont = new Font("Arial", 22, FontStyle.Bold);
            Font companyFont = new Font("Arial", 18, FontStyle.Bold);
            Font sloganFont = new Font("Arial", 12, FontStyle.Italic);
            Font headerFont = new Font("Arial", 14, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 12, FontStyle.Bold);
            Font labelFont = new Font("Arial", 10, FontStyle.Bold);
            Font valueFont = new Font("Arial", 10, FontStyle.Regular);
            Font footerFont = new Font("Arial", 9, FontStyle.Italic);

            // Professional colors
            Color primaryBlue = Color.FromArgb(25, 65, 115);
            Color accentBlue = Color.FromArgb(70, 130, 180);
            Color lightBlue = Color.FromArgb(235, 245, 255);
            Color darkText = Color.FromArgb(45, 45, 45);

            // Layout constants
            int leftMargin = 60;
            int rightMargin = 60;
            int topMargin = 60;
            int pageWidth = e.PageBounds.Width - leftMargin - rightMargin;
            int lineHeight = 22;
            int sectionSpacing = 25;
            int fieldSpacing = 18;

            int yPosition = topMargin;

            try
            {
                // COMPANY HEADER SECTION
                g.DrawString("EMONTI OPTOMETRIST", companyFont, new SolidBrush(primaryBlue), leftMargin, yPosition);
                yPosition += 28;

                g.DrawString("Your Vision. Our Priority", sloganFont, new SolidBrush(accentBlue), leftMargin, yPosition);
                yPosition += 35;

                // Report title
                g.DrawString("EYE TEST CONSULTATION REPORT", titleFont, new SolidBrush(primaryBlue), leftMargin, yPosition);
                yPosition += 40;

                // Report generation info
                string reportInfo = $"Generated: {DateTime.Now:dddd, MMMM dd, yyyy 'at' hh:mm tt}";
                g.DrawString(reportInfo, new Font("Arial", 9), new SolidBrush(darkText), leftMargin, yPosition);
                yPosition += 25;

                // Header separator line
                g.DrawLine(new Pen(primaryBlue, 2), leftMargin, yPosition, leftMargin + pageWidth, yPosition);
                yPosition += 30;

                // PATIENT INFORMATION SECTION
                DrawSectionHeader(g, "PATIENT INFORMATION", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                yPosition = DrawLabelValuePair(g, "Patient ID:", GetDisplayValue(txtCustID.Text), leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Patient Name:", GetDisplayValue(txtCustomerName.Text), leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Consultation Date:", GetDisplayValue(txtDateToday.Text), leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Optometrist:", GetDisplayValue(GetComboBoxValue(cmbOptometrist)), leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition += sectionSpacing;

                // CHIEF COMPLAINT SECTION
                DrawSectionHeader(g, "CHIEF COMPLAINT", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                yPosition = DrawMultiLineText(g, GetDisplayValue(txtChiefComplaint.Text), leftMargin + 15, yPosition, pageWidth - 30, valueFont, darkText, lineHeight);
                yPosition += sectionSpacing;

                // VISUAL ACUITY SECTION
                DrawSectionHeader(g, "VISUAL ACUITY ASSESSMENT", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                yPosition = DrawTwoColumnData(g, "Right Eye:", GetDisplayValue(vaRightEye.Text), "Left Eye:", GetDisplayValue(vaLeftEye.Text),
                                             leftMargin + 15, yPosition, pageWidth, labelFont, valueFont, darkText);
                yPosition += sectionSpacing;

                // PRESCRIPTION DETAILS SECTION
                DrawSectionHeader(g, "PRESCRIPTION DETAILS", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                // Right Eye subsection
                g.DrawString("Right Eye", subHeaderFont, new SolidBrush(accentBlue), leftMargin + 15, yPosition);
                yPosition += 25;
                yPosition = DrawLabelValuePair(g, "Sphere:", GetDisplayValue(txtRightSphere.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Cylinder:", GetDisplayValue(txtRightCylinder.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Axis:", GetDisplayValue(txtRightAxis.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition += 15;

                // Left Eye subsection
                g.DrawString("Left Eye", subHeaderFont, new SolidBrush(accentBlue), leftMargin + 15, yPosition);
                yPosition += 25;
                yPosition = DrawLabelValuePair(g, "Sphere:", GetDisplayValue(txtLeftSphere.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Cylinder:", GetDisplayValue(txtLeftCylinder.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition = DrawLabelValuePair(g, "Axis:", GetDisplayValue(txtLeftAxis.Text), leftMargin + 30, yPosition, labelFont, valueFont, darkText);
                yPosition += 15;

                // Add Reading
                yPosition = DrawLabelValuePair(g, "Add Reading:", GetDisplayValue(txtAddReading.Text), leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition += sectionSpacing;

                // EYE PRESSURE SECTION
                DrawSectionHeader(g, "EYE PRESSURE MEASUREMENTS", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                string rightPressure = GetDisplayValue(txtRightEyePressure.Text);
                if (rightPressure != "Not Applicable") rightPressure += " mmHg";

                string leftPressure = GetDisplayValue(txtLeftEyePressure.Text);
                if (leftPressure != "Not Applicable") leftPressure += " mmHg";

                yPosition = DrawTwoColumnData(g, "Right Eye:", rightPressure, "Left Eye:", leftPressure,
                                             leftMargin + 15, yPosition, pageWidth, labelFont, valueFont, darkText);
                yPosition += sectionSpacing;

                // EYE HEALTH ASSESSMENT SECTION
                DrawSectionHeader(g, "EYE HEALTH ASSESSMENT", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                yPosition = DrawLabelValuePair(g, "Overall Assessment:", GetDisplayValue(GetComboBoxValue(cmbOverallAssessment)),
                                              leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition += 10;

                g.DrawString("Health Notes:", labelFont, new SolidBrush(darkText), leftMargin + 15, yPosition);
                yPosition += fieldSpacing;
                yPosition = DrawMultiLineText(g, GetDisplayValue(txtHealthNotes.Text), leftMargin + 15, yPosition,
                                             pageWidth - 30, valueFont, darkText, lineHeight);
                yPosition += sectionSpacing;

                // RECOMMENDATIONS SECTION
                DrawSectionHeader(g, "RECOMMENDATIONS & TREATMENT", leftMargin, yPosition, pageWidth, headerFont, primaryBlue, lightBlue);
                yPosition += 35;

                g.DrawString("Treatment & Advice:", labelFont, new SolidBrush(darkText), leftMargin + 15, yPosition);
                yPosition += fieldSpacing;
                yPosition = DrawMultiLineText(g, GetDisplayValue(txtTreatmentAdvice.Text), leftMargin + 15, yPosition,
                                             pageWidth - 30, valueFont, darkText, lineHeight);
                yPosition += 15;

                yPosition = DrawLabelValuePair(g, "Next Visit:", GetDisplayValue(GetComboBoxValue(cmbNextVisit)),
                                              leftMargin + 15, yPosition, labelFont, valueFont, darkText);
                yPosition += sectionSpacing;

                // FOOTER SECTION
                int footerY = e.PageBounds.Height - 80;
                g.DrawLine(new Pen(Color.Gray, 1), leftMargin, footerY, leftMargin + pageWidth, footerY);
                footerY += 15;

                string confidentialText = "This report is confidential medical information intended solely for the patient and authorized healthcare providers.";
                g.DrawString(confidentialText, footerFont, Brushes.Gray, leftMargin, footerY);

                string pageInfo = $"Emonti Optometrist • {DateTime.Now:MM/dd/yyyy} • Page 1 of 1";
                SizeF pageInfoSize = g.MeasureString(pageInfo, footerFont);
                g.DrawString(pageInfo, footerFont, Brushes.Gray, leftMargin + pageWidth - pageInfoSize.Width, footerY);

            }
            catch (Exception ex)
            {
                g.DrawString($"Error generating report: {ex.Message}",
                            new Font("Arial", 12), Brushes.Red, leftMargin, yPosition);
            }
            finally
            {
                // Dispose fonts
                titleFont?.Dispose();
                companyFont?.Dispose();
                sloganFont?.Dispose();
                headerFont?.Dispose();
                subHeaderFont?.Dispose();
                labelFont?.Dispose();
                valueFont?.Dispose();
                footerFont?.Dispose();
            }
        }

        // Helper method to draw section headers with background
        private void DrawSectionHeader(Graphics g, string text, int x, int y, int width, Font font, Color textColor, Color bgColor)
        {
            Rectangle headerRect = new Rectangle(x, y - 5, width, 28);
            using (Brush bgBrush = new SolidBrush(bgColor))
            {
                g.FillRectangle(bgBrush, headerRect);
            }
            g.DrawRectangle(new Pen(textColor, 1), headerRect);
            g.DrawString(text, font, new SolidBrush(textColor), x + 10, y + 2);
        }

        // Helper method to draw label-value pairs
        private int DrawLabelValuePair(Graphics g, string label, string value, int x, int y, Font labelFont, Font valueFont, Color textColor)
        {
            g.DrawString(label, labelFont, new SolidBrush(textColor), x, y);
            g.DrawString(value, valueFont, new SolidBrush(Color.FromArgb(70, 130, 180)), x + 150, y);
            return y + 18;
        }

        // Helper method to draw two-column data
        private int DrawTwoColumnData(Graphics g, string label1, string value1, string label2, string value2,
                                     int x, int y, int pageWidth, Font labelFont, Font valueFont, Color textColor)
        {
            int midPoint = pageWidth / 2;
            g.DrawString(label1, labelFont, new SolidBrush(textColor), x, y);
            g.DrawString(value1, valueFont, new SolidBrush(Color.FromArgb(70, 130, 180)), x + 120, y);
            g.DrawString(label2, labelFont, new SolidBrush(textColor), x + midPoint, y);
            g.DrawString(value2, valueFont, new SolidBrush(Color.FromArgb(70, 130, 180)), x + midPoint + 120, y);
            return y + 18;
        }

        // Helper method to draw multi-line text with proper wrapping
        private int DrawMultiLineText(Graphics g, string text, int x, int y, int maxWidth, Font font, Color color, int lineHeight)
        {
            if (string.IsNullOrEmpty(text) || text == "Not Applicable")
            {
                g.DrawString(text, font, new SolidBrush(Color.FromArgb(70, 130, 180)), x, y);
                return y + lineHeight;
            }

            string[] words = text.Split(' ');
            string currentLine = "";
            int currentY = y;

            foreach (string word in words)
            {
                string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                SizeF size = g.MeasureString(testLine, font);

                if (size.Width > maxWidth && !string.IsNullOrEmpty(currentLine))
                {
                    g.DrawString(currentLine, font, new SolidBrush(color), x, currentY);
                    currentY += lineHeight;
                    currentLine = word;
                }
                else
                {
                    currentLine = testLine;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                g.DrawString(currentLine, font, new SolidBrush(color), x, currentY);
                currentY += lineHeight;
            }

            return currentY;
        }

        // Helper method to get display value or "Not Applicable"
        private string GetDisplayValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "Not Applicable" : value.Trim();
        }

        // Helper method to safely get ComboBox selected value
        private string GetComboBoxValue(ComboBox comboBox)
        {
            return comboBox?.SelectedItem?.ToString() ?? "";
        }

        private string GetValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim();
        }

        private string GetComboValue(ComboBox combo)
        {
            return combo?.SelectedItem?.ToString() ?? "N/A";
        }

        private string GetPressure(string value)
        {
            string pressure = GetValue(value);
            return pressure != "N/A" ? pressure + " mmHg" : "N/A";
        }

        private string[] WrapText(string text, int maxLength)
        {
            if (text.Length <= maxLength) return new[] { text };

            var lines = new System.Collections.Generic.List<string>();
            string[] words = text.Split(' ');
            string currentLine = "";

            foreach (string word in words)
            {
                if ((currentLine + " " + word).Length > maxLength)
                {
                    if (!string.IsNullOrEmpty(currentLine))
                    {
                        lines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                    {
                        lines.Add(word);
                    }
                }
                else
                {
                    currentLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
                lines.Add(currentLine);

            return lines.ToArray();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            // TODO: Navigate to first record
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // TODO: Navigate to previous record
        }

        private void dgvCustomers_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvCustomers.CurrentRow != null)
            {
                taConsultations.FillByCustID(dsOMS.Consultations, (int)dgvCustomers.CurrentRow.Cells[0].Value);

                // Clear form and enable for new consultation
                ClearForm();

                // Set customer info after clearing
                txtCustID.Text = dgvCustomers.CurrentRow.Cells[0].Value.ToString();
                txtCustomerName.Text = dgvCustomers.CurrentRow.Cells[1].Value.ToString() + " " + dgvCustomers.CurrentRow.Cells[2].Value.ToString();
                txtDateToday.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        // Removed the old incorrectly named IsValidSphere method
        public bool haveNoLetters(string userInput)
        {
            foreach (char c in userInput)
            {
                if (char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }

        // Event handler for consultation selection - READ ONLY MODE
        private void dgvConsultations_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvConsultations.CurrentRow != null)
            {
                PopulateFormWithConsultationData(dgvConsultations.CurrentRow);

                // Set form to read-only mode after populating data
                SetFormEditMode(false);
            }
        }

        // Populate form with consultation data using INDEX NUMBERS to avoid column name errors
        private void PopulateFormWithConsultationData(DataGridViewRow selectedRow)
        {
            try
            {
                // Using index numbers based on typical consultation table structure
                // Adjust these indices based on your actual DataGridView column order

                // Basic Info (assuming first few columns)
                txtCustID.Text = selectedRow.Cells[0]?.Value?.ToString() ?? "";

                // Date (assuming column 1 is consultation date)
                if (selectedRow.Cells[2]?.Value != null)
                {
                    DateTime consultDate = Convert.ToDateTime(selectedRow.Cells[2].Value);
                    txtDateToday.Text = consultDate.ToString("yyyy-MM-dd");
                }

                // Chief Complaint (assuming column 2)
                txtChiefComplaint.Text = selectedRow.Cells[3]?.Value?.ToString() ?? "";

                // Visual Acuity (assuming columns 3 and 4)
                vaRightEye.Text = selectedRow.Cells[4]?.Value?.ToString() ?? "";
                vaLeftEye.Text = selectedRow.Cells[5]?.Value?.ToString() ?? "";

                // Apply border coloring after setting the text
                if (!string.IsNullOrEmpty(vaRightEye.Text))
                    SetVisualAcuityBorderColor(vaRightEye, vaRightEye.Text);
                if (!string.IsNullOrEmpty(vaLeftEye.Text))
                    SetVisualAcuityBorderColor(vaLeftEye, vaLeftEye.Text);

                // Right Eye Prescription (assuming columns 5, 6, 7)
                txtRightSphere.Text = selectedRow.Cells[6]?.Value?.ToString() ?? "";
                txtRightCylinder.Text = selectedRow.Cells[7]?.Value?.ToString() ?? "";
                txtRightAxis.Text = selectedRow.Cells[8]?.Value?.ToString() ?? "";

                // Left Eye Prescription (assuming columns 8, 9, 10)
                txtLeftSphere.Text = selectedRow.Cells[9]?.Value?.ToString() ?? "";
                txtLeftCylinder.Text = selectedRow.Cells[10]?.Value?.ToString() ?? "";
                txtLeftAxis.Text = selectedRow.Cells[11]?.Value?.ToString() ?? "";

                // Add Reading (assuming column 11)
                txtAddReading.Text = selectedRow.Cells[12]?.Value?.ToString() ?? "";

                // Eye Pressure (assuming columns 12 and 13)
                txtRightEyePressure.Text = selectedRow.Cells[13]?.Value?.ToString() ?? "";
                txtLeftEyePressure.Text = selectedRow.Cells[14]?.Value?.ToString() ?? "";

                // Health Assessment (assuming column 14)
                string healthAssessment = selectedRow.Cells[15]?.Value?.ToString();
                SetComboBoxValue(cmbOverallAssessment, healthAssessment);

                // Health Notes (assuming column 15)
                txtHealthNotes.Text = selectedRow.Cells[16]?.Value?.ToString() ?? "";

                // Treatment Advice (assuming column 16)
                txtTreatmentAdvice.Text = selectedRow.Cells[17]?.Value?.ToString() ?? "";

                // Next Visit (assuming column 17)
                string nextVisit = selectedRow.Cells[18]?.Value?.ToString();
                SetComboBoxValue(cmbNextVisit, nextVisit);

                // Optometrist/Created By (assuming column 18)
                string createdBy = selectedRow.Cells[19]?.Value?.ToString();
                SetComboBoxValue(cmbOptometrist, createdBy);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading consultation data: " + ex.Message + "\nPlease check the column order in your DataGridView.", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to set combo box values safely
        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                comboBox.SelectedIndex = -1;
                return;
            }

            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if (comboBox.Items[i].ToString() == value)
                {
                    comboBox.SelectedIndex = i;
                    return;
                }
            }
            comboBox.SelectedIndex = -1; // If value not found
        }

        // Method to control form edit mode (enable/disable controls)
        private void SetFormEditMode(bool isEditing)
        {
            // Text boxes
            txtChiefComplaint.ReadOnly = !isEditing;
            vaRightEye.ReadOnly = !isEditing;
            vaLeftEye.ReadOnly = !isEditing;
            txtRightSphere.ReadOnly = !isEditing;
            txtRightCylinder.ReadOnly = !isEditing;
            txtRightAxis.ReadOnly = !isEditing;
            txtLeftSphere.ReadOnly = !isEditing;
            txtLeftCylinder.ReadOnly = !isEditing;
            txtLeftAxis.ReadOnly = !isEditing;
            txtAddReading.ReadOnly = !isEditing;
            txtRightEyePressure.ReadOnly = !isEditing;
            txtLeftEyePressure.ReadOnly = !isEditing;
            txtHealthNotes.ReadOnly = !isEditing;
            txtTreatmentAdvice.ReadOnly = !isEditing;

            // Combo boxes
            cmbOverallAssessment.Enabled = isEditing;
            cmbNextVisit.Enabled = isEditing;
            cmbOptometrist.Enabled = isEditing;

            // Buttons
            btnSave.Enabled = isEditing;

            // Change background color to indicate read-only mode
            // But preserve visual acuity color coding
            Color backgroundColor = isEditing ? SystemColors.Window : SystemColors.Control;

            txtChiefComplaint.BackColor = backgroundColor;
            // Don't change vaRightEye and vaLeftEye background colors here as they use color coding
            txtRightSphere.BackColor = backgroundColor;
            txtRightCylinder.BackColor = backgroundColor;
            txtRightAxis.BackColor = backgroundColor;
            txtLeftSphere.BackColor = backgroundColor;
            txtLeftCylinder.BackColor = backgroundColor;
            txtLeftAxis.BackColor = backgroundColor;
            txtAddReading.BackColor = backgroundColor;
            txtRightEyePressure.BackColor = backgroundColor;
            txtLeftEyePressure.BackColor = backgroundColor;
            txtHealthNotes.BackColor = backgroundColor;
            txtTreatmentAdvice.BackColor = backgroundColor;

            // For visual acuity fields, only reset if in editing mode
            if (isEditing)
            {
                // Re-apply color coding when entering edit mode
                if (!string.IsNullOrEmpty(vaRightEye.Text))
                    SetVisualAcuityBorderColor(vaRightEye, vaRightEye.Text);
                if (!string.IsNullOrEmpty(vaLeftEye.Text))
                    SetVisualAcuityBorderColor(vaLeftEye, vaLeftEye.Text);
            }
        }

        private void dgvConsultations_SelectionChanged(object sender, EventArgs e)
        {
            // Optional: You can also handle single-click selection here if needed
            // But double-click is usually better to prevent accidental selections
        }

        // Clear button - clears form and enables editing for new consultation
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
            MessageBox.Show("Form cleared. You can now add a new consultation.", "Information",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}