using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EmontiOptometrist.Web.Services;

namespace EmontiOptometrist.Web.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        AuthSession.SignOut(HttpContext);
        return RedirectToPage("/Index");
    }
}
