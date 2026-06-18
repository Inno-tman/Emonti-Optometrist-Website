using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using Emonti_Optometrist_Website.Models;
using System.Security.Cryptography;
using System.Text;
namespace Emonti_Optometrist_Website.Account
{
    public partial class Register : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rbHasMedicalAidNo.Checked = true;
                rbHasMedicalAidYes.Checked = false;
                rbIsMainMemberYes.Checked = true;
                rbIsMainMemberNo.Checked = false;
                mainMemberDetails.Style["display"] = "none";
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            bool hasMedicalAid = rbHasMedicalAidYes.Checked;
            bool isNotMainMember = rbIsMainMemberNo.Checked;
            bool showMainMemberFields = hasMedicalAid && isNotMainMember;

            rfvMainMemberName.Enabled = showMainMemberFields;
            rfvMainMemberSurname.Enabled = showMainMemberFields;
            rfvMainMemberID.Enabled = showMainMemberFields;
            revMainMemberName.Enabled = showMainMemberFields;
            revMainMemberSurname.Enabled = showMainMemberFields;
            revMainMemberID.Enabled = showMainMemberFields;
        }

        protected void rbHasMedicalAid_Changed(object sender, EventArgs e)
        {
            if (rbHasMedicalAidNo.Checked)
            {
                rbIsMainMemberYes.Checked = true;
                rbIsMainMemberNo.Checked = false;
                mainMemberDetails.Style["display"] = "none";
            }
        }

        protected void ValidateDateOfBirth(object source, ServerValidateEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Value))
            {
                args.IsValid = true;  // Date of birth is optional
                return;
            }

            if (DateTime.TryParse(args.Value, out DateTime dob))
            {
                int age = DateTime.Today.Year - dob.Year;
                if (dob > DateTime.Today.AddYears(-age)) age--; // Adjust age if birthday hasn't occurred this year

                args.IsValid = (age >= 13 && age <= 80);
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void rbIsMainMember_Changed(object sender, EventArgs e)
        {
            mainMemberDetails.Style["display"] = rbIsMainMemberNo.Checked ? "block" : "none";
            
            // Enable/disable validators based on selection
            rfvMainMemberName.Enabled = rbIsMainMemberNo.Checked;
            rfvMainMemberSurname.Enabled = rbIsMainMemberNo.Checked;
            rfvMainMemberID.Enabled = rbIsMainMemberNo.Checked;
            
            revMainMemberName.Enabled = rbIsMainMemberNo.Checked;
            revMainMemberSurname.Enabled = rbIsMainMemberNo.Checked;
            revMainMemberID.Enabled = rbIsMainMemberNo.Checked;
        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            try
            {
                // Reset messages
                ErrorMessage.Visible = false;
                SuccessMessage.Visible = false;

                if (!ValidateAccountInfo() || !ValidatePersonalInfo() || !ValidateMedicalInfo() || !ValidateAddressInfo())
                    return;

                InsertCustomerDetails();

                // Store success message in session and redirect to login
                Session["RegistrationSuccess"] = "Account created successfully! Please login to continue.";
                Response.Redirect("~/Account/Login.aspx");
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "An error occurred while creating your account: " + ex.Message;
                ErrorMessage.Visible = true;
                SuccessMessage.Visible = false;
            }
        }

        private string BuildAddress()
        {
            // Customer_Address = Street Number + Street Name + City ONLY
            // Like: "88 Camps Bay Dr, Cape Town"
            
            List<string> addressParts = new List<string>();

            // Street: Number + Name
            if (!string.IsNullOrWhiteSpace(txtStreetNumber.Text) || !string.IsNullOrWhiteSpace(txtStreetName.Text))
            {
                string street = $"{txtStreetNumber.Text.Trim()} {txtStreetName.Text.Trim()}".Trim();
                if (!string.IsNullOrWhiteSpace(street))
                    addressParts.Add(street);
            }

            // City
            if (!string.IsNullOrWhiteSpace(txtCity.Text))
                addressParts.Add(txtCity.Text.Trim());

            // Return: "Street, City" format like your records
            return addressParts.Count > 0 ? string.Join(", ", addressParts) : string.Empty;
        }

        private void InsertCustomerDetails()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string query = @"
                            INSERT INTO customer (
                                Customer_Name, Customer_Surname, Customer_DOB, Customer_Gender,
                                Customer_Email, Customer_Phone, Customer_Address,
                                Medical_Aid, Medical_Aid_Number,
                                Main_Member_Name, Main_Member_Surname, Main_Member_ID,
                                Street_Number, Street_Name, Complex_Name, Unit_Number,
                                City, Province, Postal_Code, Is_Archive, Customer_Password
                            ) VALUES (
                                @Customer_Name, @Customer_Surname, @Customer_DOB, @Customer_Gender,
                                @Customer_Email, @Customer_Phone, @Customer_Address,
                                @Medical_Aid, @Medical_Aid_Number,
                                @Main_Member_Name, @Main_Member_Surname, @Main_Member_ID,
                                @Street_Number, @Street_Name, @Complex_Name, @Unit_Number,
                                @City, @Province, @Postal_Code, 0, @Customer_Password
                            )";

                        using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                        {
                            // Required fields
                            cmd.Parameters.AddWithValue("@Customer_Name", txtFirstName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Customer_Surname", txtLastName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Customer_Email", txtEmail.Text.Trim());
                            cmd.Parameters.AddWithValue("@Customer_Phone", txtPhone.Text.Trim());
                            cmd.Parameters.AddWithValue("@Customer_Password", txtPassword.Text.Trim());

                            // Required fields that can't be NULL in database
                            cmd.Parameters.AddWithValue("@Customer_DOB",
                                string.IsNullOrWhiteSpace(txtDateOfBirth.Text) ? DateTime.Now : DateTime.Parse(txtDateOfBirth.Text));
                            
                            cmd.Parameters.AddWithValue("@Customer_Gender", 
                                string.IsNullOrWhiteSpace(ddlGender.SelectedValue) ? "Not Specified" : ddlGender.SelectedValue);

                            // Build address from individual components - store full address like your records
                            string address = BuildAddress();
                            cmd.Parameters.AddWithValue("@Customer_Address", address);

                            // Medical Aid fields - conditional
                            if (rbHasMedicalAidNo.Checked)
                            {
                                cmd.Parameters.AddWithValue("@Medical_Aid", DBNull.Value);
                                cmd.Parameters.AddWithValue("@Medical_Aid_Number", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Medical_Aid", txtMedicalAid.Text.Trim());
                                cmd.Parameters.AddWithValue("@Medical_Aid_Number", txtMedicalAidNumber.Text.Trim());
                            }

                            // Address components - optional
                            cmd.Parameters.AddWithValue("@Street_Number", 
                                string.IsNullOrWhiteSpace(txtStreetNumber.Text) ? (object)DBNull.Value : txtStreetNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@Street_Name", 
                                string.IsNullOrWhiteSpace(txtStreetName.Text) ? (object)DBNull.Value : txtStreetName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Complex_Name", 
                                string.IsNullOrWhiteSpace(txtComplexName.Text) ? (object)DBNull.Value : txtComplexName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Unit_Number", 
                                string.IsNullOrWhiteSpace(txtUnitNumber.Text) ? (object)DBNull.Value : txtUnitNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@City", 
                                string.IsNullOrWhiteSpace(txtCity.Text) ? (object)DBNull.Value : txtCity.Text.Trim());
                            cmd.Parameters.AddWithValue("@Province", 
                                string.IsNullOrWhiteSpace(ddlProvince.SelectedValue) ? (object)DBNull.Value : ddlProvince.SelectedValue);
                            cmd.Parameters.AddWithValue("@Postal_Code", 
                                string.IsNullOrWhiteSpace(txtPostalCode.Text) ? (object)DBNull.Value : txtPostalCode.Text.Trim());

                            // Main Member fields - conditional
                            if (rbHasMedicalAidNo.Checked || rbIsMainMemberYes.Checked)
                            {
                                cmd.Parameters.AddWithValue("@Main_Member_Name", DBNull.Value);
                                cmd.Parameters.AddWithValue("@Main_Member_Surname", DBNull.Value);
                                cmd.Parameters.AddWithValue("@Main_Member_ID", DBNull.Value);
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@Main_Member_Name", 
                                    string.IsNullOrWhiteSpace(txtMainMemberName.Text) ? (object)DBNull.Value : txtMainMemberName.Text.Trim());
                                cmd.Parameters.AddWithValue("@Main_Member_Surname", 
                                    string.IsNullOrWhiteSpace(txtMainMemberSurname.Text) ? (object)DBNull.Value : txtMainMemberSurname.Text.Trim());
                                cmd.Parameters.AddWithValue("@Main_Member_ID", 
                                    string.IsNullOrWhiteSpace(txtMainMemberID.Text) ? (object)DBNull.Value : txtMainMemberID.Text.Trim());
                            }

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        System.Diagnostics.Debug.WriteLine($"Database insert error: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                        throw new Exception($"Database error: {ex.Message}", ex);
                    }
                }
            }
        }

        // -----------------------------
        // VALIDATION SECTION
        // -----------------------------

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            return Regex.IsMatch(phone, @"^0\d{9}$"); // South African: starts with 0 + 9 digits
        }

        private bool IsValidPostalCode(string code)
        {
            return Regex.IsMatch(code, @"^\d{4}$"); // SA postal code: 4 digits
        }

        private bool IsValidID(string id)
        {
            return Regex.IsMatch(id, @"^\d{13}$"); // 13 digits
        }

        private bool EmailExists(string email)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE Customer_Email = @Email AND (Is_Archive = 0 OR Is_Archive IS NULL)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking email existence: {ex.Message}");
                return false; // If there's an error, allow registration to proceed
            }
        }

        private bool ValidateAccountInfo()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                ErrorMessage.Text = "Please enter a valid email address.";
                ErrorMessage.Visible = true;
                return false;
            }

            // Check if email already exists
            if (EmailExists(txtEmail.Text.Trim()))
            {
                ErrorMessage.Text = "An account with this email address already exists. Please login or use a different email.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 6)
            {
                ErrorMessage.Text = "Password must be at least 6 characters long.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (txtPassword.Text.Length > 8)
            {
                ErrorMessage.Text = "Password must be 8 characters or less (to match database limit).";
                ErrorMessage.Visible = true;
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ErrorMessage.Text = "Passwords do not match.";
                ErrorMessage.Visible = true;
                return false;
            }

            return true;
        }

        private bool ValidatePersonalInfo()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || !Regex.IsMatch(txtFirstName.Text, @"^[a-zA-Z\s\-']{2,50}$"))
            {
                ErrorMessage.Text = "Please enter a valid first name.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text) || !Regex.IsMatch(txtLastName.Text, @"^[a-zA-Z\s\-']{2,50}$"))
            {
                ErrorMessage.Text = "Please enter a valid surname.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !IsValidPhoneNumber(txtPhone.Text))
            {
                ErrorMessage.Text = "Please enter a valid South African phone number (e.g. 0821234567).";
                ErrorMessage.Visible = true;
                return false;
            }

            // Date of Birth is required
            if (string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                ErrorMessage.Text = "Please enter your date of birth.";
                ErrorMessage.Visible = true;
                return false;
            }

            // Gender is required
            if (string.IsNullOrWhiteSpace(ddlGender.SelectedValue))
            {
                ErrorMessage.Text = "Please select your gender.";
                ErrorMessage.Visible = true;
                return false;
            }

            return true;
        }

        private bool ValidateMedicalInfo()
        {
            // Skip all medical validation if user does not have medical aid
            if (rbHasMedicalAidNo.Checked)
                return true;

            // Validate medical aid provider
            if (string.IsNullOrWhiteSpace(txtMedicalAid.Text))
            {
                ErrorMessage.Text = "Please enter your medical aid provider.";
                ErrorMessage.Visible = true;
                return false;
            }

            // Validate medical aid number
            if (string.IsNullOrWhiteSpace(txtMedicalAidNumber.Text) || !Regex.IsMatch(txtMedicalAidNumber.Text, @"^[a-zA-Z0-9\s\-_\.]{3,50}$"))
            {
                ErrorMessage.Text = "Please enter a valid medical aid number.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (rbIsMainMemberNo.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtMainMemberName.Text) || !Regex.IsMatch(txtMainMemberName.Text, @"^[a-zA-Z\s\-']{2,50}$"))
                {
                    ErrorMessage.Text = "Please enter a valid main member first name.";
                    ErrorMessage.Visible = true;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtMainMemberSurname.Text) || !Regex.IsMatch(txtMainMemberSurname.Text, @"^[a-zA-Z\s\-']{2,50}$"))
                {
                    ErrorMessage.Text = "Please enter a valid main member surname.";
                    ErrorMessage.Visible = true;
                    return false;
                }

                if (string.IsNullOrWhiteSpace(txtMainMemberID.Text) || !IsValidID(txtMainMemberID.Text))
                {
                    ErrorMessage.Text = "Main member ID must be 13 digits.";
                    ErrorMessage.Visible = true;
                    return false;
                }
            }

            return true;
        }

        private bool ValidateAddressInfo()
        {
            // Address is required - check if we have at least some address info
            string address = BuildAddress();
            if (string.IsNullOrWhiteSpace(address))
            {
                ErrorMessage.Text = "Please provide at least your city and province.";
                ErrorMessage.Visible = true;
                return false;
            }

            if (!string.IsNullOrEmpty(txtPostalCode.Text) && !IsValidPostalCode(txtPostalCode.Text))
            {
                ErrorMessage.Text = "Please enter a valid 4-digit postal code.";
                ErrorMessage.Visible = true;
                return false;
            }
            return true;
        }

        private void ClearForm()
        {
            txtFirstName.Text = txtLastName.Text = txtEmail.Text = txtPhone.Text = "";
            txtPassword.Text = txtConfirmPassword.Text = "";
            txtDateOfBirth.Text = "";
            txtMedicalAid.Text = txtMedicalAidNumber.Text = "";
            txtStreetNumber.Text = txtStreetName.Text = txtComplexName.Text = txtUnitNumber.Text = "";
            txtCity.Text = txtPostalCode.Text = "";
            ddlProvince.SelectedIndex = 0;
            txtMainMemberName.Text = txtMainMemberSurname.Text = txtMainMemberID.Text = "";
        }
    }
}
