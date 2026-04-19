using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class Wishlist : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadWishlist();
            }
        }

        private void LoadWishlist()
        {
            try
            {
                string custId = Session["Cust_ID"]?.ToString();
                if (string.IsNullOrEmpty(custId))
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                int customerId = Convert.ToInt32(custId);
                var wishlistItems = WishlistDatabase.GetWishlistItems(customerId);

                if (wishlistItems.Count > 0)
                {
                    rptWishlistItems.DataSource = wishlistItems;
                    rptWishlistItems.DataBind();
                    pnlWishlistItems.Visible = true;
                    pnlEmptyWishlist.Visible = false;
                    lblWishlistCount.Text = $"{wishlistItems.Count} item(s)";
                }
                else
                {
                    pnlWishlistItems.Visible = false;
                    pnlEmptyWishlist.Visible = true;
                    lblWishlistCount.Text = "0 items";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading wishlist: {ex.Message}");
                // Show error message to user
                ScriptManager.RegisterStartupScript(this, GetType(), "WishlistError",
                    "alert('Error loading wishlist. Please try again.');", true);
            }
        }

        protected void rptWishlistItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Handle item data binding for stock status and button state
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                WishlistItem item = (WishlistItem)e.Item.DataItem;

                // Update stock status label
                Label lblStockStatus = (Label)e.Item.FindControl("lblStockStatus");
                Button btnAddToCart = (Button)e.Item.FindControl("btnAddToCart");

                if (lblStockStatus != null)
                {
                    lblStockStatus.Text = item.StockStatus;
                    
                    // Set CSS class based on stock status
                    lblStockStatus.CssClass = "item-stock-status";
                    if (item.StockQuantity > 5)
                    {
                        lblStockStatus.CssClass += " in-stock";
                    }
                    else if (item.StockQuantity <= 5 && item.StockQuantity >= 1)
                    {
                        lblStockStatus.CssClass += " limited-stock";
                    }
                    else
                    {
                        lblStockStatus.CssClass += " out-of-stock";
                    }
                }

                // Disable Add to Cart button if out of stock
                if (btnAddToCart != null)
                {
                    if (item.StockQuantity <= 0)
                    {
                        btnAddToCart.Enabled = false;
                        btnAddToCart.CssClass = "btn-add-to-cart btn-disabled";
                        btnAddToCart.Text = "Out of Stock";
                    }
                    else
                    {
                        btnAddToCart.Enabled = true;
                        btnAddToCart.CssClass = "btn-add-to-cart";
                        btnAddToCart.Text = "Add to Cart";
                    }
                }
            }
        }

        protected void rptWishlistItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string custId = Session["Cust_ID"]?.ToString();
                if (string.IsNullOrEmpty(custId))
                {
                    Response.Redirect("~/Account/Login.aspx");
                    return;
                }

                int customerId = Convert.ToInt32(custId);

                if (e.CommandName == "AddToCart")
                {
                    // Add item to cart
                    int productId = Convert.ToInt32(e.CommandArgument);
                    
                    // Get product details for cart
                    var productData = GetProductFromDatabase(productId);
                    if (productData != null)
                    {
                        // Check if product is in stock
                        int qty = 0;
                        int.TryParse(productData["QuantityOnHand"]?.ToString() ?? "0", out qty);
                        
                        if (qty <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "OutOfStockError",
                                "alert('This product is currently out of stock and cannot be added to cart.');", true);
                            return;
                        }
                        
                        // Add to database cart
                        int cartId = CartDatabase.GetOrCreateCart(custId);
                        CartDatabase.AddItemToCart(cartId, productId, 1, Convert.ToDecimal(productData["Product_Price"]));
                        Session["Cart_ID"] = cartId;

                        // Show success message
                        ScriptManager.RegisterStartupScript(this, GetType(), "AddToCartSuccess",
                            "alert('Item added to cart successfully!');", true);
                    }
                }
                else if (e.CommandName == "RemoveFromWishlist")
                {
                    // Remove item from wishlist
                    int wishlistItemId = Convert.ToInt32(e.CommandArgument);
                    WishlistDatabase.RemoveItemFromWishlist(wishlistItemId);

                    // Reload wishlist
                    LoadWishlist();

                    // Show success message
                    ScriptManager.RegisterStartupScript(this, GetType(), "RemoveSuccess",
                        "alert('Item removed from wishlist!');", true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in wishlist command: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "WishlistError",
                    "alert('Error processing request. Please try again.');", true);
            }
        }

        private System.Data.DataRow GetProductFromDatabase(int productId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
                           Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                    FROM Products2 
                    WHERE Product_ID = @ProductId";

                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    conn.Open();
                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }
}


