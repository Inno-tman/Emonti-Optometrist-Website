using System;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
 public partial class Reports : Page
 {
 protected void Page_Load(object sender, EventArgs e)
 {
     if (Session["IsStaffLoggedIn"] == null || !(bool)Session["IsStaffLoggedIn"])
     {
         Response.Redirect("~/Account/Login.aspx");
         return;
     }
 }
 }
}
