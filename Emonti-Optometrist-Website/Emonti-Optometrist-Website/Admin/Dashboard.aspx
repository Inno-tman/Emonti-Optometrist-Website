<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.Dashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-dash { padding: 2rem 2rem 3rem; max-width: 1100px; margin: 0 auto; }
        .admin-welcome { margin-bottom: 2.5rem; }
        .admin-welcome h1 { font-size: 1.75rem; font-weight: 700; color: #1a2332; margin: 0; }
        .admin-welcome p { color: #6b7a8a; margin: 0.3rem 0 0; font-size: 0.95rem; }
        .stats-row { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1.25rem; margin-bottom: 2.5rem; }
        .stat-card { background: #fff; border-radius: 14px; padding: 1.5rem; box-shadow: 0 2px 12px rgba(0,0,0,0.06); display: flex; align-items: center; gap: 1rem; }
        .stat-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 1.3rem; flex-shrink: 0; }
        .stat-icon.blue { background: #eef2ff; color: #4f6ef7; }
        .stat-icon.green { background: #e8f8ee; color: #22a45a; }
        .stat-icon.purple { background: #f3eeff; color: #7c5cfc; }
        .stat-icon.orange { background: #fef3e8; color: #e68a2e; }
        .stat-info h3 { font-size: 1.6rem; font-weight: 700; margin: 0; color: #1a2332; line-height: 1.2; }
        .stat-info p { margin: 0.15rem 0 0; font-size: 0.8rem; color: #8a9aaa; text-transform: uppercase; letter-spacing: 0.3px; }
        .section-title { font-size: 1.05rem; font-weight: 600; color: #1a2332; margin: 0 0 1rem; }
        .nav-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; }
        .nav-card { background: #fff; border-radius: 14px; padding: 1.5rem; box-shadow: 0 2px 12px rgba(0,0,0,0.06); transition: all 0.25s; text-decoration: none; display: block; }
        .nav-card:hover { transform: translateY(-3px); box-shadow: 0 6px 24px rgba(0,0,0,0.1); }
        .nav-card .nc-icon { width: 40px; height: 40px; border-radius: 10px; display: flex; align-items: center; justify-content: center; font-size: 1rem; margin-bottom: 0.75rem; }
        .nav-card .nc-icon.blue { background: #eef2ff; color: #4f6ef7; }
        .nav-card .nc-icon.green { background: #e8f8ee; color: #22a45a; }
        .nav-card .nc-icon.purple { background: #f3eeff; color: #7c5cfc; }
        .nav-card .nc-icon.orange { background: #fef3e8; color: #e68a2e; }
        .nav-card .nc-icon.teal { background: #e8f8f8; color: #14a3a3; }
        .nav-card .nc-icon.red { background: #fef0ee; color: #d95a4a; }
        .nav-card h3 { margin: 0 0 0.35rem; font-size: 0.95rem; font-weight: 600; color: #1a2332; }
        .nav-card p { margin: 0; font-size: 0.8rem; color: #8a9aaa; line-height: 1.4; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-dash">
        <div class="admin-welcome">
            <h1>Welcome back, <asp:Label ID="lblAdminName" runat="server" /></h1>
            <p>Here is what is happening at Emonti Optometrist today.</p>
        </div>

        <div class="stats-row">
            <div class="stat-card">
                <div class="stat-icon blue"><i class="fas fa-users-cog"></i></div>
                <div class="stat-info">
                    <h3><asp:Label ID="lblStaffCount" runat="server" Text="0" /></h3>
                    <p>Staff Members</p>
                </div>
            </div>
            <div class="stat-card">
                <div class="stat-icon green"><i class="fas fa-user-friends"></i></div>
                <div class="stat-info">
                    <h3><asp:Label ID="lblCustomerCount" runat="server" Text="0" /></h3>
                    <p>Customers</p>
                </div>
            </div>
            <div class="stat-card">
                <div class="stat-icon purple"><i class="fas fa-calendar-check"></i></div>
                <div class="stat-info">
                    <h3><asp:Label ID="lblAppointmentCount" runat="server" Text="0" /></h3>
                    <p>Appointments</p>
                </div>
            </div>
            <div class="stat-card">
                <div class="stat-icon orange"><i class="fas fa-chart-line"></i></div>
                <div class="stat-info">
                    <h3><asp:Label ID="lblProductCount" runat="server" Text="0" /></h3>
                    <p>Products</p>
                </div>
            </div>
        </div>

        <h2 class="section-title">Quick Actions</h2>
        <div class="nav-grid">
            <a href="ManageStaff.aspx" class="nav-card">
                <div class="nc-icon blue"><i class="fas fa-users-cog"></i></div>
                <h3>Manage Staff</h3>
                <p>Add, edit, or remove staff members and change roles</p>
            </a>
            <a href="ManageCustomers.aspx" class="nav-card">
                <div class="nc-icon green"><i class="fas fa-user-friends"></i></div>
                <h3>Manage Customers</h3>
                <p>View and manage all customer information</p>
            </a>
            <a href="../Reports.aspx" class="nav-card">
                <div class="nc-icon purple"><i class="fas fa-chart-bar"></i></div>
                <h3>BI Reports</h3>
                <p>View business intelligence reports and analytics</p>
            </a>
            <a href="ManageOrders.aspx" class="nav-card">
                <div class="nc-icon orange"><i class="fas fa-shopping-cart"></i></div>
                <h3>Manage Orders</h3>
                <p>View and manage customer orders</p>
            </a>
            <a href="ManageProducts.aspx" class="nav-card">
                <div class="nc-icon teal"><i class="fas fa-glasses"></i></div>
                <h3>Manage Products</h3>
                <p>Add and update eyewear products</p>
            </a>
            <a href="../Default.aspx" class="nav-card">
                <div class="nc-icon red"><i class="fas fa-external-link-alt"></i></div>
                <h3>View Site</h3>
                <p>Return to the public website</p>
            </a>
        </div>
    </div>
</asp:Content>
