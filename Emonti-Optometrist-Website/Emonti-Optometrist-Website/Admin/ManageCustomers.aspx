<%@ Page Title="Manage Customers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCustomers.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageCustomers" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .admin-section { padding: 2rem; max-width: 1200px; margin: 0 auto; }
        .back-link { display: inline-flex; align-items: center; gap: 0.5rem; color: #667eea; text-decoration: none; margin-bottom: 1rem; font-weight: 500; }
        .back-link:hover { text-decoration: underline; }
        .page-header { margin-bottom: 2rem; }
        .page-header h1 { margin: 0 0 1rem; font-size: 1.5rem; }
        .search-bar { display: flex; gap: 0.5rem; }
        .search-bar input { flex: 1; padding: 0.7rem 1rem; border: 2px solid #e0e0e0; border-radius: 12px; font-size: 0.9rem; }
        .search-bar input:focus { border-color: #667eea; outline: none; box-shadow: 0 0 0 4px rgba(102,126,234,0.12); }
        .customer-table { width: 100%; border-collapse: collapse; background: #fff; border-radius: 16px; overflow: hidden; box-shadow: 0 4px 20px rgba(0,0,0,0.08); }
        .customer-table th { background: #f8f9fa; padding: 1rem; text-align: left; font-weight: 600; color: #555; font-size: 0.85rem; text-transform: uppercase; letter-spacing: 0.5px; }
        .customer-table td { padding: 1rem; border-top: 1px solid #f0f0f0; color: #333; font-size: 0.9rem; }
        .customer-table tr:hover td { background: #f8f9ff; }
        .detail-row { display: none; }
        .detail-row td { padding: 0; }
        .detail-content { padding: 1rem 2rem 1.5rem; background: #f8f9ff; border-top: 1px solid #e0e0e0; }
        .detail-content p { margin: 0.3rem 0; font-size: 0.9rem; color: #555; }
        .detail-content strong { color: #333; }
        .btn { display: inline-flex; align-items: center; gap: 0.4rem; padding: 0.4rem 1rem; border-radius: 50px; border: none; cursor: pointer; font-size: 0.8rem; font-weight: 600; transition: all 0.2s; background: #667eea; color: #fff; text-decoration: none; }
        .btn:hover { opacity: 0.85; }
        .badge { display: inline-block; padding: 0.2rem 0.6rem; border-radius: 50px; font-size: 0.7rem; font-weight: 600; background: #e8f4fd; color: #2196F3; }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="admin-section">
        <a href="Dashboard.aspx" class="back-link"><i class="fas fa-arrow-left"></i> Back to Dashboard</a>
        <div class="page-header">
            <h1>Manage Customers</h1>
            <div class="search-bar">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by name, email, or phone..." />
                <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click"><i class="fas fa-search"></i> Search</asp:LinkButton>
            </div>
        </div>
        <asp:GridView ID="gvCustomers" runat="server" CssClass="customer-table" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" PageSize="20" OnPageIndexChanging="gvCustomers_PageIndexChanging" OnRowDataBound="gvCustomers_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Cust_ID" HeaderText="ID" />
                <asp:BoundField DataField="Customer_Name" HeaderText="First Name" />
                <asp:BoundField DataField="Customer_Surname" HeaderText="Surname" />
                <asp:BoundField DataField="Customer_Email" HeaderText="Email" />
                <asp:BoundField DataField="Customer_Phone" HeaderText="Phone" />
                <asp:TemplateField HeaderText="Details">
                    <ItemTemplate>
                        <button type="button" class="btn" onclick="toggleDetail(this)"><i class="fas fa-eye"></i> View</button>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div style="text-align:center;padding:2rem;color:#888;">No customers found.</div>
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
