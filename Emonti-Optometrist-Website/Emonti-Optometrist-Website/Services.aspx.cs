using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Any initialization logic can go here
            }
        }

        protected void BookAppointment_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string serviceType = btn.CommandArgument;
            
            // Redirect to booking page with pre-selected service
            Response.Redirect($"~/BookAppointment.aspx?service={serviceType}");
        }

        protected void LearnMore_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string infoType = btn.CommandArgument;
            
            // Handle different information requests
            switch (infoType)
            {
                case "eye-exams":
                    ShowServiceInfo("Comprehensive Eye Examinations", GetEyeExamInfo());
                    break;
                case "contact-lenses":
                    ShowServiceInfo("Contact Lens Services", GetContactLensInfo());
                    break;
                case "medical-aid":
                    ShowServiceInfo("Medical Aid Information", GetMedicalAidInfo());
                    break;
                default:
                    // Redirect to contact page for more information
                    Response.Redirect("~/Contact.aspx");
                    break;
            }
        }

        protected void CheckBenefits_Click(object sender, EventArgs e)
        {
            // Show the benefits modal
            string script = @"
                if (typeof openBenefitsModal === 'function') {
                    openBenefitsModal();
                }
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "CheckBenefits", script, true);
        }

        protected void btnShopFrames_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Shop.aspx");
        }

        private void ShowServiceInfo(string title, string info)
        {
            string script = $@"
                alert('{title}:\n\n{info}');
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "ServiceInfo", script, true);
        }

        private string GetEyeExamInfo()
        {
            return @"Our comprehensive eye examinations include:

• Complete medical history review
• Visual acuity testing at distance and near
• Refraction to determine prescription
• Eye muscle coordination tests
• Peripheral vision assessment
• Eye pressure measurement (Glaucoma screening)
• Dilated fundus examination
• Digital retinal photography
• OCT imaging (when indicated)

Duration: 45-60 minutes
Recommended: Annual exams, or as directed by optometrist

Call 076 463 1930 to schedule your appointment.";
        }

        private string GetContactLensInfo()
        {
            return @"Contact Lens Services Include:

INITIAL FITTING:
• Eye health evaluation
• Corneal measurements
• Lifestyle assessment
• Trial lens fitting
• Comfort evaluation
• Insertion/removal training
• Care instruction

LENS OPTIONS:
• Daily disposable lenses
• Weekly/Monthly replacement
• Toric lenses (for astigmatism)  
• Multifocal lenses (for presbyopia)
• Specialty lenses

FOLLOW-UP CARE:
• Progress evaluations
• Comfort assessments
• Prescription adjustments
• Ongoing support

Most medical aids cover contact lens fittings.";
        }

        private string GetMedicalAidInfo()
        {
            return @"Medical Aid Information:

WE ACCEPT:
• Discovery Health
• Momentum Health  
• Bonitas Medical Fund
• Medscheme
• Most other medical aids

SERVICES COVERED:
• Eye examinations (PMB benefit)
• Contact lens fittings
• Therapeutic services
• Emergency eye care

FRAMES & LENSES:
• Annual eyewear benefit
• Varies by scheme and option
• We help maximize your benefits

PROCESS:
• We submit claims directly
• Real-time benefit checking
• Pre-authorization when needed
• Detailed benefit explanations

Bring your medical aid card and ID for all visits.";
        }
    }
}