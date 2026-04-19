<%@ Page Title="My Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="Emonti_Optometrist_Website.Orders" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .orders-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
            animation: heroFadeIn 0.8s ease-out;
        }
        
        .orders-hero::before {
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
        
        .orders-hero h1 {
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
        
        .orders-hero p {
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
        
        .orders-container {
            max-width: 1200px;
            margin: -2rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .orders-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .orders-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 2rem;
            padding-bottom: 1rem;
            border-bottom: 2px solid #e0e0e0;
        }
        
        .orders-title {
            font-size: 1.8rem;
            color: #2c5aa0;
            font-weight: 600;
        }
        
        .search-section {
            margin-bottom: 2rem;
            padding: 1.5rem;
            background: #f8f9fa;
            border-radius: 10px;
        }
        
        .search-row {
            display: flex;
            gap: 1rem;
            align-items: end;
        }
        
        .search-group {
            flex: 1;
        }
        
        .search-group label {
            display: block;
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
        }
        
        .search-group input,
        .search-group select {
            width: 100%;
            padding: 0.75rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
        }
        
        .search-btn {
            background: #2c5aa0;
            color: white;
            border: none;
            padding: 0.75rem 1.5rem;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
        }
        
        .order-item {
            background: #f8f9fa;
            border: 1px solid #e0e0e0;
            border-radius: 10px;
            padding: 1.5rem;
            margin-bottom: 1rem;
            transition: all 0.3s ease;
        }
        
        .order-item:hover {
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
        }
        
        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
            padding-bottom: 0.5rem;
            border-bottom: 1px solid #e0e0e0;
        }
        
        .order-number {
            font-weight: 600;
            color: #2c5aa0;
            font-size: 1.1rem;
        }
        
        .order-date {
            color: #666;
        }
        
        .order-status {
            padding: 0.25rem 0.75rem;
            border-radius: 15px;
            font-size: 0.9rem;
            font-weight: 600;
        }
        
        .status-completed {
            background: #d4edda;
            color: #155724;
        }
        
        .status-pending {
            background: #fff3cd;
            color: #856404;
        }
        
        .status-processing {
            background: #cce5ff;
            color: #004085;
        }
        
        .status-cancelled {
            background: #f8d7da;
            color: #721c24;
        }
        
        .order-items {
            margin-bottom: 1rem;
        }
        
        .order-item-detail {
            display: flex;
            justify-content: space-between;
            margin-bottom: 0.5rem;
            padding: 0.5rem 0;
        }
        
        .order-total {
            font-weight: 600;
            color: #2c5aa0;
            font-size: 1.1rem;
            border-top: 1px solid #e0e0e0;
            padding-top: 0.5rem;
        }
        
        .order-actions {
            display: flex;
            gap: 0.5rem;
            margin-top: 1rem;
        }
        
        .action-btn {
            padding: 0.5rem 1rem;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 0.9rem;
            text-decoration: none;
            display: inline-block;
        }
        
        .btn-view {
            background: #2c5aa0;
            color: white;
        }
        
        .btn-reorder {
            background: #28a745;
            color: white;
        }
        
        .btn-cancel {
            background: #dc3545;
            color: white;
        }
        
        .no-orders {
            text-align: center;
            padding: 3rem;
            color: #666;
        }
        
        .no-orders i {
            font-size: 3rem;
            color: #ddd;
            margin-bottom: 1rem;
        }
        
        @media (max-width: 768px) {
            .orders-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 1rem;
            }
            
            .search-row {
                flex-direction: column;
            }
            
            .order-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 0.5rem;
            }
            
            .order-actions {
                flex-direction: column;
            }
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <!-- Hero Section -->
    <section class="orders-hero">
        <div class="container">
            <h1><i class="fas fa-shopping-bag"></i> My Orders</h1>
            <p>View and manage your order history</p>
        </div>
    </section>
    
    <div class="orders-container">
        <div class="orders-content">
            <div class="orders-header">
                <h2 class="orders-title">Order History</h2>
                <asp:Label ID="lblOrderCount" runat="server" CssClass="order-count" />
            </div>
            
            <!-- Search Section -->
            <div class="search-section">
                <div class="search-row">
                    <div class="search-group">
                        <label>Search by Order Number</label>
                        <asp:TextBox ID="txtSearchOrder" runat="server" placeholder="Enter order number..." />
                    </div>
                    <div class="search-group">
                        <label>Status Filter</label>
                        <asp:DropDownList ID="ddlStatusFilter" runat="server">
                            <asp:ListItem Text="All Orders" Value="" />
                            <asp:ListItem Text="Completed" Value="Completed" />
                            <asp:ListItem Text="Pending" Value="Pending" />
                            <asp:ListItem Text="Processing" Value="Processing" />
                            <asp:ListItem Text="Cancelled" Value="Cancelled" />
                        </asp:DropDownList>
                    </div>
                    <div class="search-group">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="search-btn" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
            
            <!-- Orders List -->
            <asp:Panel ID="pnlOrders" runat="server">
                <asp:Repeater ID="rptOrders" runat="server" OnItemDataBound="rptOrders_ItemDataBound">
                    <ItemTemplate>
                        <div class="order-item">
                            <div class="order-header">
                                <div>
                                    <span class="order-number">Order #<%# Eval("OrderNumber") %></span>
                                    <span class="order-date"> - <%# Eval("OrderDate", "{0:MMMM dd, yyyy}") %></span>
                                </div>
                                <span class="order-status status-<%# Eval("Status").ToString().ToLower() %>"><%# Eval("Status") %></span>
                            </div>
                            <div class="order-items">
                                <asp:Repeater ID="rptOrderItems" runat="server">
                                    <ItemTemplate>
                                        <div class="order-item-detail" style="display: flex; align-items: center; gap: 1rem; padding: 1rem; border: 1px solid #e0e0e0; border-radius: 8px; margin-bottom: 0.5rem; background: #f8f9fa;">
                                            <div style="flex-shrink: 0;">
                                                <asp:Image ID="imgProduct" runat="server" 
                                                     ImageUrl='<%# Eval("ProductImage") %>' 
                                                     AlternateText='<%# Eval("ProductName") %>' 
                                                     CssClass="order-item-image"
                                                     style="width: 60px; height: 60px; object-fit: cover; border-radius: 4px; border: 1px solid #ddd;" />
                                            </div>
                                            <div style="flex-grow: 1;">
                                                <div style="font-weight: 600; color: #333; margin-bottom: 0.25rem;"><%# Eval("ProductName") %></div>
                                                <div style="color: #666; font-size: 0.9rem;">Quantity: <%# Eval("Quantity") %></div>
                                            </div>
                                            <div style="flex-shrink: 0; font-weight: 600; color: #2c5aa0; font-size: 1.1rem;">
                                                R <%# Eval("Price", "{0:F2}") %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Panel ID="pnlNoItems" runat="server" Visible='<%# ((List<Emonti_Optometrist_Website.OrderItem>)Eval("Items")).Count == 0 %>'>
                                    <div class="order-item-detail" style="color: #666; font-style: italic; text-align: center; padding: 2rem;">
                                        <span>No items found for this order</span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="order-item-detail order-total">
                                <span>Total:</span>
                                <span>R <%# Eval("Total", "{0:F2}") %></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
            
            <!-- No Orders Message -->
            <asp:Panel ID="pnlNoOrders" runat="server" Visible="false">
                <div class="no-orders">
                    <i class="fas fa-shopping-bag"></i>
                    <h3>No Orders Found</h3>
                    <p>You haven't placed any orders yet.</p>
                    <asp:LinkButton ID="btnStartShopping" runat="server" CssClass="action-btn btn-view" 
                        OnClick="btnStartShopping_Click">
                        <i class="fas fa-shopping-cart"></i> Start Shopping
                    </asp:LinkButton>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
