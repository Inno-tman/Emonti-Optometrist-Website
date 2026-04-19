using OMS.Models;
using OMS.Repositories;
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
    public partial class AddEditCustomer : Form
    {
        private int customerid = 0;

        public AddEditCustomer()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;

            // Set main member fields to read-only by default
            SetMainMemberFieldsReadOnly(true);
        }

        public void EditCustomer(Customer customer)
        {
            this.Text = "Edit Customer";
            this.lbTitle.Text = "Edit Customer";

            // existing fields
            this.lbID.Text = "" + customer.Cust_ID;
            this.txtName.Text = customer.Customer_Name ?? "";
            this.txtSurname.Text = customer.Customer_Surname ?? "";
            this.txtDOB.Text = customer.Customer_DOB ?? "";
            this.cmbGender.Text = customer.Customer_Gender ?? "";
            this.txtEmail.Text = customer.Customer_Email ?? "";
            this.txtPhone.Text = customer.Customer_Phone ?? "";
            this.txtAddress.Text = customer.Customer_Address ?? "";
            this.txtMedical.Text = customer.Medical_Aid ?? "";
            this.txtMedicalNo.Text = customer.Medical_Aid_Number ?? "";

            // main member fields
            this.txtMainName.Text = customer.Main_Member_Name ?? "";
            this.txtMainSurname.Text = customer.Main_Member_Surname ?? "";
            this.txtMainID.Text = customer.Main_Member_ID ?? "";

            // Set comboMainMemberQ based on whether main member data exists
            if (!string.IsNullOrEmpty(customer.Main_Member_Name) ||
                !string.IsNullOrEmpty(customer.Main_Member_Surname) ||
                !string.IsNullOrEmpty(customer.Main_Member_ID))
            {
                this.comboMainMemberQ.SelectedIndex = 1; // No - they are NOT the main member
            }
            else
            {
                this.comboMainMemberQ.SelectedIndex = 0; // Yes - they ARE the main member
            }

            // address fields
            this.txtStreetNumber.Text = customer.Street_Number ?? "";
            this.txtStreetName.Text = customer.Street_Name ?? "";
            this.txtComplexName.Text = customer.Complex_Name ?? "";
            this.txtUnitNumber.Text = customer.Unit_Number ?? "";
            this.txtCity.Text = customer.City ?? "";
            this.comboProvince.Text = customer.Province ?? "";
            this.txtPostalCode.Text = customer.Postal_Code ?? "";

            this.customerid = customer.Cust_ID;
        }

        private void SetMainMemberFieldsReadOnly(bool readOnly)
        {
            txtMainName.ReadOnly = readOnly;
            txtMainSurname.ReadOnly = readOnly;
            txtMainID.ReadOnly = readOnly;

            // Optionally change background color to indicate read-only state
            Color backgroundColor = readOnly ? SystemColors.Control : SystemColors.Window;
            txtMainName.BackColor = backgroundColor;
            txtMainSurname.BackColor = backgroundColor;
            txtMainID.BackColor = backgroundColor;
        }

        private void btnRecord_Click(object sender, EventArgs e)
        {
            //if
            if (string.IsNullOrWhiteSpace(txtName.Text) || hasDigits(txtName.Text))
            {
                MessageBox.Show("Please enter a valid customer name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            // else if
            else if (string.IsNullOrWhiteSpace(txtSurname.Text) || hasDigits(txtSurname.Text))
            {
                MessageBox.Show("Please enter a  valid customer surname.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSurname.Focus();
                return;
            }


            else if (string.IsNullOrWhiteSpace(txtEmail.Text) || !isValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid customer Email, Email should end with @gmail.com.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(txtPhone.Text) || !isvalidNumber(txtPhone.Text))
            {
                MessageBox.Show("Please enter a valid customer Phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();

            }


            else if (cmbGender.SelectedIndex == -1)
            {
                MessageBox.Show("Please enter Select Gender", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbGender.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(txtStreetName.Text))
            {
                MessageBox.Show("Please enter Street Number and Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStreetName.Focus();
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Please enter City Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCity.Focus();
                return;
            }

            // Main member validation - only if "No" is selected (they are NOT the main member)
            if (comboMainMemberQ.SelectedIndex == 1) // No selected - need main member details
            {
                if (string.IsNullOrWhiteSpace(txtMainName.Text) || hasDigits(txtMainName.Text))
                {
                    MessageBox.Show("Please enter a valid main member name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMainName.Focus();
                    return;
                }

                else if (string.IsNullOrWhiteSpace(txtMainSurname.Text) || hasDigits(txtMainSurname.Text))
                {
                    MessageBox.Show("Please enter a valid main member surname.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMainSurname.Focus();
                    return;
                }

                else if (string.IsNullOrWhiteSpace(txtMainID.Text) || !isValidSAID(txtMainID.Text))
                {
                    MessageBox.Show("Please enter a valid 13-digit SA ID number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMainID.Focus();
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(txtStreetNumber.Text))
            {
                MessageBox.Show("Please enter a street number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStreetNumber.Focus();
                return;
            }

            else if (comboProvince.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a province.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboProvince.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(txtPostalCode.Text) || !isValidPostalCode(txtPostalCode.Text))
            {
                MessageBox.Show("Please enter a valid 4-digit postal code.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPostalCode.Focus();
                return;
            }
            //else
            else
            {
                try
                {
                    Customer customer = new Customer();
                    customer.Cust_ID = this.customerid;
                    customer.Customer_Name = this.validate(txtName.Text).Trim();
                    customer.Customer_Surname = this.validate(txtSurname.Text).Trim();
                    customer.Customer_DOB = this.txtDOB.Text.Trim();
                    customer.Customer_Gender = this.cmbGender.Text.Trim();
                    customer.Customer_Email = this.validategmail(txtEmail.Text).Trim();
                    customer.Customer_Phone = this.txtPhone.Text.Trim();
                    customer.Customer_Address = this.Adress(txtStreetNumber.Text, txtStreetName.Text, txtCity.Text);

                    customer.Medical_Aid = this.txtMedical.Text.Trim();
                    customer.Medical_Aid_Number = this.txtMedicalNo.Text.Trim();

                    // Set main member data based on selection
                    if (comboMainMemberQ.SelectedIndex == 1) // No selected - they are NOT the main member
                    {
                        customer.Main_Member_Name = this.validate(txtMainName.Text).Trim();
                        customer.Main_Member_Surname = this.validate(txtMainSurname.Text).Trim();
                        customer.Main_Member_ID = this.txtMainID.Text.Trim();
                    }
                    else // Yes selected - they ARE the main member, so set to null
                    {
                        customer.Main_Member_Name = null;
                        customer.Main_Member_Surname = null;
                        customer.Main_Member_ID = null;
                    }

                    customer.Street_Number = this.txtStreetNumber.Text.Trim();
                    customer.Street_Name = this.txtStreetName.Text.Trim();
                    customer.Complex_Name = this.txtComplexName.Text.Trim();
                    customer.Unit_Number = this.txtUnitNumber.Text.Trim();
                    customer.City = this.txtCity.Text.Trim();
                    customer.Province = this.comboProvince.Text.Trim();
                    customer.Postal_Code = this.txtPostalCode.Text.Trim();

                    var repo = new CustomerRepository();

                    if (customer.Cust_ID == 0)
                    {
                        repo.CreateCustomer(customer);
                        MessageBox.Show("Customer created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    else
                    {
                        repo.UpdateCustomer(customer);

                        MessageBox.Show("Customer updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            // Empty event handler
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------

        public bool hasDigits(string userInput) // this method checks if the name or surname cantains any number
        {

            foreach (char c in userInput)
            {
                if (char.IsDigit(c) || char.IsSymbol(c))
                {

                    return true;
                }

            }
            return false;


        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------

        public bool isvalidNumber(string userInput) // this code checks if the number entered is valid
        {
            return userInput.Length == 10 && userInput.StartsWith("0");
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------------

        public bool isValidEmail(string userInput)  // this code checks if the email entered is valid
        {
            return userInput.EndsWith("@gmail.com");
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------


        public string validate(string userInput)
        {


            string str = userInput[0].ToString().ToUpper();
            string input = userInput.Remove(0, 1).ToLower();
            string validated = str + input;



            return validated;
        }


        public string validategmail(string email)
        {
            return email.ToLower();
        }

        public string Adress(string streetNumber, string streetName, string city)
        {
            return streetNumber.Trim() + " " + streetName.Trim() + ", " + city.Trim();
        }

        public bool isValidSAID(string idNumber)
        {
            // Check if it's exactly 13 digits and not empty
            return !string.IsNullOrWhiteSpace(idNumber) &&
                   idNumber.Length == 13 &&
                   idNumber.All(char.IsDigit);
        }

        public bool isValidPostalCode(string postalCode)
        {
            // Postal codes in SA are typically 4 digits
            return !string.IsNullOrWhiteSpace(postalCode) &&
                   postalCode.Length == 4 &&
                   postalCode.All(char.IsDigit);
        }

        private void dtpDOB_ValueChanged(object sender, EventArgs e)
        {
            txtDOB.Text = dtpDOB.Value.ToShortDateString();
        }

        private void txtPhone_TextChanged_1(object sender, EventArgs e)
        {
            if (!isvalidNumber(txtPhone.Text))
            {
                txtPhone.ForeColor = Color.Red;
            }
            else
            {
                txtPhone.ForeColor = Color.Black;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (hasDigits(txtName.Text))
            {
                txtName.ForeColor = Color.Red;
            }
            else
            {
                txtName.ForeColor = Color.Black;
            }

        }

        private void txtSurname_TextChanged(object sender, EventArgs e)
        {
            if (hasDigits(txtSurname.Text))
            {
                txtSurname.ForeColor = Color.Red;

            }
            else
            {
                txtSurname.ForeColor = Color.Black;
            }

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            if (!isValidEmail(txtEmail.Text))
            {
                txtEmail.ForeColor = Color.Red;
            }
            else
            {
                txtEmail.ForeColor = Color.Black;
            }
        }

        private void AddEditCustomer_Load(object sender, EventArgs e)
        {
            if (lbTitle.Text == "Create Customer")
            {
                txtAddress.Visible = false;
            }
        }

        private void txtMainName_TextChanged(object sender, EventArgs e)
        {
            if (hasDigits(txtMainName.Text))
            {
                txtMainName.ForeColor = Color.Red;
            }
            else
            {
                txtMainName.ForeColor = Color.Black;
            }
        }

        private void txtMainID_TextChanged(object sender, EventArgs e)
        {
            if (!isValidSAID(txtMainID.Text) && !string.IsNullOrEmpty(txtMainID.Text))
            {
                txtMainID.ForeColor = Color.Red;
            }
            else
            {
                txtMainID.ForeColor = Color.Black;
            }
        }

        private void txtMainSurname_TextChanged(object sender, EventArgs e)
        {
            if (hasDigits(txtMainSurname.Text))
            {
                txtMainSurname.ForeColor = Color.Red;
            }
            else
            {
                txtMainSurname.ForeColor = Color.Black;
            }
        }

        private void txtPostalCode_TextChanged(object sender, EventArgs e)
        {
            if (!isValidPostalCode(txtPostalCode.Text) && !string.IsNullOrEmpty(txtPostalCode.Text))
            {
                txtPostalCode.ForeColor = Color.Red;
            }
            else
            {
                txtPostalCode.ForeColor = Color.Black;
            }
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboMainMemberQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboMainMemberQ.SelectedIndex == 0) // Yes selected - they ARE the main member
            {
                SetMainMemberFieldsReadOnly(true); // Keep read-only
                // Clear the fields since they are the main member
                txtMainName.Text = "";
                txtMainSurname.Text = "";
                txtMainID.Text = "";
            }
            else // No selected - they are NOT the main member
            {
                SetMainMemberFieldsReadOnly(false); // Enable input for main member details
            }
        }
    }
}