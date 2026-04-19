using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
	public partial class SiteMaster : MasterPage
	{
		protected global::System.Web.UI.HtmlControls.HtmlGenericControl navMenu;
		protected global::System.Web.UI.HtmlControls.HtmlGenericControl navContainer; // server-side reference to nav container
		protected global::System.Web.UI.HtmlControls.HtmlGenericControl staffNav; // server-side reference to staff nav
		protected global::System.Web.UI.HtmlControls.HtmlGenericControl mainFooter; // server-side reference to footer
		protected global::System.Web.UI.HtmlControls.HtmlGenericControl logoText; // server-side reference to logo text
		protected global::System.Web.UI.WebControls.HyperLink logoLink; // server-side reference to logo link
		
		// Navigation link references for active state
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navHome;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navServices;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navShop;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navAppointment;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navAbout;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navContact;
		protected global::System.Web.UI.HtmlControls.HtmlAnchor navHelp;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				UpdateCartCount();
				UpdateLoginState();

			// Determine staff context: either session indicates staff or current URL is under /Staff/
			bool isStaffLoggedIn = Session["IsStaffLoggedIn"] != null && (bool)Session["IsStaffLoggedIn"];
			bool isStaffPath = Request != null && Request.Path != null && Request.Path.StartsWith("/Staff/", StringComparison.OrdinalIgnoreCase);
			bool showStaffNav = isStaffLoggedIn || isStaffPath;

			// Toggle main nav and auth buttons when in staff context
			navMenu.Visible = !showStaffNav;
			btnCart.Visible = !showStaffNav;
			btnLogin.Visible = !showStaffNav;
			btnRegister.Visible = !showStaffNav;

			// Show or hide staff nav
			if (staffNav != null)
			{
				staffNav.Visible = showStaffNav;
			}

			// Hide footer when staff is logged in
			if (mainFooter != null)
			{
				mainFooter.Visible = !isStaffLoggedIn;
			}
			}
		}

	protected override void OnPreRender(EventArgs e)
	{
		base.OnPreRender(e);
		// Ensure login state is up-to-date before rendering
		UpdateLoginState();
		// Update cart count before rendering to catch any changes during the page lifecycle
		UpdateCartCount();
		
		// Set active navigation item
		SetActiveNavigationItem();

	// Re-evaluate staff context each render
	bool isStaffLoggedIn = Session["IsStaffLoggedIn"] != null && (bool)Session["IsStaffLoggedIn"];
	bool isStaffPath = Request != null && Request.Path != null && Request.Path.StartsWith("/Staff/", StringComparison.OrdinalIgnoreCase);
	bool showStaffNav = isStaffLoggedIn || isStaffPath;

	if (staffNav != null)
	{
		staffNav.Visible = showStaffNav;
	}
	// hide main nav when showing staff nav
	if (navMenu != null)
	{
		navMenu.Visible = !showStaffNav;
	}
	// Hide footer when staff is logged in
	if (mainFooter != null)
	{
		mainFooter.Visible = !isStaffLoggedIn;
	}
	
	// Hide auth buttons when staff is logged in
	bool isLoggedIn = IsUserLoggedIn();
	bool hideAuthButtons = isLoggedIn || isStaffLoggedIn;
	
	btnLogin.Visible = !hideAuthButtons;
	btnRegister.Visible = !hideAuthButtons;
	btnCart.Visible = !showStaffNav;
	
	try
	{
		var phAuth = FindControl("phAuth") as PlaceHolder;
		if (phAuth != null)
		{
			phAuth.Visible = !hideAuthButtons;
		}
	}
	catch { }
	
	// Center logo when staff is logged in and toggle between link/text
	if (navContainer != null)
	{
		if (showStaffNav)
		{
			navContainer.Attributes["class"] = "nav-container staff-mode";
			// Show text version, hide link version for staff
			if (logoText != null) logoText.Visible = true;
			if (logoLink != null) logoLink.Visible = false;
		}
		else
		{
			navContainer.Attributes["class"] = "nav-container";
			// Show link version, hide text version for regular users
			if (logoText != null) logoText.Visible = false;
			if (logoLink != null) logoLink.Visible = true;
		}
	}
	}
	
	private void SetActiveNavigationItem()
	{
		// Only set active state if main nav is visible (not in staff mode)
		bool isStaffLoggedIn = Session["IsStaffLoggedIn"] != null && (bool)Session["IsStaffLoggedIn"];
		bool isStaffPath = Request != null && Request.Path != null && Request.Path.StartsWith("/Staff/", StringComparison.OrdinalIgnoreCase);
		bool showStaffNav = isStaffLoggedIn || isStaffPath;
		
		if (showStaffNav) return; // Don't set active state for staff nav
		
		// Get current page path
		if (Request == null || Request.Path == null) return;
		
		string currentPath = Request.Path.ToLower();
		
		// Helper method to safely set/remove active class
		Action<System.Web.UI.HtmlControls.HtmlAnchor, bool> setActive = (navItem, isActive) =>
		{
			if (navItem == null) return;
			
			string currentClass = navItem.Attributes["class"] ?? "";
			currentClass = currentClass.Replace("active", "").Trim();
			
			if (isActive)
			{
				currentClass = (currentClass + " active").Trim();
			}
			
			navItem.Attributes["class"] = currentClass;
		};
		
		// Remove active class from all nav items first
		setActive(navHome, false);
		setActive(navServices, false);
		setActive(navShop, false);
		setActive(navAppointment, false);
		setActive(navAbout, false);
		setActive(navContact, false);
		setActive(navHelp, false);
		
		// Set active class based on current page
		if (currentPath.Contains("/default.aspx") || currentPath == "/" || currentPath.EndsWith("/"))
		{
			setActive(navHome, true);
		}
		else if (currentPath.Contains("/services.aspx"))
		{
			setActive(navServices, true);
		}
		else if (currentPath.Contains("/shop.aspx"))
		{
			setActive(navShop, true);
		}
		else if (currentPath.Contains("/appointmentstart.aspx") || currentPath.Contains("/bookappointment.aspx"))
		{
			setActive(navAppointment, true);
		}
		else if (currentPath.Contains("/about.aspx"))
		{
			setActive(navAbout, true);
		}
		else if (currentPath.Contains("/contact.aspx"))
		{
			setActive(navContact, true);
		}
		else if (currentPath.Contains("/help.aspx"))
		{
			setActive(navHelp, true);
		}
	}

		private void UpdateLoginState()
		{
			// Check if user is logged in using session
			bool isLoggedIn = IsUserLoggedIn();
			
			// Check if staff is logged in
			bool isStaffLoggedIn = Session["IsStaffLoggedIn"] != null && (bool)Session["IsStaffLoggedIn"];
			
			// Hide auth buttons for both customer login AND staff login
			bool hideAuthButtons = isLoggedIn || isStaffLoggedIn;

			// Try to hide the grouped login/register placeholder if present
			try
			{
				var phAuth = FindControl("phAuth") as PlaceHolder;
				if (phAuth != null)
				{
					phAuth.Visible = !hideAuthButtons;
				}
			}
			catch { }

			// Individual control visibility as fallback
			btnLogin.Visible = !hideAuthButtons;
			btnRegister.Visible = !hideAuthButtons;
			btnMyAccount.Visible = isLoggedIn; // Only show for customer login, not staff

		
		}

		public bool IsUserLoggedIn()
		{
			return Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];
		}

		public string GetCurrentCustomerId()
		{
			return Session["Cust_ID"]?.ToString();
		}

		public string GetCurrentUserEmail()
		{
			return Session["UserEmail"]?.ToString();
		}

		private void UpdateCartCount()
		{
			int totalItems = 0;
			bool isLoggedIn = IsUserLoggedIn();

			if (isLoggedIn)
			{
				string custId = GetCurrentCustomerId();
				if (!string.IsNullOrEmpty(custId))
				{
					try
					{
						int cartId = CartDatabase.GetOrCreateCart(custId);
						totalItems = CartDatabase.GetCartItemCount(cartId);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"Error getting database cart count: {ex.Message}");
						totalItems = 0;
					}
				}
			}
			else
			{
				totalItems = CartTransfer.GetTotalItems(Session.SessionID);
			}

			var cartCountSpan = btnCart.FindControl("cartCountSpan") as System.Web.UI.HtmlControls.HtmlGenericControl;
			if (cartCountSpan != null)
			{
				cartCountSpan.InnerText = totalItems.ToString();
			}
		}

		private void UpdateWishlistCount()
		{
			if (IsUserLoggedIn())
			{
				string custId = GetCurrentCustomerId();
				if (!string.IsNullOrEmpty(custId))
				{
					try
					{
						int customerId = Convert.ToInt32(custId);
						int wishlistCount = WishlistDatabase.GetWishlistItemCount(customerId);

						var wishlistCountSpan = btnWishlist.FindControl("wishlistCountSpan") as System.Web.UI.HtmlControls.HtmlGenericControl;
						if (wishlistCountSpan != null)
						{
							wishlistCountSpan.InnerText = wishlistCount.ToString();
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Debug.WriteLine($"Error getting wishlist count: {ex.Message}");
					}
				}
			}
		}

		public void UpdateCartCounter()
		{
			UpdateCartCount();
		}

		protected void btnCart_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Cart.aspx");
		}

		protected void btnLogin_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Account/Login.aspx");
		}

		protected void btnRegister_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Account/Register.aspx");
		}

		protected void btnMyAccount_Click(object sender, EventArgs e)
		{
			string script = @"
				var dropdown = document.getElementById('myAccountDropdown');
				if (dropdown.style.display === 'none' || dropdown.style.display === '') {
					dropdown.style.display = 'block';
				} else {
					dropdown.style.display = 'none';
				}
			";
			ScriptManager.RegisterStartupScript(this, GetType(), "ToggleDropdown", script, true);
		}

		protected void btnPersonalDetails_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/PersonalDetails.aspx");
		}

		protected void btnOrders_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Orders.aspx");
		}

		protected void btnAppointments_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Appointments.aspx");
		}

		protected void btnWishlist_Click(object sender, EventArgs e)
		{
			Response.Redirect("~/Wishlist.aspx");
		}

		protected void btnLogout_Click(object sender, EventArgs e)
		{
			// Clear session variables
			Session["Cust_ID"] = null;
			Session["UserEmail"] = null;
			Session["IsLoggedIn"] = null;
			Session["FirstName"] = null;
			Session["LastName"] = null;

			// Redirect to home page
			Response.Redirect("~/Default.aspx");
		}

		protected void btnStaffPortal_Click(object sender, EventArgs e)
		{
			// Redirect to staff portal login
			Response.Redirect("~/Staff/Login.aspx");
		}
	}
}