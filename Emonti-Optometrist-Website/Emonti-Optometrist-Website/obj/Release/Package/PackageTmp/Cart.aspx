<%@ Page Title="Shopping Cart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="Emonti_Optometrist_Website.Cart" EnableEventValidation="false" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function changeQty(btn, delta) {
            try {
                // find the nearest quantity input within the same quantity-controls container
                var container = btn.parentNode;
                var input = container.querySelector('input[type="text"]');
                if (!input) return false;

                var current = parseInt(input.value, 10);
                if (isNaN(current) || current < 1) current = 1;
                var newVal = current + delta;
                if (newVal < 1) newVal = 1;
                input.value = newVal;

                // trigger postback for the textbox (TextChanged handler)
                if (typeof(__doPostBack) === 'function') {
                    // use the input's name (which corresponds to UniqueID) as event target
                    __doPostBack(input.name, '');
                }
            } catch (ex) {
                console && console.log && console.log('changeQty error: ' + ex.message);
            }
            // prevent the button from doing its default postback (server handler will run via textbox change)
            return false;
        }
    </script>

    <style>
        /* ===== CART HERO SECTION ===== */
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
        .cart-hero {
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
        
        .cart-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .cart-hero .container {
            position: relative;
            z-index: 2;
            animation: fadeInUp 1s ease-out;
        }
        
        .cart-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        .cart-hero p {
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
        
        .cart-hero .container {
            position: relative;
            z-index: 1;
        }
        
        .cart-container {
            max-width: 1200px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .cart-content {
            background: white;
            border-radius: 15px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.15);
            padding: 3rem;
            animation: contentFadeIn 0.6s ease-out 0.3s both;
        }
        
        @keyframes contentFadeIn {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .cart-empty {
            text-align: center;
            padding: 4rem 2rem;
            animation: fadeIn 0.6s ease-out;
        }
        
        @keyframes fadeIn {
            from { opacity: 0; }
            to { opacity: 1; }
        }
        
        .cart-empty h2 {
            color: #2c5aa0;
            margin-bottom: 1rem;
            font-size: 2rem;
        }
        
        .cart-empty p {
            color: #666;
            margin-bottom: 2rem;
            font-size: 1.1rem;
        }
        
        .cart-items {
            margin-bottom: 2rem;
        }
        
        .cart-items h2 {
            color: #2c5aa0;
            margin-bottom: 1.5rem;
            font-size: 1.8rem;
            font-weight: 600;
        }
        
        .cart-item {
            display: grid;
            grid-template-columns: 120px 1fr auto auto auto;
            gap: 1.5rem;
            align-items: center;
            padding: 1.5rem;
            border: 2px solid #e8e9ea;
            border-radius: 12px;
            margin-bottom: 1.25rem;
            background: #ffffff;
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            animation: itemSlideIn 0.4s ease-out both;
        }
        
        .cart-item:nth-child(1) { animation-delay: 0.1s; }
        .cart-item:nth-child(2) { animation-delay: 0.15s; }
        .cart-item:nth-child(3) { animation-delay: 0.2s; }
        .cart-item:nth-child(4) { animation-delay: 0.25s; }
        .cart-item:nth-child(5) { animation-delay: 0.3s; }
        
        @keyframes itemSlideIn {
            from { 
                opacity: 0; 
                transform: translateX(-20px); 
            }
            to { 
                opacity: 1; 
                transform: translateX(0); 
            }
        }
        
        .cart-item:hover {
            border-color: #667eea;
            box-shadow: 0 4px 16px rgba(102, 126, 234, 0.15);
            transform: translateY(-2px);
        }
        
        .item-image {
            width: 120px;
            height: 120px;
            background: #f5f5f5;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
            position: relative;
            transition: transform 0.3s ease;
        }
        
        .item-image:hover {
            transform: scale(1.05);
        }
        
        .item-image img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            border-radius: 10px;
            transition: transform 0.3s ease;
        }
        
        .item-details h3 {
            color: #2c5aa0;
            margin-bottom: 0.5rem;
            font-size: 1.2rem;
            font-weight: 600;
        }
        
        .item-details p {
            color: #666;
            margin-bottom: 0.25rem;
            font-size: 0.95rem;
        }
        
        .item-price {
            font-weight: 700;
            color: #2c5aa0;
            font-size: 1.25rem;
            white-space: nowrap;
        }
        
        .quantity-controls {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            background: #f8f9fa;
            padding: 0.25rem;
            border-radius: 8px;
            border: 1px solid #e0e0e0;
        }
        
        .quantity-btn {
            width: 38px;
            height: 38px;
            border: none;
            background: white;
            color: #2c5aa0;
            border-radius: 6px;
            cursor: pointer;
            font-weight: 700;
            font-size: 1.1rem;
            transition: all 0.2s ease;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        
        .quantity-btn:hover {
            background: #2c5aa0;
            color: white;
            transform: scale(1.1);
            box-shadow: 0 2px 6px rgba(44, 90, 160, 0.3);
        }
        
        .quantity-btn:active {
            transform: scale(0.95);
        }
        
        .quantity-input {
            width: 55px;
            text-align: center;
            border: none;
            border-radius: 6px;
            padding: 0.5rem;
            font-weight: 600;
            font-size: 1rem;
            background: transparent;
            color: #2c5aa0;
        }
        
        .quantity-input:focus {
            outline: 2px solid #667eea;
            outline-offset: 2px;
        }
        
        .remove-btn {
            background: linear-gradient(135deg, #dc3545, #c82333);
            color: white;
            border: none;
            padding: 0.6rem 1.2rem;
            border-radius: 8px;
            cursor: pointer;
            font-size: 0.9rem;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 2px 6px rgba(220, 53, 69, 0.2);
        }
        
        .remove-btn:hover {
            background: linear-gradient(135deg, #c82333, #bd2130);
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(220, 53, 69, 0.3);
        }
        
        .remove-btn:active {
            transform: translateY(0);
        }
        
        .cart-summary {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            padding: 2.5rem;
            border-radius: 12px;
            border: 2px solid #e8e9ea;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            animation: summarySlideIn 0.5s ease-out 0.4s both;
        }
        
        @keyframes summarySlideIn {
            from { 
                opacity: 0; 
                transform: translateX(20px); 
            }
            to { 
                opacity: 1; 
                transform: translateX(0); 
            }
        }
        
        .cart-summary h3 {
            color: #2c5aa0;
            margin-bottom: 1.5rem;
            font-size: 1.5rem;
            font-weight: 600;
        }
        
        .summary-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 1rem;
            padding-bottom: 1rem;
            border-bottom: 1px solid #e0e0e0;
            font-size: 1rem;
            transition: all 0.2s ease;
        }
        
        .summary-row:hover {
            color: #2c5aa0;
        }
        
        .summary-row:last-child {
            border-bottom: none;
            font-weight: 700;
            font-size: 1.4rem;
            color: #2c5aa0;
            padding-top: 1rem;
            margin-top: 0.5rem;
            border-top: 2px solid #2c5aa0;
        }
        
        .promo-section {
            margin-bottom: 2rem;
            padding: 2rem;
            background: linear-gradient(135deg, rgba(102, 126, 234, 0.05) 0%, rgba(118, 75, 162, 0.05) 100%);
            border: 2px dashed #667eea;
            border-radius: 12px;
            animation: promoFadeIn 0.5s ease-out 0.35s both;
        }
        
        @keyframes promoFadeIn {
            from { opacity: 0; transform: scale(0.98); }
            to { opacity: 1; transform: scale(1); }
        }
        
        .promo-section h3 {
            color: #2c5aa0;
            margin-bottom: 1rem;
            font-size: 1.2rem;
            font-weight: 600;
        }
        
        .promo-input {
            display: flex;
            gap: 1rem;
            align-items: stretch;
        }
        
        .promo-input input {
            flex: 1;
            padding: 0.875rem 1.25rem;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 1rem;
            transition: all 0.3s ease;
        }
        
        .promo-input input:focus {
            outline: none;
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }
        
        .apply-btn {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 0.875rem 2rem;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            transition: all 0.3s ease;
            box-shadow: 0 2px 8px rgba(44, 90, 160, 0.2);
        }
        
        .apply-btn:hover {
            background: linear-gradient(135deg, #1e4080, #153d6f);
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
        }
        
        .apply-btn:active {
            transform: translateY(0);
        }
        
        .checkout-section {
            display: flex;
            gap: 1.5rem;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            margin-top: 2rem;
            padding-top: 2rem;
            border-top: 2px solid #e8e9ea;
            animation: checkoutFadeIn 0.5s ease-out 0.45s both;
        }
        
        @keyframes checkoutFadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .continue-shopping {
            background: transparent;
            color: #2c5aa0;
            border: 2px solid #2c5aa0;
            padding: 1rem 2rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .continue-shopping:hover {
            background: #2c5aa0;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
        }
        
        .continue-shopping:active {
            transform: translateY(0);
        }
        
        .checkout-btn {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1.125rem 2.5rem;
            border-radius: 10px;
            font-weight: 700;
            font-size: 1.1rem;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
            position: relative;
            overflow: hidden;
        }
        
        .checkout-btn::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
            transition: left 0.5s ease;
        }
        
        .checkout-btn:hover::before {
            left: 100%;
        }
        
        .checkout-btn:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 20px rgba(44, 90, 160, 0.4);
        }
        
        .checkout-btn:active {
            transform: translateY(-1px);
        }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 992px) {
            .cart-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .cart-hero h1 {
                font-size: 2.5rem;
            }
            
            .cart-hero p {
                font-size: 1.1rem;
            }
            
            .cart-content {
                padding: 2rem;
            }
        }
        
        @media (max-width: 768px) {
            .cart-hero {
                padding: 6rem 1rem 3rem;
                margin-top: 0;
            }
            
            .cart-hero h1 {
                font-size: 2.5rem;
            }
            
            .cart-hero p {
                font-size: 1rem;
            }
            
            .cart-container {
                margin: -2rem auto 2rem;
                padding: 0 1rem;
            }
            
            .cart-content {
                padding: 1.5rem;
            }
            
            .cart-item {
                grid-template-columns: 1fr;
                gap: 1rem;
                text-align: center;
                padding: 1.25rem;
            }
            
            .item-image {
                margin: 0 auto;
                width: 100px;
                height: 100px;
            }
            
            .item-details {
                text-align: center;
            }
            
            .item-price {
                font-size: 1.4rem;
                margin: 0.5rem 0;
            }
            
            .quantity-controls {
                justify-content: center;
                margin: 0.5rem 0;
            }
            
            .remove-btn {
                width: 100%;
                margin-top: 0.5rem;
            }
            
            .cart-summary {
                padding: 1.5rem;
            }
            
            .promo-section {
                padding: 1.5rem;
            }
            
            .promo-input {
                flex-direction: column;
            }
            
            .promo-input input {
                width: 100%;
            }
            
            .apply-btn {
                width: 100%;
            }
            
            .checkout-section {
                flex-direction: column;
                gap: 1rem;
            }
            
            .continue-shopping,
            .checkout-btn {
                width: 100%;
                justify-content: center;
            }
        }
        
        @media (max-width: 576px) {
            .cart-hero {
                padding: 5rem 0.75rem 2.5rem;
            }
            
            .cart-hero h1 {
                font-size: 2rem;
            }
            
            .cart-hero p {
                font-size: 0.95rem;
            }
            
            .cart-content {
                padding: 1rem;
            }
            
            .cart-items h2 {
                font-size: 1.4rem;
            }
            
            .cart-summary h3 {
                font-size: 1.3rem;
            }
            
            .summary-row {
                font-size: 0.95rem;
            }
            
            .summary-row:last-child {
                font-size: 1.2rem;
            }
        }
        
        /* Reduce animations on reduced motion preference */
        @media (prefers-reduced-motion: reduce) {
            .cart-hero,
            .cart-hero::before,
            .cart-hero h1,
            .cart-hero p,
            .cart-content,
            .cart-item,
            .cart-summary,
            .promo-section,
            .checkout-section {
                animation-duration: 0.01ms !important;
                animation-iteration-count: 1 !important;
            }
            
            .cart-item:hover,
            .checkout-btn:hover,
            .continue-shopping:hover,
            .remove-btn:hover,
            .quantity-btn:hover {
                transform: none !important;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero Section -->
    <section class="cart-hero">
        <div class="container">
            <h1>Your Shopping Cart</h1>
            <p>Review your selected items and proceed to checkout</p>
        </div>
    </section>

    <!-- Cart Content -->
    <div class="cart-container">
        <div class="cart-content">
            <asp:Panel ID="pnlEmptyCart" runat="server" CssClass="cart-empty" Visible="false">
                <h2>Your cart is empty</h2>
                <p>Looks like you haven't added any items to your cart yet.</p>
                <asp:LinkButton ID="btnStartShopping" runat="server" CssClass="checkout-btn" OnClick="btnStartShopping_Click">
                    Start Shopping
                </asp:LinkButton>
            </asp:Panel>

            <asp:Panel ID="pnlCartItems" runat="server">
                <div class="cart-items">
                    <h2>Shopping Cart (<asp:Literal ID="litItemCount" runat="server" /> items)</h2>
                    
                    
                    <!-- Dynamic Cart Items -->
                    <asp:Repeater ID="rptCartItems" runat="server" OnItemCommand="rptCartItems_ItemCommand">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfProductId" runat="server" Value='<%# Eval("ProductId") %>' />
                            <asp:HiddenField ID="hfCartItemId" runat="server" Value='<%# Eval("CartItemId") %>' />
                            <div class="cart-item">
                                <div class="item-image">
                                    <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' AlternateText='<%# Eval("ProductName") %>' />
                                </div>
                                <div class="item-details">
                                    <h3><%# Eval("ProductName") %></h3>
                                    <p>Brand: <%# Eval("Brand") %></p>
                                    <p>Category: <%# Eval("Category") %></p>
                                </div>
                                <div class="item-price">R <%# Eval("Price", "{0:F2}") %></div>
                                <div class="quantity-controls">
                                    <asp:Button ID="btnQtyMinus" runat="server" Text="-" CssClass="quantity-btn" 
                                        CommandName="DecreaseQuantity" CommandArgument='<%# Eval("CartItemId") %>' OnClientClick="return changeQty(this, -1);" />
                                    <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Eval("Quantity") %>' CssClass="quantity-input" 
                                        OnTextChanged="txtQuantity_TextChanged" AutoPostBack="true" />
                                    <asp:Button ID="btnQtyPlus" runat="server" Text="+" CssClass="quantity-btn" 
                                        CommandName="IncreaseQuantity" CommandArgument='<%# Eval("CartItemId") %>' OnClientClick="return changeQty(this, 1);" />
                                </div>
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="remove-btn" 
                                    CommandName="RemoveItem" CommandArgument='<%# Eval("CartItemId") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Promo Code Section -->
                <div class="promo-section">
                    <h3>Have a Promo Code?</h3>
                    <div class="promo-input">
                        <asp:TextBox ID="txtPromoCode" runat="server" placeholder="Enter promo code" />
                        <asp:Button ID="btnApplyPromo" runat="server" Text="Apply" CssClass="apply-btn" OnClick="btnApplyPromo_Click" />
                    </div>
                    <asp:Label ID="lblPromoMessage" runat="server" Visible="false" />
                </div>

                <!-- Cart Summary -->
                <div class="cart-summary">
                    <h3>Order Summary</h3>
                    <div class="summary-row">
                        <span>Subtotal (<asp:Literal ID="litSubtotalItems" runat="server" /> items):</span>
                        <span>R <asp:Literal ID="litSubtotal" runat="server" /></span>
                    </div>
                    <div class="summary-row">
                        <span>Shipping:</span>
                        <span>R <asp:Literal ID="litShipping" runat="server" /></span>
                    </div>
                    <div class="summary-row" id="discountRow" runat="server" visible="false">
                        <span>Discount (<asp:Literal ID="litDiscountCode" runat="server" />):</span>
                        <span>-R <asp:Literal ID="litDiscount" runat="server" /></span>
                    </div>
                    <div class="summary-row">
                        <span>Total:</span>
                        <span>R <asp:Literal ID="litTotal" runat="server" /></span>
                    </div>
                </div>

                <!-- Checkout Section -->
                <div class="checkout-section">
                    <asp:LinkButton ID="btnContinueShopping" runat="server" CssClass="continue-shopping" OnClick="btnContinueShopping_Click">
                        Continue Shopping
                    </asp:LinkButton>
                    <asp:Button ID="btnProceedToCheckout" runat="server" Text="Proceed to Checkout" CssClass="checkout-btn" OnClick="btnProceedToCheckout_Click" />
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>

