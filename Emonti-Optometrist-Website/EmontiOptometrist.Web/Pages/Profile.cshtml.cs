using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmontiOptometrist.Web.Pages;

[Authorize]
public class ProfileModel : PageModel
{
    public string WelcomeName { get; set; } = "";

    public void OnGet()
    {
        WelcomeName = User.Identity?.Name ?? "Guest";
    }
}
