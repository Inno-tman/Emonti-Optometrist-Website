using System;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
    /// <summary>
    /// Help page providing FAQ, contact information, and policies
    /// </summary>
    public partial class Help : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set page title and meta description for SEO
                Page.Title = "Help & Support - Emonti Optometrist";
                Page.MetaDescription = "Get help with your eye care needs. Find answers to frequently asked questions, contact information, and learn about our policies.";
            }
        }
    }
}
