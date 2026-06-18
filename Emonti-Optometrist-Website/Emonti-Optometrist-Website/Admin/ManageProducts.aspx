<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageProducts" %>

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
.btn-warning { background: #f6ad55; color: #fff; }
.btn-warning:hover { background: #ed8936; }
.btn-sm { padding: 0.35rem 0.7rem; font-size: 0.78rem; }
.section-card { background: #fff; border-radius: 12px; box-shadow: 0 2px 8px rgba(0,0,0,0.06); overflow: hidden; }
.section-header { padding: 1rem 1.25rem; border-bottom: 1px solid #f0f0f0; display: flex; align-items: center; justify-content: space-between; gap: 0.5rem; }
.section-header h2 { font-size: 1rem; font-weight: 700; color: #1a1d23; }
.section-header h2 i { color: #667eea; }
.search-bar { display: flex; gap: 0.5rem; flex: 1; max-width: 320px; }
.search-bar input { flex: 1; padding: 0.5rem 0.75rem; border: 2px solid #e2e8f0; border-radius: 8px; font-size: 0.85rem; }
.search-bar input:focus { outline: none; border-color: #667eea; }
table { width: 100%; border-collapse: collapse; }
thead { background: #f8f9fa; }
th { padding: 0.75rem 1rem; text-align: left; font-size: 0.75rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; color: #888; }
td { padding: 0.7rem 1rem; font-size: 0.85rem; color: #333; border-bottom: 1px solid #f5f5f5; vertical-align: middle; }
tr:hover td { background: rgba(102,126,234,0.02); }
.product-img { width: 40px; height: 40px; object-fit: cover; border-radius: 6px; }
.empty-state { text-align: center; padding: 3rem 1rem; color: #999; }
.empty-state i { font-size: 2.5rem; margin-bottom: 0.75rem; color: #ddd; }
.modal-overlay { display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(0,0,0,0.5); z-index: 1000; justify-content: center; align-items: center; }
.modal-overlay.show { display: flex; }
.modal { background: #fff; border-radius: 16px; padding: 2rem; width: 100%; max-width: 520px; max-height: 90vh; overflow-y: auto; box-shadow: 0 20px 60px rgba(0,0,0,0.3); }
.modal h3 { font-size: 1.15rem; margin-bottom: 1.25rem; color: #1a1d23; }
.modal .form-group { margin-bottom: 1rem; }
.modal .form-group label { display: block; font-size: 0.82rem; font-weight: 600; color: #555; margin-bottom: 0.3rem; }
.modal .form-group input, .modal .form-group select, .modal .form-group textarea { width: 100%; padding: 0.6rem 0.8rem; border: 2px solid #e2e8f0; border-radius: 8px; font-size: 0.9rem; transition: border-color 0.2s; font-family: inherit; }
.modal .form-group input:focus, .modal .form-group select:focus, .modal .form-group textarea:focus { outline: none; border-color: #667eea; }
.modal .form-group textarea { min-height: 80px; resize: vertical; }
.form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem; }
.modal-actions { display: flex; gap: 0.75rem; margin-top: 1.5rem; justify-content: flex-end; }
.modal-actions .btn-secondary { background: #e2e8f0; color: #4a5568; }
.modal-actions .btn-secondary:hover { background: #cbd5e0; }
.admin-footer { text-align: center; padding: 1.5rem; color: #999; font-size: 0.8rem; margin-top: 2rem; }
@media (max-width: 768px) { .admin-sidebar { width: 60px; } .sidebar-brand h2, .sidebar-brand small, .sidebar-nav li a span { display: none; } .sidebar-nav li a { justify-content: center; padding: 0.75rem; } .admin-main { margin-left: 60px; padding: 1rem; } .form-row { grid-template-columns: 1fr; } }
</style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="admin-wrapper">
    <aside class="admin-sidebar">
        <div class="sidebar-brand"><h2>EMONTI</h2><small>Admin Panel</small></div>
        <ul class="sidebar-nav">
            <li><a href="Dashboard.aspx"><i class="fas fa-tachometer-alt"></i><span>Dashboard</span></a></li>
            <li><a href="ManageOrders.aspx"><i class="fas fa-shopping-cart"></i><span>Orders</span></a></li>
            <li><a href="ManageProducts.aspx" class="active"><i class="fas fa-box"></i><span>Products</span></a></li>
            <li><a href="ManageCustomers.aspx"><i class="fas fa-address-book"></i><span>Customers</span></a></li>
            <li><a href="ManageStaff.aspx"><i class="fas fa-users"></i><span>Staff</span></a></li>
            <li><a href="QueryDb.aspx"><i class="fas fa-database"></i><span>Query DB</span></a></li>
            <li><a href="../Reports.aspx"><i class="fas fa-chart-bar"></i><span>Reports</span></a></li>
            <li class="divider"><a href="../Account/Logout.aspx" class="logout"><i class="fas fa-sign-out-alt"></i><span>Logout</span></a></li>
        </ul>
    </aside>
    <main class="admin-main">
        <div class="admin-header">
            <h1><i class="fas fa-box"></i> Manage Products</h1>
            <div style="display:flex;gap:0.5rem;align-items:center;">
                <div class="search-bar">
                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search products..." />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary btn-sm" OnClick="btnSearch_Click" />
                </div>
                <button class="btn btn-success" onclick="document.getElementById('addModal').classList.add('show'); return false;"><i class="fas fa-plus"></i> Add Product</button>
            </div>
        </div>
        <asp:Panel ID="pnlMessage" runat="server" Visible="false" style="padding:0.75rem 1rem;border-radius:8px;margin-bottom:1rem;font-size:0.85rem;"></asp:Panel>
        <div class="section-card">
            <div class="section-header"><i class="fas fa-box" style="color:#667eea;"></i><h2>All Products</h2></div>
            <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" GridLines="None" ShowHeaderWhenEmpty="true" DataKeyNames="Product_ID" OnRowDeleting="gvProducts_RowDeleting" OnRowEditing="gvProducts_RowEditing">
                <Columns>
                    <asp:TemplateField HeaderText="Image">
                        <ItemTemplate><img class="product-img" src='<%# Eval("Picture1") %>' alt='<%# Eval("Product_Name") %>' onerror="this.style.display='none'" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Product_Name" HeaderText="Name" />
                    <asp:BoundField DataField="Product_Brand" HeaderText="Brand" />
                    <asp:BoundField DataField="Product_Price" HeaderText="Price" DataFormatString="R {0:N2}" />
                    <asp:BoundField DataField="QuantityOnHand" HeaderText="Stock" />
                    <asp:BoundField DataField="Product_Category" HeaderText="Category" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="Edit" CssClass="btn btn-warning btn-sm" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Delete this product?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate><div class="empty-state"><i class="fas fa-box-open"></i><p>No products found.</p></div></EmptyDataTemplate>
            </asp:GridView>
        </div>
    </main>
</div>

<!-- Add Product Modal -->
<div id="addModal" class="modal-overlay">
    <div class="modal">
        <h3><i class="fas fa-plus-circle" style="color:#667eea;"></i> Add Product</h3>
        <div class="form-group"><label>Product Name</label><asp:TextBox ID="txtAddName" runat="server" /></div>
        <div class="form-group"><label>Brand</label><asp:TextBox ID="txtAddBrand" runat="server" /></div>
        <div class="form-row">
            <div class="form-group"><label>Price (R)</label><asp:TextBox ID="txtAddPrice" runat="server" TextMode="Number" step="0.01" /></div>
            <div class="form-group"><label>Stock</label><asp:TextBox ID="txtAddStock" runat="server" TextMode="Number" /></div>
        </div>
        <div class="form-group"><label>Category</label><asp:DropDownList ID="ddlAddCategory" runat="server">
            <asp:ListItem Text="Select Category" Value="" />
            <asp:ListItem Text="Eyeglasses" Value="Eyeglasses" />
            <asp:ListItem Text="Sunglasses" Value="Sunglasses" />
            <asp:ListItem Text="Contact Lenses" Value="Contact Lenses" />
            <asp:ListItem Text="Accessories" Value="Accessories" />
            <asp:ListItem Text="Eye Drops" Value="Eye Drops" />
            <asp:ListItem Text="Other" Value="Other" />
        </asp:DropDownList></div>
        <div class="form-group"><label>Description</label><asp:TextBox ID="txtAddDesc" runat="server" TextMode="MultiLine" /></div>
        <div class="form-group"><label>Image URL</label><asp:TextBox ID="txtAddImage" runat="server" /></div>
        <asp:Label ID="lblAddError" runat="server" ForeColor="Red" Visible="false" style="font-size:0.85rem;" />
        <div class="modal-actions">
            <button class="btn btn-secondary" onclick="document.getElementById('addModal').classList.remove('show'); return false;">Cancel</button>
            <asp:Button ID="btnAddProduct" runat="server" Text="Add Product" CssClass="btn btn-primary" OnClick="btnAddProduct_Click" />
        </div>
    </div>
</div>

<!-- Edit Product Modal -->
<div id="editModal" class="modal-overlay">
    <div class="modal">
        <h3><i class="fas fa-edit" style="color:#667eea;"></i> Edit Product</h3>
        <asp:HiddenField ID="hfEditId" runat="server" />
        <div class="form-group"><label>Product Name</label><asp:TextBox ID="txtEditName" runat="server" /></div>
        <div class="form-group"><label>Brand</label><asp:TextBox ID="txtEditBrand" runat="server" /></div>
        <div class="form-row">
            <div class="form-group"><label>Price (R)</label><asp:TextBox ID="txtEditPrice" runat="server" TextMode="Number" step="0.01" /></div>
            <div class="form-group"><label>Stock</label><asp:TextBox ID="txtEditStock" runat="server" TextMode="Number" /></div>
        </div>
        <div class="form-group"><label>Category</label><asp:DropDownList ID="ddlEditCategory" runat="server">
            <asp:ListItem Text="Select Category" Value="" />
            <asp:ListItem Text="Eyeglasses" Value="Eyeglasses" />
            <asp:ListItem Text="Sunglasses" Value="Sunglasses" />
            <asp:ListItem Text="Contact Lenses" Value="Contact Lenses" />
            <asp:ListItem Text="Accessories" Value="Accessories" />
            <asp:ListItem Text="Eye Drops" Value="Eye Drops" />
            <asp:ListItem Text="Other" Value="Other" />
        </asp:DropDownList></div>
        <div class="form-group"><label>Description</label><asp:TextBox ID="txtEditDesc" runat="server" TextMode="MultiLine" /></div>
        <div class="form-group"><label>Image URL</label><asp:TextBox ID="txtEditImage" runat="server" /></div>
        <asp:Label ID="lblEditError" runat="server" ForeColor="Red" Visible="false" style="font-size:0.85rem;" />
        <div class="modal-actions">
            <button class="btn btn-secondary" onclick="document.getElementById('editModal').classList.remove('show'); return false;">Cancel</button>
            <asp:Button ID="btnEditProduct" runat="server" Text="Save Changes" CssClass="btn btn-primary" OnClick="btnEditProduct_Click" />
        </div>
    </div>
</div>

<div class="admin-footer">&copy; 2026 Emonti Optometrist Admin Panel</div>
</asp:Content>
