using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Pages.Admin;

[Authorize(Roles = "Admin")]
public class ManageStaffModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ManageStaffModel(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<StaffUser> StaffUsers { get; set; } = new();
    public List<ApplicationUser> AllUsers { get; set; } = new();

    public async Task OnGetAsync()
    {
        await LoadData();
    }

    public async Task<IActionResult> OnPostAddStaffAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            if (!await _roleManager.RoleExistsAsync("Staff"))
                await _roleManager.CreateAsync(new IdentityRole("Staff"));

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            await _userManager.AddToRoleAsync(user, "Staff");
            TempData["SuccessMessage"] = $"{user.Email} added as staff.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveStaffAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.RemoveFromRoleAsync(user, "Staff");
            await _userManager.RemoveFromRoleAsync(user, "Admin");
            TempData["SuccessMessage"] = $"{user.Email} removed from staff.";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostMakeAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            await _userManager.AddToRoleAsync(user, "Admin");
            TempData["SuccessMessage"] = $"{user.Email} promoted to admin.";
        }

        return RedirectToPage();
    }

    private async Task LoadData()
    {
        AllUsers = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();

        var staffUsers = new List<StaffUser>();
        foreach (var user in AllUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any(r => r == "Staff" || r == "Admin"))
            {
                staffUsers.Add(new StaffUser
                {
                    UserId = user.Id,
                    Email = user.Email ?? "",
                    Roles = roles.ToList(),
                    IsAdmin = roles.Contains("Admin")
                });
            }
        }
        StaffUsers = staffUsers;
    }
}

public class StaffUser
{
    public string UserId { get; set; } = "";
    public string Email { get; set; } = "";
    public List<string> Roles { get; set; } = new();
    public bool IsAdmin { get; set; }
}
