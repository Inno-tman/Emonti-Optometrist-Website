using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class ProfileModel : PageModel
{
    public string WelcomeName { get; set; } = "";

    public void OnGet()
    {
        var firstName = HttpContext.Session.GetString("FirstName");
        var lastName = HttpContext.Session.GetString("LastName");
        var staffName = HttpContext.Session.GetString("StaffName");

        if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            WelcomeName = $"{firstName} {lastName}";
        else if (!string.IsNullOrEmpty(staffName))
            WelcomeName = staffName;
        else
            WelcomeName = "Guest";
    }
}
