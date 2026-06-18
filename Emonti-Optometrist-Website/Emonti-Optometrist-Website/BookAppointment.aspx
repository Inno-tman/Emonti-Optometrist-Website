<%@ Page Title="Book Appointment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookAppointment.aspx.cs" Inherits="Emonti_Optometrist_Website.BookAppointment" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        @keyframes fadeInUp { from { opacity: 0; transform: translateY(30px); } to { opacity: 1; transform: translateY(0); } }
        @keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
        @keyframes pulse { 0%, 100% { transform: scale(1); } 50% { transform: scale(1.05); } }
        @keyframes gradientShift { 0% { background-position: 0% 50%; } 50% { background-position: 100% 50%; } 100% { background-position: 0% 50%; } }
        @keyframes spin { to { transform: rotate(360deg); } }
        
        .booking-hero {
            background: linear-gradient(-45deg, #667eea, #764ba2, #667eea, #764ba2);
            background-size: 400% 400%;
            animation: gradientShift 15s ease infinite;
            color: white; padding: 5rem 2rem 4rem; text-align: center;
            position: relative; overflow: hidden;
        }
        .booking-hero::before {
            content: ''; position: absolute; top: 0; left: 0; right: 0; bottom: 0;
            background: rgba(0,0,0,0.1); z-index: 1;
        }
        .booking-hero .container { position: relative; z-index: 2; animation: fadeInUp 1s ease-out; }
        .booking-hero h1 {
            font-size: clamp(2rem, 5vw, 3.5rem); margin-bottom: 1.5rem; font-weight: 700;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }
        .booking-hero p {
            font-size: clamp(1rem, 2.5vw, 1.3rem); max-width: 700px; margin: 0 auto; line-height: 1.8; opacity: 0.95;
        }
        
        .booking-section { padding: 2rem 1.5rem; background: linear-gradient(180deg, #f8f9fa, #fff); }
        .booking-container { max-width: 800px; margin: 0 auto; }
        .booking-card {
            background: #fff; border-radius: 20px; box-shadow: 0 20px 60px rgba(0,0,0,0.12);
            padding: 2.5rem; margin-top: -3rem; position: relative; z-index: 3; animation: fadeInUp 0.8s ease-out;
        }
        .booking-card h2 {
            font-size: 1.6rem; color: #333; margin-bottom: 2rem; padding-bottom: 0.75rem;
            position: relative; font-weight: 700;
        }
        .booking-card h2::after {
            content: ''; position: absolute; bottom: 0; left: 0;
            width: 60px; height: 4px;
            background: linear-gradient(90deg, #667eea, #764ba2); border-radius: 2px;
        }
        
        .form-row { display: grid; grid-template-columns: 1fr 1fr; gap: 1.5rem; margin-bottom: 0.5rem; }
        .form-group { display: flex; flex-direction: column; position: relative; }
        .form-group label {
            font-weight: 700; color: #2c3e50; margin-bottom: 0.5rem; font-size: 0.82rem;
            text-transform: uppercase; letter-spacing: 0.5px;
            display: flex; align-items: center; gap: 0.4rem;
        }
        .form-group label i { color: #667eea; font-size: 0.85rem; width: 16px; text-align: center; }
        .form-group .required { color: #e74c3c; margin-left: 1px; }
        
        .form-group .select-wrapper { position: relative; display: block; width: 100%; }
        .form-group .select-wrapper::after {
            content: '\f078'; font-family: 'Font Awesome 6 Free', 'Font Awesome 5 Free', 'FontAwesome';
            font-weight: 900; position: absolute; right: 14px; top: 50%;
            transform: translateY(-50%); color: #667eea; font-size: 0.7rem;
            pointer-events: none; transition: transform 0.2s ease;
        }
        .form-group .select-wrapper:focus-within::after { transform: translateY(-50%) rotate(180deg); }
        
        .form-group input, .form-group select, .form-group textarea {
            padding: 0.85rem 1rem; border: 2px solid #e0e0e0; border-radius: 12px;
            font-size: 0.95rem; font-family: inherit; transition: all 0.25s ease;
            background: #fafbff; width: 100%; box-sizing: border-box; color: #2c3e50;
        }
        .form-group select {
            appearance: none; -webkit-appearance: none; -moz-appearance: none;
            padding-right: 2.5rem; cursor: pointer;
        }
        .form-group input:hover, .form-group select:hover, .form-group textarea:hover {
            border-color: #b0b8e0; background: #f0f2ff;
        }
        .form-group input:focus, .form-group select:focus, .form-group textarea:focus {
            border-color: #667eea; outline: none; box-shadow: 0 0 0 4px rgba(102,126,234,0.12); background: #fff;
        }
        /* Custom dropdown list styling */
        .form-group select option {
            padding: 0.7rem 1rem; background: #fff; color: #2c3e50;
            border-radius: 8px; margin: 2px 4px;
        }
        .form-group select option:disabled { color: #bbb; }
        .form-group select option:hover { background: #f0f2ff; }
        .form-group select option:checked { background: linear-gradient(135deg, #667eea, #764ba2); color: #fff; }
        /* Scrollbar for dropdown list */
        .form-group select::-webkit-scrollbar { width: 6px; }
        .form-group select::-webkit-scrollbar-track { background: #f1f1f1; border-radius: 3px; }
        .form-group select::-webkit-scrollbar-thumb { background: #c0c0d0; border-radius: 3px; }
        .form-group select::-webkit-scrollbar-thumb:hover { background: #a0a0b8; }
        /* Date input custom styling */
        .form-group input[type="date"] {
            appearance: none; -webkit-appearance: none; -moz-appearance: none;
            padding-right: 0.5rem; min-height: 48px;
            background: #fafbff url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='18' height='18' viewBox='0 0 24 24' fill='none' stroke='%23667eea' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3E%3Crect x='3' y='4' width='18' height='18' rx='2' ry='2'/%3E%3Cline x1='16' y1='2' x2='16' y2='6'/%3E%3Cline x1='8' y1='2' x2='8' y2='6'/%3E%3Cline x1='3' y1='10' x2='21' y2='10'/%3E%3C/svg%3E") no-repeat right 14px center;
            background-size: 18px;
        }
        .form-group input[type="date"]:focus {
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='18' height='18' viewBox='0 0 24 24' fill='none' stroke='%23667eea' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3E%3Crect x='3' y='4' width='18' height='18' rx='2' ry='2'/%3E%3Cline x1='16' y1='2' x2='16' y2='6'/%3E%3Cline x1='8' y1='2' x2='8' y2='6'/%3E%3Cline x1='3' y1='10' x2='21' y2='10'/%3E%3C/svg%3E");
        }
        /* Hide default calendar picker icon in some browsers */
        input[type="date"]::-webkit-calendar-picker-indicator {
            opacity: 0; position: absolute; right: 0; top: 0;
            width: 100%; height: 100%; cursor: pointer;
        }
        /* Date input on focus - subtle inner glow */
        .form-group input[type="date"]:focus-within {
            border-color: #667eea;
        }
        /* Optgroup styling */
        optgroup {
            font-weight: 700; color: #667eea; background: #f8f9ff;
            padding: 0.5rem 0.75rem; font-size: 0.82rem; letter-spacing: 0.5px;
            border-bottom: 1px solid #e8ecf4;
        }
        optgroup option {
            font-weight: 400; color: #2c3e50; padding: 0.5rem 1rem 0.5rem 1.8rem;
            border-radius: 6px; margin: 1px 4px;
        }
        optgroup option:hover { background: #f0f2ff; }
        
        .availability-status {
            margin-top: 0.5rem; padding: 0.5rem 0.75rem; border-radius: 8px;
            font-size: 0.82rem; font-weight: 500; display: none; align-items: center;
            gap: 0.4rem; animation: fadeIn 0.3s ease;
        }
        .availability-status.visible { display: flex; }
        .availability-status.available { background: #e8f5e9; border: 1px solid #a5d6a7; color: #2e7d32; }
        .availability-status.unavailable { background: #fce4ec; border: 1px solid #ef9a9a; color: #c62828; }
        .availability-status i { font-size: 0.85rem; }
        .availability-status .spinner {
            width: 14px; height: 14px; border: 2px solid rgba(102,126,234,0.2);
            border-top-color: #667eea; border-radius: 50%;
            animation: spin 0.6s linear infinite; display: inline-block;
        }
        
        .alert { padding: 1rem 1.25rem; border-radius: 10px; margin-bottom: 1.5rem; font-size: 0.95rem; animation: fadeIn 0.4s ease-out; }
        .alert-success { background: #d4edda; border: 1px solid #c3e6cb; color: #155724; }
        .alert-error, .alert-danger { background: #f8d7da; border: 1px solid #f5c6cb; color: #721c24; }
        
        .customer-info-box {
            background: #f8f9ff; border: 2px solid #e0e5ff; border-radius: 12px;
            padding: 1.25rem 1.5rem; margin: 1.5rem 0; animation: fadeIn 0.4s ease;
        }
        .customer-info-box h4 {
            margin: 0 0 0.75rem; font-size: 0.9rem; font-weight: 700; color: #2c3e50;
            display: flex; align-items: center; gap: 0.4rem;
        }
        .customer-info-box h4 i { color: #667eea; }
        .customer-info-row { display: flex; align-items: center; gap: 0.5rem; padding: 0.3rem 0; font-size: 0.9rem; }
        .customer-info-row .info-label { font-weight: 600; color: #667eea; min-width: 70px; display: flex; align-items: center; gap: 0.3rem; }
        .customer-info-row .info-label i { font-size: 0.75rem; width: 14px; text-align: center; }
        .customer-info-row .info-value { color: #333; font-weight: 500; }
        
        .form-actions { margin-top: 2rem; text-align: center; }
        .btn-submit {
            background: linear-gradient(135deg, #667eea, #764ba2); color: #fff;
            padding: 1rem 3rem; border: none; border-radius: 50px; font-size: 1.05rem;
            font-weight: 700; cursor: pointer; transition: all 0.4s ease;
            box-shadow: 0 6px 20px rgba(102,126,234,0.4);
            text-transform: uppercase; letter-spacing: 0.5px;
        }
        .btn-submit:hover { transform: translateY(-3px); box-shadow: 0 10px 30px rgba(102,126,234,0.5); }
        .btn-submit:active { transform: translateY(-1px); }
        .btn-submit:disabled {
            background: #6c6c8a !important; cursor: not-allowed !important;
            opacity: 0.6 !important; transform: none !important; box-shadow: none !important;
        }
        .btn-cancel {
            background: transparent; color: #666; padding: 1rem 2rem;
            border: 2px solid #ccc; border-radius: 50px; font-size: 1rem; font-weight: 600;
            cursor: pointer; transition: all 0.3s ease; margin-right: 1rem;
        }
        .btn-cancel:hover { border-color: #999; color: #333; transform: translateY(-2px); }
        
        @media (max-width: 768px) {
            .booking-hero { padding: 6rem 1rem 3rem; }
            .booking-card { padding: 1.75rem; margin-top: -2rem; }
            .form-row { grid-template-columns: 1fr; gap: 1rem; }
            .btn-submit { width: 100%; padding: 1rem 2rem; }
            .form-actions { display: flex; flex-direction: column; gap: 1rem; }
            .btn-cancel { margin-right: 0; }
        }
        @media (max-width: 480px) {
            .booking-hero { padding: 5rem 0.75rem 2.5rem; }
            .booking-card { padding: 1.25rem; }
            .booking-card h2 { font-size: 1.3rem; }
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <section class="booking-hero">
        <div class="container">
            <h1>Book an Appointment</h1>
            <p>Fill in your details below and we'll confirm your booking. All fields marked with <span style="color:#ff9999;">*</span> are required.</p>
        </div>
    </section>

    <section class="booking-section">
        <div class="booking-container">
            <div class="booking-card">
                <h2>Appointment Details</h2>

                <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:Panel>

                <div id="rebookingAlert" class="alert" style="display:none;background:#e7f3ff;border:1px solid #b8d4f0;color:#1976D2;display:none;align-items:center;gap:0.75rem;">
                    <i class="fas fa-info-circle"></i>
                    <span><strong>Rebooking:</strong> You are rebooking a missed appointment. The same optometrist has been pre-selected.</span>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label><i class="fas fa-stethoscope"></i> Appointment Type <span class="required">*</span></label>
                        <div class="select-wrapper">
                            <asp:DropDownList ID="ddlAppointmentType" runat="server">
                                <asp:ListItem Text="-- Select Type --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Eye Exam" Value="Eye Exam"></asp:ListItem>
                                <asp:ListItem Text="Contact Lens Fitting" Value="Contact Lens Fitting"></asp:ListItem>
                                <asp:ListItem Text="Follow-up" Value="Follow-up"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group">
                        <label><i class="fas fa-user-md"></i> Optometrist <span class="required">*</span></label>
                        <div class="select-wrapper">
                            <asp:DropDownList ID="ddlOptometrist" runat="server">
                                <asp:ListItem Text="-- Select Optometrist --" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label><i class="fas fa-calendar-alt"></i> Preferred Date <span class="required">*</span></label>
                        <input type="date" id="inputDate" runat="server" class="form-control" min="" />
                    </div>
                    <div class="form-group">
                        <label><i class="fas fa-clock"></i> Preferred Time <span class="required">*</span></label>
                        <div class="select-wrapper">
                                <select id="ddlTimeSlot" name="ddlTimeSlot">
                                <option value="">-- Select Time --</option>
                                <optgroup label="Morning">
                                    <option value="1">08:00 - 09:00</option>
                                    <option value="2">09:00 - 10:00</option>
                                    <option value="3">10:00 - 11:00</option>
                                    <option value="4">11:00 - 12:00</option>
                                </optgroup>
                                <optgroup label="Afternoon">
                                    <option value="5">13:00 - 14:00</option>
                                    <option value="6">14:00 - 15:00</option>
                                    <option value="7">15:00 - 16:00</option>
                                </optgroup>
                            </select>
                        </div>
                        <div id="availabilityStatus" class="availability-status"></div>
                    </div>
                </div>

                <div class="customer-info-box" id="customerInfoBox">
                    <h4><i class="fas fa-user"></i> Your Details</h4>
                    <div class="customer-info-row">
                        <span class="info-label"><i class="fas fa-user-circle"></i> Name:</span>
                        <span class="info-value" id="infoName">Loading...</span>
                    </div>
                    <div class="customer-info-row">
                        <span class="info-label"><i class="fas fa-envelope"></i> Email:</span>
                        <span class="info-value" id="infoEmail">Loading...</span>
                    </div>
                    <div class="customer-info-row">
                        <span class="info-label"><i class="fas fa-phone"></i> Phone:</span>
                        <span class="info-value" id="infoPhone">Loading...</span>
                    </div>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                    <asp:Button ID="btnBookAppointment" runat="server" Text="Book Appointment" CssClass="btn-submit" OnClick="btnBookAppointment_Click" disabled="disabled" />
                </div>

                <asp:HiddenField ID="hfCustomerName" runat="server" />
                <asp:HiddenField ID="hfCustomerEmail" runat="server" />
                <asp:HiddenField ID="hfCustomerPhone" runat="server" />
                <asp:HiddenField ID="hfSelectedTime" runat="server" />
                <asp:HiddenField ID="hfRebooking" runat="server" />
            </div>
        </div>
    </section>

    <script type="text/javascript">
        function getVal(id) { var el = document.getElementById(id); return el ? el.value : ''; }
        function setVal(id, v) { var el = document.getElementById(id); if (el) el.value = v; }
        function show(id) { var el = document.getElementById(id); if (el) el.style.display = ''; }
        function hide(id) { var el = document.getElementById(id); if (el) el.style.display = 'none'; }

        function checkAvailability() {
            var statusEl = document.getElementById('availabilityStatus');
            var btn = document.getElementById('<%= btnBookAppointment.ClientID %>');
            var date = getVal('<%= inputDate.ClientID %>');
            var time = getVal('ddlTimeSlot');
            var optometrist = getVal('<%= ddlOptometrist.ClientID %>');
            var apptType = getVal('<%= ddlAppointmentType.ClientID %>');
            var custId = window.__custId || '';
            console.log('checkAvailability called', {apptType:apptType, date:date, time:time, optometrist:optometrist, custId:custId});

            if (!apptType || !date || !time || !optometrist) {
                console.log('checkAvailability: early return - missing fields');
                if (statusEl) { statusEl.className = 'availability-status'; statusEl.style.display = 'none'; }
                if (btn) btn.disabled = true;
                return;
            }

            console.log('checkAvailability: all fields filled, fetching availability');
            if (statusEl) { statusEl.className = 'availability-status visible'; statusEl.innerHTML = '<span class="spinner"></span> Checking availability...'; }
            if (btn) btn.disabled = true;

            var url = '/CheckAvailability.ashx?date=' + encodeURIComponent(date) + '&time=' + encodeURIComponent(time) + '&optometristId=' + encodeURIComponent(optometrist) + '&custId=' + encodeURIComponent(custId);
            console.log('checkAvailability: fetch URL:', url);

            fetch(url)
                .then(function(r) {
                    console.log('checkAvailability: fetch response status:', r.status);
                    return r.json();
                })
                .then(function(data) {
                    console.log('checkAvailability: response data:', data);
                    if (data.available) {
                        statusEl.className = 'availability-status available visible';
                        statusEl.innerHTML = '<i class="fas fa-check-circle"></i> ' + data.message;
                        if (btn) { console.log('checkAvailability: ENABLING BUTTON'); btn.disabled = false; }
                    } else {
                        statusEl.className = 'availability-status unavailable visible';
                        statusEl.innerHTML = '<i class="fas fa-exclamation-circle"></i> ' + data.message;
                        if (btn) btn.disabled = true;
                    }
                })
                .catch(function(err) {
                    console.error('checkAvailability: fetch error:', err);
                    if (statusEl) { statusEl.className = 'availability-status'; statusEl.style.display = 'none'; }
                    if (btn) btn.disabled = true;
                });
        }

        function checkAvailabilityNow() {
            console.log('checkAvailabilityNow: manual trigger');
            checkAvailability();
        }

        function setupEvent(id, fn) {
            var el = document.getElementById(id);
            if (el) el.addEventListener('change', fn);
        }

        document.addEventListener('DOMContentLoaded', function() {
            // Set min date on date input
            var dateInput = document.getElementById('<%= inputDate.ClientID %>');
            if (dateInput) {
                var now = new Date();
                var minDate = new Date(now);
                if (now.getHours() >= 17) minDate.setDate(minDate.getDate() + 1);
                var yyyy = minDate.getFullYear();
                var mm = String(minDate.getMonth() + 1).padStart(2, '0');
                var dd = String(minDate.getDate()).padStart(2, '0');
                dateInput.setAttribute('min', yyyy + '-' + mm + '-' + dd);
            }

            // Load customer info from hidden fields
            var infoName = document.getElementById('infoName');
            var infoEmail = document.getElementById('infoEmail');
            var infoPhone = document.getElementById('infoPhone');
            if (infoName) infoName.textContent = getVal('<%= hfCustomerName.ClientID %>') || 'N/A';
            if (infoEmail) infoEmail.textContent = getVal('<%= hfCustomerEmail.ClientID %>') || 'N/A';
            if (infoPhone) infoPhone.textContent = getVal('<%= hfCustomerPhone.ClientID %>') || 'N/A';

            // Show rebooking alert
            if (getVal('<%= hfRebooking.ClientID %>') === 'true') {
                var ra = document.getElementById('rebookingAlert');
                if (ra) ra.style.display = 'flex';
            }

            // Wire up availability check
            setupEvent('<%= ddlAppointmentType.ClientID %>', checkAvailability);
            setupEvent('<%= ddlOptometrist.ClientID %>', checkAvailability);
            setupEvent('ddlTimeSlot', checkAvailability);
            if (dateInput) dateInput.addEventListener('change', checkAvailability);

            // Also trigger on input (fires more eagerly than change)
            if (dateInput) dateInput.addEventListener('input', checkAvailability);
            var timeSelect = document.getElementById('ddlTimeSlot');
            if (timeSelect) timeSelect.addEventListener('input', checkAvailability);
            if (timeSelect) timeSelect.addEventListener('click', checkAvailability);

            // Safety check after all handlers are set
            setTimeout(checkAvailability, 1000);
            console.log('Event handlers wired up, safety timeout set');
        });

        // Backup: also check on window load (in case DOMContentLoaded missed something)
        window.addEventListener('load', function() {
            console.log('window.load fired, running checkAvailability');
            setTimeout(checkAvailability, 500);
        });
    </script>
</asp:Content>
