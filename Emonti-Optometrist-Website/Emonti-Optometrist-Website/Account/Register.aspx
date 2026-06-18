<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Emonti_Optometrist_Website.Account.Register" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Keyframe Animations */
        @keyframes gradientShift {
            0% {
                background-position: 0% 50%;
            }
            50% {
                background-position: 100% 50%;
            }
            100% {
                background-position: 0% 50%;
            }
        }
        
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
        
        @keyframes heroTitleReveal {
            0% {
                opacity: 0;
                transform: translateY(50px) scale(0.9);
                filter: blur(10px);
            }
            100% {
                opacity: 1;
                transform: translateY(0) scale(1);
                filter: blur(0);
            }
        }
        
        @keyframes fadeSlideIn {
            from {
                opacity: 0;
                transform: translateY(30px);
                filter: blur(5px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
                filter: blur(0);
            }
        }
        
        .register-hero {
            background: linear-gradient(-45deg, #667eea, #764ba2, #667eea, #764ba2);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0 !important;
            position: relative;
            overflow: visible;
        }
        
        .register-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .register-hero .container {
            position: relative;
            z-index: 2;
            opacity: 0;
            animation: fadeInUp 1s ease-out forwards;
        }
        
        .register-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
            animation: heroTitleReveal 1.5s cubic-bezier(0.16, 1, 0.3, 1) forwards;
            text-shadow: 0 4px 12px rgba(0, 0, 0, 0.4), 
                         0 2px 4px rgba(0, 0, 0, 0.2),
                         0 0 30px rgba(102, 126, 234, 0.5);
        }
        
        .register-hero p {
            animation: fadeSlideIn 1.2s ease-out 0.5s backwards;
            text-shadow: 0 3px 10px rgba(0, 0, 0, 0.5);
        }
        
        .register-container {
            max-width: 800px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .register-content {
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
        
        .btn-register {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .btn-register:hover {
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
        
        .links-section {
            text-align: center;
            margin-top: 2rem;
            padding-top: 2rem;
            border-top: 1px solid #e0e0e0;
        }
        
        .links-section a {
            color: #2c5aa0;
            text-decoration: none;
            font-weight: 500;
        }
        
        .links-section a:hover {
            text-decoration: underline;
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
    <section class="register-hero">
        <div class="container">
            <h1><i class="fas fa-user-plus"></i> Create Account</h1>
            <p>Join us and create your comprehensive profile</p>
        </div>
    </section>
    
    <div class="register-container">
        <div class="register-content">
            <!-- Success/Error Messages -->
            <asp:Literal runat="server" ID="ErrorMessage" Visible="false">
                <div class="alert alert-error">
                    <p>An error occurred while creating your account. Please try again.</p>
                </div>
            </asp:Literal>

 
           <asp:Literal runat="server" ID="SuccessMessage" Visible="false">
             <div class="alert alert-success">
                  <p>Your account has been created successfully!</p>
           </div>
        </asp:Literal>
            
            <!-- Validation Summary -->
            <asp:ValidationSummary ID="vsRegister" runat="server" 
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
                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="Enter your first name" />
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
                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Enter your last name" />
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
                    <div class="form-group">
                        <label>Date of Birth <span class="required">*</span></label>
                        <asp:TextBox ID="txtDateOfBirth" runat="server" TextMode="Date" />
                        <asp:CustomValidator ID="cvDateOfBirth" runat="server" 
                            ControlToValidate="txtDateOfBirth" 
                            OnServerValidate="ValidateDateOfBirth" 
                            ErrorMessage="Age must be between 13 and 80 years" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Gender <span class="required">*</span></label>
                        <asp:DropDownList ID="ddlGender" runat="server">
                            <asp:ListItem Text="Select Gender" Value="" />
                            <asp:ListItem Text="Male" Value="Male" />
                            <asp:ListItem Text="Female" Value="Female" />
                            <asp:ListItem Text="Other" Value="Other" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGender" runat="server" 
                            ControlToValidate="ddlGender" 
                            ErrorMessage="Please select your gender" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>

            <!-- Medical Information Section -->
            <div class="form-section">
                <h3 class="section-title">Medical Information</h3>
                <div class="form-row">
                    <div class="form-group">
                        <label>Do you have medical aid? <span class="required">*</span></label>
                        <div style="display: flex; gap: 1rem; margin-top: 0.5rem;">
                            <label style="display: flex; align-items: center; font-weight: normal;">
                                <asp:RadioButton ID="rbHasMedicalAidYes" runat="server" GroupName="HasMedicalAid" 
                                    Text="Yes" AutoPostBack="true" OnCheckedChanged="rbHasMedicalAid_Changed" />
                            </label>
                            <label style="display: flex; align-items: center; font-weight: normal;">
                                <asp:RadioButton ID="rbHasMedicalAidNo" runat="server" GroupName="HasMedicalAid" 
                                    Text="No" AutoPostBack="true" OnCheckedChanged="rbHasMedicalAid_Changed" />
                            </label>
                        </div>
                    </div>
                </div>

                <div id="medicalAidDetails" style="display: none;">
                <div class="form-row">
                    <div class="form-group">
                        <label>Medical Aid Provider <span class="required">*</span></label>
                        <asp:TextBox ID="txtMedicalAid" runat="server" placeholder="e.g., Discovery Health" />
                    </div>
                    <div class="form-group">
                        <label>Medical Aid Number <span class="required">*</span></label>
                        <asp:TextBox ID="txtMedicalAidNumber" runat="server" placeholder="Enter your medical aid number" />
                        <asp:RegularExpressionValidator ID="revMedicalAidNumber" runat="server" 
                            ControlToValidate="txtMedicalAidNumber" 
                            ValidationExpression="^[a-zA-Z0-9\s\-_\.]{3,50}$" 
                            ErrorMessage="Medical aid number must be 3-50 characters (letters, numbers, spaces, hyphens, underscores, periods allowed)" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
                
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
                            <asp:TextBox ID="txtMainMemberName" runat="server" placeholder="Enter main member's first name" />
                            <asp:RequiredFieldValidator ID="rfvMainMemberName" runat="server" 
                                ControlToValidate="txtMainMemberName" 
                                ErrorMessage="Main member first name is required" 
                                Display="Dynamic" 
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
                            <asp:RegularExpressionValidator ID="revMainMemberName" runat="server" 
                                ControlToValidate="txtMainMemberName" 
                                ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                                ErrorMessage="Main member name must be 2-50 characters, letters, spaces, and hyphens only" 
                                Display="Dynamic" 
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
                        </div>
                        <div class="form-group">
                            <label>Main Member Last Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtMainMemberSurname" runat="server" placeholder="Enter main member's last name" />
                            <asp:RequiredFieldValidator ID="rfvMainMemberSurname" runat="server" 
                                ControlToValidate="txtMainMemberSurname" 
                                ErrorMessage="Main member last name is required" 
                                Display="Dynamic" 
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
                            <asp:RegularExpressionValidator ID="revMainMemberSurname" runat="server" 
                                ControlToValidate="txtMainMemberSurname" 
                                ValidationExpression="^[a-zA-Z\s\-]{2,50}$" 
                                ErrorMessage="Main member surname must be 2-50 characters, letters, spaces, and hyphens only" 
                                Display="Dynamic" 
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
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
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
                            <asp:RegularExpressionValidator ID="revMainMemberID" runat="server" 
                                ControlToValidate="txtMainMemberID" 
                                ValidationExpression="^\d{13}$" 
                                ErrorMessage="Main member ID must be 13 digits" 
                                Display="Dynamic" 
                                ForeColor="Red" 
                                Enabled="false" 
                                ValidationGroup="MainMemberValidation" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Address Information Section -->
            <div class="form-section">
                <h3 class="section-title">Address Information <span class="required">*</span></h3>
                <div class="form-row" style="margin-bottom:1.5rem;">
                    <div class="form-group" style="position:relative;">
                        <label><i class="fas fa-search-location"></i> Search for your address</label>
                        <input type="text" id="addressSearch" autocomplete="off" placeholder="Start typing your address..." style="background:#f0f4ff;border-color:#667eea;padding:1rem;border:2px solid #e0e0e0;border-radius:8px;font-size:1rem;width:100%;box-sizing:border-box;" />
                        <div id="addressResults" style="display:none;position:absolute;top:100%;left:0;right:0;background:#fff;border:1px solid #ddd;border-radius:8px;z-index:100;max-height:220px;overflow-y:auto;box-shadow:0 4px 12px rgba(0,0,0,0.12);"></div>
                    </div>
                </div>
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
                        <asp:DropDownList ID="ddlProvince" runat="server">
                            <asp:ListItem Text="Select Province" Value="" />
                            <asp:ListItem Text="Eastern Cape" Value="Eastern Cape" />
                            <asp:ListItem Text="Free State" Value="Free State" />
                            <asp:ListItem Text="Gauteng" Value="Gauteng" />
                            <asp:ListItem Text="KwaZulu-Natal" Value="KwaZulu-Natal" />
                            <asp:ListItem Text="Limpopo" Value="Limpopo" />
                            <asp:ListItem Text="Mpumalanga" Value="Mpumalanga" />
                            <asp:ListItem Text="Northern Cape" Value="Northern Cape" />
                            <asp:ListItem Text="North West" Value="North West" />
                            <asp:ListItem Text="Western Cape" Value="Western Cape" />
                        </asp:DropDownList>
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

            <!-- Account Information Section -->
            <div class="form-section">
                <h3 class="section-title">Account Information</h3>
                <div class="form-row">
                    <div class="form-group">
                        <label>Email Address <span class="required">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Enter your email address" />
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
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Password <span class="required">*</span> (6-8 characters)</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter your password (6-8 characters)" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                            ControlToValidate="txtPassword" 
                            ErrorMessage="Password is required" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                    <div class="form-group">
                        <label>Confirm Password <span class="required">*</span> (6-8 characters)</label>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" placeholder="Confirm your password (6-8 characters)" />
                        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" 
                            ControlToValidate="txtConfirmPassword" 
                            ErrorMessage="Please confirm your password" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                        <asp:CompareValidator ID="cvConfirmPassword" runat="server" 
                            ControlToCompare="txtPassword" 
                            ControlToValidate="txtConfirmPassword" 
                            ErrorMessage="Passwords do not match" 
                            Display="Dynamic" 
                            ForeColor="Red" />
                    </div>
                </div>
            </div>

            <!-- Create Account Button -->
            <div class="form-section" style="text-align: center; margin-top: 2rem;">
                <asp:Button ID="btnCreateAccount" runat="server" Text="Create Account" CssClass="btn-register" OnClick="btnCreateAccount_Click" style="font-size: 1.1rem; padding: 1.2rem 3rem;" OnClientClick="formChanged = false;" />
            </div>
            
            <!-- Login Link -->
            <div class="links-section">
                <asp:HyperLink runat="server" NavigateUrl="~/Account/Login.aspx" onclick="return confirmLeave();">
                    <i class="fas fa-sign-in-alt"></i> Already have an account? Sign in
                </asp:HyperLink>
            </div>
        </div>
    </div>

    <script>
        var formChanged = false;
        var formFields = [
            '<%= txtFirstName.ClientID %>',
            '<%= txtLastName.ClientID %>',
            '<%= txtPhone.ClientID %>',
            '<%= txtDateOfBirth.ClientID %>',
            '<%= txtEmail.ClientID %>',
            '<%= txtPassword.ClientID %>',
            '<%= txtConfirmPassword.ClientID %>',
            '<%= txtMedicalAid.ClientID %>',
            '<%= txtMedicalAidNumber.ClientID %>',
            '<%= txtMainMemberName.ClientID %>',
            '<%= txtMainMemberSurname.ClientID %>',
            '<%= txtMainMemberID.ClientID %>',
            '<%= txtStreetNumber.ClientID %>',
            '<%= txtStreetName.ClientID %>',
            '<%= txtComplexName.ClientID %>',
            '<%= txtUnitNumber.ClientID %>',
            '<%= txtCity.ClientID %>',
            '<%= txtPostalCode.ClientID %>'
        ];

        function trackChanges() {
            formFields.forEach(function(id) {
                var el = document.getElementById(id);
                if (el) {
                    el.addEventListener('change', function() { formChanged = true; });
                    el.addEventListener('keyup', function() { formChanged = true; });
                }
            });
            var gender = document.getElementById('<%= ddlGender.ClientID %>');
            if (gender) gender.addEventListener('change', function() { formChanged = true; });
            var province = document.getElementById('<%= ddlProvince.ClientID %>');
            if (province) province.addEventListener('change', function() { formChanged = true; });
        }

        function confirmLeave() {
            if (formChanged) {
                return confirm('You have unsaved changes. Are you sure you want to leave this page?');
            }
            return true;
        }

        function toggleMedicalAidDetails() {
            var yesChecked = document.getElementById('<%= rbHasMedicalAidYes.ClientID %>').checked;
            var details = document.getElementById('medicalAidDetails');
            details.style.display = yesChecked ? 'block' : 'none';
            if (!yesChecked) {
                document.getElementById('<%= rbIsMainMemberYes.ClientID %>').checked = true;
                document.getElementById('<%= rbIsMainMemberNo.ClientID %>').checked = false;
                document.getElementById('<%= mainMemberDetails.ClientID %>').style.display = 'none';
            }
        }

        window.onload = function() {
            trackChanges();
            toggleMedicalAidDetails();

            // === Nominatim address autocomplete ===
            var searchInput = document.getElementById('addressSearch');
            var resultsBox = document.getElementById('addressResults');
            var searchTimeout;

            function setField(id, val) {
                var el = document.getElementById(id);
                if (el) el.value = val;
            }

            function selectAddress(item) {
                var addr = item.address || {};
                searchInput.value = (item.display_name || '').split(',')[0];
                resultsBox.style.display = 'none';

                setField('<%= txtStreetNumber.ClientID %>', addr.house_number || '');
                setField('<%= txtStreetName.ClientID %>', addr.road || addr.pedestrian || addr.footway || '');
                setField('<%= txtCity.ClientID %>', addr.city || addr.town || addr.village || addr.suburb || '');
                setField('<%= txtPostalCode.ClientID %>', addr.postcode || '');

                var province = addr.state || '';
                if (province) {
                    var sel = document.getElementById('<%= ddlProvince.ClientID %>');
                    if (sel) {
                        for (var i = 0; i < sel.options.length; i++) {
                            if (sel.options[i].value.toLowerCase() === province.toLowerCase()) {
                                sel.value = sel.options[i].value; break;
                            }
                        }
                    }
                }

                var suburb = (addr.suburb || addr.neighbourhood || '').toLowerCase();
                var road = (addr.road || '').toLowerCase();
                var complexNames = ['estate', 'village', 'park', 'manor', 'heights', 'ridge', 'view', 'park', 'gardens', 'close', 'lane', 'court'];
                var matched = '';
                complexNames.forEach(function(cn) {
                    if (suburb.indexOf(cn) > -1) matched = addr.suburb;
                    if (!matched && road.indexOf(cn) > -1) matched = addr.road;
                });
                setField('<%= txtComplexName.ClientID %>', matched || '');
            }

            if (searchInput) {
                searchInput.addEventListener('input', function() {
                    clearTimeout(searchTimeout);
                    var q = this.value.trim();
                    if (q.length < 5) { resultsBox.style.display = 'none'; return; }
                    searchTimeout = setTimeout(function() {
                        fetch('https://nominatim.openstreetmap.org/search?q=' + encodeURIComponent(q) + '&format=json&addressdetails=1&limit=5&countrycodes=za', {
                            headers: { 'User-Agent': 'EmontiOptometrist/1.0' }
                        })
                        .then(function(r) { return r.json(); })
                        .then(function(data) {
                            resultsBox.innerHTML = '';
                            if (!data || data.length === 0) { resultsBox.style.display = 'none'; return; }
                            data.forEach(function(item) {
                                var div = document.createElement('div');
                                div.textContent = item.display_name;
                                div.style.cssText = 'padding:0.6rem 0.75rem;cursor:pointer;font-size:0.82rem;border-bottom:1px solid #f0f0f0;';
                                div.addEventListener('mouseenter', function(){ this.style.background = '#f0f4ff'; });
                                div.addEventListener('mouseleave', function(){ this.style.background = ''; });
                                div.addEventListener('click', function() { selectAddress(item); });
                                resultsBox.appendChild(div);
                            });
                            resultsBox.style.display = 'block';
                        })
                        .catch(function() { resultsBox.style.display = 'none'; });
                    }, 600);
                });

                document.addEventListener('click', function(e) {
                    if (!searchInput.contains(e.target) && !resultsBox.contains(e.target))
                        resultsBox.style.display = 'none';
                });
            }
        };

        window.addEventListener('beforeunload', function(e) {
            if (formChanged) {
                e.preventDefault();
                e.returnValue = '';
            }
        });
    </script>
</asp:Content>
