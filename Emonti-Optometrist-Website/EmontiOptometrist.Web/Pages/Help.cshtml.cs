using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmontiOptometrist.Web.Pages;

public class HelpModel : PageModel
{
    public void OnGet()
    {
        ViewData["Title"] = "Help & Support - Emonti Optometrist";
    }
}
