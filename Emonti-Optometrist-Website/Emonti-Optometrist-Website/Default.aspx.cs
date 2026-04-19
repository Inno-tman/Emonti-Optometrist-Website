using System;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Page load logic if needed
        }

        protected void btnBookEyeExam_Click(object sender, EventArgs e)
        {
            // Redirect to appointment start prompt (existing vs new)
            Response.Redirect("~/AppointmentStart.aspx");
        }

        protected void btnShopFrames_Click(object sender, EventArgs e)
        {
            // Redirect to shop page (create this page later)
            Response.Redirect("~/Shop.aspx");
        }

        protected void btnScheduleExam_Click(object sender, EventArgs e)
        {
            // Same as book eye exam
            Response.Redirect("~/AppointmentStart.aspx");
        }

        protected void btnBrowseEyewear_Click(object sender, EventArgs e)
        {
            // Same as shop frames
            Response.Redirect("~/Shop.aspx");
        }
    }
}