<%@ Page Title="Staff Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Emonti_Optometrist_Website.StaffDashboard" %>
<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Animations */
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

        @keyframes pulse {
            0%, 100% { transform: scale(1); }
            50% { transform: scale(1.05); }
        }

        /* Hide the nav bar on staff dashboard */
        .header.navbar-pill_component {
            display: none !important;
        }

        .staff-dashboard {
            margin-top: 0;
            padding: 2rem;
            background-image: 
                radial-gradient(circle at 10% 20%, rgba(44, 90, 160, 0.03) 0%, transparent 20%),
                radial-gradient(circle at 90% 80%, rgba(44, 90, 160, 0.03) 0%, transparent 20%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
        }

        .dashboard-container {
            width: 100%;
            max-width: 1400px;
            background: white;
            border-radius: 16px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08);
            padding: 2rem;
        }

        .dashboard-logo {
            text-align: center;
            margin: 0 0 1.5rem 0;
            padding: 1rem 0;
            animation: fadeInUp 0.6s ease-out;
            border: none;
            background: transparent;
        }

        .dashboard-logo .logo-brand {
            font-family: 'Georgia', 'Times New Roman', serif;
            font-size: 2.5rem;
            font-weight: bold;
            letter-spacing: 4px;
            margin: 0;
            padding: 0;
            color: #2c5aa0;
            white-space: nowrap;
        }

        .dashboard-logo .logo-profession {
            font-family: 'Georgia', 'Times New Roman', serif;
            font-size: 0.75rem;
            letter-spacing: 6px;
            margin: 0.3rem 0 0 0;
            padding: 0;
            color: #2c5aa0;
            font-weight: normal;
            white-space: nowrap;
        }
        
        .dashboard-header {
            background: linear-gradient(135deg, #2c5aa0 0%, #1e4080 60%, #163060 100%);
            color: white;
            padding: 2rem 2.5rem;
            border-radius: 24px;
            margin-bottom: 1.5rem;
            box-shadow: 
                0 15px 50px rgba(44, 90, 160, 0.35),
                inset 0 2px 0 rgba(255,255,255,0.15),
                0 5px 15px rgba(0,0,0,0.1);
            position: relative;
            overflow: hidden;
            animation: fadeInUp 0.6s ease-out;
            border: 1px solid rgba(255,255,255,0.1);
        }

        /* Add subtle pattern overlay */
        .dashboard-header::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-image: 
                radial-gradient(circle at 20% 50%, rgba(255,255,255,0.08) 0%, transparent 50%),
                radial-gradient(circle at 80% 80%, rgba(255,255,255,0.08) 0%, transparent 50%);
            pointer-events: none;
        }

        /* Add decorative accent line */
        .dashboard-header::after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
        }
        
        .staff-info {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            position: relative;
            z-index: 1;
        }
        
        .staff-details h1 {
            margin: 0;
            font-size: 2rem;
            font-weight: 700;
            letter-spacing: -0.5px;
            text-shadow: 0 2px 10px rgba(0,0,0,0.2);
        }
        
        .staff-details p {
            margin: 0.5rem 0 0 0;
            opacity: 0.95;
            font-size: 1rem;
            font-weight: 500;
            text-shadow: 0 1px 5px rgba(0,0,0,0.15);
        }

        .staff-details p i {
            margin: 0 0.5rem;
            font-size: 0.5rem;
            opacity: 0.7;
            vertical-align: middle;
        }
        
        .staff-actions {
            display: flex;
            gap: 1rem;
        }
        
        .btn-logout {
            background: rgba(255,255,255,0.2);
            color: white;
            border: 2px solid rgba(255,255,255,0.4);
            padding: 0.6rem 1.2rem;
            border-radius: 12px;
            text-decoration: none;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            backdrop-filter: blur(10px);
            font-weight: 600;
            font-size: 0.95rem;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }
        
        .btn-logout:hover {
            background: rgba(255,255,255,0.3);
            border-color: rgba(255,255,255,0.6);
            color: white;
            transform: translateY(-3px);
            box-shadow: 0 6px 25px rgba(0,0,0,0.25);
        }

        .btn-logout i {
            margin-right: 0.5rem;
        }
        
        .dashboard-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
            gap: 1.5rem;
            margin-bottom: 1.5rem;
        }
        
        .dashboard-card {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            box-shadow: 
                0 8px 32px rgba(0,0,0,0.08),
                0 2px 8px rgba(0,0,0,0.04);
            padding: 1.5rem;
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            border: 1px solid rgba(255,255,255,0.8);
            animation: fadeInUp 0.6s ease-out;
            animation-fill-mode: both;
        }

        .dashboard-card:nth-child(1) { animation-delay: 0.1s; }
        .dashboard-card:nth-child(2) { animation-delay: 0.2s; }
        .dashboard-card:nth-child(3) { animation-delay: 0.3s; }
        
        .dashboard-card:hover {
            transform: translateY(-8px) scale(1.02);
            box-shadow: 
                0 20px 60px rgba(44, 90, 160, 0.15),
                0 4px 16px rgba(0,0,0,0.1);
        }
        
        .card-header {
            display: flex;
            align-items: center;
            gap: 1rem;
            margin-bottom: 1rem;
        }
        
        .card-icon {
            width: 50px;
            height: 50px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.8rem;
            color: white;
            box-shadow: 0 4px 15px rgba(0,0,0,0.2);
            transition: all 0.3s ease;
            position: relative;
        }

        .dashboard-card:hover .card-icon {
            transform: rotate(5deg) scale(1.1);
            box-shadow: 0 6px 20px rgba(0,0,0,0.3);
        }

        .card-icon::after {
            content: '';
            position: absolute;
            top: -5px;
            left: -5px;
            right: -5px;
            bottom: -5px;
            border-radius: 18px;
            background: inherit;
            opacity: 0;
            transition: opacity 0.3s ease;
        }

        .dashboard-card:hover .card-icon::after {
            opacity: 0.3;
            animation: pulse 2s infinite;
        }
        
        .icon-appointments {
            background: linear-gradient(135deg, #28a745, #20c997);
        }
        
        .icon-timeslots {
            background: linear-gradient(135deg, #ffc107, #fd7e14);
        }
        
        .icon-patients {
            background: linear-gradient(135deg, #17a2b8, #6f42c1);
        }
        
        .icon-reports {
            background: linear-gradient(135deg, #dc3545, #e83e8c);
        }
        
        .card-title {
            font-size: 1.1rem;
            font-weight: 600;
            color: #333;
            margin: 0;
        }
        
        .card-content {
            margin-bottom: 1rem;
        }
        
        .stat-number {
            font-size: 2.5rem;
            font-weight: 700;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            /* Gradient text effect using background-clip */
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            color: transparent;
            background-clip: text;
            margin-bottom: 0.5rem;
            line-height: 1;
        }
        
        .stat-label {
            color: #666;
            font-size: 0.85rem;
            font-weight: 500;
        }
        
        .card-actions {
            display: flex;
            gap: 0.5rem;
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 0.6rem 1.2rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 0.9rem;
            position: relative;
            overflow: hidden;
        }

        .btn-primary::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
            transition: left 0.5s;
        }

        .btn-primary:hover::before {
            left: 100%;
        }
        
        .btn-primary:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 20px rgba(44, 90, 160, 0.4);
            color: white;
        }
        
        .btn-secondary {
            background: transparent;
            color: #2c5aa0;
            border: 2px solid #2c5aa0;
            padding: 0.6rem 1.2rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 0.9rem;
            position: relative;
        }
        
        .btn-secondary:hover {
            background: #2c5aa0;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.3);
        }
        
        .today-appointments {
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
            padding: 2rem;
            margin-bottom: 2rem;
        }
        
        .section-title {
            font-size: 1.5rem;
            font-weight: 600;
            color: #2c5aa0;
            margin-bottom: 1.5rem;
            border-bottom: 2px solid #e0e0e0;
            padding-bottom: 0.5rem;
        }
        
        .appointment-list {
            display: grid;
            gap: 1rem;
        }
        
        .appointment-item {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 1.5rem;
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
        }
        
        .appointment-info h4 {
            margin: 0 0 0.5rem 0;
            color: #2c5aa0;
        }
        
        .appointment-info p {
            margin: 0;
            color: #666;
        }
        
        .appointment-time {
            font-weight: 600;
            color: #28a745;
        }
        
        .appointment-actions {
            display: flex;
            gap: 0.5rem;
        }
        
        .btn-small {
            padding: 0.5rem 1rem;
            border-radius: 5px;
            text-decoration: none;
            font-size: 0.8rem;
            font-weight: 600;
        }
        
        .btn-success {
            background: #28a745;
            color: white;
            border: none;
        }
        
        .btn-warning {
            background: #ffc107;
            color: #212529;
            border: none;
        }
        
        .btn-danger {
            background: #dc3545;
            color: white;
            border: none;
        }
        
        .no-appointments {
            text-align: center;
            color: #666;
            padding: 2rem;
        }
        
        @media (max-width: 768px) {
            .staff-dashboard {
                padding: 1rem;
            }

            .dashboard-container {
                padding: 1.5rem;
                border-radius: 12px;
            }

            .staff-info {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }
            
            .dashboard-grid {
                grid-template-columns: 1fr;
            }
            
            .appointment-item {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }
        }

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
            width: 90%;
            max-width: 800px;
            box-shadow: 
                0 20px 60px rgba(0,0,0,0.3),
                0 0 0 1px rgba(255,255,255,0.1);
            max-height: 85vh;
            display: flex;
            flex-direction: column;
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
            overflow-y: auto;
            flex: 1;
        }

        .appointment-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
        }

        .appointment-table th,
        .appointment-table td {
            padding: 1rem;
            text-align: left;
            border-bottom: 1px solid #e0e0e0;
        }

        .appointment-table th {
            background-color: #f8f9fa;
            font-weight: 600;
            color: #2c5aa0;
        }

        .appointment-table tr:hover {
            background-color: #f8f9fa;
        }

        .appointment-table input[type="radio"] {
            margin-right: 0.5rem;
        }

        .btn-cancel-appointment {
            background: #dc3545;
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }

        .btn-cancel-appointment:hover {
            background: #c82333;
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(220, 53, 69, 0.4);
        }

        .btn-cancel-appointment:disabled {
            background: #ccc;
            cursor: not-allowed;
            transform: none;
        }

        .no-appointments-message {
            text-align: center;
            padding: 3rem;
            color: #666;
        }

        .no-appointments-message i {
            font-size: 3rem;
            color: #ccc;
            margin-bottom: 1rem;
        }

        .badge {
            display: inline-block;
            padding: 0.35rem 0.85rem;
            font-size: 0.8rem;
            font-weight: 600;
            border-radius: 20px;
            text-transform: capitalize;
            letter-spacing: 0.5px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            background-color: #6c757d; /* Default background for unknown statuses */
            color: white; /* Default text color */
        }

        .badge-scheduled {
            background-color: #28a745;
            color: white;
        }

        .badge-confirmed {
            background-color: #17a2b8;
            color: white;
        }

        .badge-cancelled {
            background-color: #dc3545;
            color: white;
        }

        .badge-completed {
            background-color: #6c757d;
            color: white;
        }

        .badge-pending {
            background-color: #ffc107;
            color: #212529;
        }

        .badge-missed {
            background-color: #fd7e14;
            color: white;
        }

        /* Welcome Message Section at Bottom */
        .welcome-message-section {
            background: linear-gradient(135deg, rgba(44, 90, 160, 0.05) 0%, rgba(30, 64, 128, 0.08) 100%);
            border-radius: 20px;
            padding: 2rem 2.5rem;
            margin-top: 2rem;
            text-align: center;
            border: 2px solid rgba(44, 90, 160, 0.1);
            box-shadow: 0 8px 32px rgba(44, 90, 160, 0.08);
            animation: fadeInUp 0.8s ease-out;
            animation-delay: 0.4s;
            animation-fill-mode: both;
        }

        .welcome-message-section h2 {
            font-size: 1.8rem;
            font-weight: 700;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            /* Gradient text effect using background-clip */
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            color: transparent;
            background-clip: text;
            margin-bottom: 0.75rem;
            line-height: 1.2;
        }

        .welcome-message-section p {
            font-size: 1rem;
            color: #555;
            line-height: 1.6;
            max-width: 900px;
            margin: 0 auto 1rem;
            font-weight: 400;
        }

        .quick-stats-row {
            display: flex;
            justify-content: center;
            gap: 2rem;
            flex-wrap: wrap;
            margin-top: 1rem;
        }

        .quick-stat-item {
            text-align: center;
        }

        .quick-stat-number {
            font-size: 2rem;
            font-weight: 800;
            background: linear-gradient(135deg, #28a745, #20c997);
            /* Gradient text effect using background-clip */
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            color: transparent;
            background-clip: text;
            line-height: 1;
            margin-bottom: 0.25rem;
        }

        .quick-stat-label {
            font-size: 0.85rem;
            color: #666;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        @media (max-width: 768px) {
            .welcome-message-section h2 {
                font-size: 1.5rem;
            }

            .welcome-message-section p {
                font-size: 0.9rem;
            }

            .quick-stat-number {
                font-size: 1.5rem;
            }

            .quick-stat-label {
                font-size: 0.75rem;
            }

            .quick-stats-row {
                gap: 1rem;
            }
        }
    </style>
</asp:Content>

    <asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="staff-dashboard">
        <div class="dashboard-container">
            <!-- Logo -->
            <div class="dashboard-logo">
                <div class="logo-brand">EMONTI</div>
                <div class="logo-profession">OPTOMETRIST</div>
            </div>

            <!-- Dashboard Header -->
            <div class="dashboard-header">
                <div class="staff-info">
                    <div class="staff-details">
                        <h1>Welcome, <asp:Literal ID="litStaffName" runat="server" /></h1>
<p><asp:Literal ID="litStaffRole" runat="server" /> <i class="fas fa-circle"></i> <asp:Literal ID="litStaffId" runat="server" /></p>                </div>
                    <div class="staff-actions">
                        <asp:LinkButton ID="btnLogout" runat="server" CssClass="btn-logout" OnClick="btnLogout_Click">
                            <i class="fas fa-sign-out-alt"></i> Logout
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <!-- Dashboard Stats -->
            <div class="dashboard-grid">
                <!-- Today's Appointments -->
                <div class="dashboard-card">
                    <div class="card-header">
                        <div class="card-icon icon-appointments">
                            <i class="fas fa-calendar-check"></i>
                        </div>
                        <h3 class="card-title">Today's Appointments</h3>
                    </div>
                    <div class="card-content">
                        <div class="stat-number">
                            <asp:Literal ID="litTodayAppointments" runat="server" />
                        </div>
                        <div class="stat-label">Appointments scheduled for today</div>
                    </div>
                    <div class="card-actions">
                        <asp:LinkButton ID="btnViewAppointments" runat="server" CssClass="btn-primary" OnClick="btnViewAppointments_Click">
                            View All
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnManageAppointments" runat="server" CssClass="btn-secondary" OnClick="btnManageAppointments_Click">
                            Manage
                        </asp:LinkButton>
                    </div>
                </div>

                <!-- All Appointments -->
                <div class="dashboard-card">
                    <div class="card-header">
                        <div class="card-icon icon-appointments">
                            <i class="fas fa-calendar-alt"></i>
                        </div>
                        <h3 class="card-title">All Appointments</h3>
                    </div>
                    <div class="card-content">
                        <div class="stat-number">
                            <asp:Literal ID="litAllAppointments" runat="server" />
                        </div>
                        <div class="stat-label">Future appointments</div>
                    </div>
                    <div class="card-actions">
                        <asp:LinkButton ID="btnManageAllAppointments" runat="server" CssClass="btn-primary" OnClick="btnManageAllAppointments_Click">
                            Manage
                        </asp:LinkButton>
                    </div>
                </div>

                <!-- BI Reports -->
                <div class="dashboard-card">
                    <div class="card-header">
                        <div class="card-icon icon-reports">
                            <i class="fas fa-chart-bar"></i>
                        </div>
                        <h3 class="card-title">BI Reports</h3>
                    </div>
                    <div class="card-content">
                        <div class="stat-label" style="font-size: 1rem; line-height: 1.6;">
                            View comprehensive analytics and business intelligence reports
                        </div>
                    </div>
                    <div class="card-actions">
                        <asp:LinkButton ID="btnViewReports" runat="server" CssClass="btn-primary" OnClick="btnViewReports_Click">
                            View Reports
                        </asp:LinkButton>
                    </div>
                </div>
            </div>

            <!-- Welcome Message Section -->
            <div class="welcome-message-section">
                <h2><i class="fas fa-chart-line"></i> Staff Dashboard Excellence</h2>
                <p>
                    Welcome to your comprehensive staff management portal. Here you have full control over appointments, 
                    patient schedules, and business analytics. Use the tools above to manage your daily tasks efficiently 
                    and access powerful reporting features to track practice performance.
                </p>
                <div class="quick-stats-row">
                    <div class="quick-stat-item">
                        <div class="quick-stat-number">
                            <i class="fas fa-users"></i>
                        </div>
                        <div class="quick-stat-label">Patient Care</div>
                    </div>
                    <div class="quick-stat-item">
                        <div class="quick-stat-number">
                            <i class="fas fa-calendar-check"></i>
                        </div>
                        <div class="quick-stat-label">Scheduling</div>
                    </div>
                    <div class="quick-stat-item">
                        <div class="quick-stat-number">
                            <i class="fas fa-chart-bar"></i>
                        </div>
                        <div class="quick-stat-label">Analytics</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- View All Appointments Modal -->
    <div id="viewAllModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">
                <h2><i class="fas fa-calendar-check"></i> Today's Appointments</h2>
                <button type="button" class="close" onclick="closeModal('viewAllModal')">&times;</button>
            </div>
            <div class="modal-body">
                <asp:Repeater ID="rptViewAllAppointments" runat="server">
                    <HeaderTemplate>
                        <table class="appointment-table">
                            <thead>
                                <tr>
                                    <th>Time</th>
                                    <th>Patient Name</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("Notes") %></td>
                            <td><%# Eval("DoctorName") %></td>
                            <td>
                                <span class="badge badge-<%# (Eval("Status") ?? "Scheduled").ToString().ToLower() %>">
                                    <%# Eval("Status") ?? "Scheduled" %>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlNoViewAllAppointments" runat="server" Visible="false" CssClass="no-appointments-message">
                    <i class="fas fa-calendar-times"></i>
                    <h3>No Appointments Today</h3>
                    <p>You have no appointments scheduled for today.</p>
                </asp:Panel>
            </div>
        </div>
    </div>

    <!-- Manage All Appointments Modal -->
    <div id="manageAllModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">
                <h2><i class="fas fa-calendar-alt"></i> Manage All Appointments</h2>
                <button type="button" class="close" onclick="closeModal('manageAllModal')">&times;</button>
            </div>
            <div class="modal-body">
                <asp:Repeater ID="rptManageAllAppointments" runat="server">
                    <HeaderTemplate>
                        <table class="appointment-table">
                            <thead>
                                <tr>
                                    <th>Select</th>
                                    <th>Date</th>
                                    <th>Time</th>
                                    <th>Patient Name</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="radio" name="selectedAllAppointment" value='<%# Eval("AppointmentId") %>' />
                            </td>
                            <td><%# ((DateTime)Eval("AppointmentDate")).ToString("MMM dd, yyyy") %></td>
                            <td><%# Eval("Notes") %></td>
                            <td><%# Eval("DoctorName") %></td>
                            <td>
                                <span class="badge badge-<%# (Eval("Status") ?? "Scheduled").ToString().ToLower() %>">
                                    <%# Eval("Status") ?? "Scheduled" %>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlNoManageAllAppointments" runat="server" Visible="false" CssClass="no-appointments-message">
                    <i class="fas fa-calendar-times"></i>
                    <h3>No Future Appointments</h3>
                    <p>You have no future appointments scheduled.</p>
                </asp:Panel>
                <div style="margin-top: 2rem; text-align: right;">
                    <input type="hidden" name="cancelReasonAll" id="cancelReasonAll" value="" />
                    <asp:Button ID="btnCancelAllAppointment" runat="server" 
                        CssClass="btn-cancel-appointment" 
                        Text="Cancel Selected Appointment" 
                        OnClick="btnCancelAllAppointment_Click" 
                        OnClientClick="return confirmCancelAll();" />
                </div>
            </div>
        </div>
    </div>

    <!-- Manage Appointments Modal -->
    <div id="manageModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">
                <h2><i class="fas fa-cog"></i> Manage Today's Appointments</h2>
                <button type="button" class="close" onclick="closeModal('manageModal')">&times;</button>
            </div>
            <div class="modal-body">
                <asp:Repeater ID="rptManageAppointments" runat="server">
                    <HeaderTemplate>
                        <table class="appointment-table">
                            <thead>
                                <tr>
                                    <th>Select</th>
                                    <th>Time</th>
                                    <th>Patient Name</th>
                                    <th>Status</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="radio" name="selectedAppointment" value='<%# Eval("AppointmentId") %>' />
                            </td>
                            <td><%# Eval("Notes") %></td>
                            <td><%# Eval("DoctorName") %></td>
                            <td>
                                <span class="badge badge-<%# (Eval("Status") ?? "Scheduled").ToString().ToLower() %>">
                                    <%# Eval("Status") ?? "Scheduled" %>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                <asp:Panel ID="pnlNoManageAppointments" runat="server" Visible="false" CssClass="no-appointments-message">
                    <i class="fas fa-calendar-times"></i>
                    <h3>No Appointments Today</h3>
                    <p>You have no appointments scheduled for today.</p>
                </asp:Panel>
                <div style="margin-top: 2rem; text-align: right;">
                    <input type="hidden" name="cancelReason" id="cancelReason" value="" />
                    <asp:Button ID="btnCancelAppointment" runat="server" 
                        CssClass="btn-cancel-appointment" 
                        Text="Cancel Selected Appointment" 
                        OnClick="btnCancelAppointment_Click" 
                        OnClientClick="return confirmCancel();" />
                </div>
            </div>
        </div>
    </div>

    <!-- Update Timeslots Modal -->
    <div id="updateTimeslotsModal" class="modal">
        <div class="modal-content" style="max-width: 1000px;">
            <div class="modal-header">
                <h2><i class="fas fa-clock"></i> Update Timeslots</h2>
                <button type="button" class="close" onclick="closeModal('updateTimeslotsModal')">&times;</button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="updTimeslots" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 2rem;">
                            <!-- Calendar Section -->
                            <div>
                                <h3 style="margin-bottom: 1rem; color: #2c5aa0;">Select Date</h3>
                                <asp:Panel ID="pnlCalendar" runat="server" style="display: grid; grid-template-columns: repeat(7, 1fr); gap: 0.5rem;">
                                    <!-- Calendar days will be populated server-side -->
                                </asp:Panel>
                                <div style="margin-top: 1rem; padding: 1rem; background: #f8f9fa; border-radius: 8px; text-align: center; font-weight: 600; color: #2c5aa0;">
                                    <asp:Literal ID="litSelectedDate" runat="server" Text="No date selected" />
                                </div>
                            </div>
                            
                            <!-- Timeslots Section -->
                            <div>
                                <h3 style="margin-bottom: 1rem; color: #2c5aa0;">Available Timeslots</h3>
                                <asp:Panel ID="pnlTimeslots" runat="server" style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 0.5rem; max-height: 400px; overflow-y: auto;">
                                    <p style="grid-column: 1 / -1; text-align: center; color: #666; padding: 2rem;">
                                        Please select a date to view timeslots
                                    </p>
                                </asp:Panel>
                                
                                <!-- Legend -->
                                <div style="margin-top: 1rem; padding: 1rem; background: #f8f9fa; border-radius: 8px;">
                                    <h4 style="margin-bottom: 0.5rem; font-size: 0.9rem;">Legend:</h4>
                                    <div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 0.5rem; font-size: 0.85rem;">
                                        <div><span style="display: inline-block; width: 20px; height: 20px; background: #28a745; border-radius: 4px; vertical-align: middle; margin-right: 0.5rem;"></span>Available</div>
                                        <div><span style="display: inline-block; width: 20px; height: 20px; background: #dc3545; border-radius: 4px; vertical-align: middle; margin-right: 0.5rem;"></span>Booked</div>
                                        <div><span style="display: inline-block; width: 20px; height: 20px; background: #ffc107; border-radius: 4px; vertical-align: middle; margin-right: 0.5rem;"></span>Blocked</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <!-- BI Reports Modal -->
    <div id="reportsModal" class="modal">
        <div class="modal-content" style="max-width: 90%; width: 1000px;">
            <div class="modal-header">
                <h2><i class="fas fa-chart-bar"></i> BI Reports</h2>
                <button type="button" class="close" onclick="closeModal('reportsModal')">&times;</button>
            </div>
            <div class="modal-body" style="padding: 0; text-align: center;">
                <iframe title="M3 REPORTS" width="1024" height="1060" src="https://app.powerbi.com/view?r=eyJrIjoiNjA0NjQ4NDctMjI5Ny00NGNiLWFhMDQtNTU2NDhiOWFlZDVlIiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9" frameborder="0" allowFullScreen="true"></iframe>
            </div>
        </div>
    </div>

    <script>
        function openModal(modalId) {
            document.getElementById(modalId).style.display = 'block';
        }

        function closeModal(modalId) {
            document.getElementById(modalId).style.display = 'none';
        }

        function confirmCancel() {
            var selected = document.querySelector('input[name="selectedAppointment"]:checked');
            if (!selected) {
                alert('Please select an appointment to cancel.');
                return false;
            }
            var reason = prompt('Enter the reason for cancellation:');
            if (reason === null) return false;
            document.getElementById('cancelReason').value = reason;
            return true;
        }

        function confirmCancelAll() {
            var selected = document.querySelector('input[name="selectedAllAppointment"]:checked');
            if (!selected) {
                alert('Please select an appointment to cancel.');
                return false;
            }
            var reason = prompt('Enter the reason for cancellation:');
            if (reason === null) return false;
            document.getElementById('cancelReasonAll').value = reason;
            return true;
        }

        // Close modal when clicking outside of it
        window.onclick = function(event) {
            var viewAllModal = document.getElementById('viewAllModal');
            var manageModal = document.getElementById('manageModal');
            var manageAllModal = document.getElementById('manageAllModal');
            var updateTimeslotsModal = document.getElementById('updateTimeslotsModal');
            var reportsModal = document.getElementById('reportsModal');
            if (event.target == viewAllModal) {
                viewAllModal.style.display = 'none';
            }
            if (event.target == manageModal) {
                manageModal.style.display = 'none';
            }
            if (event.target == manageAllModal) {
                manageAllModal.style.display = 'none';
            }
            if (event.target == updateTimeslotsModal) {
                updateTimeslotsModal.style.display = 'none';
            }
            if (event.target == reportsModal) {
                reportsModal.style.display = 'none';
            }
        }

    </script>
</asp:Content>
