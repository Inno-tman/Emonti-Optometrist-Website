<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Emonti_Optometrist_Website.Profile" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .profile-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
        }
        
        .profile-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
        }
        
        .profile-hero p {
            font-size: 1.2rem;
            max-width: 600px;
            margin: 0 auto;
        }
        
        .profile-container {
            max-width: 1200px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
        }
        
        .profile-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .profile-tabs {
            display: flex;
            gap: 1rem;
            margin-bottom: 2rem;
            border-bottom: 2px solid #e0e0e0;
            padding-bottom: 1rem;
        }
        
        .profile-tab {
            padding: 1rem 2rem;
            background: transparent;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            color: #666;
            transition: all 0.3s ease;
        }
        
        .profile-tab.active {
            background: #2c5aa0;
            color: white;
        }
        
        .profile-tab:hover {
            background: #f8f9fa;
            color: #2c5aa0;
        }
        
        .profile-tab.active:hover {
            background: #1e4080;
            color: white;
        }
        
        .tab-content {
            display: none;
        }
        
        .tab-content.active {
            display: block;
        }
        
        .form-section {
            margin-bottom: 2rem;
        }
        
        .section-title {
            font-size: 1.5rem;
            color: #2c5aa0;
            margin-bottom: 1rem;
            font-weight: 600;
            border-bottom: 2px solid #e0e0e0;
            padding-bottom: 0.5rem;
        }
        
        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 1rem;
            margin-bottom: 1rem;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
        }
        
        .form-group label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
        }
        
        .form-group input, 
        .form-group select, 
        .form-group textarea {
            padding: 1rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
            transition: border-color 0.3s ease;
        }
        
        .form-group input:focus, 
        .form-group select:focus, 
        .form-group textarea:focus {
            border-color: #2c5aa0;
            outline: none;
            box-shadow: 0 0 0 3px rgba(44, 90, 160, 0.1);
        }
        
        .required {
            color: #dc3545;
        }
        
        .save-btn {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .save-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .order-history {
            margin-top: 2rem;
        }
        
        .order-item {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1rem;
        }
        
        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 1px solid #e0e0e0;
        }
        
        .order-number {
            font-weight: 600;
            color: #2c5aa0;
        }
        
        .order-date {
            color: #666;
        }
        
        .order-status {
            padding: 0.25rem 0.75rem;
            border-radius: 15px;
            font-size: 0.9rem;
            font-weight: 600;
        }
        
        .status-completed {
            background: #d4edda;
            color: #155724;
        }
        
        .status-pending {
            background: #fff3cd;
            color: #856404;
        }
        
        .order-items {
            margin-bottom: 1rem;
        }
        
        .order-item-detail {
            display: flex;
            justify-content: space-between;
            margin-bottom: 0.5rem;
        }
        
        .order-total {
            font-weight: 600;
            color: #2c5aa0;
            font-size: 1.1rem;
        }
        
        .address-card {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1rem;
        }
        
        .address-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
        }
        
        .address-type {
            font-weight: 600;
            color: #2c5aa0;
        }
        
        .address-actions {
            display: flex;
            gap: 0.5rem;
        }
        
        .edit-btn {
            background: #2c5aa0;
            color: white;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 5px;
            cursor: pointer;
            font-size: 0.9rem;
        }
        
        .delete-btn {
            background: #dc3545;
            color: white;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 5px;
            cursor: pointer;
            font-size: 0.9rem;
        }
        
        .add-address-btn {
            background: transparent;
            color: #2c5aa0;
            border: 2px dashed #2c5aa0;
            padding: 1rem;
            border-radius: 10px;
            cursor: pointer;
            text-align: center;
            transition: all 0.3s ease;
        }
        
        .add-address-btn:hover {
            background: #2c5aa0;
            color: white;
        }
        
        @media (max-width: 768px) {
            .profile-tabs {
                flex-direction: column;
            }
            
            .form-row {
                grid-template-columns: 1fr;
            }
            
            .order-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 0.5rem;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Section -->
    <section class="profile-hero">
        <h1>My Profile</h1>
        <p>Manage your personal information, view order history, and update your preferences</p>
    </section>

    <!-- Profile Content -->
    <div class="profile-container">
        <div class="profile-content">
            <!-- Profile Tabs -->
            <div class="profile-tabs">
                <asp:Button ID="btnPersonalInfo" runat="server" Text="Personal Information" CssClass="profile-tab active" OnClick="btnPersonalInfo_Click" />
                <asp:Button ID="btnOrderHistory" runat="server" Text="Order History" CssClass="profile-tab" OnClick="btnOrderHistory_Click" />
                <asp:Button ID="btnAddresses" runat="server" Text="Saved Addresses" CssClass="profile-tab" OnClick="btnAddresses_Click" />
                <asp:Button ID="btnPreferences" runat="server" Text="Preferences" CssClass="profile-tab" OnClick="btnPreferences_Click" />
            </div>

            <!-- Personal Information Tab -->
            <asp:Panel ID="pnlPersonalInfo" runat="server" CssClass="tab-content active">
                <div class="form-section">
                    <h3 class="section-title">Personal Information</h3>
                    <div class="form-row">
                        <div class="form-group">
                            <label>First Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtFirstName" runat="server" />
                        </div>
                        <div class="form-group">
                            <label>Last Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtLastName" runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Email Address <span class="required">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
                        </div>
                        <div class="form-group">
                            <label>Phone Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtPhone" runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Date of Birth</label>
                            <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date" />
                        </div>
                        <div class="form-group">
                            <label>Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server">
                                <asp:ListItem Text="Select Gender" Value="" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <asp:Button ID="btnSavePersonalInfo" runat="server" Text="Save Changes" CssClass="save-btn" OnClick="btnSavePersonalInfo_Click" />
                </div>

                <div class="form-section">
                    <h3 class="section-title">Medical Information</h3>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Medical Aid Provider</label>
                            <asp:TextBox ID="txtMedicalAid" runat="server" />
                        </div>
                        <div class="form-group">
                            <label>Medical Aid Number</label>
                            <asp:TextBox ID="txtMedicalAidNumber" runat="server" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Allergies</label>
                            <asp:TextBox ID="txtAllergies" runat="server" TextMode="MultiLine" Rows="3" placeholder="List any allergies..." />
                        </div>
                    </div>
                    <asp:Button ID="btnSaveMedicalInfo" runat="server" Text="Save Medical Info" CssClass="save-btn" OnClick="btnSaveMedicalInfo_Click" />
                </div>
            </asp:Panel>

            <!-- Order History Tab -->
            <asp:Panel ID="pnlOrderHistory" runat="server" CssClass="tab-content">
                <div class="order-history">
                    <h3 class="section-title">Order History</h3>
                    
                    <div class="order-item">
                        <div class="order-header">
                            <div>
                                <span class="order-number">Order #EL-2024-001</span>
                                <span class="order-date"> - January 15, 2024</span>
                            </div>
                            <span class="order-status status-completed">Completed</span>
                        </div>
                        <div class="order-items">
                            <div class="order-item-detail">
                                <span>Ray-Ban Aviator Classic</span>
                                <span>R 2,450.00</span>
                            </div>
                            <div class="order-item-detail">
                                <span>Contact Lens Solution</span>
                                <span>R 180.00</span>
                            </div>
                        </div>
                        <div class="order-item-detail order-total">
                            <span>Total:</span>
                            <span>R 2,679.00</span>
                        </div>
                    </div>

                    <div class="order-item">
                        <div class="order-header">
                            <div>
                                <span class="order-number">Order #EL-2023-045</span>
                                <span class="order-date"> - December 8, 2023</span>
                            </div>
                            <span class="order-status status-completed">Completed</span>
                        </div>
                        <div class="order-items">
                            <div class="order-item-detail">
                                <span>Eye Examination</span>
                                <span>R 450.00</span>
                            </div>
                        </div>
                        <div class="order-item-detail order-total">
                            <span>Total:</span>
                            <span>R 450.00</span>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Saved Addresses Tab -->
            <asp:Panel ID="pnlAddresses" runat="server" CssClass="tab-content">
                <div class="form-section">
                    <h3 class="section-title">Saved Addresses</h3>
                    
                    <div class="address-card">
                        <div class="address-header">
                            <span class="address-type">Home Address</span>
                            <div class="address-actions">
                                <asp:Button ID="btnEditHomeAddress" runat="server" Text="Edit" CssClass="edit-btn" OnClick="btnEditHomeAddress_Click" />
                                <asp:Button ID="btnDeleteHomeAddress" runat="server" Text="Delete" CssClass="delete-btn" OnClick="btnDeleteHomeAddress_Click" />
                            </div>
                        </div>
                        <p>123 Main Street<br>Vincent, East London 5247<br>Eastern Cape, South Africa</p>
                    </div>

                    <div class="address-card">
                        <div class="address-header">
                            <span class="address-type">Work Address</span>
                            <div class="address-actions">
                                <asp:Button ID="btnEditWorkAddress" runat="server" Text="Edit" CssClass="edit-btn" OnClick="btnEditWorkAddress_Click" />
                                <asp:Button ID="btnDeleteWorkAddress" runat="server" Text="Delete" CssClass="delete-btn" OnClick="btnDeleteWorkAddress_Click" />
                            </div>
                        </div>
                        <p>456 Business Park<br>Vincent, East London 5247<br>Eastern Cape, South Africa</p>
                    </div>

                    <asp:Button ID="btnAddAddress" runat="server" Text="+ Add New Address" CssClass="add-address-btn" OnClick="btnAddAddress_Click" />
                </div>
            </asp:Panel>

            <!-- Preferences Tab -->
            <asp:Panel ID="pnlPreferences" runat="server" CssClass="tab-content">
                <div class="form-section">
                    <h3 class="section-title">Communication Preferences</h3>
                    <div class="form-row">
                        <div class="form-group">
                            <asp:CheckBox ID="chkEmailNotifications" runat="server" Text="Receive email notifications" Checked="true" />
                        </div>
                        <div class="form-group">
                            <asp:CheckBox ID="chkSMSNotifications" runat="server" Text="Receive SMS notifications" Checked="true" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <asp:CheckBox ID="chkAppointmentReminders" runat="server" Text="Appointment reminders" Checked="true" />
                        </div>
                        <div class="form-group">
                            <asp:CheckBox ID="chkPromotionalOffers" runat="server" Text="Promotional offers and updates" Checked="false" />
                        </div>
                    </div>
                    <asp:Button ID="btnSavePreferences" runat="server" Text="Save Preferences" CssClass="save-btn" OnClick="btnSavePreferences_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

