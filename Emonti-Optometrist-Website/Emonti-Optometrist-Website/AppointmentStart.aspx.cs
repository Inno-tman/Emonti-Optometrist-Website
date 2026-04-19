using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
    public partial class AppointmentStart : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is already logged in
            bool isLoggedIn = Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];
            
            if (isLoggedIn)
            {
                // If user is already logged in, redirect directly to booking page
                Response.Redirect("~/BookAppointment.aspx");
                return;

                // if user is already logged in, redurect directly to booking page
            }
        }

        protected void btnExistingCustomer_Click(object sender, EventArgs e)
        {
            // Check if user is logged in using session (consistent with Site.Master.cs)
            bool isLoggedIn = Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];
            
            if (isLoggedIn)
            {
                Response.Redirect("~/BookAppointment.aspx");
                return;
            }

            string returnUrl = HttpUtility.UrlEncode("~/BookAppointment.aspx");
            Response.Redirect("~/Account/Login.aspx?ReturnUrl=" + returnUrl);
        }

        protected void btnNewCustomer_Click(object sender, EventArgs e)
        {
            // Send to register, then back to booking
            string returnUrl = HttpUtility.UrlEncode("~/BookAppointment.aspx");
            Response.Redirect("~/Account/Register.aspx?ReturnUrl=" + returnUrl);
        }
    }
}

