<%@ Page Title="Manage Staff" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageStaff" %>

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
.btn-success { background: #48bb78; color: #fff; }
.btn-success:hover { background: #38a169; }
.btn-danger { background: #fc8181; color: #fff; }
.btn-danger:hover { background: #f56565; }
.btn-sm { padding: 0.35rem 0.7rem; font-size: 0.78rem; }
.section-card { background: #fff; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); overflow: hidden; }
.section-header { padding: 1rem 1.25rem; border-bottom: 1px solid #f0f0f0; display: flex; align-items: center; justify-content: space-between; gap: 0.5rem; }
.section-header h2 { font-size: 1rem; font-weight: 700; color: #1a1d23; }
.section-header h2 i { color: #667eea; }
table { width: 100%; border-collapse: collapse; }
thead { background: #f8f9fa; }
th { padding: 0.75rem 1rem; text-align: left; font-size: 0.75rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; color: #888; }
td { padding: 0.7rem 1rem; font-size: 0.85rem; color: #333; border-bottom: 1px solid #f5f5f5; vertical-align: middle; }
tr:hover td { background: rgba(102,126,234,0.02); }
.role-badge { display: inline-block; padding: 0.2rem 0.65rem; border-radius: 20px; font-size: 0.75rem; font-weight: 600; }
.role-badge.admin { background: #cce5ff; color: #004085; }
.role-badge.staff { background: #d4edda; color: #155724; }
.empty-state { text-align: center; padding: 3rem 1rem; color: #999; }
.empty-state i { font-size: 2.5rem; margin-bottom: 0.75rem; color: #ddd; }
.modal-overlay { display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 1000; justify-content: center; align-items: center; }
.modal-overlay.show { display: flex; }
.modal { display: block; position: relative; background: #fff; border-radius: 16px; padding: 2rem; width: 100%; max-width: 480px; max-height: 90vh; overflow-y: auto; box-shadow: 0 20px 60px rgba(0,0,0,0.3); z-index: 1001; }
.modal h3 { font-size: 1.15rem; margin-bottom: 1.25rem; color: #1a1d23; }
.modal .form-group { margin-bottom: 1rem; }
.modal .form-group label { display: block; font-size: 0.82rem; font-weight: 600; color: #555; margin-bottom: 0.3rem; }
.modal .form-group input, .modal .form-group select { width: 100%; padding: 0.6rem 0.8rem; border: 2px solid #e2e8f0; border-radius: 8px; font-size: 0.9rem; transition: border-color 0.2s; }
.modal .form-group input:focus, .modal .form-group select:focus { outline: none; border-color: #667eea; }
.modal-actions { display: flex; gap: 0.75rem; margin-top: 1.5rem; justify-content: flex-end; }
.modal-actions .btn-secondary { background: #e2e8f0; color: #4a5568; }
.modal-actions .btn-secondary:hover { background: #cbd5e0; }
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
            <li><a href="ManageCustomers.aspx"><i class="fas fa-address-book"></i><span>Customers</span></a></li>
            <li><a href="ManageStaff.aspx" class="active"><i class="fas fa-users"></i><span>Staff</span></a></li>
            <li><a href="../Reports.aspx"><i class="fas fa-chart-bar"></i><span>Reports</span></a></li>
            <li class="divider"><a href="../Account/Logout.aspx" class="logout"><i class="fas fa-sign-out-alt"></i><span>Logout</span></a></li>
        </ul>
    </aside>
    <main class="admin-main">
        <div class="admin-header">
            <h1><i class="fas fa-users"></i> Manage Staff</h1>
        </div>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert" style="padding:0.75rem 1rem;border-radius:8px;margin-bottom:1rem;font-size:0.85rem;"></asp:Panel>
        <div class="section-card">
            <div class="section-header"><i class="fas fa-users" style="color:#667eea;"></i><h2>All Staff Members</h2></div>
            <asp:GridView ID="gvStaff" runat="server" AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="true" DataKeyNames="Staff_ID">
                <Columns>
                    <asp:BoundField DataField="Staff_Name" HeaderText="First Name" />
                    <asp:BoundField DataField="Staff_Surname" HeaderText="Surname" />
                    <asp:BoundField DataField="Staff_Email" HeaderText="Email" />
                    <asp:TemplateField HeaderText="Role">
                        <ItemTemplate>
                            <span class='role-badge <%# Eval("Staff_Role").ToString().ToLower() %>'><%# Eval("Staff_Role") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <button type="button" class="btn btn-primary btn-sm" onclick="openEditModal(<%# Eval("Staff_ID") %>, '<%# Eval("Staff_Name") %>', '<%# Eval("Staff_Surname") %>', '<%# Eval("Staff_Email") %>', '<%# Eval("Staff_Role") %>'); return false;">Edit</button>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="fas fa-users"></i><p>No staff members found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
        <div style="text-align:center;margin-top:1.5rem;margin-bottom:2rem;">
            <button class="btn btn-primary" onclick="openAddModal(); return false;"><i class="fas fa-plus"></i> Add New Staff</button>
        </div>
    </main>
</div>

<!-- Staff Management Modal -->
<div id="addModal" class="modal-overlay">
    <div class="modal">
        <h3><i class="fas fa-user-plus" id="modalTitle" style="color:#667eea;"></i> <span id="modalTitleText">Add New Staff</span></h3>
        <div class="form-group"><label>First Name</label><asp:TextBox ID="txtName" runat="server" /></div>
        <div class="form-group"><label>Surname</label><asp:TextBox ID="txtSurname" runat="server" /></div>
        <div class="form-group"><label>Email</label><asp:TextBox ID="txtEmail" runat="server" TextMode="Email" /></div>
        <div class="form-group"><label>Password</label><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Text="Staff123" /></div>
        <div class="form-group"><label>Role</label><asp:DropDownList ID="ddlRole" runat="server"><asp:ListItem Text="Staff" Value="Staff" /><asp:ListItem Text="Admin" Value="Admin" /></asp:DropDownList></div>
        <asp:Label ID="lblAddError" runat="server" ForeColor="Red" Visible="false" style="font-size:0.85rem;" />
        <asp:HiddenField ID="hiddenStaffId" runat="server" Value="0" />
        <asp:HiddenField ID="hiddenMode" runat="server" Value="add" />
        <div class="modal-actions">
            <button type="button" id="btnDeleteModal" class="btn btn-danger" onclick="if(confirm('Are you sure you want to delete this staff member?')){performDelete();} return false;" style="display:none;">Delete</button>
            <button type="button" id="btnPromoteModal" class="btn btn-success" onclick="performPromote(); return false;" style="display:none;">Promote to Admin</button>
            <button type="button" class="btn btn-secondary" onclick="closeModal(); return false;">Cancel</button>
            <asp:Button ID="btnAddStaff" runat="server" Text="Add Staff" CssClass="btn btn-primary" OnClick="btnAddStaff_Click" />
        </div>
    </div>
</div>

<script>
if(window.location.hash=='#openModal'||('<%=Request.Form["__EVENTTARGET"]%>'||'').indexOf('btnAddStaff')>=0){document.getElementById('addModal').classList.add('show');}

function openAddModal() {
    var staffIdField = document.getElementById('<%=hiddenStaffId.ClientID%>');
    var modeField = document.getElementById('<%=hiddenMode.ClientID%>');

    staffIdField.value = '0';
    modeField.value = 'add';
    document.getElementById('<%=txtName.ClientID%>').value = '';
    document.getElementById('<%=txtSurname.ClientID%>').value = '';
    document.getElementById('<%=txtEmail.ClientID%>').value = '';
    document.getElementById('<%=txtPassword.ClientID%>').value = 'Staff123';
    document.getElementById('<%=ddlRole.ClientID%>').value = 'Staff';

    var lblError = document.getElementById('<%=lblAddError.ClientID%>');
    if (lblError) lblError.style.display = 'none';

    document.getElementById('btnDeleteModal').style.display = 'none';
    document.getElementById('btnPromoteModal').style.display = 'none';
    document.getElementById('<%=btnAddStaff.ClientID%>').value = 'Add Staff';
    document.getElementById('modalTitleText').textContent = 'Add New Staff';
    document.getElementById('addModal').classList.add('show');
}

function openEditModal(staffId, name, surname, email, role) {
    var staffIdField = document.getElementById('<%=hiddenStaffId.ClientID%>');
    var modeField = document.getElementById('<%=hiddenMode.ClientID%>');

    staffIdField.value = staffId;
    modeField.value = 'edit';
    document.getElementById('<%=txtName.ClientID%>').value = name;
    document.getElementById('<%=txtSurname.ClientID%>').value = surname;
    document.getElementById('<%=txtEmail.ClientID%>').value = email;
    document.getElementById('<%=txtPassword.ClientID%>').value = 'Staff123';
    document.getElementById('<%=ddlRole.ClientID%>').value = role;

    var lblError = document.getElementById('<%=lblAddError.ClientID%>');
    if (lblError) lblError.style.display = 'none';

    document.getElementById('<%=btnAddStaff.ClientID%>').value = 'Save Changes';
    document.getElementById('modalTitleText').textContent = 'Edit Staff Member';

    // Show/hide action buttons based on role
    if (role !== 'Admin') {
        document.getElementById('btnPromoteModal').style.display = 'inline-block';
        document.getElementById('btnDeleteModal').style.display = 'inline-block';
    } else {
        document.getElementById('btnPromoteModal').style.display = 'none';
        document.getElementById('btnDeleteModal').style.display = 'none';
    }

    document.getElementById('addModal').classList.add('show');
}

function closeModal() {
    document.getElementById('addModal').classList.remove('show');
}

function performDelete() {
    document.getElementById('<%=hiddenMode.ClientID%>').value = 'delete';
    document.getElementById('<%=btnAddStaff.ClientID%>').click();
}

function performPromote() {
    document.getElementById('<%=hiddenMode.ClientID%>').value = 'promote';
    document.getElementById('<%=btnAddStaff.ClientID%>').click();
}
</script>

<div class="admin-footer">&copy; 2026 Emonti Optometrist Admin Panel</div>
</asp:Content>
