<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="Emonti_Optometrist_Website.Reports" %>

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
.staff-note { margin-bottom: 1rem; padding: 0.75rem 1rem; border-radius: 8px; background: #eef2ff; color: #4338ca; font-size: 0.85rem; font-weight: 600; }
.section-card { background: #fff; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); overflow: hidden; margin-bottom: 1.5rem; }
.section-header { padding: 1rem 1.25rem; border-bottom: 1px solid #f0f0f0; display: flex; align-items: center; gap: 0.5rem; }
.section-header h2 { font-size: 1rem; font-weight: 700; color: #1a1d23; }
.section-header h2 i { color: #667eea; }
.reports-container { padding: 2rem; background: #fff; border-radius: 12px; }
.reports-container iframe { width: 100%; border-radius: 8px; }
.admin-footer { text-align: center; padding: 1.5rem; color: #999; font-size: 0.8rem; margin-top: 2rem; }
@media (max-width: 768px) { .admin-sidebar { width: 60px; } .sidebar-brand h2, .sidebar-brand small, .sidebar-nav li a span { display: none; } .sidebar-nav li a { justify-content: center; padding: 0.75rem; } .admin-main { margin-left: 60px; padding: 1rem; } }
</style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="admin-wrapper">
    <aside class="admin-sidebar">
        <div class="sidebar-brand"><h2>EMONTI</h2><small>Admin Panel</small></div>
        <ul class="sidebar-nav">
            <% if (Session["StaffRole"] != null) { %>
            <li><a href="<%= Session["StaffRole"].ToString() == "Admin" ? "Admin/Dashboard.aspx" : "Staff/Dashboard.aspx" %>"><i class="fas fa-tachometer-alt"></i><span>My Dashboard</span></a></li>
            <% } %>
            <% if (Session["StaffRole"] != null && Session["StaffRole"].ToString() == "Admin") { %>
            <li><a href="Admin/ManageOrders.aspx"><i class="fas fa-shopping-cart"></i><span>Orders</span></a></li>
            <li><a href="Admin/ManageProducts.aspx"><i class="fas fa-box"></i><span>Products</span></a></li>
            <% } %>
            <li><a href="Admin/ManageCustomers.aspx"><i class="fas fa-address-book"></i><span>Customers</span></a></li>
            <% if (Session["StaffRole"] != null && Session["StaffRole"].ToString() == "Admin") { %>
            <li><a href="Admin/ManageStaff.aspx"><i class="fas fa-users"></i><span>Staff</span></a></li>
            <li><a href="Admin/QueryDb.aspx"><i class="fas fa-database"></i><span>Query DB</span></a></li>
            <% } %>
            <li><a href="Reports.aspx" class="active"><i class="fas fa-chart-bar"></i><span>Reports</span></a></li>
            <li class="divider"><a href="Account/Logout.aspx" class="logout"><i class="fas fa-sign-out-alt"></i><span>Logout</span></a></li>
        </ul>
    </aside>
    <main class="admin-main">
        <div class="admin-header">
            <h1><i class="fas fa-chart-bar"></i> Business Intelligence Reports</h1>
        </div>
        <% if (Session["StaffRole"] != null && Session["StaffRole"].ToString() != "Admin") { %>
        <div class="staff-note"><i class="fas fa-eye"></i> Staff access: use the sidebar to move between your dashboard, customers, and reports.</div>
        <% } %>
        <div class="section-card">
            <div class="section-header"><i class="fas fa-bar-chart" style="color:#667eea;"></i><h2>Power BI Analytics Dashboard</h2></div>
            <div class="reports-container">
                <iframe title="M3 REPORTS" height="800" src="https://app.powerbi.com/view?r=eyJrIjoiNjA0NjQ4NDctMjI5Ny00NGNiLWFhMDQtNTU2NDhiOWFlZDVlIiwidCI6IjIyNjgyN2Q2LWE5ZDAtNDcwZC04YzE1LWIxNDZiMDE5MmQ1MSIsImMiOjh9" frameborder="0" allowFullScreen="true"></iframe>
            </div>
        </div>
        <div class="admin-footer">&copy; 2026 Emonti Optometrist Admin Panel</div>
    </main>
</div>
</asp:Content>
