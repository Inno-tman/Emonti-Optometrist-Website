<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="Emonti_Optometrist_Website.Admin.ManageProducts" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Theme colors match existing blue/purple gradient */
        .admin-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 6rem 2rem 3rem;
            text-align: center;
            margin-top: 0;
        }
        .admin-container {
            max-width: 1400px;
            margin: -2rem auto 3rem;
            padding: 0 2rem;
        }
        .admin-card {
            background: white;
            border-radius: 20px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            padding: 2rem;
            margin-bottom: 2rem;
        }
        .section-title {
            font-size: 1.8rem;
            color: #2c5aa0;
            border-bottom: 3px solid #667eea;
            display: inline-block;
            margin-bottom: 1.5rem;
            padding-bottom: 0.5rem;
        }
        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 1.5rem;
            margin-bottom: 1.5rem;
        }
        .form-group label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
            display: block;
        }
        .form-control {
            width: 100%;
            padding: 0.75rem;
            border: 2px solid #e0e0e0;
            border-radius: 12px;
            font-size: 1rem;
        }
        .form-control:focus {
            border-color: #667eea;
            outline: none;
        }
        .btn-primary {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 30px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
        }
        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(44,90,160,0.3);
        }
        .btn-danger {
            background: #dc3545;
            color: white;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 20px;
            cursor: pointer;
        }
        .btn-warning {
            background: #ffc107;
            color: #212529;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 20px;
            cursor: pointer;
        }
        .products-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
            gap: 1.5rem;
            margin-top: 2rem;
        }
        .product-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.08);
            overflow: hidden;
            transition: all 0.3s;
        }
        .product-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 15px 30px rgba(0,0,0,0.15);
        }
        .product-img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            background: #f0f0f0;
        }
        .product-info {
            padding: 1rem;
        }
        .product-title {
            font-size: 1.2rem;
            font-weight: 700;
            color: #2c5aa0;
            margin-bottom: 0.5rem;
        }
        .product-price {
            font-weight: 600;
            color: #28a745;
        }
        .card-actions {
            display: flex;
            gap: 0.5rem;
            margin-top: 1rem;
        }
        .alert {
            padding: 1rem;
            border-radius: 10px;
            margin-bottom: 1rem;
        }
        .alert-success { background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }
        .alert-error { background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }
        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 1000;
            align-items: center;
            justify-content: center;
        }
        .modal-content {
            background: white;
            max-width: 500px;
            width: 90%;
            border-radius: 20px;
            padding: 2rem;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="admin-hero">
        <h1><i class="fas fa-boxes"></i> Manage Products</h1>
        <p>Add, edit, delete and view your product inventory</p>
    </section>

    <div class="admin-container">
        <!-- Add/Edit Form -->
        <div class="admin-card">
            <h2 class="section-title" id="formTitle">Add New Product</h2>
            <asp:Panel ID="pnlForm" runat="server">
                <asp:HiddenField ID="hfProductId" runat="server" />
                <div class="form-row">
                    <div class="form-group">
                        <label>Product Name *</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" required="true" />
                    </div>
                    <div class="form-group">
                        <label>Brand</label>
                        <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Price (R) *</label>
                        <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" required="true" />
                    </div>
                    <div class="form-group">
                        <label>Stock Quantity</label>
                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Frames" Value="Frames" />
                            <asp:ListItem Text="Lenses" Value="Lenses" />
                            <asp:ListItem Text="Accessories" Value="Accessories" />
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Product Image</label>
                        <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" accept="image/*" />
                        <asp:Label ID="lblCurrentImage" runat="server" ForeColor="Gray" Font-Size="0.8rem" />
                    </div>
                </div>
                <div class="form-group">
                    <label>Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" />
                </div>
                <div style="display: flex; gap: 1rem; margin-top: 1rem;">
                    <asp:Button ID="btnSave" runat="server" Text="Save Product" CssClass="btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-primary" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>
            </asp:Panel>
        </div>

        <!-- Products List -->
        <div class="admin-card">
            <h2 class="section-title">Product Inventory</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />
            <div class="products-grid">
                <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
                    <ItemTemplate>
                        <div class="product-card">
                            <asp:Image ID="imgProduct" runat="server" CssClass="product-img" ImageUrl='<%# Eval("ImageUrl") %>' />
                            <div class="product-info">
                                <div class="product-title"><%# Eval("Name") %></div>
                                <div class="product-price">R <%# Eval("Price", "{0:F2}") %></div>
                                <div>Brand: <%# Eval("Brand") %></div>
                                <div>Stock: <%# Eval("Stock") %></div>
                                <div class="card-actions">
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditProduct" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn-warning">Edit</asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductId") %>' CssClass="btn-danger" OnClientClick="return confirm('Delete this product?');">Delete</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</asp:Content>