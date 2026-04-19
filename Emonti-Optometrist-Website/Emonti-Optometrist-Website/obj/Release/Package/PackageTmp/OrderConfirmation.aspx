<%@ Page Title="Order Confirmation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="Emonti_Optometrist_Website.OrderConfirmation" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ===== ORDER CONFIRMATION HERO SECTION ===== */
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
        .confirmation-hero {
            background: linear-gradient(-45deg, #28a745, #20c997, #28a745, #20c997);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
        }
        
        .confirmation-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .confirmation-hero .container {
            position: relative;
            z-index: 2;
            animation: fadeInUp 1s ease-out;
        }
        
        .confirmation-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        .confirmation-hero p {
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
        
        .confirmation-hero .container {
            position: relative;
            z-index: 1;
        }
        
        .confirmation-container {
            max-width: 800px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            position: relative;
            z-index: 10;
        }
        
        .confirmation-content {
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
        
        .success-icon {
            text-align: center;
            margin-bottom: 2rem;
            animation: successPulse 1s ease-out 0.5s both;
        }
        
        @keyframes successPulse {
            0% {
                opacity: 0;
                transform: scale(0.5);
            }
            50% {
                transform: scale(1.1);
            }
            100% {
                opacity: 1;
                transform: scale(1);
            }
        }
        
        .success-icon i {
            font-size: 5rem;
            color: #28a745;
            filter: drop-shadow(0 4px 8px rgba(40, 167, 69, 0.3));
            animation: iconBounce 0.6s ease-out 0.8s both;
        }
        
        @keyframes iconBounce {
            0%, 100% {
                transform: translateY(0);
            }
            50% {
                transform: translateY(-10px);
            }
        }
        
        .order-details {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 12px;
            padding: 2.5rem;
            margin: 2rem 0;
            border: 2px solid #e8e9ea;
            box-shadow: 0 4px 12px rgba(0,0,0,0.08);
            animation: detailsSlideIn 0.5s ease-out 0.4s both;
        }
        
        @keyframes detailsSlideIn {
            from { 
                opacity: 0; 
                transform: translateX(-20px); 
            }
            to { 
                opacity: 1; 
                transform: translateX(0); 
            }
        }
        
        .order-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1.5rem;
            padding-bottom: 1rem;
            border-bottom: 2px solid #2c5aa0;
        }
        
        .order-number {
            font-size: 1.6rem;
            font-weight: 700;
            color: #2c5aa0;
        }
        
        .order-date {
            color: #666;
            font-size: 1.1rem;
            font-weight: 500;
        }
        
        .order-item {
            display: flex;
            justify-content: space-between;
            margin-bottom: 0.75rem;
            padding-bottom: 0.75rem;
            border-bottom: 1px solid #dee2e6;
            transition: all 0.2s ease;
            animation: itemFadeIn 0.4s ease-out both;
        }
        
        .order-item:nth-child(1) { animation-delay: 0.5s; }
        .order-item:nth-child(2) { animation-delay: 0.55s; }
        .order-item:nth-child(3) { animation-delay: 0.6s; }
        .order-item:nth-child(4) { animation-delay: 0.65s; }
        .order-item:nth-child(5) { animation-delay: 0.7s; }
        
        @keyframes itemFadeIn {
            from { 
                opacity: 0; 
                transform: translateX(-10px); 
            }
            to { 
                opacity: 1; 
                transform: translateX(0); 
            }
        }
        
        .order-item:hover {
            color: #2c5aa0;
            padding-left: 0.5rem;
        }
        
        .order-item:last-child {
            border-bottom: none;
            font-weight: 700;
            font-size: 1.3rem;
            color: #2c5aa0;
            padding-top: 1rem;
            margin-top: 0.5rem;
            border-top: 2px solid #2c5aa0;
        }
        
        .shipping-info {
            background: linear-gradient(135deg, rgba(40, 167, 69, 0.05) 0%, rgba(32, 201, 151, 0.05) 100%);
            border-radius: 12px;
            padding: 2rem;
            margin: 1.5rem 0;
            border: 2px solid rgba(40, 167, 69, 0.2);
            animation: infoSlideIn 0.5s ease-out 0.6s both;
        }
        
        @keyframes infoSlideIn {
            from { 
                opacity: 0; 
                transform: translateX(20px); 
            }
            to { 
                opacity: 1; 
                transform: translateX(0); 
            }
        }
        
        .info-section {
            margin-bottom: 1.5rem;
            transition: all 0.2s ease;
        }
        
        .info-section:last-child {
            margin-bottom: 0;
        }
        
        .info-section:hover {
            transform: translateX(5px);
        }
        
        .info-title {
            font-weight: 700;
            color: #2c5aa0;
            margin-bottom: 0.75rem;
            font-size: 1.1rem;
        }
        
        .confirmation-actions {
            display: flex;
            gap: 1.5rem;
            justify-content: center;
            flex-wrap: wrap;
            margin-top: 2rem;
            padding-top: 2rem;
            border-top: 2px solid #e8e9ea;
            animation: actionsFadeIn 0.5s ease-out 0.7s both;
        }
        
        @keyframes actionsFadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }
        
        .btn-primary {
            background: linear-gradient(135deg, #2c5aa0, #1e4080);
            color: white;
            border: none;
            padding: 1.125rem 2.5rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 700;
            font-size: 1.05rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
            position: relative;
            overflow: hidden;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .btn-primary::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.2), transparent);
            transition: left 0.5s ease;
        }
        
        .btn-primary:hover::before {
            left: 100%;
        }
        
        .btn-primary:hover {
            transform: translateY(-3px);
            box-shadow: 0 6px 20px rgba(44, 90, 160, 0.4);
            color: white;
        }
        
        .btn-primary:active {
            transform: translateY(-1px);
        }
        
        .btn-secondary {
            background: transparent;
            color: #2c5aa0;
            border: 2px solid #2c5aa0;
            padding: 1.125rem 2rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .btn-secondary:hover {
            background: #2c5aa0;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.3);
        }
        
        .btn-secondary:active {
            transform: translateY(0);
        }
        
        .email-notice {
            background: linear-gradient(135deg, #d1edff 0%, #bee5eb 100%);
            border: 2px solid #bee5eb;
            color: #0c5460;
            padding: 1.25rem;
            border-radius: 10px;
            margin: 1.5rem 0;
            text-align: center;
            animation: noticeFadeIn 0.5s ease-out 0.65s both;
            box-shadow: 0 2px 8px rgba(12, 84, 96, 0.1);
        }
        
        @keyframes noticeFadeIn {
            from { opacity: 0; transform: scale(0.98); }
            to { opacity: 1; transform: scale(1); }
        }
        
        .email-notice i {
            margin-right: 0.5rem;
            font-size: 1.2rem;
        }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 992px) {
            .confirmation-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .confirmation-hero h1 {
                font-size: 2.5rem;
            }
            
            .confirmation-hero p {
                font-size: 1.1rem;
            }
            
            .confirmation-content {
                padding: 2rem;
            }
        }
        
        @media (max-width: 768px) {
            .confirmation-hero {
                padding: 6rem 1rem 3rem;
                margin-top: 0;
            }
            
            .confirmation-hero h1 {
                font-size: 2.5rem;
            }
            
            .confirmation-hero p {
                font-size: 1rem;
            }
            
            .confirmation-container {
                margin: -2rem auto 2rem;
                padding: 0 1rem;
            }
            
            .confirmation-content {
                padding: 1.5rem;
            }
            
            .success-icon i {
                font-size: 4rem;
            }
            
            .order-details {
                padding: 1.5rem;
            }
            
            .order-header {
                flex-direction: column;
                align-items: flex-start;
                gap: 0.5rem;
            }
            
            .order-number {
                font-size: 1.4rem;
            }
            
            .shipping-info {
                padding: 1.5rem;
            }
            
            .confirmation-actions {
                flex-direction: column;
                gap: 1rem;
            }
            
            .btn-primary,
            .btn-secondary {
                width: 100%;
                justify-content: center;
            }
        }
        
        @media (max-width: 576px) {
            .confirmation-hero {
                padding: 5rem 0.75rem 2.5rem;
            }
            
            .confirmation-hero h1 {
                font-size: 2rem;
            }
            
            .confirmation-hero p {
                font-size: 0.95rem;
            }
            
            .confirmation-content {
                padding: 1rem;
            }
            
            .success-icon i {
                font-size: 3.5rem;
            }
            
            .order-details {
                padding: 1.25rem;
            }
            
            .order-item:last-child {
                font-size: 1.1rem;
            }
            
            .shipping-info {
                padding: 1.25rem;
            }
        }
        
        /* Reduce animations on reduced motion preference */
        @media (prefers-reduced-motion: reduce) {
            .confirmation-hero,
            .confirmation-hero::before,
            .confirmation-hero h1,
            .confirmation-hero p,
            .confirmation-content,
            .success-icon,
            .success-icon i,
            .order-details,
            .order-item,
            .shipping-info,
            .info-section,
            .email-notice,
            .confirmation-actions {
                animation-duration: 0.01ms !important;
                animation-iteration-count: 1 !important;
            }
            
            .btn-primary:hover,
            .btn-secondary:hover,
            .order-item:hover,
            .info-section:hover {
                transform: none !important;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Confirmation Hero Section -->
    <section class="confirmation-hero">
        <div class="container">
            <h1>Order Confirmed!</h1>
            <p>Thank you for your purchase. Your order has been successfully placed.</p>
        </div>
    </section>

    <!-- Confirmation Content -->
    <div class="confirmation-container">
        <div class="confirmation-content">
            <!-- Success Icon -->
            <div class="success-icon">
                <i class="fas fa-check-circle"></i>
            </div>

            <!-- Order Details -->
            <div class="order-details">
                <div class="order-header">
                    <div class="order-number">
                        <asp:Literal ID="litOrderNumber" runat="server" />
                    </div>
                    <div class="order-date">
                        <asp:Literal ID="litOrderDate" runat="server" />
                    </div>
                </div>

                <!-- Dynamic Order Items -->
                <asp:Repeater ID="rptOrderItems" runat="server">
                    <ItemTemplate>
                        <div class="order-item">
                            <span><%# Eval("Product_Name") %> (<%# Eval("Quantity") %>x)</span>
                            <span>R <%# Eval("Subtotal", "{0:F2}") %></span>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
                <div class="order-item">
                    <span>Subtotal:</span>
                    <span>R <asp:Literal ID="litSubtotal" runat="server" /></span>
                </div>
                <div class="order-item">
                    <span>Shipping:</span>
                    <span>R <asp:Literal ID="litShipping" runat="server" /></span>
                </div>
                <div class="order-item" id="discountRow" runat="server" visible="false">
                    <span>Discount:</span>
                    <span>-R <asp:Literal ID="litDiscount" runat="server" /></span>
                </div>
                <div class="order-item">
                    <span>Total:</span>
                    <span>R <asp:Literal ID="litTotal" runat="server" /></span>
                </div>
            </div>

            <!-- Shipping Information -->
            <div class="shipping-info">
                <div class="info-section">
                    <div class="info-title">Shipping Address</div>
                    <div>
                        <asp:Literal ID="litShippingAddress" runat="server" />
                    </div>
                </div>
                <div class="info-section">
                    <div class="info-title">Payment Method</div>
                    <div>
                        <asp:Literal ID="litPaymentMethod" runat="server" />
                    </div>
                </div>
                <div class="info-section">
                    <div class="info-title">Estimated Delivery</div>
                    <div>3-5 business days</div>
                </div>
            </div>

            <!-- Email Notice -->
            <div class="email-notice">
                <i class="fas fa-envelope"></i>
                A confirmation email has been sent to your email address with order details and tracking information.
            </div>

            <!-- Next Steps -->
            <div style="text-align: center; margin: 2rem 0;">
                <h3>What's Next?</h3>
                <p>You'll receive an email confirmation shortly. Once your order is processed, you'll get a tracking number to monitor your delivery.</p>
            </div>

            <!-- Confirmation Actions -->
            <div class="confirmation-actions">
                <asp:LinkButton ID="btnViewOrder" runat="server" CssClass="btn-primary" OnClick="btnViewOrder_Click">
                    <i class="fas fa-eye"></i> View Order Details
                </asp:LinkButton>
                <asp:LinkButton ID="btnContinueShopping" runat="server" CssClass="btn-secondary" OnClick="btnContinueShopping_Click">
                    <i class="fas fa-shopping-cart"></i> Continue Shopping
                </asp:LinkButton>
                <asp:LinkButton ID="btnBookAppointment" runat="server" CssClass="btn-secondary" OnClick="btnBookAppointment_Click">
                    <i class="fas fa-calendar"></i> Book Appointment
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
