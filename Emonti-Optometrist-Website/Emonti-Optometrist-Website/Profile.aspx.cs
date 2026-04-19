using System;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
    public partial class Profile : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in using session
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            // Redirect to PersonalDetails.aspx for consistency
            Response.Redirect("~/PersonalDetails.aspx");
        }

        private void LoadUserProfile()
        {
            // Mock user data - in a real application, this would come from a database
            txtFirstName.Text = "John";
            txtLastName.Text = "Doe";
            txtEmail.Text = "john.doe@email.com";
            txtPhone.Text = "076 463 1930";
            txtDateOfBirth.Text = "1990-05-15";
            ddlGender.SelectedValue = "Male";
            txtMedicalAid.Text = "Discovery Health";
            txtMedicalAidNumber.Text = "123456789";
            txtAllergies.Text = "None";
        }

        protected void btnPersonalInfo_Click(object sender, EventArgs e)
        {
            ShowTab("pnlPersonalInfo");
            UpdateTabButtons(sender);
        }

        protected void btnOrderHistory_Click(object sender, EventArgs e)
        {
            ShowTab("pnlOrderHistory");
            UpdateTabButtons(sender);
        }

        protected void btnAddresses_Click(object sender, EventArgs e)
        {
            ShowTab("pnlAddresses");
            UpdateTabButtons(sender);
        }

        protected void btnPreferences_Click(object sender, EventArgs e)
        {
            ShowTab("pnlPreferences");
            UpdateTabButtons(sender);
        }

        private void ShowTab(string panelId)
        {
            // Hide all panels
            pnlPersonalInfo.CssClass = "tab-content";
            pnlOrderHistory.CssClass = "tab-content";
            pnlAddresses.CssClass = "tab-content";
            pnlPreferences.CssClass = "tab-content";

            // Show selected panel
            switch (panelId)
            {
                case "pnlPersonalInfo":
                    pnlPersonalInfo.CssClass = "tab-content active";
                    break;
                case "pnlOrderHistory":
                    pnlOrderHistory.CssClass = "tab-content active";
                    break;
                case "pnlAddresses":
                    pnlAddresses.CssClass = "tab-content active";
                    break;
                case "pnlPreferences":
                    pnlPreferences.CssClass = "tab-content active";
                    break;
            }
        }

        private void UpdateTabButtons(object activeButton)
        {
            // Reset all tab buttons
            btnPersonalInfo.CssClass = "profile-tab";
            btnOrderHistory.CssClass = "profile-tab";
            btnAddresses.CssClass = "profile-tab";
            btnPreferences.CssClass = "profile-tab";

            // Set active button
            if (activeButton is System.Web.UI.WebControls.Button button)
            {
                button.CssClass = "profile-tab active";
            }
        }

        protected void btnSavePersonalInfo_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPhone.Text))
            {
                // Show error message
                return;
            }

            // Save personal information to database
            // In a real application, this would update the user's profile in the database
            
            // Show success message
            ScriptManager.RegisterStartupScript(this, GetType(), "Success", 
                "alert('Personal information saved successfully!');", true);
        }

        protected void btnSaveMedicalInfo_Click(object sender, EventArgs e)
        {
            // Save medical information to database
            // In a real application, this would update the user's medical info in the database
            
            // Show success message
            ScriptManager.RegisterStartupScript(this, GetType(), "Success", 
                "alert('Medical information saved successfully!');", true);
        }

        protected void btnSavePreferences_Click(object sender, EventArgs e)
        {
            // Save preferences to database
            // In a real application, this would update the user's preferences in the database
            
            // Show success message
            ScriptManager.RegisterStartupScript(this, GetType(), "Success", 
                "alert('Preferences saved successfully!');", true);
        }

        protected void btnEditHomeAddress_Click(object sender, EventArgs e)
        {
            // Open edit address modal or redirect to edit page
            // In a real application, this would open a modal or redirect to an edit page
        }

        protected void btnDeleteHomeAddress_Click(object sender, EventArgs e)
        {
            // Confirm deletion and remove address
            // In a real application, this would show a confirmation dialog and then delete
        }

        protected void btnEditWorkAddress_Click(object sender, EventArgs e)
        {
            // Open edit address modal or redirect to edit page
            // In a real application, this would open a modal or redirect to an edit page
        }

        protected void btnDeleteWorkAddress_Click(object sender, EventArgs e)
        {
            // Confirm deletion and remove address
            // In a real application, this would show a confirmation dialog and then delete
        }

        protected void btnAddAddress_Click(object sender, EventArgs e)
        {
            // Open add address modal or redirect to add page
            // In a real application, this would open a modal or redirect to an add page
        }
    }
}

