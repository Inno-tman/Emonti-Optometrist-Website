using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

namespace Emonti_Optometrist_Website
{
    [WebService(Namespace = "http://tempuri.org/")]
    [System.Web.Script.Services.ScriptService]
    public partial class StaffDashboard : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is logged in as staff
                if (Session["IsStaffLoggedIn"] == null || !(bool)Session["IsStaffLoggedIn"])
                {
                    // Redirect to the shared account login page (customers and staff use the same login)
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            // Load staff information
            litStaffName.Text = Session["StaffName"]?.ToString() ?? "Staff Member";
            litStaffRole.Text = Session["StaffRole"]?.ToString() ?? "Staff";
            // Use Staff_ID as set by login page
            litStaffId.Text = Session["Staff_ID"]?.ToString() ?? "";

            // Load dashboard statistics
            LoadDashboardStats();
        }

        private void LoadDashboardStats()
        {
            try
            {
                // Load today's appointment count from database
                int todayCount = GetTodayAppointmentCount();
                litTodayAppointments.Text = todayCount.ToString();

                // Load future appointments count (excluding today)
                int futureCount = GetFutureAppointmentCount();
                litAllAppointments.Text = futureCount.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading dashboard stats: {ex.Message}");
                litTodayAppointments.Text = "0";
                litAllAppointments.Text = "0";
            }
        }

        private int GetTodayAppointmentCount()
        {
            // Use Staff_ID as set by login page
            string staffId = Session["Staff_ID"]?.ToString();
            if (string.IsNullOrEmpty(staffId))
                return 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Appointment 
                        WHERE Staff_ID = @StaffId 
                        AND CAST(Appointment_Date AS DATE) = CAST(GETDATE() AS DATE)
                        AND Appoinment_Status != 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        conn.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting today's appointment count: {ex.Message}");
                return 0;
            }
        }

        private int GetFutureAppointmentCount()
        {
            // Use Staff_ID as set by login page
            string staffId = Session["Staff_ID"]?.ToString();
            if (string.IsNullOrEmpty(staffId))
                return 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Appointment 
                        WHERE Staff_ID = @StaffId 
                        AND CAST(Appointment_Date AS DATE) > CAST(GETDATE() AS DATE)
                        AND Appoinment_Status != 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        conn.Open();
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting future appointment count: {ex.Message}");
                return 0;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session and redirect to login page
            Session.Clear();
            Response.Redirect("~/Account/Login.aspx");
        }

        // Navigation methods
        protected void btnViewAppointments_Click(object sender, EventArgs e)
        {
            LoadViewAllAppointments();
            string script = "openModal('viewAllModal');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenViewAllModal", script, true);
        }

        protected void btnManageAppointments_Click(object sender, EventArgs e)
        {
            LoadManageAppointments();
            string script = "openModal('manageModal');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenManageModal", script, true);
        }

        protected void btnManageAllAppointments_Click(object sender, EventArgs e)
        {
            LoadManageAllAppointments();
            string script = "openModal('manageAllModal');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenManageAllModal", script, true);
        }

        private void LoadViewAllAppointments()
        {
            try
            {
                List<AppointmentInfo> appointments = GetTodayAppointments();
                
                if (appointments.Count > 0)
                {
                    rptViewAllAppointments.DataSource = appointments;
                    rptViewAllAppointments.DataBind();
                    rptViewAllAppointments.Visible = true;
                    pnlNoViewAllAppointments.Visible = false;
                }
                else
                {
                    rptViewAllAppointments.Visible = false;
                    pnlNoViewAllAppointments.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading view all appointments: {ex.Message}");
                rptViewAllAppointments.Visible = false;
                pnlNoViewAllAppointments.Visible = true;
            }
        }

        private void LoadManageAppointments()
        {
            try
            {
                List<AppointmentInfo> appointments = GetTodayAppointments();
                
                if (appointments.Count > 0)
                {
                    rptManageAppointments.DataSource = appointments;
                    rptManageAppointments.DataBind();
                    rptManageAppointments.Visible = true;
                    pnlNoManageAppointments.Visible = false;
                    btnCancelAppointment.Enabled = true;
                }
                else
                {
                    rptManageAppointments.Visible = false;
                    pnlNoManageAppointments.Visible = true;
                    btnCancelAppointment.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading manage appointments: {ex.Message}");
                rptManageAppointments.Visible = false;
                pnlNoManageAppointments.Visible = true;
                btnCancelAppointment.Enabled = false;
            }
        }

        private List<AppointmentInfo> GetTodayAppointments()
        {
            List<AppointmentInfo> appointments = new List<AppointmentInfo>();
            string staffId = Session["Staff_ID"]?.ToString();
            
            if (string.IsNullOrEmpty(staffId))
                return appointments;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            a.Appointment_ID,
                            a.Appointment_Date,
                            a.Appoinment_Status,
                            a.AppointmentTimeID,
                            c.Customer_Name,
                            c.Customer_Surname,
                            t.Timeslot
                        FROM Appointment a
                        INNER JOIN customer c ON a.Cust_ID = c.Cust_ID
                        LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                        WHERE a.Staff_ID = @StaffId 
                        AND CAST(a.Appointment_Date AS DATE) = CAST(GETDATE() AS DATE)
                        AND a.Appoinment_Status != 'Cancelled'
                        ORDER BY t.Timeslot ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string timeslot = reader["Timeslot"]?.ToString() ?? "N/A";
                                string dbStatus = reader["Appoinment_Status"]?.ToString() ?? "Scheduled";
                                int appointmentId = Convert.ToInt32(reader["Appointment_ID"]);
                                
                                // Check if appointment time has passed and if payment exists
                                bool hasTimePassed = HasAppointmentTimePassed(timeslot);
                                bool hasPayment = CheckIfAppointmentHasPayment(appointmentId);
                                
                                // Determine status: if time passed and no payment, it's missed
                                string finalStatus = dbStatus;
                                if (hasTimePassed && !hasPayment && dbStatus != "Cancelled")
                                {
                                    finalStatus = "Missed";
                                }
                                else if (hasTimePassed && hasPayment)
                                {
                                    finalStatus = "Completed";
                                }
                                
                                var appointment = new AppointmentInfo
                                {
                                    AppointmentId = appointmentId,
                                    AppointmentDate = Convert.ToDateTime(reader["Appointment_Date"]),
                                    Status = finalStatus,
                                    ServiceType = "Eye Examination", // Default service type
                                    Notes = timeslot,
                                    DoctorName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
                                    HasPayment = hasPayment
                                };
                                
                                // Store additional info in Notes for TimeSlot and DoctorName for PatientName
                                appointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting today's appointments: {ex.Message}");
            }

            return appointments;
        }

        protected void btnCancelAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected appointment ID from form
                string selectedAppointmentId = Request.Form["selectedAppointment"];
                
                if (string.IsNullOrEmpty(selectedAppointmentId))
                {
                    ShowMessage("Please select an appointment to cancel.", false);
                    return;
                }

                // Check if appointment can be cancelled (time hasn't passed)
                if (!CanCancelAppointment(selectedAppointmentId))
                {
                    ShowMessage("Cannot cancel this appointment. The appointment time has already passed. Appointments that have already occurred cannot be cancelled.", false);
                    return;
                }

                // Cancel the appointment (this will also validate again)
                CancelAppointment(selectedAppointmentId);
                
                // Reload the manage appointments list
                LoadManageAppointments();
                
                // Update dashboard stats
                LoadDashboardStats();
                
                ShowMessage("Appointment cancelled successfully.", true);
                
                // Refresh the modal
                string script = "openModal('manageModal');";
                ScriptManager.RegisterStartupScript(this, GetType(), "RefreshManageModal", script, true);
            }
            catch (InvalidOperationException ex)
            {
                // Handle validation errors specifically
                System.Diagnostics.Debug.WriteLine($"Validation error cancelling appointment: {ex.Message}");
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cancelling appointment: {ex.Message}");
                ShowMessage("An error occurred while cancelling the appointment. " + ex.Message, false);
            }
        }

        private bool CanCancelAppointment(string appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            a.Appointment_Date,
                            a.Appoinment_Status,
                            t.Timeslot
                        FROM Appointment a
                        LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                        WHERE a.Appointment_ID = @AppointmentId 
                        AND a.Staff_ID = @StaffId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@StaffId", Session["Staff_ID"]?.ToString());
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime appointmentDate = Convert.ToDateTime(reader["Appointment_Date"]);
                                string status = reader["Appoinment_Status"]?.ToString() ?? "Scheduled";
                                string timeslot = reader["Timeslot"]?.ToString() ?? "";
                                
                                // Can't cancel if already cancelled
                                if (status == "Cancelled")
                                {
                                    System.Diagnostics.Debug.WriteLine($"Appointment {appointmentId} is already cancelled");
                                    return false;
                                }
                                
                                // Check if appointment time has passed
                                bool hasTimePassed = false;
                                
                                // For past dates, time has definitely passed
                                if (appointmentDate.Date < DateTime.Today)
                                {
                                    hasTimePassed = true;
                                    System.Diagnostics.Debug.WriteLine($"Appointment {appointmentId} is on a past date: {appointmentDate.Date}");
                                }
                                // For today's appointments, check the time slot
                                else if (appointmentDate.Date == DateTime.Today)
                                {
                                    if (!string.IsNullOrEmpty(timeslot) && timeslot != "N/A")
                                    {
                                        hasTimePassed = HasAppointmentTimePassed(timeslot);
                                        System.Diagnostics.Debug.WriteLine($"Appointment {appointmentId} time check for today: {timeslot}, hasTimePassed: {hasTimePassed}");
                                    }
                                    else
                                    {
                                        // If no timeslot, assume it's in the past if it's today (safety check)
                                        hasTimePassed = true;
                                        System.Diagnostics.Debug.WriteLine($"Appointment {appointmentId} has no timeslot for today, assuming passed");
                                    }
                                }
                                
                                // Can only cancel if time hasn't passed
                                bool canCancel = !hasTimePassed;
                                System.Diagnostics.Debug.WriteLine($"Appointment {appointmentId} canCancel: {canCancel}");
                                return canCancel;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking if appointment can be cancelled: {ex.Message}");
                return false;
            }
            
            return false;
        }

        private void CancelAppointment(string appointmentId)
        {
            // Double-check validation before cancelling (safety check)
            if (!CanCancelAppointment(appointmentId))
            {
                throw new InvalidOperationException("Cannot cancel appointment: The appointment time has already passed or the appointment is already cancelled.");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE Appointment 
                        SET Appoinment_Status = 'Cancelled' 
                        WHERE Appointment_ID = @AppointmentId 
                        AND Staff_ID = @StaffId
                        AND Appoinment_Status != 'Cancelled'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@StaffId", Session["Staff_ID"]?.ToString());
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                        if (rowsAffected == 0)
                        {
                            throw new InvalidOperationException("Appointment could not be cancelled. It may have already been cancelled or the appointment time has passed.");
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw validation exceptions
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cancelling appointment in database: {ex.Message}");
                throw;
            }
        }

        private void LoadManageAllAppointments()
        {
            try
            {
                List<AppointmentInfo> appointments = GetFutureAppointments();
                
                if (appointments.Count > 0)
                {
                    rptManageAllAppointments.DataSource = appointments;
                    rptManageAllAppointments.DataBind();
                    rptManageAllAppointments.Visible = true;
                    pnlNoManageAllAppointments.Visible = false;
                    btnCancelAllAppointment.Enabled = true;
                }
                else
                {
                    rptManageAllAppointments.Visible = false;
                    pnlNoManageAllAppointments.Visible = true;
                    btnCancelAllAppointment.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading manage all appointments: {ex.Message}");
                rptManageAllAppointments.Visible = false;
                pnlNoManageAllAppointments.Visible = true;
                btnCancelAllAppointment.Enabled = false;
            }
        }

        private List<AppointmentInfo> GetFutureAppointments()
        {
            List<AppointmentInfo> appointments = new List<AppointmentInfo>();
            string staffId = Session["Staff_ID"]?.ToString();
            
            if (string.IsNullOrEmpty(staffId))
                return appointments;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            a.Appointment_ID,
                            a.Appointment_Date,
                            a.Appoinment_Status,
                            a.AppointmentTimeID,
                            c.Customer_Name,
                            c.Customer_Surname,
                            t.Timeslot
                        FROM Appointment a
                        INNER JOIN customer c ON a.Cust_ID = c.Cust_ID
                        LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                        WHERE a.Staff_ID = @StaffId 
                        AND CAST(a.Appointment_Date AS DATE) > CAST(GETDATE() AS DATE)
                        AND a.Appoinment_Status != 'Cancelled'
                        ORDER BY a.Appointment_Date ASC, t.Timeslot ASC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", staffId);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string timeslot = reader["Timeslot"]?.ToString() ?? "N/A";
                                string dbStatus = reader["Appoinment_Status"]?.ToString() ?? "Scheduled";
                                int appointmentId = Convert.ToInt32(reader["Appointment_ID"]);
                                DateTime appointmentDate = Convert.ToDateTime(reader["Appointment_Date"]);
                                
                                // Check if appointment time has passed and if payment exists
                                bool hasTimePassed = false;
                                if (appointmentDate.Date == DateTime.Today)
                                {
                                    hasTimePassed = HasAppointmentTimePassed(timeslot);
                                }
                                else if (appointmentDate < DateTime.Today)
                                {
                                    hasTimePassed = true;
                                }
                                
                                bool hasPayment = CheckIfAppointmentHasPayment(appointmentId);
                                
                                // Determine status: if time passed and no payment, it's missed
                                string finalStatus = dbStatus;
                                if (hasTimePassed && !hasPayment && dbStatus != "Cancelled")
                                {
                                    finalStatus = "Missed";
                                }
                                else if (hasTimePassed && hasPayment)
                                {
                                    finalStatus = "Completed";
                                }
                                
                                var appointment = new AppointmentInfo
                                {
                                    AppointmentId = appointmentId,
                                    AppointmentDate = appointmentDate,
                                    Status = finalStatus,
                                    ServiceType = "Eye Examination", // Default service type
                                    Notes = timeslot,
                                    DoctorName = $"{reader["Customer_Name"]} {reader["Customer_Surname"]}",
                                    HasPayment = hasPayment
                                };
                                
                                appointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting future appointments: {ex.Message}");
            }

            return appointments;
        }

        protected void btnCancelAllAppointment_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected appointment ID from form
                string selectedAppointmentId = Request.Form["selectedAllAppointment"];
                
                if (string.IsNullOrEmpty(selectedAppointmentId))
                {
                    ShowMessage("Please select an appointment to cancel.", false);
                    return;
                }

                // Check if appointment can be cancelled (time hasn't passed)
                if (!CanCancelAppointment(selectedAppointmentId))
                {
                    ShowMessage("Cannot cancel this appointment. The appointment time has already passed. Appointments that have already occurred cannot be cancelled.", false);
                    return;
                }

                // Cancel the appointment (this will also validate again)
                CancelAppointment(selectedAppointmentId);
                
                // Reload the manage all appointments list
                LoadManageAllAppointments();
                
                // Update dashboard stats
                LoadDashboardStats();
                
                ShowMessage("Appointment cancelled successfully.", true);
                
                // Refresh the modal
                string script = "openModal('manageAllModal');";
                ScriptManager.RegisterStartupScript(this, GetType(), "RefreshManageAllModal", script, true);
            }
            catch (InvalidOperationException ex)
            {
                // Handle validation errors specifically
                System.Diagnostics.Debug.WriteLine($"Validation error cancelling appointment: {ex.Message}");
                ShowMessage(ex.Message, false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error cancelling appointment: {ex.Message}");
                ShowMessage("An error occurred while cancelling the appointment. " + ex.Message, false);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            string script = $@"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowMessage", script, true);
        }

        protected void btnUpdateTimeslots_Click(object sender, EventArgs e)
        {
            // Load calendar and open modal
            LoadFutureCalendar();
            string script = "openModal('updateTimeslotsModal');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenUpdateTimeslotsModal", script, true);
        }

        private void LoadFutureCalendar()
        {
            pnlCalendar.Controls.Clear();
            
            DateTime today = DateTime.Today;
            
            // Generate next 30 days (excluding today)
            for (int i = 1; i <= 30; i++)
            {
                DateTime date = today.AddDays(i);
                
                LinkButton btnDay = new LinkButton();
                btnDay.Text = date.Day.ToString();
                btnDay.CssClass = "calendar-day";
                btnDay.CommandArgument = date.ToString("yyyy-MM-dd");
                btnDay.Command += BtnDay_Click;
                btnDay.Style["padding"] = "0.75rem";
                btnDay.Style["text-align"] = "center";
                btnDay.Style["border"] = "2px solid #e0e0e0";
                btnDay.Style["border-radius"] = "8px";
                btnDay.Style["cursor"] = "pointer";
                btnDay.Style["background"] = "white";
                btnDay.Style["transition"] = "all 0.3s ease";
                btnDay.Style["text-decoration"] = "none";
                btnDay.Style["color"] = "#333";
                btnDay.Style["display"] = "block";
                
                pnlCalendar.Controls.Add(btnDay);
            }
        }

        protected void BtnDay_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string dateStr = btn.CommandArgument;
            DateTime selectedDate = DateTime.Parse(dateStr);
            
            // Update selected date display
            litSelectedDate.Text = selectedDate.ToString("dddd, MMMM dd, yyyy");
            
            // Highlight selected day
            foreach (Control ctrl in pnlCalendar.Controls)
            {
                if (ctrl is LinkButton)
                {
                    LinkButton lb = (LinkButton)ctrl;
                    if (lb.CommandArgument == dateStr)
                    {
                        lb.Style["background"] = "#2c5aa0";
                        lb.Style["color"] = "white";
                        lb.Style["border-color"] = "#2c5aa0";
                    }
                    else
                    {
                        lb.Style["background"] = "white";
                        lb.Style["color"] = "#333";
                        lb.Style["border-color"] = "#e0e0e0";
                    }
                }
            }
            
            // Load timeslots for selected date
            LoadTimeslotsForDate(selectedDate);
            
            updTimeslots.Update();
        }

        private void LoadTimeslotsForDate(DateTime selectedDate)
        {
            pnlTimeslots.Controls.Clear();
            
            string staffId = Session["Staff_ID"]?.ToString();
            if (string.IsNullOrEmpty(staffId))
            {
                Label lblError = new Label();
                lblError.Text = "Error: Staff ID not found.";
                lblError.Style["grid-column"] = "1 / -1";
                lblError.Style["text-align"] = "center";
                lblError.Style["color"] = "#dc3545";
                pnlTimeslots.Controls.Add(lblError);
                return;
            }
            
            // Get all timeslots from tblTime
            List<TimeslotInfo> allTimeslots = GetAllTimeslotsInternal();
            
            // Get appointments for this date
            List<int> bookedTimeIds = GetBookedTimeIds(selectedDate, staffId);
            
            // Get blocked slots for this date
            List<int> blockedTimeIds = GetBlockedTimeIds(selectedDate, staffId);
            
            // Display all timeslots
            foreach (var timeslot in allTimeslots)
            {
                bool isBooked = bookedTimeIds.Contains(timeslot.TimeID);
                bool isBlocked = blockedTimeIds.Contains(timeslot.TimeID);
                
                LinkButton btnSlot = new LinkButton();
                btnSlot.Text = timeslot.Timeslot;
                btnSlot.CommandArgument = timeslot.TimeID.ToString();
                btnSlot.Command += BtnTimeslot_Click;
                
                string bgColor = isBooked ? "#dc3545" : (isBlocked ? "#ffc107" : "#28a745");
                string cursor = isBooked ? "not-allowed" : "pointer";
                
                btnSlot.Style["padding"] = "1rem";
                btnSlot.Style["border-radius"] = "8px";
                btnSlot.Style["text-align"] = "center";
                btnSlot.Style["font-weight"] = "600";
                btnSlot.Style["color"] = "white";
                btnSlot.Style["cursor"] = cursor;
                btnSlot.Style["background"] = bgColor;
                btnSlot.Style["border"] = "2px solid " + bgColor;
                btnSlot.Style["text-decoration"] = "none";
                btnSlot.Style["display"] = "block";
                
                if (isBooked)
                {
                    btnSlot.Enabled = false;
                    btnSlot.ToolTip = "Booked - Cannot be blocked";
                }
                else if (isBlocked)
                {
                    btnSlot.ToolTip = "Click to unblock";
                }
                else
                {
                    btnSlot.ToolTip = "Click to block";
                }
                
                // Store date in ViewState for command handler
                ViewState["SelectedDate"] = selectedDate.ToString("yyyy-MM-dd");
                
                pnlTimeslots.Controls.Add(btnSlot);
            }
        }

        private List<TimeslotInfo> GetAllTimeslotsInternal()
        {
            List<TimeslotInfo> timeslots = new List<TimeslotInfo>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT TimeID, Timeslot
                        FROM tblTime
                        WHERE TimeID IS NOT NULL
                        ORDER BY TimeID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                timeslots.Add(new TimeslotInfo
                                {
                                    TimeID = Convert.ToInt32(reader["TimeID"]),
                                    Timeslot = reader["Timeslot"]?.ToString() ?? ""
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting timeslots: {ex.Message}");
            }
            
            return timeslots;
        }

        private List<int> GetBookedTimeIds(DateTime date, string staffId)
        {
            List<int> bookedIds = new List<int>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT DISTINCT AppointmentTimeID
                        FROM Appointment
                        WHERE Staff_ID = @StaffId 
                        AND CAST(Appointment_Date AS DATE) = @Date
                        AND Appoinment_Status != 'Cancelled'
                        AND AppointmentTimeID IS NOT NULL";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int staffIdInt;
                        if (int.TryParse(staffId, out staffIdInt))
                        {
                            cmd.Parameters.AddWithValue("@StaffId", staffIdInt);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@StaffId", staffId);
                        }
                        cmd.Parameters.AddWithValue("@Date", date.Date);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookedIds.Add(Convert.ToInt32(reader["AppointmentTimeID"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting booked time IDs: {ex.Message}");
            }
            
            return bookedIds;
        }

        private List<int> GetBlockedTimeIds(DateTime date, string staffId)
        {
            List<int> blockedIds = new List<int>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT TimeID
                        FROM BlockedTimeslots
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        cmd.Parameters.AddWithValue("@Date", date.Date);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                blockedIds.Add(Convert.ToInt32(reader["TimeID"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting blocked time IDs: {ex.Message}");
            }
            
            return blockedIds;
        }

        protected void BtnTimeslot_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int timeId = Convert.ToInt32(btn.CommandArgument);
            string dateStr = ViewState["SelectedDate"]?.ToString();
            
            if (string.IsNullOrEmpty(dateStr))
            {
                ShowMessage("Please select a date first.", false);
                return;
            }
            
            DateTime selectedDate = DateTime.Parse(dateStr);
            string staffId = Session["Staff_ID"]?.ToString();
            
            // Check if currently blocked
            List<int> blockedIds = GetBlockedTimeIds(selectedDate, staffId);
            bool isBlocked = blockedIds.Contains(timeId);
            
            if (isBlocked)
            {
                // Unblock
                UnblockTimeslot(selectedDate, timeId, staffId);
                ShowMessage("Timeslot unblocked successfully.", true);
            }
            else
            {
                // Block
                BlockTimeslot(selectedDate, timeId, staffId);
                ShowMessage("Timeslot blocked successfully.", true);
            }
            
            // Reload timeslots
            LoadTimeslotsForDate(selectedDate);
            updTimeslots.Update();
        }

        private void BlockTimeslot(DateTime date, int timeId, string staffId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Check if already blocked
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM BlockedTimeslots 
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date
                        AND TimeID = @TimeId";
                    
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        checkCmd.Parameters.AddWithValue("@Date", date.Date);
                        checkCmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            return; // Already blocked
                        }
                    }
                    
                    // Insert new block
                    string insertQuery = @"
                        INSERT INTO BlockedTimeslots (Staff_ID, Blocked_Date, TimeID, Created_At)
                        VALUES (@StaffId, @Date, @TimeId, GETDATE())";
                    
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        insertCmd.Parameters.AddWithValue("@Date", date.Date);
                        insertCmd.Parameters.AddWithValue("@TimeId", timeId);
                        
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error blocking timeslot: {ex.Message}");
                throw;
            }
        }

        private void UnblockTimeslot(DateTime date, int timeId, string staffId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        DELETE FROM BlockedTimeslots 
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date
                        AND TimeID = @TimeId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        cmd.Parameters.AddWithValue("@Date", date.Date);
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error unblocking timeslot: {ex.Message}");
                throw;
            }
        }

        // Helper class for timeslot info
        private class TimeslotInfo
        {
            public int TimeID { get; set; }
            public string Timeslot { get; set; }
        }

        protected void btnViewSchedule_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/Schedule.aspx");
        }

        protected void btnViewReports_Click(object sender, EventArgs e)
        {
            // Open Power BI report in modal popup
            string script = "openModal('reportsModal');";
            ScriptManager.RegisterStartupScript(this, GetType(), "OpenReportsModal", script, true);
        }

        protected void btnExportData_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Staff/ExportData.aspx");
        }

        #region Blocked Timeslots WebMethods

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public static List<Dictionary<string, object>> GetAllTimeslots()
        {
            List<Dictionary<string, object>> timeslots = new List<Dictionary<string, object>>();
            
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    System.Diagnostics.Debug.WriteLine("ERROR: Connection string is null or empty!");
                    return timeslots;
                }
                
                System.Diagnostics.Debug.WriteLine($"GetAllTimeslots: Starting query with connection: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT TimeID, Timeslot
                        FROM tblTime
                        WHERE TimeID IS NOT NULL
                        ORDER BY TimeID";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        System.Diagnostics.Debug.WriteLine("GetAllTimeslots: Connection opened successfully");
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                            {
                                var timeslot = new Dictionary<string, object>();
                                timeslot["TimeID"] = Convert.ToInt32(reader["TimeID"]);
                                timeslot["Timeslot"] = reader["Timeslot"]?.ToString() ?? "";
                                timeslots.Add(timeslot);
                                count++;
                            }
                            System.Diagnostics.Debug.WriteLine($"GetAllTimeslots: Read {count} rows from database");
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"GetAllTimeslots: Returning {timeslots.Count} timeslots");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in GetAllTimeslots: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                // Return empty list on error
            }
            
            return timeslots;
        }

        [System.Web.Services.WebMethod]
        public static List<Dictionary<string, object>> GetAppointmentsForDate(string dateStr, string staffId)
        {
            List<Dictionary<string, object>> appointments = new List<Dictionary<string, object>>();
            
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;
                
                DateTime selectedDate = DateTime.Parse(dateStr);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            a.AppointmentTimeID,
                            t.Timeslot
                        FROM Appointment a
                        LEFT JOIN tblTime t ON a.AppointmentTimeID = t.TimeID
                        WHERE a.Staff_ID = @StaffId 
                        AND CAST(a.Appointment_Date AS DATE) = @Date
                        AND a.Appoinment_Status != 'Cancelled'";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Staff_ID in Appointment table - check if it's int or varchar
                        // Try converting to int first, if it fails use as string
                        int staffIdInt;
                        if (int.TryParse(staffId, out staffIdInt))
                        {
                            cmd.Parameters.AddWithValue("@StaffId", staffIdInt);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@StaffId", staffId);
                        }
                        cmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var appointment = new Dictionary<string, object>();
                                // AppointmentTimeID should match TimeID (both are integers)
                                // Convert to int for consistent comparison
                                if (reader["AppointmentTimeID"] != DBNull.Value)
                                {
                                    appointment["TimeID"] = Convert.ToInt32(reader["AppointmentTimeID"]);
                                }
                                else
                                {
                                    appointment["TimeID"] = 0; // Or skip this appointment
                                }
                                appointment["Timeslot"] = reader["Timeslot"]?.ToString() ?? "";
                                appointments.Add(appointment);
                            }
                        }
                    }
                }
                
                System.Diagnostics.Debug.WriteLine($"GetAppointmentsForDate: Found {appointments.Count} appointments for {dateStr}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting appointments: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            
            return appointments;
        }

        [System.Web.Services.WebMethod]
        public static List<int> GetBlockedSlotsForDate(string dateStr, string staffId)
        {
            List<int> blockedSlots = new List<int>();
            
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;
                
                DateTime selectedDate = DateTime.Parse(dateStr);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT TimeID 
                        FROM BlockedTimeslots 
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        cmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                        conn.Open();
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                blockedSlots.Add(Convert.ToInt32(reader["TimeID"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting blocked slots: {ex.Message}");
            }
            
            return blockedSlots;
        }

        [System.Web.Services.WebMethod]
        public static bool BlockTimeslot(string dateStr, int timeId, string staffId)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;
                
                DateTime selectedDate = DateTime.Parse(dateStr);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Check if already blocked
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM BlockedTimeslots 
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date
                        AND TimeID = @TimeId";
                    
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        checkCmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                        checkCmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            return true; // Already blocked
                        }
                    }
                    
                    // Insert new block
                    string insertQuery = @"
                        INSERT INTO BlockedTimeslots (Staff_ID, Blocked_Date, TimeID, Created_At)
                        VALUES (@StaffId, @Date, @TimeId, GETDATE())";
                    
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        insertCmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                        insertCmd.Parameters.AddWithValue("@TimeId", timeId);
                        
                        insertCmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error blocking timeslot: {ex.Message}");
                return false;
            }
        }

        [System.Web.Services.WebMethod]
        public static bool UnblockTimeslot(string dateStr, int timeId, string staffId)
        {
            try
            {
                string connectionString = System.Configuration.ConfigurationManager
                    .ConnectionStrings["ProductConnection"].ConnectionString;
                
                DateTime selectedDate = DateTime.Parse(dateStr);
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        DELETE FROM BlockedTimeslots 
                        WHERE Staff_ID = @StaffId 
                        AND Blocked_Date = @Date
                        AND TimeID = @TimeId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", Convert.ToInt32(staffId));
                        cmd.Parameters.AddWithValue("@Date", selectedDate.Date);
                        cmd.Parameters.AddWithValue("@TimeId", timeId);
                        conn.Open();
                        
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error unblocking timeslot: {ex.Message}");
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Checks if the appointment time slot has passed (for today's appointments)
        /// </summary>
        private bool HasAppointmentTimePassed(string timeslot)
        {
            if (string.IsNullOrEmpty(timeslot) || timeslot == "N/A")
                return false;

            try
            {
                // Parse timeslot format "9h30 - 10h00" to get start time
                var startTime = timeslot.Split('-')[0].Trim(); // Gets "9h30"
                var timeParts = startTime.Replace("h", ":").Split(':'); // Gets ["9", "30"]
                
                if (timeParts.Length < 2)
                    return false;

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

        /// <summary>
        /// Checks if payment has been recorded for the appointment
        /// </summary>
        private bool CheckIfAppointmentHasPayment(int appointmentId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT COUNT(*) 
                        FROM Payments 
                        WHERE Appointment_ID = @AppointmentId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        conn.Open();
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking payment for appointment {appointmentId}: {ex.Message}");
                return false;
            }
        }

    }
}