<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.Dashboard" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
<style>
* { margin: 0; padding: 0; box-sizing: border-box; }
body { font-family: 'Segoe UI', system-ui, sans-serif; background: #f0f2f5; }
.admin-wrapper { display: flex; min-height: 100vh; }
.admin-sidebar { width: 250px; background: #1a1d23; color: #fff; padding: 1.5rem 0; flex-shrink: 0; position: fixed; top: 0; left: 0; height: 100vh; overflow-y: auto; z-index: 100; }
.sidebar-brand { padding: 0 1.25rem 1.5rem; border-bottom: 1px solid rgba(255,255,255,0.08); margin-bottom: 1rem; }
.sidebar-brand h2 { font-size: 1.1rem; font-weight: 700; color: #fff; }
.sidebar-brand small { font-size: 0.75rem; color: rgba(255,255,255,0.5); }
.sidebar-nav { list-style: none; padding: 0; }
.sidebar-nav li a { display: flex; align-items: center; gap: 0.75rem; padding: 0.75rem 1.25rem; color: rgba(255,255,255,0.65); text-decoration: none; font-size: 0.9rem; font-weight: 500; transition: all 0.2s ease; border-left: 3px solid transparent; }
.sidebar-nav li a:hover { background: rgba(255,255,255,0.06); color: #fff; }
.sidebar-nav li a.active { background: rgba(102,126,234,0.15); color: #667eea; border-left-color: #667eea; }
.sidebar-nav li a i { width: 20px; text-align: center; }
.sidebar-nav .divider { margin-top: 1rem; border-top: 1px solid rgba(255,255,255,0.08); padding-top: 0.5rem; }
.sidebar-nav .logout { color: #ff6b6b !important; }
.admin-main { margin-left: 250px; flex: 1; padding: 2rem; min-height: 100vh; }
.admin-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 2rem; flex-wrap: wrap; gap: 1rem; }
.admin-header h1 { font-size: 1.5rem; font-weight: 700; color: #1a1d23; }
.admin-header h1 i { color: #667eea; margin-right: 0.5rem; }
.stats-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem; margin-bottom: 2rem; }
.stat-card { background: #fff; padding: 1.25rem; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); transition: all 0.3s ease; border: 1px solid rgba(0,0,0,0.04); }
.stat-card:hover { transform: translateY(-3px); box-shadow: 0 8px 24px rgba(0,0,0,0.1); }
.stat-card .stat-icon { width: 48px; height: 48px; border-radius: 12px; display: flex; align-items: center; justify-content: center; font-size: 1.2rem; color: #fff; margin-bottom: 0.75rem; }
.stat-card .stat-icon.teal { background: linear-gradient(135deg, #20c997, #0ca678); }
.stat-card .stat-icon.blue { background: linear-gradient(135deg, #667eea, #5a67d8); }
.stat-card .stat-icon.orange { background: linear-gradient(135deg, #f6ad55, #ed8936); }
.stat-card .stat-icon.green { background: linear-gradient(135deg, #48bb78, #38a169); }
.stat-card .stat-icon.purple { background: linear-gradient(135deg, #a78bfa, #8b5cf6); }
.stat-card .stat-icon.pink { background: linear-gradient(135deg, #f472b6, #ec4899); }
.stat-card .stat-icon.red { background: linear-gradient(135deg, #fc8181, #f56565); }
.stat-card h3 { font-size: 0.8rem; color: #888; text-transform: uppercase; letter-spacing: 0.5px; margin-bottom: 0.25rem; }
.stat-card .stat-value { font-size: 1.75rem; font-weight: 700; color: #1a1d23; }
.stat-card .stat-value.revenue { color: #38a169; }
.quick-links { display: flex; gap: 0.75rem; margin-bottom: 2rem; flex-wrap: wrap; }
.quick-link { display: inline-flex; align-items: center; gap: 0.5rem; padding: 0.65rem 1.25rem; background: #fff; border-radius: 10px; text-decoration: none; font-size: 0.85rem; font-weight: 600; color: #1a1d23; box-shadow: 0 2px 8px rgba(0,0,0,0.06); transition: all 0.2s ease; border: 1px solid rgba(0,0,0,0.04); }
.quick-link:hover { transform: translateY(-2px); box-shadow: 0 6px 16px rgba(0,0,0,0.1); color: #667eea; }
.section-card { background: #fff; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); overflow: hidden; margin-bottom: 1.5rem; }
.section-header { padding: 1rem 1.25rem; border-bottom: 1px solid #f0f0f0; display: flex; align-items: center; gap: 0.5rem; }
.section-header h2 { font-size: 1rem; font-weight: 700; color: #1a1d23; }
.section-header h2 i { color: #667eea; }
table { width: 100%; border-collapse: collapse; }
thead { background: #f8f9fa; }
th { padding: 0.75rem 1rem; text-align: left; font-size: 0.75rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; color: #888; }
td { padding: 0.7rem 1rem; font-size: 0.85rem; color: #333; border-bottom: 1px solid #f5f5f5; }
tr:hover td { background: rgba(102,126,234,0.02); }
.status-badge { display: inline-block; padding: 0.2rem 0.65rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; text-transform: capitalize; }
.status-badge.pending, .status-badge.processing { background: #fff3cd; color: #856404; }
.status-badge.shipped { background: #cce5ff; color: #004085; }
.status-badge.delivered, .status-badge.completed { background: #d4edda; color: #155724; }
.status-badge.cancelled { background: #f8d7da; color: #721c24; }
.empty-state { text-align: center; padding: 3rem 1rem; color: #999; }
.empty-state i { font-size: 2.5rem; margin-bottom: 0.75rem; color: #ddd; }
.admin-footer { text-align: center; padding: 1.5rem; color: #999; font-size: 0.8rem; margin-top: 2rem; }
@media (max-width: 768px) { .admin-sidebar { width: 60px; } .sidebar-brand h2, .sidebar-brand small, .sidebar-nav li a span { display: none; } .sidebar-nav li a { justify-content: center; padding: 0.75rem; } .admin-main { margin-left: 60px; padding: 1rem; } .stats-grid { grid-template-columns: repeat(2, 1fr); gap: 0.75rem; } }
@media (max-width: 480px) { .stats-grid { grid-template-columns: 1fr; } }
</style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="admin-wrapper">
    <aside class="admin-sidebar">
        <div class="sidebar-brand"><h2>EMONTI</h2><small>Admin Panel</small></div>
        <ul class="sidebar-nav">
            <li><a href="Dashboard.aspx" class="active"><i class="fas fa-tachometer-alt"></i><span>Dashboard</span></a></li>
            <li><a href="ManageOrders.aspx"><i class="fas fa-shopping-cart"></i><span>Orders</span></a></li>
            <li><a href="ManageProducts.aspx"><i class="fas fa-box"></i><span>Products</span></a></li>
            <li><a href="ManageCustomers.aspx"><i class="fas fa-address-book"></i><span>Customers</span></a></li>
            <li><a href="ManageStaff.aspx"><i class="fas fa-users"></i><span>Staff</span></a></li>
            <li><a href="QueryDb.aspx"><i class="fas fa-database"></i><span>Query DB</span></a></li>
            <li><a href="../Reports.aspx"><i class="fas fa-chart-bar"></i><span>Reports</span></a></li>
            <li class="divider"><a href="../Default.aspx"><i class="fas fa-arrow-left"></i><span>Back to Site</span></a></li>
            <li><a href="../Account/Logout.aspx" class="logout"><i class="fas fa-sign-out-alt"></i><span>Logout</span></a></li>
        </ul>
    </aside>
    <main class="admin-main">
        <div class="admin-header"><h1><i class="fas fa-tachometer-alt"></i> Dashboard</h1></div>
        <div class="stats-grid">
            <div class="stat-card"><div class="stat-icon teal"><i class="fas fa-calendar-day"></i></div><h3>Orders Today</h3><div class="stat-value"><asp:Label ID="lblOrdersToday" runat="server" Text="0" /></div></div>
            <div class="stat-card"><div class="stat-icon blue"><i class="fas fa-wallet"></i></div><h3>Total Revenue</h3><div class="stat-value revenue">R <asp:Label ID="lblTotalRevenue" runat="server" Text="0.00" /></div></div>
            <div class="stat-card"><div class="stat-icon orange"><i class="fas fa-clock"></i></div><h3>Pending Orders</h3><div class="stat-value"><asp:Label ID="lblPendingOrders" runat="server" Text="0" /></div></div>
            <div class="stat-card"><div class="stat-icon green"><i class="fas fa-box"></i></div><h3>Products</h3><div class="stat-value"><asp:Label ID="lblTotalProducts" runat="server" Text="0" /></div></div>
            <div class="stat-card"><div class="stat-icon purple"><i class="fas fa-users"></i></div><h3>Staff</h3><div class="stat-value"><asp:Label ID="lblTotalStaff" runat="server" Text="0" /></div></div>
            <div class="stat-card"><div class="stat-icon red"><i class="fas fa-calendar-check"></i></div><h3>Today's Appointments</h3><div class="stat-value"><asp:Label ID="lblTodayAppointments" runat="server" Text="0" /></div></div>
            <div class="stat-card"><div class="stat-icon pink"><i class="fas fa-user-plus"></i></div><h3>New Customers (Month)</h3><div class="stat-value"><asp:Label ID="lblNewCustomers" runat="server" Text="0" /></div></div>
        </div>
        <div class="quick-links">
            <a href="ManageOrders.aspx" class="quick-link"><i class="fas fa-shopping-cart"></i> Manage Orders</a>
            <a href="ManageProducts.aspx" class="quick-link"><i class="fas fa-box"></i> Manage Products</a>
            <a href="ManageCustomers.aspx" class="quick-link"><i class="fas fa-address-book"></i> Manage Customers</a>
            <a href="ManageStaff.aspx" class="quick-link"><i class="fas fa-users"></i> Manage Staff</a>
        </div>
        <div class="section-card">
            <div class="section-header"><i class="fas fa-clock" style="color:#667eea;"></i><h2>Recent Orders</h2></div>
            <asp:GridView ID="gvRecentOrders" runat="server" AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="true">
                <Columns>
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="R {0:N2}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><span class='status-badge <%# Eval("Status").ToString().ToLower() %>'><%# Eval("Status") %></span></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="fas fa-inbox"></i><p>No orders yet.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
        <div class="admin-footer">&copy; 2026 Emonti Optometrist Admin Panel</div>
    </main>
</div>
</asp:Content>
