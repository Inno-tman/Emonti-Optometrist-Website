using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using EmontiOptometrist.Web.Models;

namespace EmontiOptometrist.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public string ModelOnlyError { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "First name is required")]
            [RegularExpression(@"^[a-zA-Z\s\-']{2,50}$", ErrorMessage = "First name must be 2-50 characters, letters and spaces only")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            [RegularExpression(@"^[a-zA-Z\s\-']{2,50}$", ErrorMessage = "Last name must be 2-50 characters, letters and spaces only")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Phone number is required")]
            [RegularExpression(@"^0\d{9}$", ErrorMessage = "Phone must be 10 digits starting with 0 (e.g., 0123456789)")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Date of birth is required")]
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }

            [Required(ErrorMessage = "Please select your gender")]
            public string Gender { get; set; }

            public string MedicalAid { get; set; }
            public string MedicalAidNumber { get; set; }

            public string IsMainMember { get; set; }
            public string MainMemberName { get; set; }
            public string MainMemberSurname { get; set; }
            public string MainMemberID { get; set; }

            public string StreetNumber { get; set; }
            public string StreetName { get; set; }
            public string ComplexName { get; set; }
            public string UnitNumber { get; set; }
            public string City { get; set; }
            public string Province { get; set; }

            [RegularExpression(@"^\d{4}$", ErrorMessage = "Postal code must be 4 digits")]
            public string PostalCode { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ErrorMessage = null;
            ModelOnlyError = null;

            if (!ModelState.IsValid)
                return Page();

            // Server-side validations
            if (string.IsNullOrEmpty(Input.Gender))
            {
                ErrorMessage = "Please select your gender.";
                return Page();
            }

            if (Input.DateOfBirth == default)
            {
                ErrorMessage = "Please enter your date of birth.";
                return Page();
            }

            int age = DateTime.Today.Year - Input.DateOfBirth.Year;
            if (Input.DateOfBirth > DateTime.Today.AddYears(-age)) age--;
            if (age < 13 || age > 80)
            {
                ErrorMessage = "Age must be between 13 and 80 years.";
                return Page();
            }

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(Input.Email);
            if (existingUser != null)
            {
                ErrorMessage = "An account with this email address already exists. Please login or use a different email.";
                return Page();
            }

            // Validate main member fields if "No" is selected
            bool isMainMember = string.IsNullOrEmpty(Input.IsMainMember) || Input.IsMainMember == "true";
            if (!isMainMember)
            {
                if (string.IsNullOrWhiteSpace(Input.MainMemberName) || !Regex.IsMatch(Input.MainMemberName, @"^[a-zA-Z\s\-']{2,50}$"))
                {
                    ErrorMessage = "Please enter a valid main member first name.";
                    return Page();
                }
                if (string.IsNullOrWhiteSpace(Input.MainMemberSurname) || !Regex.IsMatch(Input.MainMemberSurname, @"^[a-zA-Z\s\-']{2,50}$"))
                {
                    ErrorMessage = "Please enter a valid main member surname.";
                    return Page();
                }
                if (string.IsNullOrWhiteSpace(Input.MainMemberID) || !Regex.IsMatch(Input.MainMemberID, @"^\d{13}$"))
                {
                    ErrorMessage = "Main member ID must be 13 digits.";
                    return Page();
                }
            }

            // Validate address - at least city required
            if (string.IsNullOrWhiteSpace(Input.City))
            {
                ErrorMessage = "Please provide at least your city.";
                return Page();
            }

            // Create Identity user
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                FullName = $"{Input.FirstName} {Input.LastName}"
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                ModelOnlyError = string.Join(" ", result.Errors.Select(e => e.Description));
                return Page();
            }

            _logger.LogInformation("User created a new account with password.");

            // Insert extended profile into customer table
            try
            {
                InsertCustomerProfile(user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to insert customer profile for user {Email}", Input.Email);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        private void InsertCustomerProfile(string aspNetUserId)
        {
            string address = BuildAddress();
            string connStr = _configuration.GetConnectionString("DefaultConnection") ?? "DataSource=app.db;Cache=Shared";

            using var conn = new SqliteConnection(connStr);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO customer (
                    Cust_ID, Customer_Name, Customer_Surname, Customer_DOB, Customer_Gender,
                    Customer_Email, Customer_Phone, Customer_Address,
                    Medical_Aid, Medical_Aid_Number,
                    Main_Member_Name, Main_Member_Surname, Main_Member_ID,
                    Street_Number, Street_Name, Complex_Name, Unit_Number,
                    City, Province, Postal_Code, Is_Archive,
                    Cust_FirstName, Cust_LastName, Cust_Email, Cust_Phone
                ) VALUES (
                    @Cust_ID, @FirstName, @LastName, @DOB, @Gender,
                    @Email, @Phone, @Address,
                    @MedicalAid, @MedicalAidNumber,
                    @MainMemberName, @MainMemberSurname, @MainMemberID,
                    @StreetNumber, @StreetName, @ComplexName, @UnitNumber,
                    @City, @Province, @PostalCode, 0,
                    @FirstName, @LastName, @Email, @Phone
                )";

            cmd.Parameters.AddWithValue("@Cust_ID", aspNetUserId);
            cmd.Parameters.AddWithValue("@FirstName", Input.FirstName.Trim());
            cmd.Parameters.AddWithValue("@LastName", Input.LastName.Trim());
            cmd.Parameters.AddWithValue("@DOB", Input.DateOfBirth.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@Gender", Input.Gender);
            cmd.Parameters.AddWithValue("@Email", Input.Email.Trim());
            cmd.Parameters.AddWithValue("@Phone", Input.Phone.Trim());
            cmd.Parameters.AddWithValue("@Address", address);

            cmd.Parameters.AddWithValue("@MedicalAid",
                string.IsNullOrWhiteSpace(Input.MedicalAid) ? DBNull.Value : Input.MedicalAid.Trim());
            cmd.Parameters.AddWithValue("@MedicalAidNumber",
                string.IsNullOrWhiteSpace(Input.MedicalAidNumber) ? DBNull.Value : Input.MedicalAidNumber.Trim());

            bool isMainMember = string.IsNullOrEmpty(Input.IsMainMember) || Input.IsMainMember == "true";
            if (isMainMember)
            {
                cmd.Parameters.AddWithValue("@MainMemberName", DBNull.Value);
                cmd.Parameters.AddWithValue("@MainMemberSurname", DBNull.Value);
                cmd.Parameters.AddWithValue("@MainMemberID", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@MainMemberName",
                    string.IsNullOrWhiteSpace(Input.MainMemberName) ? DBNull.Value : Input.MainMemberName.Trim());
                cmd.Parameters.AddWithValue("@MainMemberSurname",
                    string.IsNullOrWhiteSpace(Input.MainMemberSurname) ? DBNull.Value : Input.MainMemberSurname.Trim());
                cmd.Parameters.AddWithValue("@MainMemberID",
                    string.IsNullOrWhiteSpace(Input.MainMemberID) ? DBNull.Value : Input.MainMemberID.Trim());
            }

            cmd.Parameters.AddWithValue("@StreetNumber",
                string.IsNullOrWhiteSpace(Input.StreetNumber) ? DBNull.Value : Input.StreetNumber.Trim());
            cmd.Parameters.AddWithValue("@StreetName",
                string.IsNullOrWhiteSpace(Input.StreetName) ? DBNull.Value : Input.StreetName.Trim());
            cmd.Parameters.AddWithValue("@ComplexName",
                string.IsNullOrWhiteSpace(Input.ComplexName) ? DBNull.Value : Input.ComplexName.Trim());
            cmd.Parameters.AddWithValue("@UnitNumber",
                string.IsNullOrWhiteSpace(Input.UnitNumber) ? DBNull.Value : Input.UnitNumber.Trim());
            cmd.Parameters.AddWithValue("@City",
                string.IsNullOrWhiteSpace(Input.City) ? DBNull.Value : Input.City.Trim());
            cmd.Parameters.AddWithValue("@Province",
                string.IsNullOrWhiteSpace(Input.Province) ? DBNull.Value : Input.Province.Trim());
            cmd.Parameters.AddWithValue("@PostalCode",
                string.IsNullOrWhiteSpace(Input.PostalCode) ? DBNull.Value : Input.PostalCode.Trim());

            cmd.ExecuteNonQuery();
        }

        private string BuildAddress()
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Input.StreetNumber) || !string.IsNullOrWhiteSpace(Input.StreetName))
            {
                string street = $"{Input.StreetNumber?.Trim()} {Input.StreetName?.Trim()}".Trim();
                if (!string.IsNullOrWhiteSpace(street))
                    parts.Add(street);
            }
            if (!string.IsNullOrWhiteSpace(Input.City))
                parts.Add(Input.City.Trim());

            return parts.Count > 0 ? string.Join(", ", parts) : string.Empty;
        }
    }
}
