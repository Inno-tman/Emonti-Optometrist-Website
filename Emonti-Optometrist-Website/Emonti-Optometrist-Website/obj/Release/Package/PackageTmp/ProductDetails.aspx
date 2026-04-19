<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="Emonti_Optometrist_Website.ProductDetails" %>

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
        .product-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
        }
        
        .product-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
        }
        
        .product-container {
            max-width: 1200px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
        }
        
        .product-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
        }
        
        .product-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 3rem;
            margin-bottom: 3rem;
        }
        
        .product-images {
            position: relative;
        }
        
        .main-image {
            width: 100%;
            height: 400px;
            background: linear-gradient(45deg, #f0f0f0, #e0e0e0);
            border-radius: 15px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 8rem;
            color: #999;
            margin-bottom: 1rem;
            overflow: hidden;
        }
        
        .product-img {
            width: 100%;
            height: 400px;
            object-fit: cover;
            border-radius: 15px;
        }
        
        .thumbnail-images {
            display: flex;
            gap: 1rem;
        }
        
        .thumbnail {
            width: 80px;
            height: 80px;
            background: #f0f0f0;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 2rem;
            cursor: pointer;
            transition: all 0.3s ease;
            overflow: hidden;
        }
        
        .thumbnail:hover {
            background: #e0e0e0;
        }
        
        .thumbnail.active {
            border: 2px solid #2c5aa0;
        }
        
        .thumbnail-img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
        
        /* Hide empty thumbnail containers */
        .thumbnail.hidden-thumb {
            display: none !important;
        }
        
        .product-info {
            display: flex;
            flex-direction: column;
        }
        
        .product-brand {
            color: #666;
            font-size: 1.1rem;
            margin-bottom: 0.5rem;
        }
        
        .product-title {
            font-size: 2.5rem;
            font-weight: 600;
            color: #333;
            margin-bottom: 1rem;
        }
        
        .product-price {
            font-size: 2rem;
            font-weight: bold;
            color: #2c5aa0;
            margin-bottom: 1.5rem;
        }
        
        .product-description {
            color: #666;
            line-height: 1.6;
            margin-bottom: 2rem;
        }
        
        .product-stock-wrapper {
            margin-bottom: 2rem;
        }
        
        .product-stock {
            font-size: 1rem;
            color: #28a745;
            font-weight: 600;
            display: inline-block;
            padding: 0.5rem 1rem;
            border-radius: 5px;
            background: rgba(40, 167, 69, 0.1);
        }
        
        .product-stock.limited-stock {
            color: #ff9800;
            background: rgba(255, 152, 0, 0.1);
        }
        
        .product-stock.out-of-stock {
            color: #dc3545;
            background: rgba(220, 53, 69, 0.1);
        }
        
        .product-options {
            margin-bottom: 2rem;
        }
        
        .option-group {
            margin-bottom: 1.5rem;
        }
        
        .option-label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.5rem;
            display: block;
        }
        
        .option-buttons {
            display: flex;
            gap: 0.5rem;
            flex-wrap: wrap;
        }
        
        .option-btn {
            padding: 0.5rem 1rem;
            border: 2px solid #e0e0e0;
            background: white;
            border-radius: 25px;
            cursor: pointer;
            transition: all 0.3s ease;
            font-size: 0.9rem;
        }
        
        .option-btn:hover {
            border-color: #2c5aa0;
            background: #f0f4ff;
        }
        
        .option-btn.selected {
            background: #2c5aa0;
            color: white;
            border-color: #2c5aa0;
        }
        
        .quantity-section {
            display: flex;
            align-items: center;
            gap: 1rem;
            margin-bottom: 2rem;
        }
        
        .quantity-controls {
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .quantity-btn {
            width: 40px;
            height: 40px;
            border: 2px solid #2c5aa0;
            background: white;
            color: #2c5aa0;
            border-radius: 8px;
            cursor: pointer;
            font-weight: bold;
            font-size: 1.2rem;
        }
        
        .quantity-btn:hover {
            background: #2c5aa0;
            color: white;
        }
        
        .quantity-input {
            width: 60px;
            text-align: center;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            padding: 0.5rem;
            font-size: 1rem;
        }
        
        .action-buttons {
            display: flex;
            gap: 1rem;
            margin-bottom: 2rem;
        }
        
        .btn-add-cart {
            flex: 1;
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1rem 2rem;
            border-radius: 8px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
            font-size: 1.1rem;
        }
        
        .btn-add-cart:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
        }
        
        .btn-add-cart.btn-disabled,
        .btn-disabled {
            background: #cccccc !important;
            color: #666666 !important;
            cursor: not-allowed !important;
            opacity: 0.6;
            pointer-events: none;
        }
        
        .btn-add-cart.btn-disabled:hover {
            transform: none !important;
            box-shadow: none !important;
        }
        
        .quantity-btn.btn-disabled {
            background: #f0f0f0 !important;
            color: #999999 !important;
            border-color: #cccccc !important;
            cursor: not-allowed !important;
            opacity: 0.6;
            pointer-events: none;
        }
        
        .quantity-btn.btn-disabled:hover {
            background: #f0f0f0 !important;
            color: #999999 !important;
        }
        
        .quantity-input.input-disabled {
            background: #f5f5f5 !important;
            color: #999999 !important;
            border-color: #cccccc !important;
            cursor: not-allowed !important;
        }
        
        .out-of-stock-message {
            margin-top: 1rem;
            padding: 1rem;
            background: #fff3cd;
            border: 1px solid #ffc107;
            border-radius: 8px;
            color: #856404;
        }
        
        .out-of-stock-message p {
            margin: 0;
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .out-of-stock-message i {
            font-size: 1.2rem;
        }
        
        .btn-wishlist {
            background: transparent;
            color: #2c5aa0;
            border: 2px solid #2c5aa0;
            padding: 1rem;
            border-radius: 8px;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .btn-wishlist:hover {
            background: #2c5aa0;
            color: white;
        }
        
        .btn-wishlist.in-wishlist {
            background: #28a745;
            color: white;
            border-color: #28a745;
        }
        
        .btn-wishlist.in-wishlist:hover {
            background: #218838;
            border-color: #218838;
        }
        
        .product-features {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 2rem;
            margin-bottom: 2rem;
        }
        
        .features-title {
            font-size: 1.3rem;
            font-weight: 600;
            color: #2c5aa0;
            margin-bottom: 1rem;
        }
        
        .features-list {
            list-style: none;
            padding: 0;
        }
        
        .features-list li {
            padding: 0.5rem 0;
            border-bottom: 1px solid #e0e0e0;
        }
        
        .features-list li:last-child {
            border-bottom: none;
        }
        
        .features-list li:before {
            content: "✓ ";
            color: #28a745;
            font-weight: bold;
        }
        
        .related-products {
            margin-top: 3rem;
        }
        
        .related-title {
            font-size: 1.5rem;
            font-weight: 600;
            color: #2c5aa0;
            margin-bottom: 1.5rem;
            text-align: center;
        }
        
        .related-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 2rem;
        }
        
        .related-product {
            background: #f8f9fa;
            border-radius: 10px;
            padding: 1.5rem;
            text-align: center;
            transition: all 0.3s ease;
        }
        
        .related-product:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        
        .related-image {
            width: 100px;
            height: 100px;
            background: #e0e0e0;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 2rem;
            margin: 0 auto 1rem;
        }
        
        /* Responsive Design */
        @media (max-width: 1024px) {
            .product-container {
                padding: 0 1.5rem;
            }
            
            .product-content {
                padding: 2rem;
            }
            
            .product-grid {
                gap: 2rem;
            }
        }
        
        @media (max-width: 768px) {
            .product-hero {
                padding: 6rem 1.5rem 3rem;
                margin-top: 0;
            }
            
            .product-hero h1 {
                font-size: 2rem;
            }
            
            .product-hero p {
                font-size: 1rem;
            }
            
            .product-container {
                padding: 0 1rem;
                margin: -2rem auto 2rem;
            }
            
            .product-content {
                padding: 1.5rem;
                border-radius: 10px;
            }
            
            .product-grid {
                grid-template-columns: 1fr;
                gap: 2rem;
            }
            
            .product-title {
                font-size: 1.75rem;
            }
            
            .product-price {
                font-size: 1.5rem;
            }
            
            .main-image,
            .product-img {
                height: 300px;
            }
            
            .thumbnail-images {
                gap: 0.75rem;
            }
            
            .thumbnail {
                width: 70px;
                height: 70px;
            }
            
            .quantity-section {
                flex-direction: column;
                align-items: flex-start;
                gap: 0.75rem;
            }
            
            .action-buttons {
                flex-direction: column;
                gap: 0.75rem;
            }
            
            .btn-add-cart {
                width: 100%;
            }
            
            .btn-wishlist {
                width: 100%;
            }
            
            .related-grid {
                grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
                gap: 1.5rem;
            }
        }
        
        @media (max-width: 480px) {
            .product-hero {
                padding: 5rem 1rem 2rem;
            }
            
            .product-hero h1 {
                font-size: 1.75rem;
            }
            
            .product-container {
                padding: 0 0.75rem;
                margin: -1.5rem auto 1.5rem;
            }
            
            .product-content {
                padding: 1rem;
            }
            
            .product-title {
                font-size: 1.5rem;
            }
            
            .product-price {
                font-size: 1.25rem;
            }
            
            .product-brand {
                font-size: 1rem;
            }
            
            .product-description {
                font-size: 0.9rem;
            }
            
            .main-image,
            .product-img {
                height: 250px;
            }
            
            .thumbnail-images {
                gap: 0.5rem;
                flex-wrap: wrap;
            }
            
            .thumbnail {
                width: 60px;
                height: 60px;
            }
            
            .quantity-controls {
                width: 100%;
                justify-content: center;
            }
            
            .quantity-input {
                width: 80px;
            }
            
            .product-stock {
                font-size: 0.9rem;
                padding: 0.4rem 0.8rem;
            }
            
            .related-grid {
                grid-template-columns: 1fr;
            }
        }
        
        @media (max-width: 360px) {
            .product-hero h1 {
                font-size: 1.5rem;
            }
            
            .product-title {
                font-size: 1.25rem;
            }
            
            .product-price {
                font-size: 1.1rem;
            }
            
            .main-image,
            .product-img {
                height: 200px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <!-- Product Hero Section -->
    <section class="product-hero">
        <h1>Product Details</h1>
        <p>Explore our premium eyewear collection</p>
    </section>

    <!-- Product Content -->
    <div class="product-container">
        <div class="product-content">
            <!-- Product Grid -->
            <div class="product-grid">
                <!-- Product Images -->
                <div class="product-images">
                    <div class="main-image" id="mainImage">
                        <asp:Image ID="imgMainProduct" runat="server" CssClass="product-img" />
                    </div>
                    <div class="thumbnail-images">
                        <div class="thumbnail active" id="thumb1" onclick="changeImage(this)">
                            <asp:Image ID="imgThumb1" runat="server" CssClass="thumbnail-img" />
                        </div>
                        <div class="thumbnail" id="thumb2" onclick="changeImage(this)">
                            <asp:Image ID="imgThumb2" runat="server" CssClass="thumbnail-img" />
                        </div>
                        <div class="thumbnail" id="thumb3" onclick="changeImage(this)">
                            <asp:Image ID="imgThumb3" runat="server" CssClass="thumbnail-img" />
                        </div>
                        <div class="thumbnail" id="thumb4" onclick="changeImage(this)">
                            <asp:Image ID="imgThumb4" runat="server" CssClass="thumbnail-img" />
                        </div>
                    </div>
                </div>

                <!-- Product Information -->
                <div class="product-info">
                    <div class="product-brand">
                        <asp:Label ID="lblBrand" runat="server" Text="Brand"></asp:Label>
                    </div>
                    <h1 class="product-title">
                        <asp:Label ID="lblProductName" runat="server" Text="Product Name"></asp:Label>
                    </h1>
                    <div class="product-price">
                        R <asp:Label ID="lblPrice" runat="server" Text="0.00"></asp:Label>
                    </div>
                    
                    <div class="product-description">
                        <asp:Label ID="lblDescription" runat="server" Text="Product description will appear here."></asp:Label>
                    </div>

                    <div class="product-stock-wrapper">
                        <asp:Label ID="lblStock" runat="server" Text="In stock" CssClass="product-stock"></asp:Label>
                    </div>

                    <!-- Product Options -->
                    <div class="product-options">
                        <!-- Frame Type, Lens Type, and Size options have been removed -->
                    </div>

                    <!-- Quantity and Actions -->
                    <div class="quantity-section">
                        <label class="option-label">Quantity:</label>
                        <div class="quantity-controls">
                            <asp:Button ID="btnQtyMinus" runat="server" Text="-" CssClass="quantity-btn" OnClick="btnQtyMinus_Click" />
                            <asp:TextBox ID="txtQuantity" runat="server" Text="1" CssClass="quantity-input" />
                            <asp:Button ID="btnQtyPlus" runat="server" Text="+" CssClass="quantity-btn" OnClick="btnQtyPlus_Click" />
                        </div>
                    </div>

                    <div class="action-buttons">
                        <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn-add-cart" OnClick="btnAddToCart_Click" />
                        <asp:Button ID="btnWishlist" runat="server" Text="&#9825;" CssClass="btn-wishlist" OnClick="btnWishlist_Click" />
                    </div>
                    
                    <asp:Panel ID="outOfStockMessage" runat="server" CssClass="out-of-stock-message" Visible="false">
                        <p><i class="fas fa-exclamation-circle"></i> This product is currently unavailable. Please check back later or contact us for availability updates.</p>
                    </asp:Panel>
                </div>
            </div>

        </div>
    </div>

    <script type="text/javascript">
        function changeImage(thumbnailElement) {
            // Get the image source from the clicked thumbnail
            const thumbnailImg = thumbnailElement.querySelector('img');
            const imageUrl = thumbnailImg.src;
            
            // Update the main image
            document.getElementById('<%= imgMainProduct.ClientID %>').src = imageUrl;
            
            // Update active thumbnail
            document.querySelectorAll('.thumbnail').forEach(thumb => {
                thumb.classList.remove('active');
            });
            thumbnailElement.classList.add('active');
        }

        // Hide empty thumbnail containers after page load
        window.addEventListener('DOMContentLoaded', function() {
            document.querySelectorAll('.thumbnail').forEach(function(thumb) {
                const img = thumb.querySelector('img');
                if (img && (img.style.display === 'none' || 
                           img.offsetParent === null || 
                           !img.src || 
                           img.src.includes('placeholder') && img.style.display === 'none')) {
                    thumb.classList.add('hidden-thumb');
                }
            });
        });
    </script>
</asp:Content>