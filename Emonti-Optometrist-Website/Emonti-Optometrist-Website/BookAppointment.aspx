<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.cs" Inherits="Emonti_Optometrist_Website.BookAppointment" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* ===== ANIMATION KEYFRAMES ===== */
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
        
        @keyframes slideInLeft {
            from {
                opacity: 0;
                transform: translateX(-30px);
            }
            to {
                opacity: 1;
                transform: translateX(0);
            }
        }
        
        @keyframes slideInRight {
            from {
                opacity: 0;
                transform: translateX(30px);
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
        
        @keyframes gradientShift {
            0%, 100% {
                background-position: 0% 50%;
            }
            50% {
                background-position: 100% 50%;
            }
        }
        
        @keyframes float {
            0%, 100% {
                transform: translateY(0px);
            }
            50% {
                transform: translateY(-10px);
            }
        }
        
        /* ===== HERO SECTION ===== */
        .appointment-hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            background-size: 200% 200%;
            animation: gradientShift 8s ease infinite;
            color: white;
            padding: 8rem 2rem 4rem;
            text-align: center;
            margin-top: 0;
            position: relative;
            overflow: hidden;
        }
        
        .appointment-hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: radial-gradient(circle at 20% 50%, rgba(255, 255, 255, 0.1) 0%, transparent 50%),
                        radial-gradient(circle at 80% 80%, rgba(255, 255, 255, 0.1) 0%, transparent 50%);
            animation: float 6s ease-in-out infinite;
        }
        
        .appointment-hero .container {
            position: relative;
            z-index: 1;
            animation: fadeInUp 0.8s ease-out;
        }
        
        .appointment-hero h1 {
            font-size: 3rem;
            margin-bottom: 1rem;
            animation: fadeInUp 0.8s ease-out 0.2s backwards;
            text-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
        }
        
        .appointment-hero p {
            font-size: 1.2rem;
            max-width: 600px;
            margin: 0 auto;
            animation: fadeInUp 0.8s ease-out 0.4s backwards;
            opacity: 0.95;
        }
        
        /* ===== BOOKING CONTAINER ===== */
        .booking-container {
            max-width: 1000px;
            margin: -3rem auto 4rem;
            padding: 0 2rem;
            animation: fadeInUp 1s ease-out 0.6s backwards;
        }
        
        .booking-form {
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.15),
                        0 0 0 1px rgba(102, 126, 234, 0.1);
            padding: 3rem;
            position: relative;
            overflow: hidden;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }
        
        .booking-form::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: linear-gradient(90deg, transparent, rgba(102, 126, 234, 0.05), transparent);
        }
        
        .booking-form:hover {
            transform: translateY(-5px);
            box-shadow: 0 25px 70px rgba(0, 0, 0, 0.2),
                        0 0 0 1px rgba(102, 126, 234, 0.15);
        }
        
        /* ===== FORM SECTIONS ===== */
        .form-section {
            margin-bottom: 2.5rem;
            animation: fadeIn 0.6s ease-out;
        }
        
        .form-section:nth-child(1) {
            animation-delay: 0.1s;
        }
        
        .form-section:nth-child(2) {
            animation-delay: 0.2s;
        }
        
        .form-section:nth-child(3) {
            animation-delay: 0.3s;
        }
        
        .section-title {
            font-size: 1.5rem;
            color: #667eea;
            margin-bottom: 1.5rem;
            font-weight: 700;
            padding-bottom: 0.75rem;
            position: relative;
            display: inline-block;
            width: 100%;
        }
        
        .section-title::after {
            content: '';
            position: absolute;
            bottom: 0;
            left: 0;
            width: 60px;
            height: 4px;
            background: linear-gradient(90deg, #667eea, #764ba2);
            border-radius: 2px;
            animation: slideInLeft 0.6s ease-out;
        }
        
        /* ===== FORM ROWS & GROUPS ===== */
        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
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
            color: #667eea;
        }
        
        .form-group input, 
        .form-group select, 
        .form-group textarea,
        .form-control {
            padding: 1rem 1.25rem;
            border: 2px solid #e0e0e0;
            border-radius: 12px;
            font-size: 1rem;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            background: white;
            position: relative;
            width: 100%;
            box-sizing: border-box;
        }
        
        .form-group input:hover, 
        .form-group select:hover, 
        .form-group textarea:hover,
        .form-control:hover {
            border-color: #b8c5e0;
        }
        
        .form-group input:focus, 
        .form-group select:focus, 
        .form-group textarea:focus,
        .form-control:focus {
            border-color: #667eea;
            outline: none;
            box-shadow: 0 0 0 4px rgba(102, 126, 234, 0.1),
                        0 4px 12px rgba(102, 126, 234, 0.15);
            transform: translateY(-2px);
        }
        
        .required {
            color: #dc3545;
            font-weight: 700;
            margin-left: 3px;
        }
        
        .text-danger {
            color: #dc3545;
            font-size: 0.875rem;
            margin-top: 0.5rem;
            display: block;
            animation: fadeIn 0.3s ease-out;
        }
        
        /* ===== CALENDAR SECTION ===== */
        .calendar-section {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 2rem;
        }
        
        .date-picker {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 15px;
            padding: 1.5rem;
            border: 2px solid #e9ecef;
            transition: all 0.3s ease;
            animation: slideInLeft 0.6s ease-out;
        }
        
        .date-picker:hover {
            border-color: #667eea;
            box-shadow: 0 8px 25px rgba(102, 126, 234, 0.1);
            transform: translateY(-3px);
        }
        
        .date-picker h3 {
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 1.2rem;
        }
        
        .time-slots {
            background: linear-gradient(135deg, #f8f9fa 0%, #ffffff 100%);
            border-radius: 15px;
            padding: 1.5rem;
            border: 2px solid #e9ecef;
            transition: all 0.3s ease;
            animation: slideInRight 0.6s ease-out;
        }
        
        .time-slots:hover {
            border-color: #667eea;
            box-shadow: 0 8px 25px rgba(102, 126, 234, 0.1);
            transform: translateY(-3px);
        }
        
        .time-slots h3 {
            color: #667eea;
            margin-bottom: 1rem;
            font-size: 1.2rem;
        }
        
        .selected-date {
            display: block;
            padding: 0.75rem 1rem;
            background: white;
            border-radius: 8px;
            margin-bottom: 1rem;
            font-weight: 600;
            color: #667eea;
            border: 2px solid #e0e0e0;
            transition: all 0.3s ease;
        }
        
        .time-slots-grid {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 0.75rem;
            margin-top: 1rem;
        }
        
        .time-slot {
            padding: 1rem 0.75rem;
            background: white;
            border: 2px solid #e0e0e0;
            border-radius: 12px;
            text-align: center;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            font-weight: 600;
            font-size: 0.95rem;
            position: relative;
            overflow: hidden;
        }
        
        .time-slot::before {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 0;
            height: 0;
            border-radius: 50%;
            background: rgba(102, 126, 234, 0.1);
            transform: translate(-50%, -50%);
            transition: width 0.4s ease, height 0.4s ease;
        }
        
        .time-slot:hover {
            border-color: #667eea;
            background: linear-gradient(135deg, #f0f4ff, #ffffff);
            transform: translateY(-3px) scale(1.02);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.2);
        }
        
        .time-slot:hover::before {
            width: 200px;
            height: 200px;
        }
        
        .time-slot.selected {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
            border-color: #667eea;
            transform: translateY(-3px) scale(1.05);
            box-shadow: 0 8px 25px rgba(102, 126, 234, 0.4);
            animation: pulse 2s ease-in-out infinite;
        }
        
        .time-slot.selected::before {
            background: rgba(255, 255, 255, 0.2);
            width: 200px;
            height: 200px;
        }
        
        .time-slot.unavailable {
            background: #f5f5f5;
            color: #999;
            cursor: not-allowed;
            opacity: 0.5;
            position: relative;
        }
        
        .time-slot.unavailable::after {
            content: '';
            position: absolute;
            top: 50%;
            left: 10%;
            right: 10%;
            height: 2px;
            background: #999;
            transform: translateY(-50%);
        }
        
        .time-slot input[type="radio"] {
            display: none;
        }
        
        /* ===== SUMMARY SECTION ===== */
        .summary-section {
            background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
            border-radius: 15px;
            padding: 2rem;
            margin-top: 2rem;
            border: 2px solid #e0e0e0;
            animation: fadeInUp 0.6s ease-out 0.4s backwards;
            position: relative;
            overflow: hidden;
        }
        
        .summary-section::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 4px;
            background: linear-gradient(90deg, #667eea, #764ba2);
        }
        
        .summary-section h3 {
            color: #667eea;
            margin-bottom: 1.5rem;
            font-size: 1.3rem;
        }
        
        .summary-item {
            display: flex;
            justify-content: space-between;
            margin-bottom: 1rem;
            padding-bottom: 1rem;
            border-bottom: 1px solid #dee2e6;
            transition: all 0.3s ease;
            animation: fadeIn 0.5s ease-out;
        }
        
        .summary-item:hover {
            padding-left: 0.5rem;
            color: #667eea;
        }
        
        .summary-item:last-child {
            border-bottom: none;
            font-weight: 700;
            font-size: 1.2rem;
            color: #667eea;
            padding-top: 0.5rem;
            margin-top: 0.5rem;
            border-top: 2px solid #667eea;
        }
        
        /* ===== FORM ACTIONS ===== */
        .form-actions {
            text-align: center;
            margin-top: 2.5rem;
            display: flex;
            gap: 1.5rem;
            justify-content: center;
            flex-wrap: wrap;
            animation: fadeInUp 0.6s ease-out 0.5s backwards;
        }
        
        .btn-book {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            background-size: 200% 200%;
            animation: gradientShift 3s ease infinite;
            color: white;
            padding: 1.1rem 3.5rem;
            border: none;
            border-radius: 50px;
            font-size: 1.1rem;
            font-weight: 700;
            cursor: pointer;
            transition: all 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4);
            position: relative;
            overflow: hidden;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .btn-book::before {
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
        
        .btn-book:hover::before {
            width: 300px;
            height: 300px;
        }
        
        .btn-book:hover {
            transform: translateY(-4px) scale(1.05);
            box-shadow: 0 10px 30px rgba(102, 126, 234, 0.5);
        }
        
        .btn-book:active {
            transform: translateY(-2px) scale(1.02);
        }
        
        .btn-cancel {
            background: transparent;
            color: #666;
            padding: 1.1rem 3.5rem;
            border: 2px solid #ccc;
            border-radius: 50px;
            font-size: 1.1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
        }
        
        .btn-cancel::before {
            content: '';
            position: absolute;
            top: 0;
            left: -100%;
            width: 100%;
            height: 100%;
            background: #f5f5f5;
            transition: left 0.3s ease;
            z-index: -1;
        }
        
        .btn-cancel:hover {
            border-color: #999;
            color: #333;
            transform: translateY(-2px);
        }
        
        .btn-cancel:hover::before {
            left: 0;
        }
        
        /* ===== MODAL POPUP ===== */
        .modal-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.6);
            z-index: 10000;
            animation: fadeIn 0.3s ease-out;
            backdrop-filter: blur(4px);
        }
        
        .modal-overlay.show {
            display: flex !important;
            align-items: center;
            justify-content: center;
        }
        
        /* Ensure modal is visible when show class is added */
        .modal-overlay.show {
            visibility: visible !important;
            opacity: 1 !important;
        }
        
        .modal-popup {
            background: white;
            border-radius: 20px;
            padding: 0;
            max-width: 500px;
            width: 90%;
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.3);
            animation: modalSlideIn 0.4s cubic-bezier(0.4, 0, 0.2, 1);
            position: relative;
            overflow: hidden;
            z-index: 10001;
        }
        
        @keyframes modalSlideIn {
            from {
                opacity: 0;
                transform: translateY(-50px) scale(0.9);
            }
            to {
                opacity: 1;
                transform: translateY(0) scale(1);
            }
        }
        
        .modal-header {
            padding: 2rem 2rem 1rem;
            border-bottom: 2px solid #e9ecef;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        
        .modal-icon {
            width: 60px;
            height: 60px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 2rem;
            margin-right: 1rem;
            flex-shrink: 0;
        }
        
        .modal-icon.success {
            background: linear-gradient(135deg, #28a745, #20c997);
            color: white;
        }
        
        .modal-icon.error {
            background: linear-gradient(135deg, #dc3545, #c82333);
            color: white;
        }
        
        .modal-title {
            font-size: 1.5rem;
            font-weight: 700;
            margin: 0;
            flex: 1;
        }
        
        .modal-title.success {
            color: #28a745;
        }
        
        .modal-title.error {
            color: #dc3545;
        }
        
        .modal-close {
            background: transparent;
            border: none;
            font-size: 1.5rem;
            color: #999;
            cursor: pointer;
            padding: 0.5rem;
            line-height: 1;
            transition: all 0.3s ease;
            border-radius: 50%;
            width: 36px;
            height: 36px;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        
        .modal-close:hover {
            background: #f5f5f5;
            color: #333;
            transform: rotate(90deg);
        }
        
        .modal-body {
            padding: 1.5rem 2rem 2rem;
            color: #333;
            line-height: 1.6;
            font-size: 1rem;
        }
        
        .modal-footer {
            padding: 1rem 2rem;
            border-top: 2px solid #e9ecef;
            display: flex;
            justify-content: flex-end;
            gap: 1rem;
        }
        
        .modal-btn {
            padding: 0.75rem 2rem;
            border: none;
            border-radius: 8px;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s ease;
        }
        
        .modal-btn-primary {
            background: linear-gradient(135deg, #667eea, #764ba2);
            color: white;
        }
        
        .modal-btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
        }
        
        /* Hide inline alerts */
        .alert {
            display: none !important;
        }
        
        /* ===== MODAL RESPONSIVE ===== */
        @media (max-width: 768px) {
            .modal-popup {
                max-width: 95%;
                margin: 1rem;
            }
            
            .modal-header {
                padding: 1.5rem 1.5rem 1rem;
            }
            
            .modal-icon {
                width: 50px;
                height: 50px;
                font-size: 1.5rem;
            }
            
            .modal-title {
                font-size: 1.2rem;
            }
            
            .modal-body {
                padding: 1rem 1.5rem 1.5rem;
                font-size: 0.95rem;
            }
            
            .modal-footer {
                padding: 1rem 1.5rem;
            }
        }
        
        /* ===== CALENDAR STYLING ===== */
        .appointment-calendar {
            width: 100%;
            border: none !important;
        }
        
        .appointment-calendar table {
            width: 100%;
            border-collapse: separate;
            border-spacing: 4px;
        }
        
        .appointment-calendar td,
        .appointment-calendar th {
            border-radius: 8px;
            transition: all 0.3s ease;
        }
        
        .appointment-calendar td:hover {
            background: #f0f4ff !important;
            transform: scale(1.1);
        }
        
        .appointment-calendar .today {
            background: linear-gradient(135deg, #667eea, #764ba2) !important;
            color: white !important;
            font-weight: 700;
        }
        
        .appointment-calendar .selected {
            background: linear-gradient(135deg, #667eea, #764ba2) !important;
            color: white !important;
            font-weight: 700;
            box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
        }
        
        /* ===== RESPONSIVE DESIGN ===== */
        @media (max-width: 1024px) {
            .calendar-section {
                gap: 1.5rem;
            }
            
            .time-slots-grid {
                grid-template-columns: repeat(2, 1fr);
            }
        }
        
        @media (max-width: 768px) {
            .appointment-hero {
                padding: 6rem 1.5rem 3rem;
            }
            
            .appointment-hero h1 {
                font-size: 2rem;
            }
            
            .appointment-hero p {
                font-size: 1rem;
            }
            
            .booking-container {
                margin: -2rem auto 3rem;
                padding: 0 1rem;
            }
            
            .booking-form {
                padding: 2rem 1.5rem;
                border-radius: 15px;
            }
            
            .calendar-section {
                grid-template-columns: 1fr;
                gap: 1.5rem;
            }
            
            .date-picker,
            .time-slots {
                animation: fadeInUp 0.6s ease-out;
            }
            
            .time-slots-grid {
                grid-template-columns: repeat(2, 1fr);
                gap: 0.5rem;
            }
            
            .form-row {
                grid-template-columns: 1fr;
                gap: 1rem;
            }
            
            .form-actions {
                flex-direction: column;
                align-items: stretch;
                gap: 1rem;
            }
            
            .btn-book,
            .btn-cancel {
                width: 100%;
                padding: 1rem 2rem;
            }
            
            .section-title {
                font-size: 1.3rem;
            }
        }
        
        @media (max-width: 480px) {
            .appointment-hero {
                padding: 5rem 1rem 2.5rem;
            }
            
            .appointment-hero h1 {
                font-size: 1.75rem;
            }
            
            .booking-form {
                padding: 1.5rem 1rem;
            }
            
            .time-slots-grid {
                grid-template-columns: 1fr;
            }
            
            .time-slot {
                padding: 1rem;
            }
            
            .summary-section {
                padding: 1.5rem;
            }
            
            .form-actions {
                margin-top: 2rem;
            }
        }
        
        /* ===== SCROLL REVEAL ANIMATIONS ===== */
        .scroll-reveal {
            opacity: 0;
            transform: translateY(30px);
            transition: all 0.8s cubic-bezier(0.4, 0, 0.2, 1);
        }
        
        .scroll-reveal.active {
            opacity: 1;
            transform: translateY(0);
        }
        
        /* ===== LOADING STATES ===== */
        .loading {
            position: relative;
            pointer-events: none;
            opacity: 0.6;
        }
        
        .loading::after {
            content: '';
            position: absolute;
            top: 50%;
            left: 50%;
            width: 20px;
            height: 20px;
            margin: -10px 0 0 -10px;
            border: 3px solid #667eea;
            border-top-color: transparent;
            border-radius: 50%;
            animation: spin 0.8s linear infinite;
        }
        
        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }
        
        /* ===== STEP PROGRESS INDICATOR ===== */
        .step-progress {
            display: flex;
            justify-content: center;
            gap: 1rem;
            margin-bottom: 2.5rem;
            padding: 1rem;
            background: #f8f9fa;
            border-radius: 12px;
        }
        .step-item {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            font-size: 0.85rem;
            color: #999;
            font-weight: 500;
        }
        .step-item .step-circle {
            width: 32px;
            height: 32px;
            border-radius: 50%;
            background: #e0e0e0;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 0.8rem;
            font-weight: 700;
            transition: all 0.3s ease;
        }
        .step-item.active .step-circle {
            background: linear-gradient(135deg, #667eea, #764ba2);
            box-shadow: 0 4px 12px rgba(102,126,234,0.4);
        }
        .step-item.active {
            color: #667eea;
        }
        .step-item.completed .step-circle {
            background: #28a745;
        }
        .step-item.completed {
            color: #28a745;
        }
        .step-connector {
            width: 40px;
            height: 2px;
            background: #e0e0e0;
            align-self: center;
        }
        .step-connector.completed {
            background: #28a745;
        }
        
        /* ===== BOOK BUTTON DISABLED STATE ===== */
        .btn-book.disabled {
            background: #ccc !important;
            animation: none !important;
            cursor: not-allowed !important;
            transform: none !important;
            box-shadow: none !important;
            pointer-events: none;
        }
        .btn-book.disabled:hover {
            transform: none !important;
            box-shadow: none !important;
        }

        /* ===== SECTION STATUS INDICATOR ===== */
        .section-status {
            font-size: 0.8rem;
            margin-top: 0.5rem;
            display: flex;
            align-items: center;
            gap: 0.3rem;
        }
        .section-status.valid { color: #28a745; }
        .section-status.invalid { color: #dc3545; }
        
        /* ===== NO TIME SLOTS MESSAGE ===== */
        .no-slots-message {
            text-align: center;
            padding: 2rem;
            color: #999;
            font-size: 0.95rem;
            background: white;
            border-radius: 12px;
            border: 2px dashed #e0e0e0;
        }
    </style>
    
    <!-- Modal Popup JavaScript Functions - Load Early -->
    <script type="text/javascript">
        // Define modal functions in global scope immediately
        window.showMessageModal = function(type, message) {
            try {
                var modal = document.getElementById('messageModal');
                if (!modal) {
                    modal = document.querySelector('[id*="messageModal"]') || document.querySelector('.modal-overlay');
                }
                
                if (!modal) {
                    alert('Modal not found. Message: ' + message);
                    return;
                }
                
                var modalIcon = document.getElementById('modalIcon');
                var modalIconContent = document.getElementById('modalIconContent');
                var modalTitle = document.getElementById('modalTitle');
                var modalMessage = document.getElementById('modalMessage');
                
                if (type === 'success') {
                    if (modalIcon) modalIcon.className = 'modal-icon success';
                    if (modalIconContent) modalIconContent.textContent = '✓';
                    if (modalTitle) {
                        modalTitle.className = 'modal-title success';
                        modalTitle.textContent = 'Success';
                    }
                } else if (type === 'error') {
                    if (modalIcon) modalIcon.className = 'modal-icon error';
                    if (modalIconContent) modalIconContent.textContent = '✕';
                    if (modalTitle) {
                        modalTitle.className = 'modal-title error';
                        modalTitle.textContent = 'Error';
                    }
                }
                
                if (modalMessage) {
                    modalMessage.textContent = message.replace(/[✅❌]/g, '').trim();
                }
                
                modal.classList.add('show');
                modal.style.display = 'flex';
                modal.style.visibility = 'visible';
                modal.style.opacity = '1';
                modal.style.zIndex = '10000';
                modal.style.position = 'fixed';
                modal.style.top = '0';
                modal.style.left = '0';
                modal.style.width = '100%';
                modal.style.height = '100%';
                document.body.style.overflow = 'hidden';
            } catch (e) {
                alert('Error: ' + e.message + '\nMessage: ' + message);
            }
        };
        
        window.closeMessageModal = function() {
            var modal = document.getElementById('messageModal');
            if (!modal) {
                modal = document.querySelector('[id*="messageModal"]') || document.querySelector('.modal-overlay');
            }
            if (modal) {
                modal.classList.remove('show');
                modal.style.display = 'none';
                modal.style.visibility = 'hidden';
                document.body.style.overflow = '';
            }
        };
        
        // Also create non-window versions for compatibility
        var showMessageModal = window.showMessageModal;
        var closeMessageModal = window.closeMessageModal;
    </script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Appointment Hero Section -->
    <section class="appointment-hero">
        <div class="container">
            <h1>Book Your Appointment</h1>
            <p>Schedule your eye examination or consultation with our experienced optometrists. Easy online booking in just a few steps.</p>
        </div>
    </section>

    <!-- Modal Popup for Messages -->
    <div id="messageModal" class="modal-overlay">
        <div class="modal-popup">
            <div class="modal-header">
                <div style="display: flex; align-items: center; flex: 1;">
                    <div id="modalIcon" class="modal-icon">
                        <span id="modalIconContent"></span>
                    </div>
                    <h3 id="modalTitle" class="modal-title"></h3>
                </div>
                <button type="button" class="modal-close" onclick="closeMessageModal()" aria-label="Close">
                    ×
                </button>
            </div>
            <div class="modal-body">
                <p id="modalMessage"></p>
            </div>
            <div class="modal-footer">
                <button type="button" class="modal-btn modal-btn-primary" onclick="closeMessageModal()">OK</button>
            </div>
        </div>
    </div>

    <!-- Step Progress Indicator -->
    <div class="booking-container">
        <div class="step-progress">
            <div class="step-item" id="step1">
                <div class="step-circle">1</div>
                <span>Optometrist</span>
            </div>
            <div class="step-connector" id="conn1"></div>
            <div class="step-item" id="step2">
                <div class="step-circle">2</div>
                <span>Date & Time</span>
            </div>
            <div class="step-connector" id="conn2"></div>
            <div class="step-item" id="step3">
                <div class="step-circle">3</div>
                <span>Confirm</span>
            </div>
        </div>
        <div class="booking-form">
            <asp:Panel ID="pnlMessage" runat="server" Visible="false" CssClass="alert">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </asp:Panel>

            <!-- Appointment Details Section -->
            <div class="form-section">
                <h2 class="section-title">Appointment Details</h2>
                
                <!-- Optometrist Selection -->
                <div class="form-row">
                    <div class="form-group">
                        <label>Select Optometrist <span class="required">*</span></label>
                        <asp:DropDownList ID="ddlOptometrist" runat="server" CssClass="form-control" 
                            OnSelectedIndexChanged="ddlOptometrist_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Please select an optometrist" Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvOptometrist" runat="server" ControlToValidate="ddlOptometrist" 
                            ErrorMessage="Please select an optometrist" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div id="statusOptometrist" class="section-status invalid">✗ Optometrist not selected</div>
                </div>

            </div>



            <!-- Date and Time Selection -->
            <div class="form-section">
                <h2 class="section-title">Select Date & Time</h2>
                <div class="calendar-section">
                    <div class="date-picker">
                        <h3>Choose Date</h3>
                        <asp:Calendar ID="calAppointment" runat="server" OnSelectionChanged="calAppointment_SelectionChanged"
                            CssClass="appointment-calendar" FirstDayOfWeek="Monday">
                        </asp:Calendar>
                    </div>
                    <div class="time-slots">
                        <h3>Available Times</h3>
                        <asp:Label ID="lblSelectedDate" runat="server" Text="Please select a date" CssClass="selected-date"></asp:Label>
                        <div class="time-slots-grid" id="timeSlots">
                            <!-- Time slots will be populated via JavaScript/code-behind -->
                        </div>
                        <asp:HiddenField ID="hfSelectedTime" runat="server" />
                    </div>
                    <div id="statusDateTime" class="section-status invalid">✗ Date and time not selected</div>
                </div>
            </div>



            <!-- Appointment Summary -->
            <div class="summary-section">
                <h3>Appointment Summary</h3>
                <div class="summary-item">
                    <span>Optometrist:</span>
                    <span id="summaryOptometrist">Please select an optometrist</span>
                </div>
                <div class="summary-item">
                    <span>Date:</span>
                    <span id="summaryDate">Please select a date</span>
                </div>
                <div class="summary-item">
                    <span>Time:</span>
                    <span id="summaryTime">Please select a time</span>
                </div>
            </div>

            <!-- Form Actions -->
            <div class="form-actions">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                <asp:Button ID="btnBookAppointment" runat="server" Text="Book Appointment" CssClass="btn-book disabled" OnClick="btnBookAppointment_Click" UseSubmitBehavior="false" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // ===== BOOK BUTTON STATE MANAGER =====
        function setBookButtonState() {
            var btn = document.getElementById('<%= btnBookAppointment.ClientID %>');
            var ddl = document.getElementById('<%= ddlOptometrist.ClientID %>');
            var hf = document.getElementById('<%= hfSelectedTime.ClientID %>');
            var optSelected = ddl && ddl.value !== '';
            var dateSelected = true;
            var timeSelected = hf && hf.value !== '';
            
            // Check if date is selected via calendar
            var calTables = document.querySelectorAll('.appointment-calendar table');
            if (calTables.length > 0) {
                var selectedCells = document.querySelectorAll('.appointment-calendar .selected');
                dateSelected = selectedCells.length > 0;
            }
            
            var canBook = optSelected && dateSelected && timeSelected;
            
            if (btn) {
                if (canBook) {
                    btn.classList.remove('disabled');
                    btn.disabled = false;
                } else {
                    btn.classList.add('disabled');
                    btn.disabled = true;
                }
            }
            
            // Update step progress
            var s1 = document.getElementById('step1');
            var s2 = document.getElementById('step2');
            var s3 = document.getElementById('step3');
            var c1 = document.getElementById('conn1');
            var c2 = document.getElementById('conn2');
            
            if (s1) { s1.className = 'step-item' + (optSelected ? ' completed' : ''); }
            if (s2) { s2.className = 'step-item' + (timeSelected ? ' completed' : ''); }
            if (s3) { s3.className = 'step-item' + (canBook ? ' active' : ''); }
            if (c1) { c1.className = 'step-connector' + (optSelected ? ' completed' : ''); }
            if (c2) { c2.className = 'step-connector' + (timeSelected ? ' completed' : ''); }
            
            // Update section status indicators
            var st1 = document.getElementById('statusOptometrist');
            var st2 = document.getElementById('statusDateTime');
            if (st1) {
                st1.className = 'section-status ' + (optSelected ? 'valid' : 'invalid');
                st1.innerHTML = optSelected ? '✓ Optometrist selected' : '✗ Optometrist not selected';
            }
            if (st2) {
                st2.className = 'section-status ' + (timeSelected ? 'valid' : 'invalid');
                st2.innerHTML = timeSelected ? '✓ Date and time selected' : (dateSelected ? '✗ Please select a time slot' : '✗ Date and time not selected');
            }
        }
        
        // Wait for page to load
        window.addEventListener('DOMContentLoaded', function () {
            
            // ===== RESET BUTTON STATE ON PAGE LOAD =====
            // In case of postback, reset any loading states
            var bookButton = document.querySelector('.btn-book');
            if (bookButton) {
                bookButton.classList.remove('loading');
                bookButton.style.pointerEvents = '';
                bookButton.style.opacity = '';
            }
            // Let setBookButtonState handle the disabled state
            setBookButtonState();
            
            // ===== SCROLL REVEAL ANIMATIONS =====
            const revealElements = document.querySelectorAll('.form-section, .summary-section, .form-actions');
            
            function revealOnScroll() {
                const windowHeight = window.innerHeight;
                const revealPoint = 100;
                
                revealElements.forEach(function (element, index) {
                    const elementTop = element.getBoundingClientRect().top;
                    
                    if (elementTop < windowHeight - revealPoint) {
                        element.style.opacity = '1';
                        element.style.transform = 'translateY(0)';
                    }
                });
            }
            
            // Initial check
            revealOnScroll();
            
            // Check on scroll
            window.addEventListener('scroll', revealOnScroll);
            
            // ===== ENHANCED TIME SLOT INTERACTIONS =====
            document.addEventListener('click', function(e) {
                if (e.target.classList.contains('time-slot') && !e.target.classList.contains('unavailable')) {
                    // Remove selected class from all time slots
                    document.querySelectorAll('.time-slot').forEach(function(slot) {
                        slot.classList.remove('selected');
                    });
                    
                    // Add selected class to clicked slot
                    e.target.classList.add('selected');
                    
                    // Update book button state
                    if (typeof setBookButtonState === 'function') {
                        setTimeout(setBookButtonState, 50);
                    }
                    
                    // Add a subtle animation
                    e.target.style.animation = 'pulse 0.3s ease';
                    setTimeout(function() {
                        e.target.style.animation = '';
                    }, 300);
                }
            });
            
            // ===== MUTATION OBSERVER FOR DYNAMIC TIME SLOTS =====
            var timeSlotsContainer = document.getElementById('timeSlots');
            if (timeSlotsContainer) {
                var slotObserver = new MutationObserver(function() {
                    if (typeof setBookButtonState === 'function') {
                        setBookButtonState();
                    }
                });
                slotObserver.observe(timeSlotsContainer, { childList: true, subtree: true });
            }
            
            // ===== FORM INPUT ANIMATIONS =====
            const formInputs = document.querySelectorAll('.form-group input, .form-group select, .form-group textarea');
            
            formInputs.forEach(function(input) {
                // Add focus animation
                input.addEventListener('focus', function() {
                    this.parentElement.style.transform = 'scale(1.02)';
                });
                
                input.addEventListener('blur', function() {
                    this.parentElement.style.transform = 'scale(1)';
                });
            });
            
            // ===== BUTTON CLICK ANIMATIONS =====
            // Use mousedown for ripple effect to avoid interfering with click events
            const buttons = document.querySelectorAll('.btn-book, .btn-cancel');
            
            buttons.forEach(function(button) {
                button.addEventListener('mousedown', function(e) {
                    // Only create ripple effect, don't interfere with button functionality
                    // Create ripple effect
                    const ripple = document.createElement('span');
                    const rect = this.getBoundingClientRect();
                    const size = Math.max(rect.width, rect.height);
                    const x = e.clientX - rect.left - size / 2;
                    const y = e.clientY - rect.top - size / 2;
                    
                    ripple.style.width = ripple.style.height = size + 'px';
                    ripple.style.left = x + 'px';
                    ripple.style.top = y + 'px';
                    ripple.style.position = 'absolute';
                    ripple.style.borderRadius = '50%';
                    ripple.style.background = 'rgba(255, 255, 255, 0.5)';
                    ripple.style.transform = 'scale(0)';
                    ripple.style.animation = 'ripple 0.6s ease-out';
                    ripple.style.pointerEvents = 'none';
                    ripple.style.zIndex = '1';
                    
                    // Ensure button has proper positioning
                    if (getComputedStyle(this).position === 'static') {
                        this.style.position = 'relative';
                    }
                    this.style.overflow = 'hidden';
                    this.appendChild(ripple);
                    
                    setTimeout(function() {
                        if (ripple.parentNode) {
                            ripple.remove();
                        }
                    }, 600);
                });
            });
            
            // Add ripple animation keyframes
            const style = document.createElement('style');
            style.textContent = `
                @keyframes ripple {
                    to {
                        transform: scale(4);
                        opacity: 0;
                    }
                }
            `;
            document.head.appendChild(style);
            
            // ===== CALENDAR DATE HOVER EFFECTS =====
            const calendarCells = document.querySelectorAll('.appointment-calendar td');
            
            calendarCells.forEach(function(cell) {
                cell.addEventListener('mouseenter', function() {
                    if (!this.classList.contains('unavailable') && !this.classList.contains('other-month')) {
                        this.style.transition = 'all 0.3s ease';
                    }
                });
            });
            
            // ===== SUMMARY SECTION UPDATES ANIMATION =====
            const summaryItems = document.querySelectorAll('.summary-item');
            
            // Observe summary changes
            const observer = new MutationObserver(function(mutations) {
                mutations.forEach(function(mutation) {
                    if (mutation.type === 'childList' || mutation.type === 'characterData') {
                        mutation.target.style.animation = 'fadeIn 0.5s ease-out';
                    }
                });
            });
            
            summaryItems.forEach(function(item) {
                observer.observe(item, {
                    childList: true,
                    characterData: true,
                    subtree: true
                });
            });
            
            // ===== SMOOTH SCROLLING FOR FORM SECTIONS =====
            document.querySelectorAll('.section-title').forEach(function(title) {
                title.addEventListener('click', function() {
                    const section = this.closest('.form-section');
                    if (section) {
                        section.scrollIntoView({
                            behavior: 'smooth',
                            block: 'nearest'
                        });
                    }
                });
            });
            
            // Close modal when clicking overlay
            var modalOverlay = document.getElementById('messageModal');
            if (modalOverlay) {
                modalOverlay.onclick = function(e) {
                    if (e.target === modalOverlay) {
                        closeMessageModal();
                    }
                };
            }
            
            // Close modal with Escape key
            document.onkeydown = function(e) {
                if (e.key === 'Escape') {
                    closeMessageModal();
                }
            };
            
            // ===== LOADING STATE FOR BUTTONS =====
            // Only add visual loading state, don't interfere with ASP.NET postback
            const bookButton = document.querySelector('.btn-book');
            if (bookButton) {
                // Find the form that contains this button
                const form = bookButton.closest('form') || document.forms[0];
                
                if (form) {
                    // Listen for form submit - this happens after validation
                    form.addEventListener('submit', function(e) {
                        // Only add loading state if form is actually submitting
                        // Don't disable the button - let ASP.NET handle it
                        if (bookButton && !bookButton.disabled) {
                            bookButton.classList.add('loading');
                            // Make button appear disabled but don't actually disable it
                            bookButton.style.pointerEvents = 'none';
                            bookButton.style.opacity = '0.7';
                        }
                    }, false);
                }
            }
            
            // ===== PARALLAX EFFECT FOR HERO SECTION =====
            window.addEventListener('scroll', function() {
                const hero = document.querySelector('.appointment-hero');
                if (hero) {
                    const scrolled = window.pageYOffset;
                    const rate = scrolled * 0.5;
                    hero.style.transform = 'translateY(' + rate + 'px)';
                }
            });
            
            // ===== ENHANCED FORM VALIDATION FEEDBACK =====
            const formControls = document.querySelectorAll('input[type="text"], input[type="email"], select, textarea');
            
            formControls.forEach(function(control) {
                control.addEventListener('invalid', function() {
                    this.style.borderColor = '#dc3545';
                    this.style.animation = 'shake 0.5s ease';
                });
                
                control.addEventListener('input', function() {
                    if (this.validity.valid) {
                        this.style.borderColor = '#28a745';
                        this.style.animation = '';
                    }
                });
            });
            
            // Add shake animation
            const shakeStyle = document.createElement('style');
            shakeStyle.textContent = `
                @keyframes shake {
                    0%, 100% { transform: translateX(0); }
                    25% { transform: translateX(-10px); }
                    75% { transform: translateX(10px); }
                }
            `;
            document.head.appendChild(shakeStyle);
        });
        
        // Time slots are now loaded dynamically from the database via LoadAvailableTimeSlots()
        // No hardcoded time slots - all data comes from tblTime table
    </script>
</asp:Content>