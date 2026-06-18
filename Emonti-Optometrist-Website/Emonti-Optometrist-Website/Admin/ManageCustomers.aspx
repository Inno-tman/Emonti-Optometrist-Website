<%@ Page Title="Manage Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCustomers.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageCustomers" %>

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
.btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.55rem 1.1rem; border-radius: 8px; font-weight: 600; font-size: 0.85rem; cursor: pointer; border: none; transition: all 0.2s; text-decoration: none; }
.btn-primary { background: #667eea; color: #fff; }
.btn-primary:hover { background: #5a67d8; transform: translateY(-1px); }
.btn-sm { padding: 0.35rem 0.7rem; font-size: 0.78rem; }
.search-bar { display: flex; gap: 0.5rem; }
.search-bar input { padding: 0.5rem 0.75rem; border: 2px solid #e2e8f0; border-radius: 8px; font-size: 0.85rem; width: 250px; }
.search-bar input:focus { outline: none; border-color: #667eea; }
.section-card { background: #fff; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); overflow: hidden; }
.section-header { padding: 1rem 1.25rem; border-bottom: 1px solid #f0f0f0; display: flex; align-items: center; justify-content: space-between; gap: 0.5rem; }
.section-header h2 { font-size: 1rem; font-weight: 700; color: #1a1d23; }
.section-header h2 i { color: #667eea; }
table { width: 100%; border-collapse: collapse; }
thead { background: #f8f9fa; }
th { padding: 0.75rem 1rem; text-align: left; font-size: 0.75rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; color: #888; }
td { padding: 0.7rem 1rem; font-size: 0.85rem; color: #333; border-bottom: 1px solid #f5f5f5; vertical-align: middle; }
tr:hover td { background: rgba(102,126,234,0.02); }
.empty-state { text-align: center; padding: 3rem 1rem; color: #999; }
.empty-state i { font-size: 2.5rem; margin-bottom: 0.75rem; color: #ddd; }
.admin-footer { text-align: center; padding: 1.5rem; color: #999; font-size: 0.8rem; margin-top: 2rem; }
@media (max-width: 768px) { .admin-sidebar { width: 60px; } .sidebar-brand h2, .sidebar-brand small, .sidebar-nav li a span { display: none; } .sidebar-nav li a { justify-content: center; padding: 0.75rem; } .admin-main { margin-left: 60px; padding: 1rem; } }
</style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="admin-wrapper">
    <aside class="admin-sidebar">
        <div class="sidebar-brand"><h2>EMONTI</h2><small>Admin Panel</small></div>
        <ul class="sidebar-nav">
            <li><a href="Dashboard.aspx"><i class="fas fa-tachometer-alt"></i><span>Dashboard</span></a></li>
            <li><a href="ManageOrders.aspx"><i class="fas fa-shopping-cart"></i><span>Orders</span></a></li>
            <li><a href="ManageProducts.aspx"><i class="fas fa-box"></i><span>Products</span></a></li>
            <li><a href="ManageCustomers.aspx" class="active"><i class="fas fa-address-book"></i><span>Customers</span></a></li>
            <li><a href="ManageStaff.aspx"><i class="fas fa-users"></i><span>Staff</span></a></li>
            <li><a href="../Reports.aspx"><i class="fas fa-chart-bar"></i><span>Reports</span></a></li>
            <li class="divider"><a href="../Account/Logout.aspx" class="logout"><i class="fas fa-sign-out-alt"></i><span>Logout</span></a></li>
        </ul>
    </aside>
    <main class="admin-main">
        <div class="admin-header">
            <h1><i class="fas fa-address-book"></i> Manage Customers</h1>
            <div class="search-bar">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name or email..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" />
            </div>
        </div>
        <div class="section-card">
            <div class="section-header"><i class="fas fa-address-book" style="color:#667eea;"></i><h2>All Customers</h2></div>
            <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="true" DataKeyNames="Cust_ID">
                <Columns>
                    <asp:BoundField DataField="Customer_Name" HeaderText="Name" />
                    <asp:BoundField DataField="Customer_Surname" HeaderText="Surname" />
                    <asp:BoundField DataField="Customer_Email" HeaderText="Email" />
                    <asp:BoundField DataField="Customer_Phone" HeaderText="Phone" />
                    <asp:BoundField DataField="OrderCount" HeaderText="Orders" />
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="fas fa-users"></i><p>No customers found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </main>
</div>
<div class="admin-footer">&copy; 2026 Emonti Optometrist Admin Panel</div>
</asp:Content>
