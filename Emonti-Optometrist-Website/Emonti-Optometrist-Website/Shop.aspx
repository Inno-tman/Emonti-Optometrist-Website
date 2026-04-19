<%@ Page Title="Shop" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="Emonti_Optometrist_Website.Shop" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function showSuccessMessage(message) {
            // Create a toast notification
            var toast = document.createElement('div');
            toast.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: #28a745;
                color: white;
                padding: 15px 20px;
                border-radius: 5px;
                box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                z-index: 9999;
                font-weight: 500;
                animation: slideIn 0.3s ease-out;
            `;
            toast.textContent = message;
            document.body.appendChild(toast);
            
            // Remove after 3 seconds
            setTimeout(function() {
                toast.style.animation = 'slideOut 0.3s ease-in';
                setTimeout(function() {
                    document.body.removeChild(toast);
                }, 300);
            }, 3000);
        }
        
        function showErrorMessage(message) {
            // Create a toast notification
            var toast = document.createElement('div');
            toast.style.cssText = `
                position: fixed;
                top: 20px;
                right: 20px;
                background: #dc3545;
                color: white;
                padding: 15px 20px;
                border-radius: 5px;
                box-shadow: 0 4px 12px rgba(0,0,0,0.15);
                z-index: 9999;
                font-weight: 500;
                animation: slideIn 0.3s ease-out;
            `;
            toast.textContent = message;
            document.body.appendChild(toast);
            
            // Remove after 3 seconds
            setTimeout(function() {
                toast.style.animation = 'slideOut 0.3s ease-in';
                setTimeout(function() {
                    document.body.removeChild(toast);
                }, 300);
            }, 3000);
        }
        
        function updateCartCounter() {
            // This will be handled by the master page
            // The cart counter will be updated on next page load
        }
        
        // Add CSS animations
        var style = document.createElement('style');
        style.textContent = `
            @keyframes slideIn {
                from { transform: translateX(100%); opacity: 0; }
                to { transform: translateX(0); opacity: 1; }
            }
            @keyframes slideOut {
                from { transform: translateX(0); opacity: 1; }
                to { transform: translateX(100%); opacity: 0; }
            }
        `;
        document.head.appendChild(style);
    </script>
    <style>
        /* ===== SHOP HERO SECTION ===== */
        /* Keyframe Animations */
        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(30px);
            }
            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        @keyframes gradientShift {
            0% {
                background-position: 0% 50%;
            }
            50% {
                background-position: 100% 50%;
            }
            100% {
                background-position: 0% 50%;
            }
        }
        
        /* Hero Section */
        .shop-hero {
            background: linear-gradient(-45deg, #667eea, #764ba2, #667eea, #764ba2);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
        }
        
        .shop-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .shop-hero .container {
            position: relative;
            z-index: 2;
            opacity: 0;
            animation: fadeInUp 1s ease-out forwards;
        }
        
        .shop-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        @keyframes slideInDown {
            from { opacity: 0; transform: translateY(-30px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .shop-hero p {
            font-size: clamp(1rem, 2.5vw, 1.3rem);
            max-width: 700px;
            margin: 0 auto;
            line-height: 1.8;
            opacity: 0.95;
        }
        
        @keyframes slideInUp {
            from { opacity: 0; transform: translateY(30px); }
            to { opacity: 1; transform: translateY(0); }
        }

        .shop-filters {
            background: white;
            padding: 1.25rem;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            margin: -2rem 1.5rem 1.5rem;
            border-radius: 12px;
            border: 1px solid rgba(102, 126, 234, 0.1);
            position: relative;
            z-index: 10;
        }
        
        .filter-row {
            display: flex;
            gap: 1rem;
            align-items: center;
            flex-wrap: wrap;
            justify-content: center;
        }
        
        .filter-group {
            display: flex;
            flex-direction: column;
            min-width: 150px;
        }
        
        .filter-group label {
            font-weight: 600;
            margin-bottom: 0.5rem;
            color: #2c5aa0;
            font-size: 0.9rem;
        }
        
        .filter-group select, .filter-group input {
            padding: 0.5rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
            transition: all 0.3s ease;
        }
        
        .filter-group select:focus, .filter-group input:focus {
            border-color: #2c5aa0;
            outline: none;
            box-shadow: 0 0 0 3px rgba(44, 90, 160, 0.1);
        }
        
        .search-btn {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            padding: 0.75rem 2rem;
            border: none;
            border-radius: 25px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            align-self: end;
            position: relative;
            overflow: hidden;
        }
        
        .search-btn::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            transform: translate(-50%, -50%);
            transition: width 0.6s, height 0.6s;
        }
        
        .search-btn:hover::before {
            width: 300px;
            height: 300px;
        }
        
        .search-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .search-btn:active {
            transform: translateY(0);
        }
        
        .shop-content {
            padding: 1.5rem;
            max-width: 1200px;
            margin: 0 auto;
        }
        
        .shop-content > * {
            opacity: 0;
            transform: translateY(30px);
            animation: fadeInUp 0.6s ease-out forwards;
        }
        
        .shop-content > *:nth-child(1) { animation-delay: 0.1s; }
        .shop-content > *:nth-child(2) { animation-delay: 0.2s; }
        .shop-content > *:nth-child(3) { animation-delay: 0.3s; }
        
        .category-tabs {
            display: flex;
            gap: 0.75rem;
            margin-bottom: 1.25rem;
            justify-content: center;
            flex-wrap: wrap;
        }
        
        .category-tab {
            padding: 0.75rem 1.5rem;
            background: #f8f9fa;
            border: 2px solid transparent;
            border-radius: 25px;
            text-decoration: none;
            color: #333;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 0.9rem;
        }
        
        .category-tab:hover, .category-tab.active {
            background: #2c5aa0;
            color: white;
            text-decoration: none;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
        }
        
        .products-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 1.25rem;
            margin-top: 1.25rem;
        }
        
        /* ===== ANIMATED PRODUCT CARDS ===== */
        .product-card {
            background: white;
            border-radius: 15px;
            box-shadow: 0 5px 20px rgba(0,0,0,0.1);
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            overflow: hidden;
            opacity: 0;
            transform: translateY(20px);
            animation: fadeInUp 0.6s ease-out forwards;
            position: relative;
        }
        
        /* Stagger animation for each card */
        .product-card:nth-child(1) { animation-delay: 0.1s; }
        .product-card:nth-child(2) { animation-delay: 0.2s; }
        .product-card:nth-child(3) { animation-delay: 0.3s; }
        .product-card:nth-child(4) { animation-delay: 0.4s; }
        .product-card:nth-child(5) { animation-delay: 0.5s; }
        .product-card:nth-child(6) { animation-delay: 0.6s; }
        .product-card:nth-child(n+7) { animation-delay: 0.7s; }
        
        .product-card:hover {
            transform: translateY(-8px) scale(1.02);
            box-shadow: 0 20px 40px rgba(0,0,0,0.15);
        }
        
        .product-image {
            width: 100%;
            height: 200px;
            background: linear-gradient(45deg, #f0f0f0, #e0e0e0);
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 3rem;
            color: #999;
            overflow: hidden;
            position: relative;
        }
        
        .product-img {
            width: 100%;
            height: 200px;
            object-fit: cover;
            border-radius: 10px 10px 0 0;
            transition: transform 0.5s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        .product-card:hover .product-img {
            transform: scale(1.1);
        }
        
        .product-info {
            padding: 1.25rem;
        }
        
        .product-title {
            font-size: 1.3rem;
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
            transition: color 0.3s ease;
        }
        
        .product-card:hover .product-title {
            color: #2c5aa0;
        }
        
        .product-brand {
            color: #666;
            font-size: 0.9rem;
            margin-bottom: 0.5rem;
        }
        
        .product-price {
            font-size: 1.5rem;
            font-weight: bold;
            color: #2c5aa0;
            margin-bottom: 0.75rem;
        }
        
        .product-description {
            font-size: 0.9rem;
            color: #666;
            margin-bottom: 0.5rem;
            line-height: 1.4;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
        }
        
        .product-stock {
            font-size: 0.9rem;
            font-weight: 600;
            margin-bottom: 0.75rem;
            display: inline-block;
            padding: 0.4rem 0.8rem;
            border-radius: 5px;
        }
        
        /* In stock - green */
        .product-stock:not(.limited-stock):not(.out-of-stock) {
            color: #28a745 !important;
            background: rgba(40, 167, 69, 0.1) !important;
        }
        
        /* Limited stock - orange */
        .product-stock.limited-stock {
            color: #ff9800 !important;
            background: rgba(255, 152, 0, 0.1) !important;
        }
        
        /* Out of stock - red */
        .product-stock.out-of-stock {
            color: #dc3545 !important;
            background: rgba(220, 53, 69, 0.1) !important;
        }
        
        .product-actions {
            display: flex;
            gap: 0.5rem;
        }
        
        .btn-view {
            flex: 1;
            padding: 0.75rem;
            background: #f8f9fa;
            border: 2px solid #2c5aa0;
            color: #2c5aa0;
            border-radius: 25px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
        }
        
        .btn-view::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(44, 90, 160, 0.1);
            transform: translate(-50%, -50%);
            transition: width 0.4s, height 0.4s;
        }
        
        .btn-view:hover::before {
            width: 200px;
            height: 200px;
        }
        
        .btn-view:hover {
            background: #2c5aa0;
            color: white;
            transform: translateY(-2px);
        }
        
        .btn-cart {
            flex: 1;
            padding: 0.75rem;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            border-radius: 25px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
        }
        
        .btn-cart::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            transform: translate(-50%, -50%);
            transition: width 0.4s, height 0.4s;
        }
        
        .btn-cart:hover::before {
            width: 200px;
            height: 200px;
        }
        
        .btn-cart:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .btn-cart:active {
            transform: scale(0.98);
        }
        
        .btn-cart.btn-disabled,
        .btn-cart:disabled {
            background: #cccccc !important;
            color: #666666 !important;
            cursor: not-allowed !important;
            opacity: 0.6;
            pointer-events: none;
        }
        
        .btn-cart.btn-disabled:hover,
        .btn-cart:disabled:hover {
            transform: none !important;
            box-shadow: none !important;
        }
        
        .btn-wishlist {
            padding: 0.75rem 1rem;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            border-radius: 25px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            margin-left: 0.5rem;
            position: relative;
            overflow: hidden;
        }
        
        .btn-wishlist::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.3);
            transform: translate(-50%, -50%);
            transition: width 0.4s, height 0.4s;
        }
        
        .btn-wishlist:hover::before {
            width: 150px;
            height: 150px;
        }
        
        .btn-wishlist:hover {
            transform: translateY(-2px) scale(1.1);
            box-shadow: 0 4px 15px rgba(220, 53, 69, 0.4);
        }
        
        .btn-wishlist.in-wishlist {
            background: linear-gradient(135deg, #28a745, #218838);
        }
        
        .btn-wishlist.in-wishlist:hover {
            box-shadow: 0 4px 15px rgba(40, 167, 69, 0.4);
        }
        
        .results-info {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
            flex-wrap: wrap;
            gap: 1rem;
        }
        
        .sort-options {
            display: flex;
            align-items: center;
            gap: 1rem;
        }
        
        .sort-options select {
            padding: 0.5rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            transition: all 0.3s ease;
        }
        
        .sort-options select:focus {
            border-color: #2c5aa0;
            outline: none;
            box-shadow: 0 0 0 3px rgba(44, 90, 160, 0.1);
        }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 992px) {
            .shop-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .shop-hero h1 {
                font-size: 2.5rem;
            }
            
            .shop-hero p {
                font-size: 1.1rem;
            }
            
            .products-grid {
                grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                gap: 1.25rem;
            }
        }
        
        @media (max-width: 768px) {
            .shop-hero {
                padding: 6rem 1rem 3rem;
                margin-top: 0;
            }
            
            .shop-hero h1 {
                font-size: 2.5rem;
            }
            
            .shop-hero p {
                font-size: 1rem;
            }
            
            .shop-filters {
                padding: 1.25rem;
                margin: -2rem 1rem 1.25rem;
            }
            
            .filter-row {
                flex-direction: column;
                align-items: stretch;
                gap: 0.75rem;
            }
            
            .filter-group {
                min-width: 100%;
            }
            
            .search-btn {
                width: 100%;
                align-self: stretch;
            }
            
            .shop-content {
                padding: 1rem;
            }
            
            .category-tabs {
                flex-direction: row;
                overflow-x: auto;
                padding-bottom: 0.5rem;
                -webkit-overflow-scrolling: touch;
                scrollbar-width: thin;
            }
            
            .category-tabs::-webkit-scrollbar {
                height: 4px;
            }
            
            .category-tabs::-webkit-scrollbar-thumb {
                background: #667eea;
                border-radius: 2px;
            }
            
            .category-tab {
                padding: 0.6rem 1.25rem;
                white-space: nowrap;
                font-size: 0.85rem;
            }
            
            .products-grid {
                grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                gap: 1rem;
            }
            
            .results-info {
                flex-direction: column;
                align-items: stretch;
                text-align: center;
                gap: 0.75rem;
            }
            
            .sort-options {
                justify-content: center;
                width: 100%;
            }
            
            .sort-options select {
                flex: 1;
                max-width: 250px;
            }
        }
        
        @media (max-width: 576px) {
            .shop-hero {
                padding: 5rem 0.75rem 2.5rem;
            }
            
            .shop-hero h1 {
                font-size: 2rem;
            }
            
            .shop-hero p {
                font-size: 0.95rem;
            }
            
            .shop-filters {
                margin: -1.5rem 0.75rem 1rem;
                padding: 1rem;
            }
            
            .shop-content {
                padding: 0.75rem;
            }
            
            .products-grid {
                grid-template-columns: 1fr;
                gap: 1rem;
            }
            
            .product-card {
                border-radius: 12px;
            }
            
            .product-info {
                padding: 1rem;
            }
            
            .product-title {
                font-size: 1.1rem;
            }
            
            .product-price {
                font-size: 1.3rem;
            }
            
            .product-actions {
                flex-direction: column;
            }
            
            .btn-wishlist {
                margin-left: 0;
                margin-top: 0.5rem;
            }
        }
        
        /* Smooth scrolling */
        html {
            scroll-behavior: smooth;
        }
        
        
        /* ===== LOADING STATE ===== */
        .product-card.loading {
            opacity: 0.5;
            pointer-events: none;
        }
        
        /* ===== SMOOTH SCROLL REVEAL ===== */
        @media (prefers-reduced-motion: no-preference) {
            .product-card {
                will-change: transform, opacity;
            }
        }
        
        @media (prefers-reduced-motion: reduce) {
            .product-card {
                animation: none;
                opacity: 1;
                transform: none;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Shop Hero Section -->
    <section class="shop-hero">
        <div class="container">
            <h1>Shop Eyewear</h1>
            <p>Discover our premium collection of frames, lenses, and accessories. Quality eyewear that combines style with functionality.</p>
        </div>
    </section>

    <!-- Search & Filter Section -->
    <section class="shop-filters">
        <div class="filter-row">
            <div class="filter-group">
                <label>Search Products</label>
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search frames, lenses..." CssClass="form-control"></asp:TextBox>
            </div>
            <div class="filter-group">
                <label>Category</label>
                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All Products" Value="all"></asp:ListItem>
                    <asp:ListItem Text="Frames" Value="frames"></asp:ListItem>
                    <asp:ListItem Text="Lenses" Value="lenses"></asp:ListItem>
                    <asp:ListItem Text="Accessories" Value="accessories"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>Brand</label>
                <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All Brands" Value="all"></asp:ListItem>
                    <asp:ListItem Text="Ray-Ban" Value="rayban"></asp:ListItem>
                    <asp:ListItem Text="Oakley" Value="oakley"></asp:ListItem>
                    <asp:ListItem Text="Prada" Value="prada"></asp:ListItem>
                    <asp:ListItem Text="Gucci" Value="gucci"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="filter-group">
                <label>Price Range</label>
                <asp:DropDownList ID="ddlPriceRange" runat="server" CssClass="form-control">
                    <asp:ListItem Text="All Prices" Value="all"></asp:ListItem>
                    <asp:ListItem Text="Under R500" Value="0-500"></asp:ListItem>
                    <asp:ListItem Text="R500 - R1000" Value="500-1000"></asp:ListItem>
                    <asp:ListItem Text="R1000 - R2000" Value="1000-2000"></asp:ListItem>
                    <asp:ListItem Text="Over R2000" Value="2000+"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="search-btn" OnClick="btnSearch_Click" />
        </div>
    </section>

    <!-- Shop Content -->
    <section class="shop-content">
        <!-- Category Navigation -->
        <div class="category-tabs">
            <asp:LinkButton ID="lnkAllProducts" runat="server" CssClass="category-tab active" OnClick="FilterByCategory" CommandArgument="all">All Products</asp:LinkButton>
            <asp:Repeater ID="rptCategories" runat="server">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCategory" runat="server" CssClass="category-tab" OnClick="FilterByCategory" 
                        CommandArgument='<%# Eval("Product_Category") %>' Text='<%# Eval("Product_Category") %>' />
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Results Info & Sorting -->
        <div class="results-info">
            <asp:Label ID="lblResultsCount" runat="server" Text="Showing 24 products" CssClass="results-count"></asp:Label>
            <div class="sort-options">
                <label>Sort by:</label>
                <asp:DropDownList ID="ddlSortBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SortProducts">
                    <asp:ListItem Text="Featured" Value="featured"></asp:ListItem>
                    <asp:ListItem Text="Price: Low to High" Value="price_asc"></asp:ListItem>
                    <asp:ListItem Text="Price: High to Low" Value="price_desc"></asp:ListItem>
                    <asp:ListItem Text="Name A-Z" Value="name_asc"></asp:ListItem>
                    <asp:ListItem Text="Newest" Value="date_desc"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <!-- Products Grid -->
        <div class="products-grid">
            <asp:Repeater ID="rptProducts" runat="server" OnItemDataBound="rptProducts_ItemDataBound">
                <ItemTemplate>
                    <div class="product-card">
                        <div class="product-image">
                            <asp:Image ID="imgProduct" runat="server" CssClass="product-img" />
                        </div>
                        <div class="product-info">
                            <div class="product-brand"><%# Eval("Product_Brand") %></div>
                            <h3 class="product-title"><%# Eval("Product_Name") %></h3>
                            <div class="product-price">R<%# Eval("Product_Price", "{0:F2}") %></div>
                            <div class="product-description"><%# Eval("Product_Description") %></div>
                            <asp:Label ID="lblStock" runat="server" CssClass="product-stock" />
                            <div class="product-actions">
                                <asp:Button ID="btnViewDetails" runat="server" Text="View Details" CssClass="btn-view" 
                                    CommandArgument='<%# Eval("Product_Brand") + "_" + Eval("Product_Name") %>' OnClick="ViewProductDetails" />
                                <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn-cart" 
                                    CommandArgument='<%# Eval("Product_Brand") + "_" + Eval("Product_Name") %>' OnClick="AddToCart" />
                                <asp:Button ID="btnWishlist" runat="server" Text="♡" CssClass="btn-wishlist" 
                                    CommandArgument='<%# Eval("Product_ID") %>' OnClick="ToggleWishlist" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Load More Button -->
        <div style="text-align: center; margin-top: 2rem;">
            <asp:Button ID="btnLoadMore" runat="server" Text="Load More Products" CssClass="cta-button" OnClick="LoadMoreProducts" />
        </div>
    </section>
</asp:Content>