<%@ Page Title="Log in" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Emonti_Optometrist_Website.Account.Login" %>

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
        
        .login-hero {
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
        
        .login-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .login-hero .container {
            position: relative;
            z-index: 2;
            opacity: 0;
            animation: fadeInUp 1s ease-out forwards;
        }
        
        .login-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
            animation: heroTitleReveal 1.5s cubic-bezier(0.16, 1, 0.3, 1) forwards;
            text-shadow: 0 4px 12px rgba(0, 0, 0, 0.4), 
                         0 2px 4px rgba(0, 0, 0, 0.2),
                         0 0 30px rgba(102, 126, 234, 0.5);
        }
        
        .login-hero p {
            animation: fadeSlideIn 1.2s ease-out 0.5s backwards;
            text-shadow: 0 3px 10px rgba(0, 0, 0, 0.5);
        }
        
        .login-container {
            max-width: 800px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .login-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .login-form {
            max-width: 400px;
            margin: 0 auto;
        }
        
        .form-group {
            margin-bottom: 1.5rem;
        }
        
        .form-group label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
            display: block;
        }
        
        .form-group input {
            width: 100%;
            padding: 1rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
            transition: border-color 0.3s ease;
            box-sizing: border-box;
        }
        
        .form-group input:focus {
            border-color: #2c5aa0;
            outline: none;
            box-shadow: 0 0 0 3px rgba(44, 90, 160, 0.1);
        }
        
        .btn-login {
            width: 100%;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            font-size: 1.1rem;
            margin-top: 1rem;
        }
        
        .btn-login:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .alert {
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1rem;
        }
        
        .alert-error {
            background-color: #f8d7da;
            border: 1px solid #f5c6cb;
            color: #721c24;
        }
        
        .alert-success {
            background-color: #d4edda;
            border: 1px solid #c3e6cb;
            color: #155724;
        }
        
        .checkbox-group {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin: 1rem 0;
        }
        
        .checkbox-group input[type="checkbox"] {
            width: auto;
            margin: 0;
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
            margin: 0 1rem;
            font-weight: 500;
        }
        
        .links-section a:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    <section class="login-hero">
        <div class="container">
            <h1><i class="fas fa-sign-in-alt"></i> Welcome Back</h1>
            <p>Sign in to your account to continue</p>
        </div>
    </section>
    
    <div class="login-container">
        <div class="login-content">
            <div class="login-form">
                <h2 style="text-align: center; color: #2c5aa0; margin-bottom: 2rem;">
                    <i class="fas fa-user-circle"></i> Login
                </h2>
                
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                    <div class="alert alert-error">
                        <asp:Literal runat="server" ID="FailureText" />
                    </div>
                </asp:PlaceHolder>
                
                <div class="form-group">
                    <label><i class="fas fa-envelope"></i> Email Address or Staff ID</label>
                    <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="SingleLine" placeholder="Enter your email address or staff ID" />
                </div>
                
                <div class="form-group">
                    <label><i class="fas fa-lock"></i> Password</label>
                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="Enter your password" />
                </div>
                
                <div class="checkbox-group">
                    <asp:CheckBox runat="server" ID="RememberMe" />
                    <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me</asp:Label>
                </div>
                
                <asp:Button runat="server" OnClick="LogIn" Text="Sign In" CssClass="btn-login" />
                
                <div class="links-section">
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">
                        <i class="fas fa-user-plus"></i> Create New Account
                    </asp:HyperLink>
                    <br />
                    <a href="javascript:void(0);" onclick="openForgotPasswordModal()" style="color: #2c5aa0; text-decoration: none; margin: 0 1rem; font-weight: 500;">
                        <i class="fas fa-key"></i> Forgot Password?
                    </a>
                </div>
            </div>
        </div>
    </div>

    <!-- Forgot Password Modal -->
    <div id="forgotPasswordModal" class="modal" style="display: none;">
        <div class="modal-content" style="max-width: 500px;">
            <div class="modal-header">
                <h2><i class="fas fa-key"></i> Forgot Password</h2>
                <button type="button" class="close" onclick="closeForgotPasswordModal()">&times;</button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="updForgotPassword" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder runat="server" ID="ForgotPasswordErrorMessage" Visible="false">
                            <div class="alert alert-error">
                                <asp:Literal runat="server" ID="ForgotPasswordFailureText" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="ForgotPasswordSuccessMessage" Visible="false">
                            <div class="alert alert-success">
                                <asp:Literal runat="server" ID="ForgotPasswordSuccessText" />
                            </div>
                        </asp:PlaceHolder>
                        
                        <div id="forgotPasswordForm">
                            <p style="margin-bottom: 1.5rem; color: #666;">
                                Enter your email address and we'll send you a password reset code.
                            </p>
                            <div class="form-group">
                                <label><i class="fas fa-envelope"></i> Email Address</label>
                                <asp:TextBox runat="server" ID="txtForgotPasswordEmail" CssClass="form-control" TextMode="Email" placeholder="Enter your email address" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtForgotPasswordEmail"
                                    CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" />
                            </div>
                            <asp:Button runat="server" ID="btnSendResetCode" OnClick="btnSendResetCode_Click" 
                                Text="Send Reset Code" CssClass="btn-login" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- Reset Password Modal -->
    <div id="resetPasswordModal" class="modal" style="display: none;">
        <div class="modal-content" style="max-width: 500px;">
            <div class="modal-header">
                <h2><i class="fas fa-lock"></i> Reset Password</h2>
                <button type="button" class="close" onclick="closeResetPasswordModal()">&times;</button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="updResetPassword" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder runat="server" ID="ResetPasswordErrorMessage" Visible="false">
                            <div class="alert alert-error">
                                <asp:Literal runat="server" ID="ResetPasswordFailureText" />
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="ResetPasswordSuccessMessage" Visible="false">
                            <div class="alert alert-success">
                                <asp:Literal runat="server" ID="ResetPasswordSuccessText" />
                            </div>
                        </asp:PlaceHolder>
                        
                        <div id="resetPasswordForm">
                            <div class="form-group">
                                <label><i class="fas fa-envelope"></i> Email Address</label>
                                <asp:TextBox runat="server" ID="txtResetPasswordEmail" CssClass="form-control" TextMode="Email" placeholder="Enter your email address" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtResetPasswordEmail"
                                    CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" />
                            </div>
                            <div class="form-group">
                                <label><i class="fas fa-key"></i> Reset Code</label>
                                <asp:TextBox runat="server" ID="txtResetCode" CssClass="form-control" placeholder="Enter the code sent to your email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtResetCode"
                                    CssClass="text-danger" ErrorMessage="Reset code is required." Display="Dynamic" />
                            </div>
                            <div class="form-group">
                                <label><i class="fas fa-lock"></i> New Password</label>
                                <asp:TextBox runat="server" ID="txtNewPassword" TextMode="Password" CssClass="form-control" placeholder="Enter your new password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNewPassword"
                                    CssClass="text-danger" ErrorMessage="New password is required." Display="Dynamic" />
                            </div>
                            <div class="form-group">
                                <label><i class="fas fa-lock"></i> Confirm New Password</label>
                                <asp:TextBox runat="server" ID="txtConfirmNewPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm your new password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmNewPassword"
                                    CssClass="text-danger" ErrorMessage="Please confirm your password." Display="Dynamic" />
                                <asp:CompareValidator runat="server" ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmNewPassword"
                                    CssClass="text-danger" ErrorMessage="Passwords do not match." Display="Dynamic" />
                            </div>
                            <asp:Button runat="server" ID="btnResetPassword" OnClick="btnResetPassword_Click" 
                                Text="Reset Password" CssClass="btn-login" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <style>
        /* Modal Styles */
        .modal {
            display: none;
            position: fixed;
            z-index: 1000;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0,0,0,0.6);
            backdrop-filter: blur(5px);
            animation: fadeIn 0.3s ease;
        }

        .modal-content {
            background-color: #fefefe;
            margin: 5% auto;
            padding: 0;
            border: none;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            animation: slideUp 0.4s cubic-bezier(0.4, 0, 0.2, 1);
        }

        .modal-header {
            padding: 1.5rem 2rem;
            background: linear-gradient(135deg, #2c5aa0 0%, #1e4080 100%);
            color: white;
            border-radius: 15px 15px 0 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .modal-header h2 {
            margin: 0;
            font-size: 1.5rem;
        }

        .close {
            color: white;
            font-size: 28px;
            font-weight: bold;
            cursor: pointer;
            background: none;
            border: none;
            padding: 0;
            width: 30px;
            height: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .close:hover,
        .close:focus {
            opacity: 0.7;
        }

        .modal-body {
            padding: 2rem;
        }

        @keyframes fadeIn {
            from { opacity: 0; }
            to { opacity: 1; }
        }

        @keyframes slideUp {
            from {
                transform: translateY(50px);
                opacity: 0;
            }
            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .text-danger {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.25rem;
            display: block;
        }
    </style>

    <script>
        function openForgotPasswordModal() {
            document.getElementById('forgotPasswordModal').style.display = 'block';
        }

        function closeForgotPasswordModal() {
            document.getElementById('forgotPasswordModal').style.display = 'none';
        }

        function openResetPasswordModal() {
            closeForgotPasswordModal();
            document.getElementById('resetPasswordModal').style.display = 'block';
        }

        function closeResetPasswordModal() {
            document.getElementById('resetPasswordModal').style.display = 'none';
        }

        // Close modal when clicking outside of it
        window.onclick = function(event) {
            var forgotModal = document.getElementById('forgotPasswordModal');
            var resetModal = document.getElementById('resetPasswordModal');
            if (event.target == forgotModal) {
                closeForgotPasswordModal();
            }
            if (event.target == resetModal) {
                closeResetPasswordModal();
            }
        }
    </script>
</asp:Content>

