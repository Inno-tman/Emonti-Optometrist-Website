<%@ Page Title="Manage Staff" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStaff.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageStaff" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-section { padding: 2rem; max-width: 1100px; margin: 0 auto; }
        .page-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1.5rem; flex-wrap: wrap; gap: 1rem; }
        .page-header h1 { margin: 0; font-size: 1.4rem; font-weight: 700; color: #1a2332; }
        .btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.6rem 1.25rem; border-radius: 50px; border: none; cursor: pointer; font-size: 0.85rem; font-weight: 600; transition: all 0.2s; text-decoration: none; }
        .btn-primary { background: #4f6ef7; color: #fff; }
        .btn-primary:hover { background: #3d5bd9; }
        .btn-sm { padding: 0.4rem 1rem; font-size: 0.78rem; }
        .btn-danger { background: #dc3545; color: #fff; }
        .btn-danger:hover { background: #c82333; }
        .btn-success { background: #22a45a; color: #fff; }
        .btn-success:hover { background: #1b8a4b; }
        .btn-secondary { background: #e8eaed; color: #4a5568; }
        .btn-secondary:hover { background: #d1d5db; }
        .staff-table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 14px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
        .staff-table th { background: #f8f9fc; padding: 0.85rem 1rem; text-align: left; font-weight: 600; color: #6b7a8a; font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.5px; }
        .staff-table td { padding: 0.85rem 1rem; border-top: 1px solid #f0f2f5; color: #2d3748; font-size: 0.88rem; }
        .staff-table tr:hover td { background: #f8f9ff; }
        .role-badge { display: inline-block; padding: 0.2rem 0.7rem; border-radius: 50px; font-size: 0.72rem; font-weight: 600; }
        .role-Admin { background: #eef2ff; color: #4f6ef7; }
        .role-Staff { background: #f0f2f5; color: #6b7a8a; }
        .role-Optometrist { background: #e8f8ee; color: #22a45a; }
        .action-cell { display: flex; gap: 0.4rem; flex-wrap: wrap; }
        .alert { padding: 0.85rem 1.25rem; border-radius: 10px; margin-bottom: 1.25rem; font-weight: 500; font-size: 0.88rem; }
        .alert-success { background: #e8f8ee; color: #1a7a3a; border: 1px solid #c3e6cb; }
        .alert-danger { background: #fef0ee; color: #b83a2a; border: 1px solid #f5c6cb; }
        .modal-overlay { position: fixed; inset: 0; background: rgba(0,0,0,0.4); display: flex; align-items: center; justify-content: center; z-index: 1000; }
        .modal { background: #fff; border-radius: 18px; padding: 2rem; width: 90%; max-width: 480px; box-shadow: 0 20px 60px rgba(0,0,0,0.2); }
        .modal h2 { margin: 0 0 1.25rem; font-size: 1.15rem; color: #1a2332; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; font-weight: 600; color: #4a5568; margin-bottom: 0.3rem; font-size: 0.82rem; }
        .form-group input, .form-group select { width: 100%; padding: 0.65rem 1rem; border: 2px solid #e2e6ea; border-radius: 10px; font-size: 0.88rem; transition: border-color 0.2s; box-sizing: border-box; }
        .form-group input:focus, .form-group select:focus { border-color: #4f6ef7; outline: none; box-shadow: 0 0 0 3px rgba(79,110,247,0.1); }
        .modal-actions { display: flex; gap: 0.75rem; justify-content: flex-end; margin-top: 1.5rem; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-section">
        <div class="page-header">
            <h1><i class="fas fa-users-cog" style="color:#4f6ef7;margin-right:0.5rem;"></i>Manage Staff</h1>
            <button type="button" class="btn btn-primary" onclick="showAddModal()"><i class="fas fa-plus"></i> Add Staff</button>
        </div>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />
        <asp:GridView ID="gvStaff" runat="server" CssClass="staff-table" AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvStaff_RowCommand">
            <Columns>
                <asp:BoundField DataField="Staff_ID" HeaderText="ID" />
                <asp:BoundField DataField="Staff_Name" HeaderText="Name" />
                <asp:BoundField DataField="Staff_Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Staff_Email" HeaderText="Email" />
                <asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label ID="lblRoleBadge" runat="server" CssClass='<%# "role-badge role-" + Eval("Staff_Role").ToString() %>' Text='<%# Eval("Staff_Role") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <div class="action-cell">
                            <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-sm btn-danger" CommandName="DeleteStaff" CommandArgument='<%# Eval("Staff_ID") %>' OnClientClick="return confirm('Are you sure you want to delete this staff member?');"><i class="fas fa-trash"></i> Delete</asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div style="text-align:center;padding:2rem;color:#8a9aaa;">No staff members found.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div id="addStaffModal" class="modal-overlay" style="display:none;" onclick="if(event.target===this)hideAddModal()">
        <div class="modal">
            <h2><i class="fas fa-user-plus" style="color:#4f6ef7;margin-right:0.4rem;"></i>Add Staff Member</h2>
            <asp:Panel ID="pnlAddStaff" runat="server" DefaultButton="btnSaveStaff">
                <div class="form-group">
                    <label for="txtFirstName">First Name</label>
                    <asp:TextBox ID="txtFirstName" runat="server" placeholder="Enter first name" />
                </div>
                <div class="form-group">
                    <label for="txtSurname">Surname</label>
                    <asp:TextBox ID="txtSurname" runat="server" placeholder="Enter surname" />
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter email address" TextMode="Email" />
                </div>
                <div class="form-group">
                    <label for="txtPassword">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" placeholder="Enter password" TextMode="Password" />
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
                    <button type="button" class="btn btn-secondary" onclick="hideAddModal()">Cancel</button>
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
