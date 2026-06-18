<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.Dashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-hero { background: linear-gradient(135deg, #2c3e50, #3498db); color: #fff; padding: 3rem 2rem; text-align: center; }
        .admin-hero h1 { font-size: 2rem; margin-bottom: 0.5rem; }
        .admin-section { padding: 2rem; }
        .stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1.5rem; margin-bottom: 2rem; }
        .stat-card { background: #fff; border-radius: 16px; padding: 1.5rem; box-shadow: 0 4px 20px rgba(0,0,0,0.08); text-align: center; }
        .stat-card .num { font-size: 2.5rem; font-weight: 700; color: #2c3e50; }
        .stat-card .label { font-size: 0.85rem; color: #888; text-transform: uppercase; letter-spacing: 0.5px; margin-top: 0.25rem; }
        .stat-card .icon { font-size: 2rem; margin-bottom: 0.5rem; }
        .nav-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 1.5rem; }
        .nav-card { background: #fff; border-radius: 16px; padding: 2rem; box-shadow: 0 4px 20px rgba(0,0,0,0.08); text-align: center; cursor: pointer; transition: all 0.3s ease; text-decoration: none; display: block; color: inherit; }
        .nav-card:hover { transform: translateY(-4px); box-shadow: 0 8px 30px rgba(0,0,0,0.15); }
        .nav-card .icon { font-size: 2.5rem; margin-bottom: 1rem; }
        .nav-card h3 { margin: 0 0 0.5rem; font-size: 1.1rem; }
        .nav-card p { margin: 0; color: #888; font-size: 0.9rem; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-hero">
        <h1>Admin Dashboard</h1>
        <p>Welcome, <asp:Label ID="lblAdminName" runat="server" /></p>
    </div>
    <div class="admin-section">
        <div class="stats-grid">
            <div class="stat-card"><div class="icon">👥</div><div class="num"><asp:Label ID="lblStaffCount" runat="server" Text="0" /></div><div class="label">Staff</div></div>
            <div class="stat-card"><div class="icon">👤</div><div class="num"><asp:Label ID="lblCustomerCount" runat="server" Text="0" /></div><div class="label">Customers</div></div>
            <div class="stat-card"><div class="icon">📅</div><div class="num"><asp:Label ID="lblAppointmentCount" runat="server" Text="0" /></div><div class="label">Appointments</div></div>
        </div>
        <h2 style="margin-bottom:1.5rem;font-size:1.3rem;">Quick Actions</h2>
        <div class="nav-grid">
            <a href="ManageStaff.aspx" class="nav-card"><div class="icon">👥</div><h3>Manage Staff</h3><p>Add, edit, or remove staff members and change roles</p></a>
            <a href="ManageCustomers.aspx" class="nav-card"><div class="icon">👤</div><h3>Manage Customers</h3><p>View and manage all customer information</p></a>
            <a href="ManageOrders.aspx" class="nav-card"><div class="icon">📦</div><h3>Manage Orders</h3><p>View and manage customer orders</p></a>
            <a href="ManageProducts.aspx" class="nav-card"><div class="icon">🕶️</div><h3>Manage Products</h3><p>Add and update eyewear products</p></a>
        </div>
    </div>
</asp:Content>
