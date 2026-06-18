<%@ WebHandler Language="C#" Class="CheckAvailability" %>

using System;
using System.Web;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using System.Configuration;

public class CheckAvailability : IHttpHandler
{
    private readonly JavaScriptSerializer _json = new JavaScriptSerializer();

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.AppendHeader("Cache-Control", "no-cache");

        try
        {
            string dateStr = context.Request.QueryString["date"];
            string time = context.Request.QueryString["time"];
            string optometristId = context.Request.QueryString["optometristId"];

            if (string.IsNullOrEmpty(dateStr) || string.IsNullOrEmpty(time) || string.IsNullOrEmpty(optometristId))
            {
                WriteJson(context, new { available = false, message = "Please fill in all fields." });
                return;
            }

            if (!DateTime.TryParse(dateStr, out DateTime parsedDate))
            {
                WriteJson(context, new { available = false, message = "Invalid date." });
                return;
            }

            if (parsedDate.Date < DateTime.Today)
            {
                WriteJson(context, new { available = false, message = "This date has already passed. Please select a future date." });
                return;
            }

            if (parsedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                WriteJson(context, new { available = false, message = "We are closed on Sundays. Please select a different date." });
                return;
            }

            // Parse time slot
            var timeSlots = new[] {
                new { Value = "1", Text = "08:00 - 09:00", StartHour = 8, StartMin = 0 },
                new { Value = "2", Text = "09:00 - 10:00", StartHour = 9, StartMin = 0 },
                new { Value = "3", Text = "10:00 - 11:00", StartHour = 10, StartMin = 0 },
                new { Value = "4", Text = "11:00 - 12:00", StartHour = 11, StartMin = 0 },
                new { Value = "5", Text = "13:00 - 14:00", StartHour = 13, StartMin = 0 },
                new { Value = "6", Text = "14:00 - 15:00", StartHour = 14, StartMin = 0 },
                new { Value = "7", Text = "15:00 - 16:00", StartHour = 15, StartMin = 0 },
            };

            var slot = Array.Find(timeSlots, s => s.Value == time);
            if (slot == null)
            {
                WriteJson(context, new { available = false, message = "Invalid time slot." });
                return;
            }

            var slotStart = new TimeSpan(slot.StartHour, slot.StartMin, 0);
            var businessClose = parsedDate.DayOfWeek == DayOfWeek.Saturday ? new TimeSpan(14, 0, 0) : new TimeSpan(17, 0, 0);

            if (slotStart < new TimeSpan(8, 0, 0) || slotStart >= businessClose)
            {
                string closeTime = parsedDate.DayOfWeek == DayOfWeek.Saturday ? "2:00 PM" : "5:00 PM";
                WriteJson(context, new { available = false, message = $"This slot is outside our business hours (8 AM - {closeTime})." });
                return;
            }

            if (parsedDate == DateTime.Today)
            {
                var slotDateTime = DateTime.Today.Add(slotStart);
                if (DateTime.Now >= slotDateTime)
                {
                    WriteJson(context, new { available = false, message = "This time has already passed. Please select a future slot." });
                    return;
                }
                if (slotDateTime <= DateTime.Now.AddHours(2))
                {
                    WriteJson(context, new { available = false, message = "Same-day bookings need at least 2 hours notice." });
                    return;
                }
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Check customer has no appointment on this date
                string custId = context.Request.QueryString["custId"];
                if (!string.IsNullOrEmpty(custId))
                {
                    using (var perDayCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Appointment WHERE Cust_ID = @CustId AND CAST(Appointment_Date AS DATE) = @Date AND Appoinment_Status != 'Cancelled'", conn))
                    {
                        perDayCmd.Parameters.AddWithValue("@CustId", custId);
                        perDayCmd.Parameters.AddWithValue("@Date", parsedDate.Date);
                        int count = (int)perDayCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            WriteJson(context, new { available = false, message = "You already have an appointment on this date. Only one appointment per day allowed." });
                            return;
                        }
                    }
                }

                // Check slot is not booked or blocked
                using (var checkCmd = new SqlCommand(@"
                    SELECT
                        (SELECT COUNT(*) FROM Appointment
                         WHERE Staff_ID = @StaffId AND CAST(Appointment_Date AS DATE) = @Date
                         AND AppointmentTimeID = @TimeId AND Appoinment_Status != 'Cancelled') +
                        (SELECT COUNT(*) FROM BlockedTimeslots
                         WHERE Staff_ID = @StaffId AND Blocked_Date = @Date
                         AND TimeID = @TimeId) AS TotalCount", conn))
                {
                    checkCmd.Parameters.AddWithValue("@StaffId", optometristId);
                    checkCmd.Parameters.AddWithValue("@Date", parsedDate.Date);
                    checkCmd.Parameters.AddWithValue("@TimeId", time);
                    int total = (int)checkCmd.ExecuteScalar();
                    if (total > 0)
                    {
                        WriteJson(context, new { available = false, message = "This slot is already booked or blocked." });
                        return;
                    }
                }
            }

            WriteJson(context, new { available = true, message = "This slot is available." });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            WriteJson(context, new { available = false, message = $"Error: {ex.Message}" });
        }
    }

    private void WriteJson(HttpContext context, object data)
    {
        context.Response.Write(_json.Serialize(data));
    }

    public bool IsReusable => false;
}
