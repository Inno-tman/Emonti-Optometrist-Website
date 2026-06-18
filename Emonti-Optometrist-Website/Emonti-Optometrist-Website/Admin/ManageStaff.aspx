<%@ Page Title="Manage Staff" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageStaff" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-section { padding: 2rem; max-width: 1200px; margin: 0 auto; }
        .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 2rem; flex-wrap: wrap; gap: 1rem; }
        .page-header h1 { margin: 0; font-size: 1.5rem; }
        .btn { padding: 0.6rem 1.5rem; border-radius: 50px; border: none; cursor: pointer; font-size: 0.9rem; font-weight: 600; transition: all 0.2s; }
        .btn-primary { background: linear-gradient(135deg, #667eea, #764ba2); color: #fff; }
        .btn-primary:hover { opacity: 0.9; }
        .btn-sm { padding: 0.35rem 1rem; font-size: 0.8rem; }
        .btn-danger { background: #dc3545; color: #fff; }
        .btn-danger:hover { background: #c82333; }
        .btn-success { background: #28a745; color: #fff; }
        .btn-success:hover { background: #218838; }
        .staff-table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.08); }
        .staff-table th { background: #f8f9fa; padding: 1rem; text-align: left; font-weight: 600; color: #555; font-size: 0.85rem; text-transform: uppercase; letter-spacing: 0.5px; }
        .staff-table td { padding: 1rem; border-top: 1px solid #f0f0f0; color: #333; }
        .staff-table tr:hover td { background: #f8f9ff; }
        .role-badge { display: inline-block; padding: 0.25rem 0.75rem; border-radius: 50px; font-size: 0.75rem; font-weight: 600; }
        .role-admin { background: #e8f4fd; color: #2196F3; }
        .role-staff { background: #f0f0f0; color: #666; }
        .role-optometrist { background: #e8f8e8; color: #28a745; }
        .modal-overlay { position: fixed; inset: 0; background: rgba(0,0,0,0.5); display: flex; align-items: center; justify-content: center; z-index: 1000; }
        .modal { background: #fff; border-radius: 20px; padding: 2rem; width: 90%; max-width: 500px; box-shadow: 0 20px 60px rgba(0,0,0,0.3); }
        .modal h2 { margin: 0 0 1.5rem; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; font-weight: 600; color: #555; margin-bottom: 0.3rem; font-size: 0.85rem; }
        .form-group input, .form-group select { width: 100%; padding: 0.7rem 1rem; border: 2px solid #e0e0e0; border-radius: 12px; font-size: 0.9rem; transition: border-color 0.2s; }
        .form-group input:focus, .form-group select:focus { border-color: #667eea; outline: none; box-shadow: 0 0 0 4px rgba(102,126,234,0.12); }
        .modal-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 1.5rem; }
        .modal-actions .btn { min-width: 100px; }
        .alert { padding: 1rem; border-radius: 12px; margin-bottom: 1.5rem; font-weight: 500; }
        .alert-success { background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
        .alert-danger { background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
        .action-cell { display: flex; gap: 0.5rem; flex-wrap: wrap; }
        .back-link { display: inline-flex; align-items: center; gap: 0.5rem; color: #667eea; text-decoration: none; margin-bottom: 1rem; font-weight: 500; }
        .back-link:hover { text-decoration: underline; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-section">
        <a href="Dashboard.aspx" class="back-link"><i class="fas fa-arrow-left"></i> Back to Dashboard</a>
        <div class="page-header">
            <h1>Manage Staff</h1>
            <button type="button" class="btn btn-primary" onclick="showAddModal()"><i class="fas fa-plus"></i> Add Staff</button>
        </div>
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />
        <asp:GridView ID="gvStaff" runat="server" CssClass="staff-table" AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvStaff_RowCommand">
            <Columns>
                <asp:BoundField DataField="Staff_ID" HeaderText="ID" />
                <asp:BoundField DataField="Staff_Name" HeaderText="Name" />
                <asp:BoundField DataField="Staff_Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Staff_Email" HeaderText="Email" />
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label ID="lblRoleBadge" runat="server" CssClass='<%# "role-badge role-" + Eval("Staff_Role").ToString().ToLower() %>' Text='<%# Eval("Staff_Role") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <div class="action-cell">
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-primary" CommandName="EditStaff" CommandArgument='<%# Eval("Staff_ID") %>'><i class="fas fa-edit"></i> Edit</asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="DeleteStaff" CommandArgument='<%# Eval("Staff_ID") %>' OnClientClick="return confirm('Are you sure you want to delete this staff member?');"><i class="fas fa-trash"></i> Delete</asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div style="text-align:center;padding:2rem;color:#888;">No staff members found.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div id="addStaffModal" class="modal-overlay" style="display:none;" onclick="if(event.target===this)hideAddModal()">
        <div class="modal">
            <h2>Add Staff Member</h2>
            <asp:Panel ID="pnlAddStaff" runat="server" DefaultButton="btnSaveStaff">
                <div class="form-group">
                    <label for="txtFirstName">First Name</label>
                    <asp:TextBox ID="txtFirstName" runat="server" placeholder="First name" />
                </div>
                <div class="form-group">
                    <label for="txtSurname">Surname</label>
                    <asp:TextBox ID="txtSurname" runat="server" placeholder="Surname" />
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" placeholder="Email address" TextMode="Email" />
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password" />
                </div>
                <div class="form-group">
                    <label for="ddlRole">Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server">
                        <asp:ListItem Text="Staff" Value="Staff" />
                        <asp:ListItem Text="Optometrist" Value="Optometrist" />
                        <asp:ListItem Text="Admin" Value="Admin" />
                    </asp:DropDownList>
                </div>
                <div class="modal-actions">
                    <button type="button" class="btn btn-secondary" onclick="hideAddModal()" style="background:#e0e0e0;color:#555;">Cancel</button>
                    <asp:Button ID="btnSaveStaff" runat="server" CssClass="btn btn-success" Text="Save" OnClick="btnSaveStaff_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>

    <script>
        function showAddModal() { document.getElementById('addStaffModal').style.display = 'flex'; }
        function hideAddModal() { document.getElementById('addStaffModal').style.display = 'none'; }
    </script>
</asp:Content>
