using Microsoft.AspNetCore.Identity;

namespace EmontiOptometrist.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
