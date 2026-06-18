<%@ Page Title="Manage Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCustomers.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageCustomers" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-section { padding: 2rem; max-width: 1100px; margin: 0 auto; }
        .page-header { margin-bottom: 1.5rem; }
        .page-header h1 { margin: 0 0 1rem; font-size: 1.4rem; font-weight: 700; color: #1a2332; }
        .search-bar { display: flex; gap: 0.5rem; }
        .search-bar input { flex: 1; padding: 0.65rem 1rem; border: 2px solid #e2e6ea; border-radius: 10px; font-size: 0.88rem; transition: border-color 0.2s; }
        .search-bar input:focus { border-color: #4f6ef7; outline: none; box-shadow: 0 0 0 3px rgba(79,110,247,0.1); }
        .btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.6rem 1.25rem; border-radius: 50px; border: none; cursor: pointer; font-size: 0.85rem; font-weight: 600; transition: all 0.2s; text-decoration: none; }
        .btn-primary { background: #4f6ef7; color: #fff; }
        .btn-primary:hover { background: #3d5bd9; }
        .btn-sm { padding: 0.4rem 1rem; font-size: 0.78rem; }
        .customer-table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 14px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
        .customer-table th { background: #f8f9fc; padding: 0.85rem 1rem; text-align: left; font-weight: 600; color: #6b7a8a; font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.5px; }
        .customer-table td { padding: 0.85rem 1rem; border-top: 1px solid #f0f2f5; color: #2d3748; font-size: 0.88rem; }
        .customer-table tr:hover td { background: #f8f9ff; }
        .detail-row td { padding: 0; }
        .detail-content { padding: 1rem 2rem 1.25rem; background: #f8f9fc; border-top: 1px solid #e2e6ea; font-size: 0.88rem; line-height: 1.6; }
        .detail-content p { margin: 0.25rem 0; color: #4a5568; }
        .detail-content strong { color: #2d3748; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-section">
        <div class="page-header">
            <h1><i class="fas fa-user-friends" style="color:#22a45a;margin-right:0.5rem;"></i>Manage Customers</h1>
            <div class="search-bar">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name, email, or phone..." />
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="btnSearch_Click"><i class="fas fa-search"></i> Search</asp:LinkButton>
            </div>
        </div>
        <asp:GridView ID="gvCustomers" runat="server" CssClass="customer-table" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="20" OnPageIndexChanging="gvCustomers_PageIndexChanging" OnRowDataBound="gvCustomers_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Cust_ID" HeaderText="ID" />
                <asp:BoundField DataField="Customer_Name" HeaderText="First Name" />
                <asp:BoundField DataField="Customer_Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Customer_Email" HeaderText="Email" />
                <asp:BoundField DataField="Customer_Phone" HeaderText="Phone" />
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <button type="button" class="btn btn-sm btn-primary" onclick="toggleDetail(this)"><i class="fas fa-eye"></i></button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div style="text-align:center;padding:2rem;color:#8a9aaa;">No customers found.</div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <script>
        function toggleDetail(btn) {
            var row = btn.closest('tr').nextElementSibling;
            if (row && row.classList.contains('detail-row')) {
                var expanded = row.style.display !== 'table-row';
                document.querySelectorAll('.detail-row').forEach(function(r) { r.style.display = 'none'; });
                row.style.display = expanded ? 'table-row' : 'none';
            }
        }
    </script>
</asp:Content>
