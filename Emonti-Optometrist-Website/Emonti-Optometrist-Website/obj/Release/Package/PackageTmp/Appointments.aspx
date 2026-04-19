<%@ Page Title="My Appointments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Appointments.aspx.cs" Inherits="Emonti_Optometrist_Website.Appointments" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
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
        .appointments-hero {
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
        
        .appointments-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .appointments-hero .container {
            position: relative;
            z-index: 2;
            animation: fadeInUp 1s ease-out;
        }
        
        .appointments-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        .appointments-hero p {
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
        
        .appointments-container {
            max-width: 1200px;
            margin: -2rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .appointments-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .appointments-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 2rem;
            padding-bottom: 1rem;
            border-bottom: 2px solid #e0e0e0;
        }
        
        .appointments-title {
            font-size: 1.8rem;
            color: #2c5aa0;
            font-weight: 600;
        }
        
        .book-appointment-btn {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }
        
        .book-appointment-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .filter-section {
            margin-bottom: 2rem;
            padding: 1.5rem;
            background: #f8f9fa;
            border-radius: 10px;
        }
        
        .filter-row {
            display: flex;
            gap: 1rem;
            align-items: end;
        }
        
        .filter-group {
            flex: 1;
        }
        
        .filter-group label {
            display: block;
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
        }
        
        .filter-group input,
        .filter-group select {
            width: 100%;
            padding: 0.75rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
        }
        
        .filter-btn {
            background: #2c5aa0;
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
        }
        
        .appointment-item {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1rem;
            transition: all 0.3s ease;
        }
        
        .appointment-item:hover {
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }
        
        .appointment-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 1px solid #e0e0e0;
        }
        
        .appointment-id {
            font-weight: 600;
            color: #2c5aa0;
            font-size: 1.1rem;
        }
        
        .appointment-date {
            color: #666;
        }
        
        .appointment-status {
            padding: 0.25rem 0.75rem;
            border-radius: 15px;
            font-size: 0.9rem;
            font-weight: 600;
        }
        
        .status-scheduled {
            background: #cce5ff;
            color: #004085;
        }
        
        .status-completed {
            background: #d4edda;
            color: #155724;
        }
        
        .status-cancelled {
            background: #f8d7da;
            color: #721c24;
        }
        
        .status-rescheduled {
            background: #fff3cd;
            color: #856404;
        }
        
        .status-missed {
            background: #ffc107;
            color: #212529;
        }
        
        .status-upcoming {
            background: #cce5ff;
            color: #004085;
        }
        
        .status-completed {
            background: #d4edda;
            color: #155724;
        }
        
        .status-cancelled {
            background: #f8d7da;
            color: #721c24;
        }
        
        .appointment-details {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 1rem;
            margin-bottom: 1rem;
        }
        
        .detail-item {
            display: flex;
            flex-direction: column;
        }
        
        .detail-label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.25rem;
        }
        
        .detail-value {
            color: #666;
        }
        
        .appointment-actions {
            display: flex;
            gap: 0.5rem;
            margin-top: 1rem;
        }
        
        .action-btn {
            padding: 0.5rem 1rem;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 0.9rem;
            text-decoration: none;
            display: inline-block;
        }
        
        .btn-view {
            background: #2c5aa0;
            color: white;
        }
        
        .btn-reschedule {
            background: #ffc107;
            color: #212529;
        }
        
        .btn-cancel {
            background: #dc3545;
            color: white;
        }
        
        .btn-rebook {
            background: #28a745;
            color: white;
        }
        
        .btn-rebook:hover {
            background: #218838;
            color: white;
        }
        
        .missed-warning {
            background: #fff3cd;
            border: 1px solid #ffeaa7;
            color: #856404;
            padding: 1rem;
            border-radius: 8px;
            margin-bottom: 1rem;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .appointment-item.missed {
            border-left: 4px solid #ffc107;
            background: #fffbf0;
        }
        
        .appointment-item.upcoming {
            border-left: 4px solid #007bff;
            background: #f8f9ff;
        }
        
        .appointment-item.completed {
            border-left: 4px solid #28a745;
            background: #f8fff8;
        }
        
        .appointment-item.cancelled {
            border-left: 4px solid #dc3545;
            background: #fff8f8;
        }
        
        .no-appointments {
            text-align: center;
            padding: 3rem;
            color: #666;
        }
        
        .no-appointments i {
            font-size: 3rem;
            color: #ddd;
            margin-bottom: 1rem;
        }
        
        @media (max-width: 768px) {
            .appointments-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }
            
            .filter-row {
                flex-direction: column;
            }
            
            .appointment-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 0.5rem;
            }
            
            .appointment-details {
                grid-template-columns: 1fr;
            }
            
            .appointment-actions {
                flex-direction: column;
            }
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <!-- Hero Section -->
    <section class="appointments-hero">
        <div class="container">
            <h1><i class="fas fa-calendar"></i> My Appointments</h1>
            <p>View and manage your appointment history</p>
        </div>
    </section>
    
    <div class="appointments-container">
        <div class="appointments-content">
            <div class="appointments-header">
                <h2 class="appointments-title">Appointment History</h2>
                <asp:LinkButton ID="btnBookAppointment" runat="server" CssClass="book-appointment-btn" 
                    OnClick="btnBookAppointment_Click">
                    <i class="fas fa-plus"></i> Book New Appointment
                </asp:LinkButton>
            </div>
            
            <!-- Filter Section -->
            <div class="filter-section">
                <div class="filter-row">
                    <div class="filter-group">
                        <label>Date From</label>
                        <asp:TextBox ID="txtDateFrom" runat="server" TextMode="Date" />
                    </div>
                    <div class="filter-group">
                        <label>Date To</label>
                        <asp:TextBox ID="txtDateTo" runat="server" TextMode="Date" />
                    </div>
                    <div class="filter-group">
                        <label>Status Filter</label>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server">
                            <asp:ListItem Text="All Appointments" Value="" />
                            <asp:ListItem Text="Scheduled" Value="Scheduled" />
                            <asp:ListItem Text="Completed" Value="Completed" />
                            <asp:ListItem Text="Cancelled" Value="Cancelled" />
                            <asp:ListItem Text="Rescheduled" Value="Rescheduled" />
                        </asp:DropDownList>
                    </div>
                    <div class="filter-group">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="filter-btn" OnClick="btnFilter_Click" />
                    </div>
                </div>
            </div>
            
            <!-- Appointments List -->
            <asp:Panel ID="pnlAppointments" runat="server">
                <asp:Repeater ID="rptAppointments" runat="server" OnItemCommand="rptAppointments_ItemCommand">
                    <ItemTemplate>
                        <div class="appointment-item <%# GetAppointmentCssClass(Eval("AppointmentType")) %>">
                            <div class="appointment-header">
                                <div>
                                    <span class="appointment-id">Appointment #<%# Eval("AppointmentId") %></span>
                                    <span class="appointment-date"> - <%# Eval("AppointmentDate", "{0:MMMM dd, yyyy}") %></span>
                                </div>
                                <span class="appointment-status status-<%# Eval("AppointmentType").ToString().ToLower() %>">
                                    <%# Eval("AppointmentType") %>
                                </span>
                            </div>
                            
                            <!-- Missed Appointment Warning -->
                            <asp:Panel ID="pnlMissedWarning" runat="server" 
                                Visible='<%# Eval("AppointmentType").ToString() == "Missed" %>'
                                CssClass="missed-warning">
                                <i class="fas fa-exclamation-triangle"></i>
                                <strong>Missed Appointment:</strong> You did not attend this appointment and no payment was recorded.
                            </asp:Panel>
                            
                            <div class="appointment-details">
                                <asp:Panel ID="pnlOptometrist" runat="server" Visible='<%# !string.IsNullOrEmpty(Eval("DoctorName").ToString()) %>'>
                                    <div class="detail-item">
                                        <span class="detail-label">Optometrist</span>
                                        <span class="detail-value"><%# Eval("DoctorName") %></span>
                                    </div>
                                </asp:Panel>
                                
                                <asp:Panel ID="pnlTime" runat="server" Visible='<%# !string.IsNullOrEmpty(Eval("Notes").ToString()) %>'>
                                    <div class="detail-item">
                                        <span class="detail-label">Time</span>
                                        <span class="detail-value"><%# Eval("Notes") %></span>
                                    </div>
                                </asp:Panel>
                                
                                <div class="detail-item">
                                    <span class="detail-label">Payment Status</span>
                                    <span class="detail-value">
                                        <%# GetPaymentStatusDisplay(Eval("HasPayment"), Eval("PaymentStatus"), Eval("TotalPayable")) %>
                                    </span>
                                </div>
                            </div>
                            
                            <div class="appointment-actions">
                                <!-- Cancel - Only for upcoming appointments -->
                                <asp:LinkButton ID="btnCancelAppointment" runat="server" CssClass="action-btn btn-cancel" 
                                    CommandName="CancelAppointment" CommandArgument='<%# Eval("AppointmentId") %>'
                                    Visible='<%# Eval("AppointmentType").ToString() == "Upcoming" %>'
                                    OnClientClick="return confirm('Are you sure you want to cancel this appointment?');">
                                    <i class="fas fa-times"></i> Cancel
                                </asp:LinkButton>
                                
                                <!-- Rebook - Only for missed appointments -->
                                <asp:LinkButton ID="btnRebookAppointment" runat="server" CssClass="action-btn btn-rebook" 
                                    CommandName="RebookAppointment" CommandArgument='<%# Eval("AppointmentId") %>'
                                    Visible='<%# Eval("AppointmentType").ToString() == "Missed" %>'>
                                    <i class="fas fa-calendar-plus"></i> Rebook
                                </asp:LinkButton>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
            
            <!-- No Appointments Message -->
            <asp:Panel ID="pnlNoAppointments" runat="server" Visible="false">
                <div class="no-appointments">
                    <i class="fas fa-calendar"></i>
                    <h3>No Appointments Found</h3>
                    <p>You haven't booked any appointments yet.</p>
                    <asp:LinkButton ID="btnBookFirstAppointment" runat="server" CssClass="book-appointment-btn" 
                        OnClick="btnBookAppointment_Click">
                        <i class="fas fa-calendar-plus"></i> Book Your First Appointment
                    </asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
