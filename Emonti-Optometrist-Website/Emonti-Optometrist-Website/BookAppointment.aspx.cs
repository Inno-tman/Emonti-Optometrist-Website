using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
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
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.ToString()));
                return;
            }

            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void InitializePage()
        {
            LoadOptometrists();

            // Load customer info into hidden fields
            hfCustomerName.Value = $"{Session["FirstName"]} {Session["LastName"]}";
            hfCustomerEmail.Value = Session["UserEmail"]?.ToString() ?? "";
            hfCustomerPhone.Value = Session["Cellphone"]?.ToString() ?? "";

            // Store Cust_ID for AJAX availability check
            string custIdScript = $"window.__custId = '{Session["Cust_ID"]}';";
            ClientScript.RegisterStartupScript(GetType(), "CustId", custIdScript, true);

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
                string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT a.Staff_ID, s.Staff_Name, s.Staff_Surname
                        FROM Appointment a
                        INNER JOIN Staff s ON a.Staff_ID = s.Staff_ID
                        WHERE a.Appointment_ID = @AppointmentId
                        AND a.Cust_ID = @CustomerId";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                        cmd.Parameters.AddWithValue("@CustomerId", Session["Cust_ID"]);
                        conn.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string staffId = reader["Staff_ID"].ToString();
                                if (ddlOptometrist.Items.FindByValue(staffId) != null)
                                {
                                    ddlOptometrist.SelectedValue = staffId;
                                }
                                hfRebooking.Value = "true";
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

        private void LoadOptometrists()
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT Staff_ID, Staff_Name, Staff_Surname
                        FROM Staff
                        WHERE Staff_Role = 'Optometrist'
                        ORDER BY Staff_Name, Staff_Surname";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            ddlOptometrist.Items.Clear();
                            ddlOptometrist.Items.Add(new ListItem("-- Select Optometrist --", ""));

                            while (reader.Read())
                            {
                                string staffId = reader["Staff_ID"].ToString();
                                string staffName = $"{reader["Staff_Name"]} {reader["Staff_Surname"]}";
                                ddlOptometrist.Items.Add(new ListItem(staffName, staffId));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading optometrists: {ex.Message}");
            }
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
            // Validate appointment type
            if (string.IsNullOrEmpty(ddlAppointmentType.SelectedValue))
            {
                ShowErrorMessage("Please select an appointment type.");
                return false;
            }

            // Validate optometrist
            if (string.IsNullOrEmpty(ddlOptometrist.SelectedValue))
            {
                ShowErrorMessage("Please select an optometrist.");
                return false;
            }

            // Validate date
            DateTime selectedDate;
            if (string.IsNullOrEmpty(inputDate.Value) || !DateTime.TryParse(inputDate.Value, out selectedDate))
            {
                ShowErrorMessage("Please select a valid date.");
                return false;
            }

            if (selectedDate.Date < DateTime.Today)
            {
                ShowErrorMessage("Please select a future date.");
                return false;
            }

            if (selectedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                ShowErrorMessage("We are closed on Sundays. Please select a different date.");
                return false;
            }

            // Validate time slot
            if (string.IsNullOrEmpty(Request.Form["ddlTimeSlot"] ?? ""))
            {
                ShowErrorMessage("Please select an appointment time.");
                return false;
            }

            // Validate same-day 2-hour rule server-side
            var timeSlots = new[] {
                new { Value = "1", StartHour = 8, StartMin = 0 },
                new { Value = "2", StartHour = 9, StartMin = 0 },
                new { Value = "3", StartHour = 10, StartMin = 0 },
                new { Value = "4", StartHour = 11, StartMin = 0 },
                new { Value = "5", StartHour = 13, StartMin = 0 },
                new { Value = "6", StartHour = 14, StartMin = 0 },
                new { Value = "7", StartHour = 15, StartMin = 0 },
            };

            var slot = Array.Find(timeSlots, s => s.Value == (Request.Form["ddlTimeSlot"] ?? ""));
            if (slot == null)
            {
                ShowErrorMessage("Invalid time slot.");
                return false;
            }

            var slotStart = new TimeSpan(slot.StartHour, slot.StartMin, 0);
            var businessClose = selectedDate.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(14, 0, 0) : new TimeSpan(17, 0, 0);

            if (slotStart < new TimeSpan(8, 0, 0) || slotStart >= businessClose)
            {
                ShowErrorMessage("The selected time is outside our business hours.");
                return false;
            }

            if (selectedDate == DateTime.Today)
            {
                var slotDateTime = DateTime.Today.Add(slotStart);
                if (DateTime.Now >= slotDateTime)
                {
                    ShowErrorMessage("This time has already passed.");
                    return false;
                }
                if (slotDateTime <= DateTime.Now.AddHours(2))
                {
                    ShowErrorMessage("Same-day bookings need at least 2 hours notice.");
                    return false;
                }
            }

            // Check for duplicate appointment per day
            if (CustomerHasAppointmentOnDate(Session["Cust_ID"].ToString(), selectedDate))
            {
                ShowErrorMessage("You already have an appointment on this date. Only one appointment per day allowed.");
                return false;
            }

            return true;
        }

        private bool CustomerHasAppointmentOnDate(string customerId, DateTime date)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT COUNT(*)
                        FROM Appointment
                        WHERE Cust_ID = @CustomerId
                        AND CAST(Appointment_Date AS DATE) = @Date
                        AND Appoinment_Status != 'Cancelled'";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@Date", date);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private bool SaveAppointment()
        {
            try
            {
                if (Session["Cust_ID"] == null)
                {
                    ShowErrorMessage("Customer session expired. Please log in again.");
                    return false;
                }

                DateTime selectedDate = DateTime.Parse(inputDate.Value);

                string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    string query = @"
                        INSERT INTO Appointment
                        (Cust_ID, Staff_ID, Appointment_Date, AppointmentTimeID, Appoinment_Status, Appointment_Type)
                        VALUES (@CustId, @StaffId, @AppointmentDate, @AppointmentTimeId, @Status, @Type)";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CustId", Session["Cust_ID"]);
                        cmd.Parameters.AddWithValue("@StaffId", ddlOptometrist.SelectedValue);
                        cmd.Parameters.AddWithValue("@AppointmentDate", selectedDate);
                        cmd.Parameters.AddWithValue("@AppointmentTimeId", Request.Form["ddlTimeSlot"] ?? "");
                        cmd.Parameters.AddWithValue("@Status", "Pending");
                        cmd.Parameters.AddWithValue("@Type", ddlAppointmentType.SelectedValue);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            SendConfirmationEmail(selectedDate);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving appointment: {ex.Message}");
                ShowErrorMessage($"Failed to save appointment: {ex.Message}");
                return false;
            }
        }

        private void SendConfirmationEmail(DateTime appointmentDate)
        {
            try
            {
                string customerEmail = Session["UserEmail"]?.ToString();
                if (string.IsNullOrEmpty(customerEmail)) return;

                string firstName = Session["FirstName"]?.ToString() ?? "";
                string lastName = Session["LastName"]?.ToString() ?? "";
                string customerName = $"{firstName} {lastName}".Trim();
                if (string.IsNullOrEmpty(customerName)) customerName = "Valued Customer";

                string optometristName = ddlOptometrist.SelectedItem?.Text ?? "Selected Optometrist";
                string timeSlot = GetTimeSlotText(Request.Form["ddlTimeSlot"] ?? "");
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
<p style=""margin:0 0 20px 0;color:#333;font-size:16px;line-height:1.6;"">Dear {HttpUtility.HtmlEncode(customerName)},</p>
<p style=""margin:0 0 25px 0;color:#555;font-size:15px;line-height:1.6;"">Your appointment has been successfully confirmed. We're looking forward to seeing you!</p>
<div style=""background-color:#f8f9fa;padding:20px;margin:20px 0;border-radius:4px;border-left:4px solid #667eea;"">
<table width=""100%"" cellpadding=""0"" cellspacing=""0"">
<tr><td style=""padding:8px 0;color:#666;font-size:14px;width:120px;""><strong>Type:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{HttpUtility.HtmlEncode(ddlAppointmentType.SelectedItem?.Text ?? "Eye Exam")}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Optometrist:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{HttpUtility.HtmlEncode(optometristName)}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Date:</strong></td><td style=""padding:8px 0;color:#333;font-size:14px;"">{appointmentDate:dddd, MMMM dd, yyyy}</td></tr>
<tr><td style=""padding:8px 0;color:#666;font-size:14px;""><strong>Time:</strong></td><td style=""padding:8px 0;color:#667eea;font-size:14px;font-weight:600;"">{HttpUtility.HtmlEncode(timeSlot)}</td></tr>
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

                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
                int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");
                string smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"];
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                string smtpFromName = ConfigurationManager.AppSettings["SmtpFromName"] ?? "Emonti Optometrist";
                bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["SmtpEnableSsl"] ?? "true");

                if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword)) return;

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpEmail, smtpPassword);
                    smtp.EnableSsl = enableSsl;
                    smtp.Timeout = 30000;

                    using (var message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpEmail, smtpFromName);
                        message.To.Add(customerEmail);
                        message.Subject = "Appointment Confirmation - Emonti Optometrist";
                        message.Body = body;
                        message.IsBodyHtml = true;
                        smtp.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        private string GetTimeSlotText(string timeId)
        {
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    string query = "SELECT Timeslot FROM tblTime WHERE TimeID = @TimeId";
                    using (var cmd = new SqlCommand(query, conn))
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
            DateTime appointmentDate = DateTime.Parse(inputDate.Value);
            string timeSlot = GetTimeSlotText(Request.Form["ddlTimeSlot"] ?? "");
            string message = $"Appointment booked successfully! Confirmation details have been sent to your email. We look forward to seeing you on {appointmentDate:MMMM dd, yyyy} at {timeSlot}.";

            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-success";
            lblMessage.Text = message;

            ClientScript.RegisterStartupScript(GetType(), "ScrollToTop", "window.scrollTo({top:0,behavior:'smooth'});", true);
        }

        private void ShowErrorMessage(string message)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = "alert alert-danger";
            lblMessage.Text = message;

            ClientScript.RegisterStartupScript(GetType(), "ScrollToTop", "window.scrollTo({top:0,behavior:'smooth'});", true);
        }

        private void ClearForm()
        {
            ddlAppointmentType.SelectedIndex = 0;
            ddlOptometrist.SelectedIndex = 0;
            inputDate.Value = "";
            hfRebooking.Value = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        private string GetLogoBase64()
        {
            try
            {
                string logoPath = Server.MapPath("~/Images/Logo/Emonti Logo Banner.png");
                byte[] imageBytes = System.IO.File.ReadAllBytes(logoPath);
                string base64 = Convert.ToBase64String(imageBytes);
                return $"data:image/png;base64,{base64}";
            }
            catch
            {
                return "";
            }
        }
    }
}
