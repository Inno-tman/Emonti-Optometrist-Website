using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class ProductDetails : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
        private DataRow productData;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Always fetch product data so it's available on postbacks (e.g. Add to Cart)
            FetchProductData();

            if (!IsPostBack)
            {
                if (productData == null)
                {
                    Response.Redirect("~/Shop.aspx");
                    return;
                }

                PopulateProductInfo();
            }
        }

        // Fetches product data into the productData field without repopulating UI
        private void FetchProductData()
        {
            try
            {
                // If already fetched, skip
                if (productData != null) return;

                // Get product information from query string
                string productInfo = Request.QueryString["product"];
                if (string.IsNullOrEmpty(productInfo))
                {
                    productData = null;
                    return;
                }

                var parsed = ParseProductQueryString(productInfo);
                if (parsed == null)
                {
                    productData = null;
                    return;
                }

                productData = GetProductFromDatabase(parsed.Value.brand, parsed.Value.productName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching product data: {ex.Message}");
                productData = null;
            }
        }

        // Parses the product query string into brand and product name
        private (string brand, string productName)? ParseProductQueryString(string productInfo)
        {
            if (string.IsNullOrEmpty(productInfo)) return null;

            // URL decode the entire string first
            productInfo = System.Web.HttpUtility.UrlDecode(productInfo);
            
            string[] parts = productInfo.Split('_');
            if (parts.Length < 2) return null;

            string brand = parts[0];
            string productName = string.Join("_", parts, 1, parts.Length - 1);

            return (brand, productName);
        }

        private void LoadProductDetails()
        {
            try
            {
                // previously LoadProductDetails handled fetching and populating, keep for compatibility
                FetchProductData();

                if (productData != null)
                {
                    PopulateProductInfo();
                }
                else
                {
                    Response.Redirect("~/Shop.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading product details: {ex.Message}");
                Response.Redirect("~/Shop.aspx");
            }
        }

        private DataRow GetProductFromDatabase(string brand, string productName)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Long_Description, 
                           Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                    FROM Products2 
                    WHERE Product_Brand = @brand AND Product_Name = @productName";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add(new SqlParameter("@brand", brand));
                    cmd.Parameters.Add(new SqlParameter("@productName", productName));

                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private void PopulateProductInfo()
        {
            try
            {
                if (productData != null)
                {
                    // Set basic product information
                    lblBrand.Text = productData["Product_Brand"].ToString();
                    lblProductName.Text = productData["Product_Name"].ToString();
                    lblPrice.Text = productData["Product_Price"].ToString();
                    lblDescription.Text = productData["Long_Description"].ToString();
                    
                    // Set stock status text instead of number
                    int qty = 0;
                    int.TryParse(productData["QuantityOnHand"]?.ToString() ?? "0", out qty);
                    
                    // Clear any existing CSS classes first
                    lblStock.CssClass = "product-stock";
                    
                    if (qty > 5)
                    {
                        lblStock.Text = "In stock";
                        lblStock.CssClass = "product-stock";
                    }
                    else if (qty <= 5 && qty >= 1)
                    {
                        lblStock.Text = "Limited stock";
                        lblStock.CssClass = "product-stock limited-stock";
                    }
                    else
                    {
                        lblStock.Text = "Out of stock";
                        lblStock.CssClass = "product-stock out-of-stock";
                    }

                    // Disable Add to Cart button and quantity controls if out of stock
                    if (qty <= 0)
                    {
                        btnAddToCart.Enabled = false;
                        btnAddToCart.CssClass = "btn-add-cart btn-disabled";
                        btnAddToCart.Text = "Out of Stock";
                        btnQtyMinus.Enabled = false;
                        btnQtyPlus.Enabled = false;
                        txtQuantity.Enabled = false;
                        btnQtyMinus.CssClass = "quantity-btn btn-disabled";
                        btnQtyPlus.CssClass = "quantity-btn btn-disabled";
                        txtQuantity.CssClass = "quantity-input input-disabled";
                        
                        // Show out of stock message
                        Panel outOfStockPanel = (Panel)FindControl("outOfStockMessage");
                        if (outOfStockPanel != null)
                        {
                            outOfStockPanel.Visible = true;
                        }
                    }
                    else
                    {
                        // Ensure controls are enabled if in stock
                        btnAddToCart.Enabled = true;
                        btnAddToCart.CssClass = "btn-add-cart";
                        btnAddToCart.Text = "Add to Cart";
                        btnQtyMinus.Enabled = true;
                        btnQtyPlus.Enabled = true;
                        txtQuantity.Enabled = true;
                        btnQtyMinus.CssClass = "quantity-btn";
                        btnQtyPlus.CssClass = "quantity-btn";
                        txtQuantity.CssClass = "quantity-input";
                        
                        // Limit quantity to available stock
                        int maxQty = Math.Min(qty, 99); // Cap at 99 for UI purposes
                        txtQuantity.Attributes["max"] = maxQty.ToString();
                        
                        // Hide out of stock message if in stock
                        Panel outOfStockPanel = (Panel)FindControl("outOfStockMessage");
                        if (outOfStockPanel != null)
                        {
                            outOfStockPanel.Visible = false;
                        }
                    }

                    // Set product images
                    SetProductImages();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error populating product info: {ex.Message}");
                // Show error message to user
                lblProductName.Text = "Error loading product";
                lblDescription.Text = "Please try again later.";
            }
        }

        private void SetProductImages()
        {
            try
            {
                string picture1 = productData["Picture1"]?.ToString();
                string picture2 = productData["Picture2"]?.ToString();

                // Set main image
                if (!string.IsNullOrEmpty(picture1))
                {
                    string imageUrl = GetProductImage(picture1, picture2);
                    imgMainProduct.ImageUrl = imageUrl;
                    imgMainProduct.AlternateText = productData["Product_Name"].ToString();

                    // Set thumbnail
                    imgThumb1.ImageUrl = imageUrl;
                    imgThumb1.AlternateText = productData["Product_Name"].ToString();
                }
                else
                {
                    // Use placeholder
                    imgMainProduct.ImageUrl = "~/Images/Products/placeholder.jpg";
                    imgThumb1.ImageUrl = "~/Images/Products/placeholder.jpg";
                }

                // Set second image if available
                if (!string.IsNullOrEmpty(picture2))
                {
                    string imageUrl2 = GetProductImage(picture2, null);
                    imgThumb2.ImageUrl = imageUrl2;
                    imgThumb2.AlternateText = productData["Product_Name"].ToString();
                }
                else
                {
                    imgThumb2.Visible = false;
                }

                // Hide unused thumbnails
                imgThumb3.Visible = false;
                imgThumb4.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting product images: {ex.Message}");
                // Don't throw - just log the error so page can still render
            }
        }

        private string GetProductImage(string picture1, string picture2)
        {
            if (!string.IsNullOrEmpty(picture1))
            {
                // If the path already contains Images/Products, use it as is
                if (picture1.StartsWith("Images\\Products\\") || picture1.StartsWith("Images/Products/"))
                {
                    return $"~/{picture1.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{picture1}";
                }
            }
            else if (!string.IsNullOrEmpty(picture2))
            {
                // If the path already contains Images/Products, use it as is
                if (picture2.StartsWith("Images\\Products\\") || picture2.StartsWith("Images/Products/"))
                {
                    return $"~/{picture2.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{picture2}";
                }
            }
            else
            {
                return "~/Images/Products/placeholder.jpg";
            }
        }


        protected void btnQtyMinus_Click(object sender, EventArgs e)
        {
            int currentQty = int.Parse(txtQuantity.Text);
            if (currentQty > 1)
            {
                txtQuantity.Text = (currentQty - 1).ToString();
            }
        }

        protected void btnQtyPlus_Click(object sender, EventArgs e)
        {
            // Ensure productData is available
            if (productData == null)
            {
                FetchProductData();
            }
            
            if (productData != null)
            {
                int currentQty = int.Parse(txtQuantity.Text);
                int availableStock = 0;
                int.TryParse(productData["QuantityOnHand"]?.ToString() ?? "0", out availableStock);
                
                // Don't allow quantity to exceed available stock
                int maxQty = Math.Min(availableStock, 99);
                if (currentQty < maxQty)
                {
                    txtQuantity.Text = (currentQty + 1).ToString();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "MaxQuantityReached",
                        $"showErrorMessage('Maximum quantity available is {maxQty}.');", true);
                }
            }
            else
            {
                int currentQty = int.Parse(txtQuantity.Text);
                txtQuantity.Text = (currentQty + 1).ToString();
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            try
            {
                // Ensure productData is available (fetch if necessary)
                if (productData == null)
                {
                    FetchProductData();
                }

                if (productData != null)
                {
                    // Check if product is in stock
                    int qty = 0;
                    int.TryParse(productData["QuantityOnHand"]?.ToString() ?? "0", out qty);
                    
                    if (qty <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "OutOfStockError",
                            "showErrorMessage('This product is currently out of stock and cannot be added to cart.');", true);
                        return;
                    }
                    
                    int productId = Convert.ToInt32(productData["Product_ID"]);
                    int quantity = int.Parse(txtQuantity.Text);
                    
                    // Validate quantity doesn't exceed available stock
                    if (quantity > qty)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "InsufficientStock",
                            $"showErrorMessage('Only {qty} item(s) available in stock. Please adjust quantity.');", true);
                        return;
                    }
                    
                    if (quantity <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "InvalidQuantity",
                            "showErrorMessage('Please select a valid quantity.');", true);
                        return;
                    }
                    
                    bool isLoggedIn = Session["IsLoggedIn"] != null && (bool)Session["IsLoggedIn"];

                    if (isLoggedIn)
                    {
                        // Add to database cart for logged-in users
                        string custId = Session["Cust_ID"]?.ToString();
                        if (!string.IsNullOrEmpty(custId))
                        {
                            try
                            {
                                int cartId = CartDatabase.GetOrCreateCart(custId);
                                CartDatabase.AddItemToCart(cartId, productId, quantity, Convert.ToDecimal(productData["Product_Price"]));
                                Session["Cart_ID"] = cartId;
                                System.Diagnostics.Debug.WriteLine($"Successfully added item to database cart: CartId={cartId}, ProductId={productId}, Quantity={quantity}");

                                // get updated total
                                int totalItems = CartDatabase.GetCartItemCount(cartId);
                                var span = Master.FindControl("cartCountSpan");
                                string clientId = span != null ? (span as System.Web.UI.Control).ClientID : "";

                                string successMessage = $"Added {quantity} x {productData["Product_Name"]} to cart!";
                                ScriptManager.RegisterStartupScript(this, GetType(), "AddToCartSuccess",
                                    $"document.getElementById('{clientId}').innerText = '{totalItems}'; showSuccessMessage('{successMessage}');", true);
                                return;
                            }
                            catch (Exception dbEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Database cart error: {dbEx.Message}");
                                throw new Exception($"Failed to add item to cart: {dbEx.Message}", dbEx);
                            }
                        }
                        else
                        {
                            throw new Exception("Customer ID not found in session");
                        }
                    }
                    else
                    {
                        // Add to session cart for guests
                        var cartItem = new CartItem
                        {
                            ProductId = productId.ToString(), // Convert to string for session cart
                            ProductName = productData["Product_Name"].ToString(),
                            Brand = productData["Product_Brand"].ToString(),
                            Category = productData["Product_Category"].ToString(),
                            Price = Convert.ToDecimal(productData["Product_Price"]),
                            Quantity = quantity,
                            ImageUrl = GetProductImage(productData["Picture1"]?.ToString(), productData["Picture2"]?.ToString())
                        };

                        AddToCartItem(cartItem);

                        int totalItems = CartTransfer.GetTotalItems(Session.SessionID);
                        var span = Master.FindControl("cartCountSpan");
                        string clientId = span != null ? (span as System.Web.UI.Control).ClientID : "";

                        string successMessage2 = $"Added {quantity} x {productData["Product_Name"]} to cart!";
                        ScriptManager.RegisterStartupScript(this, GetType(), "AddToCartSuccessGuest",
                            $"document.getElementById('{clientId}').innerText = '{totalItems}'; showSuccessMessage('{successMessage2}');", true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding to cart: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "AddToCartError",
                    "showErrorMessage('Error adding product to cart. Please try again.');", true);
            }
        }

        protected void btnWishlist_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if user is logged in
                if (Session["IsLoggedIn"] == null || !(bool)Session["IsLoggedIn"]) 
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "LoginRequired",
                        "showErrorMessage('Please login to add items to your wishlist.');", true);
                    return;
                }

                if (productData != null)
                {
                    int productId = Convert.ToInt32(productData["Product_ID"]);
                    string custId = Session["Cust_ID"]?.ToString();
                    
                    if (!string.IsNullOrEmpty(custId))
                    {
                        int customerId = Convert.ToInt32(custId);
                        
                        // Check if item is already in wishlist
                        bool isInWishlist = WishlistDatabase.IsInWishlist(customerId, productId);
                        
                        if (isInWishlist)
                        {
                            // Remove from wishlist
                            ScriptManager.RegisterStartupScript(this, GetType(), "WishlistRemove",
                                "showSuccessMessage('Item removed from wishlist!');", true);
                        }
                        else
                        {
                            // Add to wishlist
                            WishlistDatabase.AddItemToWishlist(customerId, productId);
                            
                            // Update button appearance
                            btnWishlist.Text = "&#9829;";
                            btnWishlist.CssClass = "btn-wishlist in-wishlist";
                            
                            ScriptManager.RegisterStartupScript(this, GetType(), "WishlistAdd",
                                "showSuccessMessage('Item added to wishlist!');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling wishlist: {ex.Message}");
                ScriptManager.RegisterStartupScript(this, GetType(), "WishlistError",
                    "showErrorMessage('Error updating wishlist. Please try again.');", true);
            }
        }

        private void AddToCartItem(CartItem newItem)
        {
            // Get existing cart from CartTransfer (same as Shop page)
            List<CartItem> cart = CartTransfer.GetCart(Session.SessionID);

            // Check if item already exists
            var existingItem = cart.FirstOrDefault(x => x.ProductId == newItem.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;
            }
            else
            {
                cart.Add(newItem);
            }

            // Save back to CartTransfer
            CartTransfer.SaveCart(Session.SessionID, cart);
        }


        private void UpdateCartCounter()
        {
            // Update the cart counter in the master page
            var masterPage = (SiteMaster)this.Master;
            if (masterPage != null)
            {
                masterPage.UpdateCartCounter();
            }
        }
    }
}