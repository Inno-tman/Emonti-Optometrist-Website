<%@ Page Title="Personal Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PersonalDetails.aspx.cs" Inherits="Emonti_Optometrist_Website.PersonalDetails" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .personal-details-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
            animation: heroFadeIn 0.8s ease-out;
        }
        
        .personal-details-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(circle at 20% 50%, rgba(255,255,255,0.1) 0%, transparent 50%);
        }
        
        @keyframes heroFadeIn {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .personal-details-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
            font-weight: 700;
            position: relative;
            z-index: 1;
            animation: slideInDown 0.8s ease-out 0.2s both;
        }
        
        @keyframes slideInDown {
            from { opacity: 0; transform: translateY(-30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .personal-details-hero p {
            font-size: 1.2rem;
            max-width: 600px;
            margin: 0 auto;
            position: relative;
            z-index: 1;
            animation: slideInUp 0.8s ease-out 0.4s both;
            opacity: 0.95;
        }
        
        @keyframes slideInUp {
            from { opacity: 0; transform: translateY(30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .personal-details-container {
            max-width: 800px;
            margin: -2rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .personal-details-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
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
        
        .alert {
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1rem;
        }
        
        .alert-success {
            background-color: #d4edda;
            border: 1px solid #c3e6cb;
            color: #155724;
        }
        
        .alert-error {
            background-color: #f8d7da;
            border: 1px solid #f5c6cb;
            color: #721c24;
        }
        
        @media (max-width: 768px) {
            .form-row {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <!-- Hero Section -->
    <section class="personal-details-hero">
        <div class="container">
            <h1><i class="fas fa-user"></i> Personal Details</h1>
            <p>Manage your personal information and medical details</p>
        </div>
    </section>
    
    <div class="personal-details-container">
        <div class="personal-details-content">
            <!-- Success/Error Messages -->
            <asp:PlaceHolder ID="MessagePlaceHolder" runat="server" Visible="false">
                <div class="alert" id="MessageDiv" runat="server">
                    <asp:Literal ID="MessageText" runat="server" />
                </div>
            </asp:PlaceHolder>
            
            <!-- Validation Summary -->
            <asp:ValidationSummary ID="vsPersonalDetails" runat="server" 
                DisplayMode="BulletList" 
                HeaderText="Please correct the following errors:" 
                CssClass="alert alert-error" 
                ShowMessageBox="false" 
                ShowSummary="true" />
            
            <!-- Personal Information Section -->
            <div class="form-section">
                <h3 class="section-title">Personal Information</h3>
                <div class="form-row">
                    <div class="form-group">
                        <label>First Name <span class="required">*</span></label>
                        <asp:TextBox ID="txtFirstName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" 
                            ControlToValidate="txtFirstName" 
                            ErrorMessage="First name is required" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="revFirstName" runat="server" 
                            ControlToValidate="txtFirstName" 
                            ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                            ErrorMessage="First name must be 2-50 characters, letters, spaces, and hyphens only" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label>Last Name <span class="required">*</span></label>
                        <asp:TextBox ID="txtLastName" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" 
                            ControlToValidate="txtLastName" 
                            ErrorMessage="Last name is required" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="revLastName" runat="server" 
                            ControlToValidate="txtLastName" 
                            ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                            ErrorMessage="Last name must be 2-50 characters, letters, spaces, and hyphens only" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Email Address <span class="required">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" 
                            ControlToValidate="txtEmail" 
                            ErrorMessage="Email is required" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                            ControlToValidate="txtEmail" 
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                            ErrorMessage="Please enter a valid email address" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label>Phone Number <span class="required">*</span></label>
                        <asp:TextBox ID="txtPhone" runat="server" placeholder="0123456789" />
                        <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                            ControlToValidate="txtPhone" 
                            ErrorMessage="Phone number is required" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                        <asp:RegularExpressionValidator ID="revPhone" runat="server" 
                            ControlToValidate="txtPhone" 
                            ValidationExpression="^0\d{9}$" 
                            ErrorMessage="Phone must be 10 digits starting with 0 (e.g., 0123456789)" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Date of Birth</label>
                        <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date" />
                        <asp:CustomValidator ID="cvDateOfBirth" runat="server" 
                            ControlToValidate="txtDateOfBirth" 
                            OnServerValidate="ValidateDateOfBirth" 
                            ErrorMessage="Age must be between 13 and 80 years" 
                            Display="Dynamic" 
                            ForeColor="Red" />
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
            </div>

            <!-- Medical Information Section -->
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
                        <asp:RegularExpressionValidator ID="revMedicalAidNumber" runat="server" 
                            ControlToValidate="txtMedicalAidNumber" 
                            ValidationExpression="^[a-zA-Z0-9\s\-_\.]{3,50}$" 
                            ErrorMessage="Medical aid number must be 3-50 characters (letters, numbers, spaces, hyphens, underscores, periods allowed)" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                
                <!-- Main Member Question -->
                <div class="form-row">
                    <div class="form-group">
                        <label>Are you the main member on this medical aid? <span class="required">*</span></label>
                        <div style="display: flex; gap: 1rem; margin-top: 0.5rem;">
                            <label style="display: flex; align-items: center; font-weight: normal;">
                                <asp:RadioButton ID="rbIsMainMemberYes" runat="server" GroupName="IsMainMember" 
                                    Text="Yes" AutoPostBack="true" OnCheckedChanged="rbIsMainMember_Changed" />
                            </label>
                            <label style="display: flex; align-items: center; font-weight: normal;">
                                <asp:RadioButton ID="rbIsMainMemberNo" runat="server" GroupName="IsMainMember" 
                                    Text="No" AutoPostBack="true" OnCheckedChanged="rbIsMainMember_Changed" />
                            </label>
                        </div>
                    </div>
                </div>
                
                <!-- Main Member Details (shown only if "No" is selected) -->
                <div id="mainMemberDetails" runat="server" style="display: none;">
                    <div class="form-row">
                        <div class="form-group">
                            <label>Main Member First Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtMainMemberName" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvMainMemberName" runat="server" 
                                ControlToValidate="txtMainMemberName" 
                                ErrorMessage="Main member first name is required" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="revMainMemberName" runat="server" 
                                ControlToValidate="txtMainMemberName" 
                                ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                                ErrorMessage="Main member name must be 2-50 characters, letters, spaces, and hyphens only" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                        </div>
                        <div class="form-group">
                            <label>Main Member Last Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtMainMemberSurname" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvMainMemberSurname" runat="server" 
                                ControlToValidate="txtMainMemberSurname" 
                                ErrorMessage="Main member last name is required" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="revMainMemberSurname" runat="server" 
                                ControlToValidate="txtMainMemberSurname" 
                                ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                                ErrorMessage="Main member surname must be 2-50 characters, letters, spaces, and hyphens only" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Main Member ID Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtMainMemberID" runat="server" placeholder="e.g., 1234567890123" />
                            <asp:RequiredFieldValidator ID="rfvMainMemberID" runat="server" 
                                ControlToValidate="txtMainMemberID" 
                                ErrorMessage="Main member ID number is required" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="revMainMemberID" runat="server" 
                                ControlToValidate="txtMainMemberID" 
                                ValidationExpression="^\d{13}$" 
                                ErrorMessage="Main member ID must be 13 digits" 
                                Display="Dynamic" 
                                ForeColor="Red" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Address Information Section -->
            <div class="form-section">
                <h3 class="section-title">Address Information</h3>
                <div class="form-row">
                    <div class="form-group">
                        <label>Street Number</label>
                        <asp:TextBox ID="txtStreetNumber" runat="server" placeholder="e.g., 123" />
                    </div>
                    <div class="form-group">
                        <label>Street Name</label>
                        <asp:TextBox ID="txtStreetName" runat="server" placeholder="e.g., Main Street" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Complex Name</label>
                        <asp:TextBox ID="txtComplexName" runat="server" placeholder="e.g., Waterfall Estate" />
                    </div>
                    <div class="form-group">
                        <label>Unit Number</label>
                        <asp:TextBox ID="txtUnitNumber" runat="server" placeholder="e.g., 1, 2A, etc." />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>City</label>
                        <asp:TextBox ID="txtCity" runat="server" placeholder="e.g., Cape Town" />
                    </div>
                    <div class="form-group">
                        <label>Province</label>
                        <asp:TextBox ID="txtProvince" runat="server" placeholder="e.g., Western Cape" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Postal Code</label>
                        <asp:TextBox ID="txtPostalCode" runat="server" placeholder="e.g., 8001" />
                        <asp:RegularExpressionValidator ID="revPostalCode" runat="server" 
                            ControlToValidate="txtPostalCode" 
                            ValidationExpression="^\d{4}$" 
                            ErrorMessage="Postal code must be 4 digits" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>

            <!-- Password Confirmation Section -->
            <div class="form-section">
                <h3 class="section-title">Security Confirmation</h3>
                <div class="form-row">
                    <div class="form-group">
                        <label>Current Password <span class="required">*</span></label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your current password to save changes" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                            ControlToValidate="txtPassword" 
                            ErrorMessage="Password is required to save changes" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                <p style="color: #666; font-size: 0.9rem; margin-top: 0.5rem;">
                    <i class="fas fa-lock"></i> Your password is required to save any changes to your personal information.
                </p>
            </div>

            <!-- Save Changes Button -->
            <div class="form-section" style="text-align: center; margin-top: 2rem;">
                <asp:Button ID="btnSaveChanges" runat="server" Text="Save All Changes" CssClass="save-btn" OnClick="btnSaveChanges_Click" style="font-size: 1.1rem; padding: 1.2rem 3rem;" />
            </div>
        </div>
    </div>
</asp:Content>
