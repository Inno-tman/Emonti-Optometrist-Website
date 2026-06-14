namespace EmontiOptometrist.Web.Services;

public static class AuthSession
{
    public const string IsLoggedIn = "IsLoggedIn";
    public const string CustId = "Cust_ID";
    public const string UserEmail = "UserEmail";
    public const string FirstName = "FirstName";
    public const string LastName = "LastName";

    public const string IsStaffLoggedIn = "IsStaffLoggedIn";
    public const string StaffId = "Staff_ID";
    public const string StaffName = "StaffName";
    public const string StaffRole = "StaffRole";

    public static bool IsCustomerLoggedIn(HttpContext? ctx) =>
        ctx?.Session.GetString(IsLoggedIn) == "true";

    public static string? GetCustId(HttpContext? ctx) =>
        ctx?.Session.GetString(CustId);

    public static bool IsStaffLoggedInCheck(HttpContext? ctx) =>
        ctx?.Session.GetString(IsStaffLoggedIn) == "true";

    public static bool IsAdmin(HttpContext? ctx) =>
        ctx?.Session.GetString(StaffRole) == "Admin";

    public static void SignInCustomer(HttpContext ctx, string custId, string email, string firstName, string lastName)
    {
        ctx.Session.SetString(IsLoggedIn, "true");
        ctx.Session.SetString(CustId, custId);
        ctx.Session.SetString(UserEmail, email);
        ctx.Session.SetString(FirstName, firstName);
        ctx.Session.SetString(LastName, lastName);
    }

    public static void SignInStaff(HttpContext ctx, string staffId, string name, string role)
    {
        ctx.Session.SetString(IsStaffLoggedIn, "true");
        ctx.Session.SetString(StaffId, staffId);
        ctx.Session.SetString(StaffName, name);
        ctx.Session.SetString(StaffRole, role);
    }

    public static void SignOut(HttpContext ctx)
    {
        ctx.Session.Remove(IsLoggedIn);
        ctx.Session.Remove(CustId);
        ctx.Session.Remove(UserEmail);
        ctx.Session.Remove(FirstName);
        ctx.Session.Remove(LastName);
        ctx.Session.Remove(IsStaffLoggedIn);
        ctx.Session.Remove(StaffId);
        ctx.Session.Remove(StaffName);
        ctx.Session.Remove(StaffRole);
    }
}
