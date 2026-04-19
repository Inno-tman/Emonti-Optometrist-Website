<%@ Page Title="Start Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AppointmentStart.aspx.cs" Inherits="Emonti_Optometrist_Website.AppointmentStart" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ===== APPOINTMENT START HERO SECTION ===== */
        /* Keyframe Animations */
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

        /* Hero Section */
        .start-hero {
            background: linear-gradient(-45deg, #667eea, #764ba2, #667eea, #764ba2);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
        }
        
        .start-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .start-hero .container {
            position: relative;
            z-index: 2;
            animation: fadeInUp 1s ease-out;
        }
        
        .start-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        .start-hero p {
            font-size: clamp(1rem, 2.5vw, 1.3rem);
            max-width: 700px;
            margin: 0 auto;
            line-height: 1.8;
            opacity: 0.95;
        }
        
        @keyframes slideInUp {
            from { opacity: 0; transform: translateY(30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .start-hero .container {
            position: relative;
            z-index: 1;
        }

        .start-container { 
            max-width: 900px; 
            margin: -3rem auto 4rem; 
            padding: 0 2rem; 
            position: relative;
            z-index: 10;
        }
        .choice-card { background: white; border-radius: 15px; box-shadow: 0 10px 30px rgba(0,0,0,0.15); padding: 2rem; display: grid; grid-template-columns: 1fr 1fr; gap: 2rem; }
        .panel { border: 2px solid #e0e0e0; border-radius: 12px; padding: 2rem; }
        .panel h3 { color: #2c5aa0; margin-bottom: 0.5rem; }
        .panel p { color: #666; margin-bottom: 1rem; }
        .primary-btn { background: linear-gradient(135deg, #2c5aa0, #1e4080); color: white; padding: 0.75rem 1.5rem; border: none; border-radius: 25px; font-weight: 600; cursor: pointer; }
        .secondary-btn { background: #ffffff; color: #2c5aa0 !important; padding: 0.75rem 1.5rem; border: 2px solid #2c5aa0; border-radius: 25px; font-weight: 600; cursor: pointer; display: inline-block; }
        .secondary-btn:hover { background: #2c5aa0; color: #ffffff !important; }
        .note { font-size: 0.9rem; color: #666; margin-top: 0.75rem; }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 992px) {
            .start-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .start-hero h1 {
                font-size: 2.5rem;
            }
            
            .start-hero p {
                font-size: 1.1rem;
            }
        }
        
        @media (max-width: 768px) {
            .start-hero {
                padding: 6rem 1rem 3rem;
                margin-top: 0;
            }
            
            .start-hero h1 {
                font-size: 2.5rem;
            }
            
            .start-hero p {
                font-size: 1rem;
            }
            
            .choice-card { 
                grid-template-columns: 1fr; 
            } 
        }
        
        @media (max-width: 576px) {
            .start-hero {
                padding: 5rem 0.75rem 2.5rem;
            }
            
            .start-hero h1 {
                font-size: 2rem;
            }
            
            .start-hero p {
                font-size: 0.95rem;
            }
        }
        
        /* Reduce animations on reduced motion preference */
        @media (prefers-reduced-motion: reduce) {
            .start-hero,
            .start-hero::before,
            .start-hero h1,
            .start-hero p {
                animation-duration: 0.01ms !important;
                animation-iteration-count: 1 !important;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="start-hero">
        <div class="container">
            <h1>Book Appointment</h1>
            <p>Please tell us if you are an existing or a new customer to continue.</p>
        </div>
    </section>

    <div class="start-container">
        <div class="choice-card">
            <div class="panel">
                <h3>Existing Customer</h3>
                <p>If you have booked with us before or already have an account, continue as an existing customer.</p>
                <asp:Button ID="btnExistingCustomer" runat="server" Text="Continue as Existing Customer" CssClass="primary-btn" OnClick="btnExistingCustomer_Click" />
                <div class="note">You may be asked to log in if you are not already.</div>
            </div>
            <div class="panel">
                <h3>Don't Have An Account</h3>
                <p>If this is your first time, please register so we can create your profile and complete your booking.</p>
                <asp:Button ID="btnNewCustomer" runat="server" Text="Register" CssClass="primary-btn" OnClick="btnNewCustomer_Click" />
                <div class="note">Registration takes less than 2 minutes.</div>
            </div>
        </div>
    </div>
</asp:Content>

