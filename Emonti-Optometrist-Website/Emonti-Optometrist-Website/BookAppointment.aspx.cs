using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class BookAppointment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in using session
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.ToString()));
                return;
            }

            if (!IsPostBack)
            {
                InitializePage();
            }
            else
            {
                // On postback, restore summary state from server controls
                RestoreSummaryState();
            }
        }

        private void InitializePage()
        {
            // Set minimum date to today (allow same-day bookings)
            calAppointment.SelectedDate = DateTime.Today;
            calAppointment.VisibleDate = DateTime.Today;

            // Set calendar properties
            calAppointment.SelectionMode = CalendarSelectionMode.Day;

            // Update selected date label
            UpdateSelectedDateLabel();

            // Update summary date on initial load
            string script = $"document.getElementById('summaryDate').textContent = '{calAppointment.SelectedDate:dddd, MMMM dd, yyyy}';";
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSummaryDateInitial", script, true);

            // Load available optometrists
            LoadOptometrists();
            
            // Check if this is a rebooking
            CheckForRebooking();
        }
        
        private void CheckForRebooking()
        {
            string rebookId = Request.QueryString["rebook"];
            if (!string.IsNullOrEmpty(rebookId))
            {
                LoadRebookingData(rebookId);
            }
        }
        
        private void LoadRebookingData(string appointmentId)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT a.Appointment_Date, a.Staff_ID, s.Staff_Name, s.Staff_Surname
                        FROM Appointment a
                        INNER JOIN Staff s ON a.Staff_ID = s.Staff_ID
                        WHERE a.Appointment_ID = @AppointmentId 
                        AND a.Cust_ID = @CustomerId";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@CustomerId", Session["Cust_ID"]);
                        conn.Open();
                        
                        using (System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Pre-select the same optometrist
                                string staffId = reader["Staff_ID"].ToString();
                                ddlOptometrist.SelectedValue = staffId;
                                
                                // Update summary using the helper method
                                UpdateOptometristSummary();
                                
                                // Show rebooking message
                                ShowRebookingMessage();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading rebooking data: {ex.Message}");
            }
        }
        
        private void ShowRebookingMessage()
        {
            string script = @"
                var messageDiv = document.createElement('div');
                messageDiv.className = 'alert alert-info';
                messageDiv.innerHTML = '<i class=""fas fa-info-circle""></i> <strong>Rebooking Appointment:</strong> You are rebooking a missed appointment. The same optometrist has been pre-selected for your convenience.';
                document.querySelector('.booking-form').insertBefore(messageDiv, document.querySelector('.booking-form').firstChild);
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowRebookingMessage", script, true);
        }

        private void LoadOptometrists()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT Staff_ID, Staff_Name, Staff_Surname 
                        FROM Staff 
                        WHERE Staff_Role = 'Optometrist' 
                        ORDER BY Staff_Name, Staff_Surname";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        conn.Open();
                        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();

                        ddlOptometrist.Items.Clear();
                        ddlOptometrist.Items.Add(new ListItem("Please select an optometrist", ""));

                        while (reader.Read())
                        {
                            string staffId = reader["Staff_ID"].ToString();
                            string staffName = $"{reader["Staff_Name"]} {reader["Staff_Surname"]}";
                            ddlOptometrist.Items.Add(new ListItem(staffName, staffId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading optometrists: {ex.Message}");
            }
        }

        protected void calAppointment_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectedDateLabel();

            // Update summary
            string script = $"document.getElementById('summaryDate').textContent = '{calAppointment.SelectedDate:dddd, MMMM dd, yyyy}';";
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSummaryDate", script, true);

            // Load time slots if optometrist is selected
            if (!string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
            {
                LoadAvailableTimeSlots();
            }
        }

        protected void ddlOptometrist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
            {
                LoadAvailableTimeSlots();
                
                // Update optometrist summary
                UpdateOptometristSummary();
            }
            else
            {
                // Clear summary and time slots if optometrist is deselected
                string clearScript = @"
                    document.getElementById('summaryOptometrist').textContent = 'Please select an optometrist';
                    document.getElementById('summaryTime').textContent = 'Please select a time';
                    document.getElementById('timeSlots').innerHTML = '';
                    document.getElementById('" + hfSelectedTime.ClientID + @"').value = '';
                    if (typeof setBookButtonState === 'function') setBookButtonState();
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "ClearOptometristSummary", clearScript, true);
            }
        }

        private void LoadAvailableTimeSlots()
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    // Get all time slots from database (TimeID 1-15)
                    string allSlotsQuery = @"
                        SELECT t.TimeID, t.Timeslot
                        FROM tblTime t
                        WHERE t.TimeID IS NOT NULL
                        ORDER BY t.TimeID";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(allSlotsQuery, conn))
                    {
                        conn.Open();
                        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();

                        // Clear existing time slots
                        string clearScript = "document.getElementById('timeSlots').innerHTML = '';";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ClearTimeSlots", clearScript, true);

                        while (reader.Read())
                        {
                            string timeId = reader["TimeID"].ToString();
                            string timeSlot = reader["Timeslot"].ToString();
                            
                            // Check if this time slot is available (not booked)
                            bool isAvailable = IsTimeSlotAvailable(timeId, ddlOptometrist.SelectedValue, calAppointment.SelectedDate.Date);
                            
                            // Check if time slot is within business hours
                            bool isWithinBusinessHours = IsWithinBusinessHours(calAppointment.SelectedDate.Date, timeSlot);
                            
                            // Check if time has passed (only for today)
                            bool hasTimePassed = false;
                            if (calAppointment.SelectedDate.Date == DateTime.Today.Date)
                            {
                                hasTimePassed = HasTimeSlotPassed(timeSlot);
                            }
                            
                            // Check if booking time is valid (2-hour advance notice for same-day)
                            bool isValidBookingTime = IsValidBookingTime(calAppointment.SelectedDate.Date, timeSlot);
                            
                            // Determine if slot should be disabled
                            bool isDisabled = !isAvailable || hasTimePassed || !isWithinBusinessHours || !isValidBookingTime;
                            string disabledClass = isDisabled ? " unavailable" : "";
                            string disabledAttr = isDisabled ? " disabled" : "";
                            
                            // Determine tooltip message
                            string tooltip = "";
                            if (!isAvailable)
                                tooltip = "This time slot is already booked";
                            else if (hasTimePassed)
                                tooltip = "This time slot has already passed";
                            else if (!isWithinBusinessHours)
                                tooltip = "This time slot is outside business hours";
                            else if (!isValidBookingTime)
                                tooltip = "Same-day appointments require at least 2 hours advance notice";
                            
                            // Add time slot to JavaScript with proper database format
                            string addSlotScript = $@"
                                var timeSlots = document.getElementById('timeSlots');
                                var slot = document.createElement('label');
                                slot.className = 'time-slot{disabledClass}';
                                slot.title = '{tooltip}';
                                slot.innerHTML = '<input type=""radio"" name=""timeSlot"" value=""{timeId}""{disabledAttr}>{timeSlot}';
                                {(!isDisabled ? $@"slot.addEventListener('click', function() {{
                                    document.querySelectorAll('.time-slot').forEach(s => s.classList.remove('selected'));
                                    this.classList.add('selected');
                                    document.getElementById('{hfSelectedTime.ClientID}').value = '{timeId}';
                                    document.getElementById('summaryTime').textContent = '{timeSlot}';
                                    if (typeof setBookButtonState === 'function') setBookButtonState();
                                }});" : "")}
                                timeSlots.appendChild(slot);
                            ";
                            ScriptManager.RegisterStartupScript(this, GetType(), $"AddSlot_{timeId}", addSlotScript, true);
                        }
                        
                        // After all slots are rendered, update button state
                        string finalScript = "if (typeof setBookButtonState === 'function') setTimeout(setBookButtonState, 50);";
                        ScriptManager.RegisterStartupScript(this, GetType(), "FinalSlotState", finalScript, true);
                        
                        // Check if no slots were added and show message
                        string noSlotsCheck = @"
                            setTimeout(function() {
                                var ts = document.getElementById('timeSlots');
                                if (ts && ts.children.length === 0) {
                                    ts.innerHTML = '<div class=""no-slots-message"">No available time slots for this date. Please try another date.</div>';
                                    if (typeof setBookButtonState === 'function') setBookButtonState();
                                }
                            }, 100);
                        ";
                        ScriptManager.RegisterStartupScript(this, GetType(), "NoSlotsCheck", noSlotsCheck, true);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading time slots: {ex.Message}");
            }
        }

        private bool IsTimeSlotAvailable(string timeId, string staffId, DateTime date)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    // Check if slot is booked OR blocked
                    string query = @"
                        SELECT 
                            (SELECT COUNT(*) 
                             FROM Appointment a
                             WHERE a.Staff_ID = @StaffId 
                             AND CAST(a.Appointment_Date AS DATE) = @Date
                             AND a.AppointmentTimeID = @TimeId
                             AND a.Appoinment_Status != 'Cancelled') +
                            (SELECT COUNT(*) 
                             FROM BlockedTimeslots b
                             WHERE b.Staff_ID = @StaffId 
                             AND b.Blocked_Date = @Date
                             AND b.TimeID = @TimeId) as TotalCount";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        cmd.Parameters.AddWithValue("@Date", date.Date);
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count == 0; // Available only if NOT booked AND NOT blocked
                    }
                }
            }
            catch
            {
                return false; // If error, assume not available
            }
        }

        private bool CustomerHasAppointmentOnDate(string customerId, DateTime date)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Appointment a
                        WHERE a.Cust_ID = @CustomerId 
                        AND CAST(a.Appointment_Date AS DATE) = @Date
                        AND a.Appoinment_Status != 'Cancelled'";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@Date", date);
                        
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0; // Returns true if customer has appointment on this date
                    }
                }
            }
            catch
            {
                return false; // If error, assume no existing appointment
            }
        }

        private bool IsWithinBusinessHours(DateTime appointmentDate, string timeSlot)
        {
            try
            {
                var dayOfWeek = appointmentDate.DayOfWeek;
                
                // Check if it's Sunday (closed)
                if (dayOfWeek == DayOfWeek.Sunday)
                    return false;
                
                // Parse time slot to get start time
                var startTime = ParseTimeSlot(timeSlot);
                if (startTime == TimeSpan.Zero)
                    return false;
                
                // Check business hours
                if (dayOfWeek >= DayOfWeek.Monday && dayOfWeek <= DayOfWeek.Friday)
                {
                    // Monday-Friday: 8:00 AM - 5:00 PM
                    return startTime >= new TimeSpan(8, 0, 0) && startTime < new TimeSpan(17, 0, 0);
                }
                else if (dayOfWeek == DayOfWeek.Saturday)
                {
                    // Saturday: 8:00 AM - 2:00 PM
                    return startTime >= new TimeSpan(8, 0, 0) && startTime < new TimeSpan(14, 0, 0);
                }
                
                return false;
            }
            catch
            {
                return false; // If parsing fails, assume not within business hours
            }
        }

        private TimeSpan ParseTimeSlot(string timeSlot)
        {
            try
            {
                // Parse database time slot format "9h30 - 10h00" to get start time
                var startTime = timeSlot.Split('-')[0].Trim(); // Gets "9h30"
                var timeParts = startTime.Replace("h", ":").Split(':'); // Gets ["9", "30"]
                
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                
                return new TimeSpan(hour, minute, 0);
            }
            catch
            {
                return TimeSpan.Zero; // If parsing fails, return zero
            }
        }

        private bool HasTimeSlotPassed(string timeSlot)
        {
            try
            {
                // Parse database time slot format "9h30 - 10h00" to get start time
                var startTime = timeSlot.Split('-')[0].Trim(); // Gets "9h30"
                var timeParts = startTime.Replace("h", ":").Split(':'); // Gets ["9", "30"]
                
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);
                
                var slotDateTime = new DateTime(
                    DateTime.Today.Year, 
                    DateTime.Today.Month, 
                    DateTime.Today.Day, 
                    hour, 
                    minute, 
                    0
                );
                
                return DateTime.Now >= slotDateTime;
            }
            catch
            {
                return false; // If parsing fails, assume not passed
            }
        }

        private bool IsValidBookingTime(DateTime appointmentDate, string timeSlot)
        {
            var currentTime = DateTime.Now;
            
            // For same-day bookings, require at least 2 hours advance notice
            if (appointmentDate.Date == currentTime.Date)
            {
                var startTime = ParseTimeSlot(timeSlot);
                if (startTime == TimeSpan.Zero)
                    return false;
                
                var appointmentDateTime = appointmentDate.Date.Add(startTime);
                var minimumAdvanceTime = currentTime.AddHours(2);
                
                if (appointmentDateTime < minimumAdvanceTime)
                {
                    return false; // Too soon for same-day booking
                }
            }
            
            return true;
        }

        private string GetTimeSlotFromDatabase(string timeId)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT t.Timeslot
                        FROM tblTime t
                        WHERE t.TimeID = @TimeId";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "";
                    }
                }
            }
            catch
            {
                return ""; // If error, return empty string
            }
        }

        private void UpdateSelectedDateLabel()
        {
            if (calAppointment.SelectedDate != DateTime.MinValue)
            {
                lblSelectedDate.Text = calAppointment.SelectedDate.ToString("dddd, MMMM dd, yyyy");
            }
            else
            {
                lblSelectedDate.Text = "Please select a date";
            }
        }

        private void RestoreSummaryState()
        {
            // Restore optometrist in summary if selected
            if (!string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
            {
                UpdateOptometristSummary();
            }
            
            // Restore date in summary if selected
            if (calAppointment.SelectedDate != DateTime.MinValue)
            {
                string dateScript = $"document.getElementById('summaryDate').textContent = '{calAppointment.SelectedDate:dddd, MMMM dd, yyyy}';";
                ScriptManager.RegisterStartupScript(this, GetType(), "RestoreDateSummary", dateScript, true);
            }
            
            // Restore time in summary if selected
            if (!string.IsNullOrEmpty(hfSelectedTime.Value))
            {
                string timeSlot = GetTimeSlotFromDatabase(hfSelectedTime.Value);
                if (!string.IsNullOrEmpty(timeSlot))
                {
                    string timeScript = $"document.getElementById('summaryTime').textContent = '{EscapeJavaScriptString(timeSlot)}';";
                    ScriptManager.RegisterStartupScript(this, GetType(), "RestoreTimeSummary", timeScript, true);
                }
            }
        }

        private void UpdateOptometristSummary()
        {
            if (!string.IsNullOrEmpty(ddlOptometrist.SelectedValue) && ddlOptometrist.SelectedItem != null)
            {
                string optometristName = EscapeJavaScriptString(ddlOptometrist.SelectedItem.Text);
                string script = $"document.getElementById('summaryOptometrist').textContent = '{optometristName}';";
                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateOptometristSummary", script, true);
            }
        }

        private string EscapeJavaScriptString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            // Escape single quotes, backslashes, and newlines for JavaScript
            return input.Replace("\\", "\\\\")
                        .Replace("'", "\\'")
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n");
        }

        protected void btnBookAppointment_Click(object sender, EventArgs e)
        {
            if (ValidateBookingForm())
            {
                if (SaveAppointment())
                {
                    ShowSuccessMessage();
                    ClearForm();
                }
                else
                {
                    ShowErrorMessage("Failed to book appointment. Please try again.");
                }
            }
        }

        private bool ValidateBookingForm()
        {
            bool isValid = true;

            // Check if date is selected
            if (calAppointment.SelectedDate == DateTime.MinValue || calAppointment.SelectedDate < DateTime.Today)
            {
                ShowErrorMessage("Please select a valid date for your appointment (today or future).");
                isValid = false;
            }

            // Check if it's Sunday (closed)
            if (isValid && calAppointment.SelectedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                ShowErrorMessage("We are closed on Sundays. Please select a different date.");
                isValid = false;
            }

            // Check if optometrist is selected
            if (string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
            {
                ShowErrorMessage("Please select an optometrist.");
                isValid = false;
            }

            // Check if time is selected
            if (string.IsNullOrEmpty(hfSelectedTime.Value))
            {
                ShowErrorMessage("Please select an appointment time.");
                isValid = false;
            }

            // Additional validation for selected time slot
            if (isValid && !string.IsNullOrEmpty(hfSelectedTime.Value))
            {
                // Get the selected time slot from the database
                string selectedTimeSlot = GetTimeSlotFromDatabase(hfSelectedTime.Value);
                
                if (!string.IsNullOrEmpty(selectedTimeSlot))
                {
                    // Check if time slot is within business hours
                    if (!IsWithinBusinessHours(calAppointment.SelectedDate.Date, selectedTimeSlot))
                    {
                        ShowErrorMessage("The selected time slot is outside our business hours.");
                        isValid = false;
                    }
                    // Check if booking time is valid (2-hour advance notice for same-day)
                    else if (!IsValidBookingTime(calAppointment.SelectedDate.Date, selectedTimeSlot))
                    {
                        ShowErrorMessage("Same-day appointments require at least 2 hours advance notice.");
                        isValid = false;
                    }
                }
            }

            // Check if customer already has an appointment on this date
            if (isValid && Session["Cust_ID"] != null)
            {
                if (CustomerHasAppointmentOnDate(Session["Cust_ID"].ToString(), calAppointment.SelectedDate.Date))
                {
                    ShowErrorMessage("You already have an appointment scheduled on this date. You can only book one appointment per day.");
                    isValid = false;
                }
            }

            return isValid;
        }

        private bool SaveAppointment()
        {
            try
            {
                // Debug: Log all values before saving
                System.Diagnostics.Debug.WriteLine($"=== SAVE APPOINTMENT DEBUG ===");
                System.Diagnostics.Debug.WriteLine($"Cust_ID: {Session["Cust_ID"]}");
                System.Diagnostics.Debug.WriteLine($"Staff_ID: {ddlOptometrist.SelectedValue}");
                System.Diagnostics.Debug.WriteLine($"Appointment_Date: {calAppointment.SelectedDate}");
                System.Diagnostics.Debug.WriteLine($"AppointmentTimeID: {hfSelectedTime.Value}");
                System.Diagnostics.Debug.WriteLine($"===============================");

                // Validate required data
                if (Session["Cust_ID"] == null)
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Cust_ID is null");
                    ShowErrorMessage("Customer session expired. Please log in again.");
                    return false;
                }

                if (string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: No optometrist selected");
                    ShowErrorMessage("Please select an optometrist.");
                    return false;
                }

                if (string.IsNullOrEmpty(hfSelectedTime.Value))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: No time slot selected");
                    ShowErrorMessage("Please select a time slot.");
                    return false;
                }

                // Final check to prevent double booking
                if (CustomerHasAppointmentOnDate(Session["Cust_ID"].ToString(), calAppointment.SelectedDate.Date))
                {
                    ShowErrorMessage("You already have an appointment scheduled on this date. You can only book one appointment per day.");
                    return false;
                }

                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Appointment 
                        (Cust_ID, Staff_ID, Appointment_Date, AppointmentTimeID, Appoinment_Status) 
                        VALUES (@CustId, @StaffId, @AppointmentDate, @AppointmentTimeId, @Status)";

                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", Session["Cust_ID"]);
                        cmd.Parameters.AddWithValue("@StaffId", ddlOptometrist.SelectedValue);
                        cmd.Parameters.AddWithValue("@AppointmentDate", calAppointment.SelectedDate);
                        cmd.Parameters.AddWithValue("@AppointmentTimeId", hfSelectedTime.Value);
                        cmd.Parameters.AddWithValue("@Status", "Pending");

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        System.Diagnostics.Debug.WriteLine($"Rows affected: {rowsAffected}");

                        if (rowsAffected > 0)
                        {
                            // Send confirmation email
                            SendConfirmationEmail();
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving appointment: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ShowErrorMessage($"Failed to save appointment: {ex.Message}");
                return false;
            }
        }

        private void SendConfirmationEmail()
        {
            try
            {
                // Get customer email and name from session
                string customerEmail = Session["UserEmail"]?.ToString();
                if (string.IsNullOrEmpty(customerEmail))
                {
                    System.Diagnostics.Debug.WriteLine("No customer email found in session");
                    return;
                }

                // Get customer name from session
                string firstName = Session["FirstName"]?.ToString() ?? "";
                string lastName = Session["LastName"]?.ToString() ?? "";
                string customerName = $"{firstName} {lastName}".Trim();
                if (string.IsNullOrEmpty(customerName))
                {
                    customerName = "Valued Customer";
                }

                // Get optometrist name
                string optometristName = ddlOptometrist.SelectedItem?.Text ?? "Selected Optometrist";
                
                // Get time slot
                string timeSlot = GetTimeSlotText(hfSelectedTime.Value);
                string logoBase64 = GetLogoBase64();

                string body = $@"<!DOCTYPE html>
<html>
<head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""></head>
<body style=""margin:0;padding:0;font-family:Arial,sans-serif;background-color:#f5f5f5;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f5f5f5;padding:20px;"">
<tr><td align=""center"">
<table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;border-radius:8px;overflow:hidden;"">
<tr><td style=""background-color:#667eea;padding:25px;text-align:center;"">
<img src=""{logoBase64}"" alt=""Emonti Optometrist"" style=""max-width:350px;height:auto;display:block;margin:0 auto 15px;"" />
<h1 style=""margin:0;color:#ffffff;font-size:24px;font-weight:600;"">Appointment Confirmed</h1>
</td></tr>
<tr><td style=""padding:30px;"">
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {System.Web.HttpUtility.HtmlEncode(customerName)},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;line-height:1.6;"">Your appointment has been successfully confirmed. We're looking forward to seeing you!</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #667eea;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;width:120px;""><strong>Optometrist:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{System.Web.HttpUtility.HtmlEncode(optometristName)}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{calAppointment.SelectedDate:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#667eea;font-size:14px;font-weight:600;"">{System.Web.HttpUtility.HtmlEncode(timeSlot)}</td></tr>
</table></div>
<div style=""background-color:#e7f3ff;border-left:4px solid #2196F3;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0 0 8px 0;color:#1976D2;font-size:14px;font-weight:600;"">Important Reminders</p>
<ul style=""margin:0;padding-left:20px;color:#1565C0;font-size:13px;line-height:1.8;"">
<li>Please arrive <strong>15 minutes early</strong> for your appointment</li>
<li>Bring your current eyeglasses or contact lenses if you have them</li>
<li>Bring your medical aid card if applicable</li>
</ul>
</div>
<div style=""background-color:#fff3cd;border-left:4px solid #ffc107;padding:15px;margin:20px 0;border-radius:4px;"">
<p style=""margin:0;color:#856404;font-size:14px;line-height:1.6;""><strong>Need to reschedule or cancel?</strong> Please call us at <a href=""tel:0764631930"" style=""color:#856404;text-decoration:underline;"">076 463 1930</a> at least 24 hours before your appointment.</p>
</div>
<p style=""margin:25px 0 0 0;color:#555;font-size:15px;line-height:1.6;"">We look forward to seeing you soon!</p>
</td></tr>
<tr><td style=""background-color:#f8f9fa;padding:20px;text-align:center;border-top:1px solid #e0e0e0;"">
<p style=""margin:0 0 5px 0;color:#666;font-size:13px;font-weight:600;"">Emonti Optometrist</p>
<p style=""margin:0;color:#888;font-size:12px;"">Shop 7 New Colonnade, Devereaux Ave, Vincent, East London 5247</p>
</td></tr>
</table></td></tr></table></body></html>";

                // Get SMTP configuration from web.config
                // NOTE: For Gmail, you must use an App Password, not your regular Gmail password
                // To create an App Password: Google Account > Security > 2-Step Verification > App Passwords
                string smtpHost = System.Configuration.ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpEmail = System.Configuration.ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpPassword"];
                string smtpFromName = System.Configuration.ConfigurationManager.AppSettings["SmtpFromName"] ?? "Emonti Optometrist";
                bool enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");

                // Validate SMTP credentials are configured
                if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: SMTP credentials not configured in web.config");
                    System.Diagnostics.Debug.WriteLine("For Gmail, ensure you're using an App Password in web.config (SmtpPassword)");
                    return;
                }

                // Use using statements to ensure proper resource disposal
                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                    smtp.EnableSsl = enableSsl;
                    smtp.Timeout = 30000; // 30 seconds timeout

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpEmail, smtpFromName);
                        message.To.Add(customerEmail);
                        message.Subject = "Appointment Confirmation - Emonti Optometrist";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        smtp.Send(message);
                        System.Diagnostics.Debug.WriteLine($"Confirmation email sent to: {customerEmail}");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                System.Diagnostics.Debug.WriteLine($"SMTP Error sending email: {smtpEx.Message}");
                System.Diagnostics.Debug.WriteLine($"SMTP Status Code: {smtpEx.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {smtpEx.InnerException?.Message ?? "None"}");
                // Common Gmail errors:
                // - 535: Authentication failed (usually means need App Password)
                // - 534: Authentication failed (check credentials)
                if ((int)smtpEx.StatusCode == 535 || (int)smtpEx.StatusCode == 534)
                {
                    System.Diagnostics.Debug.WriteLine("TIP: Gmail requires an App Password. Check web.config SmtpPassword setting.");
                }
                // Log but don't fail appointment booking if email fails
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message ?? "None"}");
                // Log but don't fail appointment booking if email fails
            }
        }

        private string GetTimeSlotText(string timeId)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;

                using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string query = "SELECT Timeslot FROM tblTime WHERE TimeID = @TimeId";
                    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return result?.ToString() ?? "Selected Time";
                    }
                }
            }
            catch
            {
                return "Selected Time";
            }
        }


        private void ShowSuccessMessage()
        {
            string timeSlot = GetTimeSlotText(hfSelectedTime.Value);
            string message = $"Appointment booked successfully! Confirmation details have been sent to your email. We look forward to seeing you on {calAppointment.SelectedDate:MMMM dd, yyyy} at {timeSlot}.";
            
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-success";
            lblMessage.Text = $"✅ {message}";
            
            // Register script to show modal popup
            string escapedMessage = EscapeJavaScriptString(message);
            string script = $@"
                var retryCount = 0;
                function tryShowSuccessModal() {{
                    retryCount++;
                    var modal = document.getElementById('messageModal');
                    if (modal && typeof window.showMessageModal === 'function') {{
                        window.showMessageModal('success', '{escapedMessage}');
                    }} else if (retryCount < 20) {{
                        setTimeout(tryShowSuccessModal, 100);
                    }}
                }}
                tryShowSuccessModal();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowSuccessModal", script, true);
        }

        private void ShowErrorMessage(string message)
        {
            // Remove emoji if present
            string cleanMessage = message.Replace("❌", "").Trim();
            
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-error";
            lblMessage.Text = "❌ " + cleanMessage;
            
            // Register script to show modal popup
            string escapedMessage = EscapeJavaScriptString(cleanMessage);
            string script = $@"
                var retryCount = 0;
                function tryShowErrorModal() {{
                    retryCount++;
                    var modal = document.getElementById('messageModal');
                    if (modal && typeof window.showMessageModal === 'function') {{
                        window.showMessageModal('error', '{escapedMessage}');
                    }} else if (retryCount < 20) {{
                        setTimeout(tryShowErrorModal, 100);
                    }}
                }}
                tryShowErrorModal();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowErrorModal", script, true);
        }

        private void ClearMessages()
        {
            pnlMessage.Visible = false;
            pnlMessage.CssClass = "alert";
            lblMessage.Text = "";
        }

        private void ClearForm()
        {
            ddlOptometrist.SelectedIndex = 0;
            calAppointment.SelectedDate = DateTime.Today.AddDays(1);
            hfSelectedTime.Value = "";

            // Clear time selection UI
            string clearScript = @"
                document.querySelectorAll('.time-slot').forEach(slot => slot.classList.remove('selected'));
                document.getElementById('summaryTime').textContent = 'Please select a time';
                document.getElementById('summaryOptometrist').textContent = 'Please select an optometrist';
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "ClearTimeSelection", clearScript, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Redirect back to home page or previous page
            Response.Redirect("~/Default.aspx");
        }

        // Calendar day render event to disable past dates and weekends if needed
        protected void calAppointment_DayRender(object sender, DayRenderEventArgs e)
        {
            // Disable past dates
            if (e.Day.Date < DateTime.Today)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }

            // Optionally disable Sundays (if practice is closed)
            if (e.Day.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
                e.Cell.ToolTip = "Closed on Sundays";
            }
        }

        private string GetLogoBase64()
        {
            try
            {
                string logoPath = HttpContext.Current.Server.MapPath("~/Images/Logo/Emonti Logo Banner.png");
                byte[] imageBytes = System.IO.File.ReadAllBytes(logoPath);
                string base64 = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading logo: {ex.Message}");
                return "";
            }
        }
    }
}