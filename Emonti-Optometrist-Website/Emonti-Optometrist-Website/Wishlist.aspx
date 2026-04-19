<%@ Page Title="Wishlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs" Inherits="Emonti_Optometrist_Website.Wishlist" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .wishlist-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
            animation: heroFadeIn 0.8s ease-out;
        }
        
        .wishlist-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(circle at 20% 50%, rgba(255,255,255,0.1) 0%, transparent 50%);
        }
        
        @keyframes heroFadeIn {
            from { opacity: 0; transform: translateY(-20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .wishlist-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
            font-weight: 700;
            position: relative;
            z-index: 1;
            animation: slideInDown 0.8s ease-out 0.2s both;
        }
        
        @keyframes slideInDown {
            from { opacity: 0; transform: translateY(-30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .wishlist-hero p {
            font-size: 1.2rem;
            max-width: 600px;
            margin: 0 auto;
            position: relative;
            z-index: 1;
            animation: slideInUp 0.8s ease-out 0.4s both;
            opacity: 0.95;
        }
        
        @keyframes slideInUp {
            from { opacity: 0; transform: translateY(30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .wishlist-container {
            max-width: 1200px;
            margin: -2rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .wishlist-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .wishlist-item {
            display: flex;
            align-items: center;
            padding: 2rem;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            margin-bottom: 1.5rem;
            transition: all 0.3s ease;
        }
        
        .wishlist-item:hover {
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
            transform: translateY(-2px);
        }
        
        .item-image {
            width: 120px;
            height: 120px;
            margin-right: 2rem;
            border-radius: 8px;
            overflow: hidden;
        }
        
        .item-image img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
        
        .item-details {
            flex: 1;
        }
        
        .item-details h3 {
            font-size: 1.5rem;
            margin-bottom: 0.5rem;
            color: #333;
        }
        
        .item-details p {
            color: #666;
            margin-bottom: 0.5rem;
        }
        
        .item-price {
            font-size: 1.5rem;
            font-weight: bold;
            color: #2c5aa0;
            margin-bottom: 1rem;
        }
        
        .item-actions {
            display: flex;
            gap: 1rem;
        }
        
        .btn-add-to-cart {
            background: #2c5aa0;
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 5px;
            cursor: pointer;
            transition: background 0.3s ease;
        }
        
        .btn-add-to-cart:hover {
            background: #1e3f73;
        }
        
        .btn-remove {
            background: #dc3545;
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 5px;
            cursor: pointer;
            transition: background 0.3s ease;
        }
        
        .btn-remove:hover {
            background: #c82333;
        }
        
        .empty-wishlist {
            text-align: center;
            padding: 4rem 2rem;
        }
        
        .empty-wishlist h3 {
            font-size: 2rem;
            margin-bottom: 1rem;
            color: #666;
        }
        
        .empty-wishlist p {
            font-size: 1.1rem;
            color: #888;
            margin-bottom: 2rem;
        }
        
        .btn-continue-shopping {
            background: #2c5aa0;
            color: white;
            text-decoration: none;
            padding: 1rem 2rem;
            border-radius: 5px;
            display: inline-block;
            transition: background 0.3s ease;
        }
        
        .btn-continue-shopping:hover {
            background: #1e3f73;
            color: white;
            text-decoration: none;
        }
        
        .wishlist-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 2rem;
            padding-bottom: 1rem;
            border-bottom: 2px solid #e0e0e0;
        }
        
        .wishlist-title {
            font-size: 2rem;
            color: #333;
        }
        
        .wishlist-count {
            color: #666;
            font-size: 1.1rem;
        }
        
        .item-stock-status {
            font-size: 0.9rem;
            font-weight: 600;
            margin-bottom: 0.5rem;
            display: inline-block;
            padding: 0.4rem 0.8rem;
            border-radius: 5px;
        }
        
        /* In stock - green */
        .item-stock-status.in-stock {
            color: #28a745;
            background: rgba(40, 167, 69, 0.1);
        }
        
        /* Limited stock - orange */
        .item-stock-status.limited-stock {
            color: #ff9800;
            background: rgba(255, 152, 0, 0.1);
        }
        
        /* Out of stock - red */
        .item-stock-status.out-of-stock {
            color: #dc3545;
            background: rgba(220, 53, 69, 0.1);
        }
        
        .item-color {
            color: #666;
            margin-bottom: 0.5rem;
        }
        
        .item-color strong {
            color: #333;
        }
        
        .btn-add-to-cart:disabled,
        .btn-add-to-cart.btn-disabled {
            background: #6c757d;
            color: white;
            cursor: not-allowed;
            opacity: 0.6;
        }
        
        .btn-add-to-cart:disabled:hover,
        .btn-add-to-cart.btn-disabled:hover {
            background: #6c757d;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="wishlist-hero">
        <div class="container">
            <h1><i class="fas fa-heart"></i> My Wishlist</h1>
            <p>Save your favorite products for later</p>
        </div>
    </section>

    <div class="wishlist-container">
        <div class="wishlist-content">
            <!-- Wishlist Header -->
            <div class="wishlist-header">
                <h2 class="wishlist-title">My Wishlist</h2>
                <asp:Label ID="lblWishlistCount" runat="server" CssClass="wishlist-count"></asp:Label>
            </div>

            <!-- Wishlist Items -->
            <asp:Panel ID="pnlWishlistItems" runat="server" Visible="false">
                <asp:Repeater ID="rptWishlistItems" runat="server" OnItemCommand="rptWishlistItems_ItemCommand" OnItemDataBound="rptWishlistItems_ItemDataBound">
                    <ItemTemplate>
                        <asp:HiddenField ID="hfWishlistItemId" runat="server" Value='<%# Eval("WishlistItemId") %>' />
                        <asp:HiddenField ID="hfProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                        <div class="wishlist-item">
                            <div class="item-image">
                                <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' AlternateText='<%# Eval("ProductName") %>' />
                            </div>
                            <div class="item-details">
                                <h3><%# Eval("ProductName") %></h3>
                                <p><strong>Brand:</strong> <%# Eval("Brand") %></p>
                                <p><strong>Category:</strong> <%# Eval("Category") %></p>
                                <asp:Label ID="lblStockStatus" runat="server" CssClass="item-stock-status" />
                                <p><strong>Added:</strong> <%# Eval("AddedAt", "{0:MMM dd, yyyy}") %></p>
                                <div class="item-price">R <%# Eval("Price", "{0:F2}") %></div>
                            </div>
                            <div class="item-actions">
                                <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn-add-to-cart" 
                                    CommandName="AddToCart" CommandArgument='<%# Eval("ProductId") %>' />
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn-remove" 
                                    CommandName="RemoveFromWishlist" CommandArgument='<%# Eval("WishlistItemId") %>' />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>

            <!-- Empty Wishlist -->
            <asp:Panel ID="pnlEmptyWishlist" runat="server" Visible="false">
                <div class="empty-wishlist">
                    <h3><i class="fas fa-heart-broken"></i> Your wishlist is empty</h3>
                    <p>Start adding products you love to your wishlist!</p>
                    <a href="Shop.aspx" class="btn-continue-shopping">Continue Shopping</a>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>


