<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="Emonti_Optometrist_Website.Checkout" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ===== FONT AWESOME ICON FIXES ===== */
        .fas, .far, .fab, .fal, .fad {
            font-family: "Font Awesome 6 Free", "FontAwesome", sans-serif;
            font-weight: 900;
            font-style: normal;
            display: inline-block;
        }
        
        /* Ensure icons in buttons display properly */
        .btn-back i,
        .btn-place-order i,
        .alert-info i {
            font-family: "Font Awesome 6 Free", "FontAwesome", sans-serif;
            font-weight: 900;
            display: inline-block;
        }
        
        /* ===== ANIMATIONS ===== */
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
        
        @keyframes fadeIn {
            from {
                opacity: 0;
            }
            to {
                opacity: 1;
            }
        }
        
        @keyframes slideInRight {
            from {
                opacity: 0;
                transform: translateX(-20px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }
        
        @keyframes pulse {
            0%, 100% {
                transform: scale(1);
            }
            50% {
                transform: scale(1.05);
            }
        }
        
        @keyframes shimmer {
            0% {
                background-position: -1000px 0;
            }
            100% {
                background-position: 1000px 0;
            }
        }
        
        @keyframes checkmark {
            0% {
                transform: scale(0);
            }
            50% {
                transform: scale(1.2);
            }
            100% {
                transform: scale(1);
            }
        }
        
        /* ===== HERO SECTION ===== */
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
        .checkout-hero {
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
        
        .checkout-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: rgba(0, 0, 0, 0.1);
            z-index: 1;
        }
        
        .checkout-hero .container {
            position: relative;
            z-index: 2;
            animation: fadeInUp 1s ease-out;
        }
        
        .checkout-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem);
            margin-bottom: 1.5rem;
            font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
            letter-spacing: -0.5px;
        }
        
        .checkout-hero p {
            font-size: clamp(1rem, 2.5vw, 1.3rem);
            max-width: 700px;
            margin: 0 auto;
            line-height: 1.8;
            opacity: 0.95;
        }
        
        /* ===== CONTAINER ===== */
        .checkout-container {
            max-width: 1200px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            animation: fadeInUp 0.8s ease-out 0.3s both;
        }
        
        .checkout-content {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.12), 0 0 0 1px rgba(0,0,0,0.05);
            padding: 3rem;
            position: relative;
            transition: box-shadow 0.3s ease;
        }
        
        .checkout-content:hover {
            box-shadow: 0 25px 70px rgba(0,0,0,0.15), 0 0 0 1px rgba(0,0,0,0.05);
        }
        
        /* ===== CHECKOUT STEPS ===== */
        .checkout-steps {
            display: flex;
            justify-content: center;
            margin-bottom: 3rem;
            gap: 1.5rem;
            flex-wrap: wrap;
            position: relative;
        }
        
        .checkout-steps::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 10%;
            right: 10%;
            height: 2px;
            background: linear-gradient(to right, #28a745 0%, #2c5aa0 50%, #e0e0e0 50%);
            z-index: 0;
            transform: translateY(-50%);
        }
        
        .step {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.75rem;
            padding: 1rem 1.5rem;
            border-radius: 30px;
            font-weight: 600;
            text-align: center;
            position: relative;
            z-index: 1;
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            cursor: default;
            background: white;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
        }
        
        .step:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.12);
        }
        
        .step.active {
            background: linear-gradient(135deg, #2c5aa0, #3d6bb3);
            color: white;
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.3);
            animation: pulse 2s ease-in-out infinite;
        }
        
        .step.completed {
            background: linear-gradient(135deg, #28a745, #34ce57);
            color: white;
            box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
        }
        
        
        .step.pending {
            background: #f8f9fa;
            color: #666;
            border: 2px solid #e0e0e0;
        }
        
        .step-number {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            font-size: 0.9rem;
            background: rgba(255,255,255,0.2);
            transition: all 0.3s ease;
            position: relative;
        }
        
        /* Step numbers for each step */
        .step.completed .step-number::before {
            content: '\f00c'; /* Font Awesome check icon (fa-check) */
            font-family: "Font Awesome 6 Free", "FontAwesome", Arial, sans-serif;
            font-weight: 900;
            font-size: 1rem;
            color: white;
            animation: checkmark 0.5s ease-out;
        }
        
        /* Fallback: Use Unicode checkmark if Font Awesome fails */
        .step.completed .step-number {
            position: relative;
        }
        
        /* This will only show if ::before content doesn't render properly */
        .step.completed .step-number::after {
            content: '✓';
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-family: Arial, sans-serif;
            font-size: 1.2rem;
            color: white;
            font-weight: bold;
            opacity: 0;
            pointer-events: none;
        }
        
        .step.active .step-number::before {
            content: '2';
            color: white;
            font-size: 0.9rem;
            font-family: Arial, sans-serif;
            font-weight: bold;
        }
        
        .step.pending .step-number::before {
            content: '3';
            color: #666;
            font-size: 0.9rem;
            font-family: Arial, sans-serif;
            font-weight: bold;
        }
        
        /* Ensure step numbers display properly */
        .step-number::before {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 100%;
            height: 100%;
        }
        
        .step.active .step-number {
            background: rgba(255,255,255,0.25);
            box-shadow: 0 0 0 4px rgba(255,255,255,0.2);
        }
        
        .step.active .step-number::before {
            color: white;
        }
        
        .step.pending .step-number::before {
            color: #666;
        }
        
        .step span {
            text-align: center;
            white-space: nowrap;
        }
        
        /* ===== SECTIONS ===== */
        .checkout-section {
            margin-bottom: 3rem;
            opacity: 0;
            transform: translateY(20px);
            transition: opacity 0.6s ease-out, transform 0.6s ease-out;
        }
        
        .checkout-section.animate-in {
            opacity: 1;
            transform: translateY(0);
        }
        
        .section-title {
            font-size: 1.75rem;
            color: #2c5aa0;
            margin-bottom: 1.5rem;
            font-weight: 700;
            border-bottom: 3px solid #e0e0e0;
            padding-bottom: 0.75rem;
            position: relative;
            display: inline-block;
            width: 100%;
        }
        
        .section-title::after {
            content: '';
            position: absolute;
            bottom: -3px;
            left: 0;
            width: 60px;
            height: 3px;
            background: linear-gradient(90deg, #2c5aa0, #667eea);
            border-radius: 2px;
            transition: width 0.3s ease;
        }
        
        .checkout-section:hover .section-title::after {
            width: 120px;
        }
        
        /* ===== FORM STYLING ===== */
        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
            gap: 1.5rem;
            margin-bottom: 1.5rem;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
            position: relative;
        }
        
        .form-group label {
            font-weight: 600;
            color: #333;
            margin-bottom: 0.75rem;
            font-size: 0.95rem;
            transition: color 0.3s ease;
        }
        
        .form-group:focus-within label {
            color: #2c5aa0;
        }
        
        .form-group input, 
        .form-group select, 
        .form-group textarea {
            padding: 1rem 1.25rem;
            border: 2px solid #e0e0e0;
            border-radius: 10px;
            font-size: 1rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            background: #fff;
            font-family: inherit;
        }
        
        .form-group input:hover,
        .form-group select:hover,
        .form-group textarea:hover {
            border-color: #c0c0c0;
        }
        
        .form-group input:focus, 
        .form-group select:focus, 
        .form-group textarea:focus {
            border-color: #2c5aa0;
            outline: none;
            box-shadow: 0 0 0 4px rgba(44, 90, 160, 0.1);
            transform: translateY(-1px);
        }
        
        .form-group input:valid:not(:placeholder-shown):not([readonly]) {
            border-color: #28a745;
            background-image: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="%2328a745" stroke-width="2"><polyline points="20 6 9 17 4 12"></polyline></svg>');
            background-repeat: no-repeat;
            background-position: right 12px center;
            padding-right: 40px;
        }
        
        .required {
            color: #dc3545;
            font-weight: 700;
            margin-left: 2px;
        }
        
        /* ===== ADDRESS SELECTION ===== */
        .address-selection {
            margin-bottom: 2rem;
            padding: 2rem;
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 12px;
            border: 2px solid #e0e0e0;
            transition: all 0.3s ease;
        }
        
        .address-selection:hover {
            border-color: #2c5aa0;
            box-shadow: 0 4px 12px rgba(44, 90, 160, 0.1);
        }
        
        .address-selection label {
            font-weight: 600;
            margin-bottom: 1rem;
            display: block;
            color: #333;
            text-align: center;
        }
        
        .address-selection-buttons {
            display: flex;
            gap: 1rem;
            flex-wrap: wrap;
            justify-content: center;
            align-items: center;
        }
        
        .btn-address-option {
            background: white;
            color: #2c5aa0;
            border: 2px solid #2c5aa0;
            padding: 0.875rem 2rem;
            border-radius: 10px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 1rem;
            position: relative;
            overflow: hidden;
        }
        
        .btn-back,
        .btn-place-order {
            position: relative;
            overflow: hidden;
        }
        
        .btn-address-option::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(44, 90, 160, 0.1);
            transform: translate(-50%, -50%);
            transition: width 0.6s, height 0.6s;
        }
        
        .btn-address-option:hover::before {
            width: 300px;
            height: 300px;
        }
        
        .btn-address-option:hover {
            background: #2c5aa0;
            color: white;
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(44, 90, 160, 0.3);
        }
        
        .btn-address-option.active {
            background: linear-gradient(135deg, #2c5aa0, #3d6bb3);
            color: white;
            box-shadow: 0 4px 15px rgba(44, 90, 160, 0.4);
            border-color: #2c5aa0;
        }
        
        .btn-address-option:active {
            transform: translateY(0);
        }
        
        /* ===== ORDER SUMMARY ===== */
        .order-summary {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 12px;
            padding: 2.5rem;
            border: 2px solid #e0e0e0;
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.06);
            transition: all 0.3s ease;
        }
        
        .order-summary:hover {
            border-color: #2c5aa0;
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.06), 0 4px 12px rgba(44, 90, 160, 0.1);
        }
        
        .order-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 1rem;
            padding-bottom: 1rem;
            border-bottom: 1px solid #dee2e6;
            transition: all 0.3s ease;
            animation: fadeInUp 0.5s ease-out both;
        }
        
        .order-item:nth-child(1) { animation-delay: 0.1s; }
        .order-item:nth-child(2) { animation-delay: 0.2s; }
        .order-item:nth-child(3) { animation-delay: 0.3s; }
        .order-item:nth-child(4) { animation-delay: 0.4s; }
        
        .order-item:hover {
            padding-left: 0.5rem;
            color: #2c5aa0;
        }
        
        .order-item:last-child {
            border-bottom: none;
            font-weight: 700;
            font-size: 1.35rem;
            color: #2c5aa0;
            margin-top: 1rem;
            padding-top: 1rem;
            border-top: 2px solid #2c5aa0;
            background: linear-gradient(135deg, rgba(44, 90, 160, 0.05), transparent);
            padding: 1rem;
            border-radius: 8px;
        }
        
        /* ===== BUTTONS ===== */
        .checkout-actions {
            display: flex;
            gap: 1.5rem;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            margin-top: 3rem;
            padding-top: 2rem;
            border-top: 2px solid #e0e0e0;
        }
        
        .btn-back {
            background: transparent;
            color: #666;
            border: 2px solid #ccc;
            padding: 1rem 2.5rem;
            border-radius: 10px;
            text-decoration: none;
            font-weight: 600;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 1rem;
            display: inline-flex;
            align-items: center;
            gap: 0.5rem;
        }
        
        .btn-back:hover {
            background: #f5f5f5;
            border-color: #999;
            color: #333;
            transform: translateX(-3px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .btn-back:active {
            transform: translateX(0);
        }
        
        .btn-place-order {
            background: linear-gradient(135deg, #28a745, #20c997);
            color: white;
            border: none;
            padding: 1.125rem 3.5rem;
            border-radius: 10px;
            font-weight: 700;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-size: 1.1rem;
            position: relative;
            overflow: hidden;
            box-shadow: 0 4px 15px rgba(40, 167, 69, 0.3);
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .btn-place-order::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(255,255,255,0.3), transparent);
            transition: left 0.5s;
        }
        
        .btn-place-order:hover::before {
            left: 100%;
        }
        
        .btn-place-order:hover {
            transform: translateY(-3px);
            box-shadow: 0 8px 25px rgba(40, 167, 69, 0.5);
            background: linear-gradient(135deg, #34ce57, #28d9a3);
        }
        
        .btn-place-order:active {
            transform: translateY(-1px);
        }
        
        .btn-place-order:disabled {
            opacity: 0.6;
            cursor: not-allowed;
            transform: none;
        }
        
        /* ===== ALERTS ===== */
        .alert {
            padding: 1.25rem 1.5rem;
            border-radius: 12px;
            margin-bottom: 1.5rem;
            animation: slideInRight 0.5s ease-out, fadeIn 0.5s ease-out;
            border-left: 4px solid;
            box-shadow: 0 4px 12px rgba(0,0,0,0.1);
        }
        
        .alert-success {
            background: linear-gradient(135deg, #d1edff, #e7f5ff);
            border-left-color: #0c5460;
            color: #0c5460;
        }
        
        .alert-error {
            background: linear-gradient(135deg, #f8d7da, #ffe6e8);
            border-left-color: #721c24;
            color: #721c24;
        }
        
        .alert-info {
            background: linear-gradient(135deg, #d1ecf1, #e7f7fa);
            border: 2px solid #bee5eb;
            color: #0c5460;
            padding: 1.5rem;
            border-radius: 12px;
            border-left: 4px solid #0c5460;
            box-shadow: 0 4px 12px rgba(12, 84, 96, 0.1);
        }
        
        .alert-info i {
            margin-right: 0.75rem;
            font-size: 1.25rem;
        }
        
        .payment-notice {
            margin: 1.5rem 0;
        }
        
        .text-danger {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.5rem;
            display: block;
            animation: fadeIn 0.3s ease-out;
            font-weight: 500;
        }
        
        .form-group input:invalid:not(:placeholder-shown),
        .form-group input.is-invalid {
            border-color: #dc3545;
            box-shadow: 0 0 0 4px rgba(220, 53, 69, 0.15);
            animation: shake 0.5s ease-in-out;
        }
        
        @keyframes shake {
            0%, 100% { transform: translateX(0); }
            25% { transform: translateX(-5px); }
            75% { transform: translateX(5px); }
        }
        
        .form-group input[readonly] {
            background: linear-gradient(135deg, #f8f9fa, #ffffff);
            cursor: not-allowed;
            border-color: #dee2e6;
            opacity: 0.8;
        }
        
        .form-group input[readonly]:focus {
            border-color: #dee2e6;
            box-shadow: none;
            outline: none;
            transform: none;
        }
        
        .empty-cart {
            text-align: center;
            padding: 3rem;
            background: #f8f9fa;
            border-radius: 12px;
            border: 2px dashed #dee2e6;
        }
        
        /* ===== LOADING STATE ===== */
        .loading {
            position: relative;
            pointer-events: none;
            opacity: 0.8;
        }
        
        .loading::after {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 20px;
            height: 20px;
            margin: -10px 0 0 -10px;
            border: 3px solid #f3f3f3;
            border-top: 3px solid #2c5aa0;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        .spinner {
            display: inline-block;
            width: 16px;
            height: 16px;
            border: 2px solid rgba(255,255,255,0.3);
            border-top: 2px solid white;
            border-radius: 50%;
            animation: spin 0.8s linear infinite;
            margin-right: 8px;
            vertical-align: middle;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        
        /* ===== RIPPLE EFFECT ===== */
        .ripple {
            position: absolute;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.6);
            transform: scale(0);
            animation: ripple-animation 0.6s ease-out;
            pointer-events: none;
        }
        
        @keyframes ripple-animation {
            to {
                transform: scale(4);
                opacity: 0;
            }
        }
        
        /* ===== FIELD STATES ===== */
        .field-focused {
            transform: translateY(0);
        }
        
        .field-filled input:valid {
            border-color: #28a745;
        }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 1024px) {
            .checkout-container {
                padding: 0 1.5rem;
            }
            
            .checkout-content {
                padding: 2.5rem;
            }
            
            .checkout-steps::before {
                display: none;
            }
        }
        
        @media (max-width: 768px) {
            .checkout-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .checkout-hero h1 {
                font-size: 2.25rem;
            }
            
            .checkout-hero p {
                font-size: 1.1rem;
            }
            
            .checkout-container {
                margin: -2rem auto 3rem;
                padding: 0 1rem;
            }
            
            .checkout-content {
                padding: 2rem 1.5rem;
                border-radius: 15px;
            }
            
            .checkout-steps {
                flex-direction: column;
                align-items: stretch;
                gap: 1rem;
            }
            
            .checkout-steps::before {
                display: none;
            }
            
            .step {
                width: 100%;
                justify-content: flex-start;
                padding: 1rem 1.25rem;
            }
            
            .section-title {
                font-size: 1.5rem;
            }
            
            .form-row {
                grid-template-columns: 1fr;
                gap: 1.25rem;
            }
            
            .checkout-actions {
                flex-direction: column;
                align-items: stretch;
                gap: 1rem;
            }
            
            .btn-back,
            .btn-place-order {
                width: 100%;
                justify-content: center;
            }
            
            .order-summary {
                padding: 1.5rem;
            }
            
            .address-selection {
                padding: 1.5rem;
            }
            
            .btn-address-option {
                width: 100%;
                justify-content: center;
            }
        }
        
        @media (max-width: 480px) {
            .checkout-hero {
                padding: 5rem 1rem 2.5rem;
                margin-top: 0;
            }
            
            .checkout-hero h1 {
                font-size: 1.75rem;
            }
            
            .checkout-hero p {
                font-size: 1rem;
            }
            
            .checkout-content {
                padding: 1.5rem 1rem;
            }
            
            .section-title {
                font-size: 1.25rem;
            }
            
            .step span {
                font-size: 0.9rem;
            }
            
            .order-item:last-child {
                font-size: 1.15rem;
            }
        }
        
        /* ===== SMOOTH SCROLLING ===== */
        html {
            scroll-behavior: smooth;
        }
        
        /* ===== ACCESSIBILITY ===== */
        @media (prefers-reduced-motion: reduce) {
            *,
            *::before,
            *::after {
                animation-duration: 0.01ms !important;
                animation-iteration-count: 1 !important;
                transition-duration: 0.01ms !important;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <!-- Checkout Hero Section -->
    <section class="checkout-hero">
        <div class="container">
            <h1>Checkout</h1>
            <p>Complete your purchase and secure your order</p>
        </div>
    </section>

    <!-- Checkout Content -->
    <div class="checkout-container">
        <div class="checkout-content">
            <!-- Checkout Steps -->
            <div class="checkout-steps">
                <div class="step completed">
                    <div class="step-number"></div>
                    <span>Step 1: Cart</span>
                </div>
                <div class="step active">
                    <div class="step-number"></div>
                    <span>Step 2: Checkout</span>
                </div>
                <div class="step pending">
                    <div class="step-number"></div>
                    <span>Step 3: Confirmation</span>
                </div>
            </div>

            <asp:Panel ID="pnlMessage" runat="server" CssClass="alert" style="display: none;">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Shipping Information -->
            <div class="checkout-section">
                <h2 class="section-title">Shipping Information</h2>
                
                <!-- Address Selection Options -->
                <div class="address-selection">
                    <label>Choose Shipping Address:</label>
                    <div class="address-selection-buttons">
                        <asp:Button ID="btnUseHomeAddress" runat="server" Text="Use Home Address" 
                            CssClass="btn-address-option" OnClick="btnUseHomeAddress_Click" />
                        <asp:Button ID="btnEnterRecipientInfo" runat="server" Text="Enter Recipient Information" 
                            CssClass="btn-address-option" OnClick="btnEnterRecipientInfo_Click" />
                    </div>
                </div>
                
                <div class="form-row">
                    <div class="form-group">
                        <label>First Name <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingFirstName" runat="server" MaxLength="50" />
                        <asp:RequiredFieldValidator ID="rfvShippingFirstName" runat="server" ControlToValidate="txtShippingFirstName" 
                            ErrorMessage="First name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingFirstName" runat="server" ControlToValidate="txtShippingFirstName"
                            ValidationExpression="^[\p{L}\s\-'\.]{2,50}$" ErrorMessage="First name must be 2-50 characters and contain only letters (including accented), spaces, hyphens, apostrophes, or periods"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group">
                        <label>Last Name <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingLastName" runat="server" MaxLength="50" />
                        <asp:RequiredFieldValidator ID="rfvShippingLastName" runat="server" ControlToValidate="txtShippingLastName" 
                            ErrorMessage="Last name is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingLastName" runat="server" ControlToValidate="txtShippingLastName"
                            ValidationExpression="^[\p{L}\s\-'\.]{2,50}$" ErrorMessage="Last name must be 2-50 characters and contain only letters (including accented), spaces, hyphens, apostrophes, or periods"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Email Address <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingEmail" runat="server" TextMode="Email" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvShippingEmail" runat="server" ControlToValidate="txtShippingEmail" 
                            ErrorMessage="Email address is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingEmail" runat="server" ControlToValidate="txtShippingEmail"
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" 
                            ErrorMessage="Please enter a valid email address (e.g. name@example.com)"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvShippingEmail" runat="server" ControlToValidate="txtShippingEmail"
                            OnServerValidate="ValidateEmail" ErrorMessage="Please enter a valid email address"
                            CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
                    </div>
                    <div class="form-group">
                        <label>Phone Number <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingPhone" runat="server" MaxLength="10" placeholder="0821234567" />
                        <asp:RequiredFieldValidator ID="rfvShippingPhone" runat="server" ControlToValidate="txtShippingPhone" 
                            ErrorMessage="Phone number is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingPhone" runat="server" ControlToValidate="txtShippingPhone"
                            ValidationExpression="^0\d{9}$" ErrorMessage="Please enter a valid South African phone number (e.g. 0821234567 - 10 digits starting with 0)"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvShippingPhone" runat="server" ControlToValidate="txtShippingPhone"
                            OnServerValidate="ValidatePhoneNumber" ErrorMessage="Phone number must be 10 digits starting with 0"
                            CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>Address Line 1 <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingAddress1" runat="server" MaxLength="150" placeholder="Street number and name, or complex name" />
                        <asp:RequiredFieldValidator ID="rfvShippingAddress1" runat="server" ControlToValidate="txtShippingAddress1" 
                            ErrorMessage="Address line 1 is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingAddress1" runat="server" ControlToValidate="txtShippingAddress1"
                            ValidationExpression="^.{5,150}$" ErrorMessage="Address must be between 5 and 150 characters"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group">
                        <label>Address Line 2</label>
                        <asp:TextBox ID="txtShippingAddress2" runat="server" MaxLength="150" placeholder="Unit number, apartment, etc. (optional)" />
                        <asp:RegularExpressionValidator ID="revShippingAddress2" runat="server" ControlToValidate="txtShippingAddress2"
                            ValidationExpression="^.{0,150}$" ErrorMessage="Address line 2 cannot exceed 150 characters"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label>City <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingCity" runat="server" MaxLength="100" placeholder="e.g. Johannesburg, Cape Town" />
                        <asp:RequiredFieldValidator ID="rfvShippingCity" runat="server" ControlToValidate="txtShippingCity" 
                            ErrorMessage="City is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingCity" runat="server" ControlToValidate="txtShippingCity"
                            ValidationExpression="^[a-zA-Z\s\-']{2,100}$" ErrorMessage="City must be 2-100 characters and contain only letters, spaces, hyphens, or apostrophes"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group">
                        <label>Postal Code <span class="required">*</span></label>
                        <asp:TextBox ID="txtShippingPostalCode" runat="server" MaxLength="4" placeholder="0000" />
                        <asp:RequiredFieldValidator ID="rfvShippingPostalCode" runat="server" ControlToValidate="txtShippingPostalCode" 
                            ErrorMessage="Postal code is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revShippingPostalCode" runat="server" ControlToValidate="txtShippingPostalCode"
                            ValidationExpression="^\d{4}$" ErrorMessage="Postal code must be exactly 4 digits (e.g. 2000)"
                            CssClass="text-danger" Display="Dynamic"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="cvShippingPostalCode" runat="server" ControlToValidate="txtShippingPostalCode"
                            OnServerValidate="ValidatePostalCode" ErrorMessage="Postal code must be 4 digits"
                            CssClass="text-danger" Display="Dynamic"></asp:CustomValidator>
                    </div>
                </div>
            </div>

            <!-- Payment Information -->
            <div class="checkout-section">
                <h2 class="section-title">Payment Information</h2>
                <div class="payment-notice">
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle"></i>
                        <strong>Secure Payment Processing</strong><br>
                        Your payment will be processed securely through Paystack. You'll be redirected to a secure payment page to complete your transaction.
                    </div>
                </div>
            </div>

            <!-- Order Summary -->
            <div class="checkout-section">
                <h2 class="section-title">Order Summary</h2>
                
                <!-- Order Items Panel -->
                <asp:Panel ID="pnlOrderSummary" runat="server" Visible="false">
                    <div class="order-summary">
                        <asp:Repeater ID="rptOrderItems" runat="server">
                            <ItemTemplate>
                                <div class="order-item">
                                    <span><%# Eval("ProductName") %> (Qty: <%# Eval("Quantity") %>)</span>
                                    <span>R <%# Eval("Subtotal", "{0:F2}") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="order-item">
                            <span>Subtotal (<asp:Literal ID="litItemCount" runat="server"></asp:Literal> items):</span>
                            <span>R <asp:Literal ID="litSubtotal" runat="server"></asp:Literal></span>
                        </div>
                        <div class="order-item">
                            <span>Shipping:</span>
                            <span>R <asp:Literal ID="litShipping" runat="server"></asp:Literal></span>
                        </div>
                        <asp:Panel ID="pnlDiscount" runat="server" Visible="false">
                            <div class="order-item">
                                <span>Discount (<asp:Literal ID="litDiscountCode" runat="server"></asp:Literal>):</span>
                                <span>-R <asp:Literal ID="litDiscount" runat="server"></asp:Literal></span>
                            </div>
                        </asp:Panel>
                        <div class="order-item">
                            <span>Total:</span>
                            <span>R <asp:Literal ID="litTotal" runat="server"></asp:Literal></span>
                        </div>
                    </div>
                </asp:Panel>
                
                <!-- Empty Cart Panel -->
                <asp:Panel ID="pnlEmptyCart" runat="server" Visible="false">
                    <div class="empty-cart">
                        <h3>Your cart is empty</h3>
                        <p>Please add some items to your cart before proceeding to checkout.</p>
                        <asp:LinkButton ID="btnContinueShopping" runat="server" CssClass="btn-primary" PostBackUrl="~/Shop.aspx">
                            Continue Shopping
                        </asp:LinkButton>
                    </div>
                </asp:Panel>
            </div>

            <!-- Checkout Actions -->
            <div class="checkout-actions">
                <asp:LinkButton ID="btnBackToCart" runat="server" CssClass="btn-back" OnClick="btnBackToCart_Click">
                    <i class="fas fa-arrow-left" style="margin-right: 0.5rem;"></i> Back to Cart
                </asp:LinkButton>
                <asp:Button ID="btnPlaceOrder" runat="server" Text="Pay with Paystack" CssClass="btn-place-order" OnClientClick="return payWithPaystack();" />
            </div>
        </div>
    </div>

    <!-- Simple Paystack Integration -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://js.paystack.co/v1/inline.js"></script>
    <script>
        // ===== ENHANCED INTERACTIONS & ANIMATIONS =====
        $(document).ready(function() {
            // Add smooth animations to form fields
            $('.form-group input, .form-group select, .form-group textarea').on('focus', function() {
                $(this).closest('.form-group').addClass('field-focused');
            }).on('blur', function() {
                $(this).closest('.form-group').removeClass('field-focused');
                // Validate on blur
                if ($(this).val().trim() !== '') {
                    $(this).closest('.form-group').addClass('field-filled');
                } else {
                    $(this).closest('.form-group').removeClass('field-filled');
                }
            });
            
            // Add ripple effect to buttons
            $('.btn-address-option, .btn-place-order, .btn-back').on('click', function(e) {
                const button = $(this);
                const ripple = $('<span class="ripple"></span>');
                const rect = this.getBoundingClientRect();
                const size = Math.max(rect.width, rect.height);
                const x = e.clientX - rect.left - size / 2;
                const y = e.clientY - rect.top - size / 2;
                
                ripple.css({
                    width: size,
                    height: size,
                    left: x + 'px',
                    top: y + 'px'
                });
                
                button.append(ripple);
                
                setTimeout(function() {
                    ripple.remove();
                }, 600);
            });
            
            // Animate order items on load
            $('.order-item').each(function(index) {
                $(this).css({
                    'animation-delay': (index * 0.1) + 's'
                });
            });
            
            // Smooth scroll to errors
            $('.text-danger').on('DOMNodeInserted', function() {
                if ($(this).is(':visible')) {
                    $('html, body').animate({
                        scrollTop: $(this).offset().top - 100
                    }, 500);
                }
            });
            
            // Add loading state to place order button
            $('.btn-place-order').on('click', function() {
                const btn = $(this);
                if (!btn.prop('disabled')) {
                    btn.addClass('loading');
                    btn.prop('disabled', true);
                    btn.data('original-text', btn.text());
                    btn.html('<span class="spinner"></span> Processing...');
                }
            });
            
            // Address option button active state management
            $('.btn-address-option').on('click', function() {
                $('.btn-address-option').removeClass('active');
                $(this).addClass('active');
            });
            
            // Intersection Observer for scroll animations
            if ('IntersectionObserver' in window) {
                const observer = new IntersectionObserver(function(entries) {
                    entries.forEach(function(entry) {
                        if (entry.isIntersecting) {
                            entry.target.classList.add('animate-in');
                            observer.unobserve(entry.target); // Only animate once
                        }
                    });
                }, {
                    threshold: 0.1,
                    rootMargin: '0px 0px -50px 0px'
                });
                
                document.querySelectorAll('.checkout-section').forEach(function(section) {
                    // Check if section is already visible on load
                    const rect = section.getBoundingClientRect();
                    const isVisible = rect.top < window.innerHeight && rect.bottom > 0;
                    
                    if (isVisible) {
                        // Animate immediately if already visible
                        setTimeout(function() {
                            section.classList.add('animate-in');
                        }, 100);
                    } else {
                        // Observe for scroll
                        observer.observe(section);
                    }
                });
            } else {
                // Fallback for browsers without IntersectionObserver
                $('.checkout-section').each(function(index) {
                    const section = $(this);
                    setTimeout(function() {
                        section.addClass('animate-in');
                    }, index * 200);
                });
            }
            
            // Add number formatting to order totals
            $('.order-item span:last-child').each(function() {
                const text = $(this).text();
                if (text.includes('R')) {
                    // Already formatted, skip
                    return;
                }
            });
            
            // Check if Font Awesome loaded properly for step icons
            setTimeout(function() {
                const completedStep = document.querySelector('.step.completed .step-number');
                if (completedStep) {
                    const computedStyle = window.getComputedStyle(completedStep, ':before');
                    const content = computedStyle.getPropertyValue('content');
                    const fontFamily = computedStyle.getPropertyValue('font-family');
                    
                    // If Font Awesome didn't load, show fallback checkmark
                    if (!fontFamily.includes('Font Awesome') || content === 'none' || content === '""') {
                        const afterStyle = window.getComputedStyle(completedStep, ':after');
                        if (afterStyle.getPropertyValue('opacity') === '0') {
                            // Show the fallback checkmark
                            $(completedStep).css({
                                '--fallback-opacity': '1'
                            });
                            const style = document.createElement('style');
                            style.textContent = '.step.completed .step-number::after { opacity: 1 !important; }';
                            document.head.appendChild(style);
                        }
                    }
                }
            }, 1000);
        });
        
        // Client-side validation functions for South African context
        function validateSouthAfricanPhone(phone) {
            // South African phone: 10 digits starting with 0
            const phoneRegex = /^0\d{9}$/;
            return phoneRegex.test(phone);
        }
        
        function validateSouthAfricanPostalCode(postalCode) {
            // South African postal code: exactly 4 digits
            const postalRegex = /^\d{4}$/;
            return postalRegex.test(postalCode);
        }
        
        function validateEmail(email) {
            const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
            return emailRegex.test(email);
        }
        
        function validateName(name) {
            // Name: 2-50 characters, letters (including accented), spaces, hyphens, apostrophes, periods
            // Supports names like: O'Brien, José, François, Van der Merwe, Jr., III, etc.
            const nameRegex = /^[\p{L}\s\-'\.]{2,50}$/u;
            return nameRegex.test(name);
        }
        
        function validateAddress(address) {
            // Address: 5-150 characters
            return address && address.trim().length >= 5 && address.trim().length <= 150;
        }
        
        function validateCity(city) {
            // City: 2-100 characters, letters, spaces, hyphens, apostrophes
            const cityRegex = /^[a-zA-Z\s\-']{2,100}$/;
            return cityRegex.test(city);
        }
        
        function validateRecipientInfo() {
            // Check if "Use Home Address" button is active
            const useHomeAddressBtn = document.getElementById('<%= btnUseHomeAddress.ClientID %>');
            if (useHomeAddressBtn && useHomeAddressBtn.classList.contains('active')) {
                // Skip validation if home address is selected (data from database, assumed correct)
                return true;
            }
            
            // Continue with validation for recipient information
            const firstName = document.getElementById('<%= txtShippingFirstName.ClientID %>').value.trim();
            const lastName = document.getElementById('<%= txtShippingLastName.ClientID %>').value.trim();
            const email = document.getElementById('<%= txtShippingEmail.ClientID %>').value.trim();
            const phone = document.getElementById('<%= txtShippingPhone.ClientID %>').value.trim();
            const address1 = document.getElementById('<%= txtShippingAddress1.ClientID %>').value.trim();
            const city = document.getElementById('<%= txtShippingCity.ClientID %>').value.trim();
            const postalCode = document.getElementById('<%= txtShippingPostalCode.ClientID %>').value.trim();
            
            let errors = [];
            
            if (!firstName || !validateName(firstName)) {
                errors.push('First name must be 2-50 characters and contain only letters (including accented), spaces, hyphens, apostrophes, or periods.');
            }
            
            if (!lastName || !validateName(lastName)) {
                errors.push('Last name must be 2-50 characters and contain only letters (including accented), spaces, hyphens, apostrophes, or periods.');
            }
            
            if (!email || !validateEmail(email)) {
                errors.push('Please enter a valid email address (e.g. name@example.com).');
            }
            
            if (!phone || !validateSouthAfricanPhone(phone)) {
                errors.push('Please enter a valid South African phone number (10 digits starting with 0, e.g. 0821234567).');
            }
            
            if (!address1 || !validateAddress(address1)) {
                errors.push('Address line 1 must be between 5 and 150 characters.');
            }
            
            if (!city || !validateCity(city)) {
                errors.push('City must be 2-100 characters and contain only letters, spaces, hyphens, or apostrophes.');
            }
            
            if (!postalCode || !validateSouthAfricanPostalCode(postalCode)) {
                errors.push('Postal code must be exactly 4 digits (e.g. 2000).');
            }
            
            if (errors.length > 0) {
                // Display errors in the message panel instead of alert
                const messagePanel = document.getElementById('<%= pnlMessage.ClientID %>');
                const messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
                
                if (messagePanel && messageLabel) {
                    messageLabel.innerHTML = '<strong>Please correct the following errors:</strong><ul style="margin: 0.5rem 0; padding-left: 1.5rem;">' + 
                        errors.map(err => '<li>' + err + '</li>').join('') + '</ul>';
                    messagePanel.className = 'alert alert-error';
                    messagePanel.style.display = 'block';
                    messagePanel.style.visibility = 'visible';
                    
                    // Scroll to the message panel
                    messagePanel.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
                }
                return false;
            }
            
            // Clear any previous error messages
            const messagePanel = document.getElementById('<%= pnlMessage.ClientID %>');
            if (messagePanel) {
                messagePanel.style.display = 'none';
            }
            
            return true;
        }
        
        // ===== PAYSTACK CONFIGURATION =====
        const PAYSTACK_CONFIG = {
            publicKey: '<%= System.Configuration.ConfigurationManager.AppSettings["PaystackPublicKey"] ?? "pk_test_a75d2d4cb1a8e7173cf1e64dddcc52c1a29104bb" %>',
            currency: 'ZAR',
            // Paystack requires amounts in the smallest currency unit (cents for ZAR)
            // So we multiply ZAR amount by 100 to convert to cents
            convertToSmallestUnit: function(amount) {
                return Math.round(amount * 100);
            }
        };

        /**
         * Initialize Paystack payment
         * @returns {boolean} Returns false to prevent form submission
         */
        function payWithPaystack() {
            try {
                // Validate recipient information first
                if (!validateRecipientInfo()) {
                    return false;
                }
                
                // Get order total from the page
                const orderTotal = getOrderTotal();
                if (orderTotal <= 0) {
                    showPaymentError('Invalid order total. Please refresh the page and try again.');
                    return false;
                }
                
                // Get customer information
                const customerEmail = getCustomerEmail();
                const customerName = getCustomerName();
                
                if (!customerEmail || !customerName) {
                    showPaymentError('Please provide valid customer information.');
                    return false;
                }
                
                // Generate unique reference
                const orderRef = generateOrderReference();
                
                // Convert amount to smallest currency unit (cents for ZAR)
                const amountInCents = PAYSTACK_CONFIG.convertToSmallestUnit(orderTotal);
                
                console.log('Payment Details:', {
                    orderTotal: orderTotal,
                    amountInCents: amountInCents,
                    customerEmail: customerEmail,
                    customerName: customerName,
                    orderRef: orderRef
                });
                
                // Initialize Paystack payment handler
                const handler = PaystackPop.setup({
                    key: PAYSTACK_CONFIG.publicKey,
                    email: customerEmail,
                    amount: amountInCents,
                    currency: PAYSTACK_CONFIG.currency,
                    ref: orderRef,
                    metadata: {
                        customer_name: customerName,
                        order_reference: orderRef,
                        order_total: orderTotal.toString()
                    },
                    callback: function(response) {
                        console.log('Payment successful:', response);
                        handlePaymentSuccess(response, orderTotal);
                    },
                    onClose: function() {
                        console.log('Payment cancelled by user');
                        handlePaymentCancelled();
                    }
                });
                
                handler.openIframe();
                return false;
                
            } catch (error) {
                console.error('Paystack initialization error:', error);
                showPaymentError('Error initializing payment. Please try again.');
                return false;
            }
        }

        /**
         * Get customer email from form
         * @returns {string} Customer email address
         */
        function getCustomerEmail() {
            const emailElement = document.getElementById('<%= txtShippingEmail.ClientID %>');
            return emailElement ? emailElement.value.trim() : '';
        }

        /**
         * Get customer full name from form
         * @returns {string} Customer full name
         */
        function getCustomerName() {
            const firstNameElement = document.getElementById('<%= txtShippingFirstName.ClientID %>');
            const lastNameElement = document.getElementById('<%= txtShippingLastName.ClientID %>');
            const firstName = firstNameElement ? firstNameElement.value.trim() : '';
            const lastName = lastNameElement ? lastNameElement.value.trim() : '';
            return (firstName + ' ' + lastName).trim();
        }

        /**
         * Generate unique order reference
         * @returns {string} Unique order reference
         */
        function generateOrderReference() {
            const timestamp = Date.now();
            const random = Math.random().toString(36).substring(2, 11);
            return 'EL_' + timestamp + '_' + random;
        }

        /**
         * Handle successful payment
         * @param {object} response - Paystack response object
         * @param {number} orderTotal - Original order total in ZAR
         */
        function handlePaymentSuccess(response, orderTotal) {
            if (response && response.reference) {
                // Redirect to order confirmation page
                const redirectUrl = 'OrderConfirmation.aspx?ref=' + encodeURIComponent(response.reference) + 
                                   '&amount=' + encodeURIComponent(orderTotal);
                window.location.href = redirectUrl;
            } else {
                showPaymentError('Payment was successful but no reference was returned. Please contact support.');
            }
        }

        /**
         * Handle payment cancellation
         */
        function handlePaymentCancelled() {
            // Re-enable the payment button
            const paymentButton = document.querySelector('.btn-place-order');
            if (paymentButton) {
                paymentButton.disabled = false;
                paymentButton.classList.remove('loading');
            }
            // Show user-friendly message
            showPaymentError('Payment was cancelled. You can try again when ready.');
        }

        /**
         * Show payment error message
         * @param {string} message - Error message to display
         */
        function showPaymentError(message) {
            const messagePanel = document.getElementById('<%= pnlMessage.ClientID %>');
            const messageLabel = document.getElementById('<%= lblMessage.ClientID %>');
            
            if (messagePanel && messageLabel) {
                messageLabel.innerHTML = '<strong>Payment Error:</strong><br>' + message;
                messagePanel.className = 'alert alert-error';
                messagePanel.style.display = 'block';
                messagePanel.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            } else {
                alert(message);
            }
        }
        
        /**
         * Extract numeric value from currency text
         * @param {string} text - Text containing currency value (e.g., "R 235.00" or "R1,500.50")
         * @returns {number} Parsed numeric value or NaN
         */
        function parseCurrencyValue(text) {
            if (!text) return NaN;
            // Remove currency symbols, spaces, and commas, then parse
            const cleaned = text.replace(/[R\s,]/g, '');
            return parseFloat(cleaned);
        }

        /**
         * Get order total from the total element
         * @returns {number|null} Order total or null if not found
         */
        function getOrderTotalFromElement() {
            const totalElement = document.getElementById('<%= litTotal.ClientID %>');
            if (!totalElement) return null;
            
            const totalText = totalElement.textContent || totalElement.innerText;
            const total = parseCurrencyValue(totalText);
            
            if (!isNaN(total) && total > 0) {
                console.log('Order total from element:', total);
                return total;
            }
            return null;
        }

        /**
         * Calculate order total from subtotal and shipping
         * @returns {number|null} Calculated total or null if calculation fails
         */
        function calculateOrderTotal() {
            const subtotalElement = document.getElementById('<%= litSubtotal.ClientID %>');
            const shippingElement = document.getElementById('<%= litShipping.ClientID %>');
            
            if (!subtotalElement || !shippingElement) return null;
            
            const subtotalText = subtotalElement.textContent || subtotalElement.innerText;
            const shippingText = shippingElement.textContent || shippingElement.innerText;
            
            const subtotal = parseCurrencyValue(subtotalText);
            const shipping = parseCurrencyValue(shippingText);
            
            if (!isNaN(subtotal) && !isNaN(shipping) && subtotal >= 0 && shipping >= 0) {
                const total = subtotal + shipping;
                console.log('Calculated order total:', total, '(Subtotal:', subtotal, '+ Shipping:', shipping, ')');
                return total;
            }
            return null;
        }

        /**
         * Extract order total from page text using regex
         * @returns {number|null} Order total or null if not found
         */
        function getOrderTotalFromPageText() {
            const pageText = document.body.innerText || document.body.textContent;
            // Match patterns like "Total: R 235.00" or "Total R1,500.50"
            const totalMatch = pageText.match(/Total[:\s]*R[\s]*([0-9,]+\.?[0-9]*)/i);
            
            if (totalMatch && totalMatch[1]) {
                const total = parseFloat(totalMatch[1].replace(/,/g, ''));
                if (!isNaN(total) && total > 0) {
                    console.log('Order total from page text:', total);
                    return total;
                }
            }
            return null;
        }

        /**
         * Get order total using multiple fallback methods
         * @returns {number} Order total in ZAR, or 0 if unable to determine
         */
        function getOrderTotal() {
            // Method 1: Get from the total element (most reliable)
            let total = getOrderTotalFromElement();
            if (total !== null) {
                return total;
            }
            
            // Method 2: Calculate from subtotal + shipping
            total = calculateOrderTotal();
            if (total !== null) {
                return total;
            }
            
            // Method 3: Extract from page text (fallback)
            total = getOrderTotalFromPageText();
            if (total !== null) {
                return total;
            }
            
            // All methods failed
            console.error('Could not determine order total using any method');
            return 0;
        }
        
        function getOrderId() {
            return '<%= Session["LastOrderId"] ?? "0" %>';
        }
        
    </script>

</asp:Content>
