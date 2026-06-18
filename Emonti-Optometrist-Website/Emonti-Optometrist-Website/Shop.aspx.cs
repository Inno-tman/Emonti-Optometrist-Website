using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website
{
    public partial class Shop : System.Web.UI.Page
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    LoadProducts();
                    PopulateFilterDropdowns();
                }
                catch (Exception ex)
                {
                    // Display error on page
                    lblResultsCount.Text = $"Page Load Error: {ex.Message}";
                    System.Diagnostics.Debug.WriteLine($"Page_Load Error: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }
		
		private void LoadProducts()
        {
            try
            {
                DataTable dt = GetProductDataFromDatabase();
                
                // Debug: Check if data is retrieved
                System.Diagnostics.Debug.WriteLine($"Retrieved {dt.Rows.Count} products from database");
                
                if (dt.Rows.Count == 0)
                {
                    lblResultsCount.Text = "No products found in database";
                    rptProducts.DataSource = null;
                    rptProducts.DataBind();
                }
                else
                {
                    rptProducts.DataSource = dt;
                    rptProducts.DataBind();
                    lblResultsCount.Text = $"Showing {dt.Rows.Count} products";
                }
            }
            catch (Exception ex)
            {
                // Handle error - show message to user
                lblResultsCount.Text = $"Database Error: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Database error: {ex.Message}");
                
                // Clear the repeater if database fails
                rptProducts.DataSource = null;
                rptProducts.DataBind();
                
                ScriptManager.RegisterStartupScript(this, GetType(), "Error", 
                    $"alert('Database Error: {ex.Message}');", true);
            }
        }

        private DataTable GetProductDataFromDatabase()
        {
            DataTable dt = new DataTable();
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"Connection string: {connectionString}");
                
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Query to get all products from database product2 table (including those with 0 stock for testing)
                    string query = @"
                        SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
                               Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                        FROM Products2 
                        ORDER BY Product_Brand, Product_Name";
                    
                    System.Diagnostics.Debug.WriteLine($"SQL Query: {query}");
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        System.Diagnostics.Debug.WriteLine("Database connection opened successfully");
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(dt);
                        
                        System.Diagnostics.Debug.WriteLine($"DataTable filled with {dt.Rows.Count} rows");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database error in GetProductDataFromDatabase: {ex.Message}");
                throw; // Re-throw to be caught by calling method
            }
            
            return dt;
        }

        // Helper method to get the best available image
        protected string GetProductImage(object picture1, object picture2)
        {
            string img1 = picture1?.ToString();
            string img2 = picture2?.ToString();
            
            // Handle the image path correctly
            if (!string.IsNullOrEmpty(img1))
            {
                // If the path already contains Images/Products, use it as is
                if (img1.StartsWith("Images\\Products\\") || img1.StartsWith("Images/Products/"))
                {
                    return $"~/{img1.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{img1}";
                }
            }
            else if (!string.IsNullOrEmpty(img2))
            {
                // If the path already contains Images/Products, use it as is
                if (img2.StartsWith("Images\\Products\\") || img2.StartsWith("Images/Products/"))
                {
                    return $"~/{img2.Replace("\\", "/")}";
                }
                // Otherwise, add the Images/Products prefix
                else
                {
                    return $"~/Images/Products/{img2}";
                }
            }
            else
            {
                return "~/Images/Products/placeholder.jpg"; // Default placeholder image
            }
        }

        private void PopulateFilterDropdowns()
        {
            try
            {
                // Populate category dropdown from database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string categoryQuery = "SELECT DISTINCT Product_Category FROM Products2 WHERE Product_Category IS NOT NULL ORDER BY Product_Category";
                    using (SqlCommand cmd = new SqlCommand(categoryQuery, conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlCategory.Items.Clear();
                        ddlCategory.Items.Add(new ListItem("All Categories", "all"));
                        while (reader.Read())
                        {
                            ddlCategory.Items.Add(new ListItem(reader["Product_Category"].ToString(), reader["Product_Category"].ToString()));
                        }
                        reader.Close();
                    }

                    // Populate brand dropdown from database
                    string brandQuery = "SELECT DISTINCT Product_Brand FROM Products2 WHERE Product_Brand IS NOT NULL ORDER BY Product_Brand";
                    using (SqlCommand cmd = new SqlCommand(brandQuery, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        ddlBrand.Items.Clear();
                        ddlBrand.Items.Add(new ListItem("All Brands", "all"));
                        while (reader.Read())
                        {
                            ddlBrand.Items.Add(new ListItem(reader["Product_Brand"].ToString(), reader["Product_Brand"].ToString()));
                        }
                        reader.Close();
                    }

                    // Populate category tabs from database
                    PopulateCategoryTabs();
                }
            }
            catch (Exception ex)
            {
                // If database fails, show error in dropdowns
                ddlCategory.Items.Clear();
                ddlCategory.Items.Add(new ListItem("Database Error", "error"));
                ddlBrand.Items.Clear();
                ddlBrand.Items.Add(new ListItem("Database Error", "error"));
                System.Diagnostics.Debug.WriteLine($"Error populating filters: {ex.Message}");
            }
        }

        private void PopulateCategoryTabs()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string categoryQuery = "SELECT DISTINCT Product_Category FROM Products2 WHERE Product_Category IS NOT NULL ORDER BY Product_Category";
                    using (SqlCommand cmd = new SqlCommand(categoryQuery, conn))
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        
                        rptCategories.DataSource = dt;
                        rptCategories.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error populating category tabs: {ex.Message}");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable filteredProducts = GetFilteredProducts();
                rptProducts.DataSource = filteredProducts;
                rptProducts.DataBind();
                lblResultsCount.Text = $"Showing {filteredProducts.Rows.Count} products";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "SearchError", 
                    $"alert('Error searching products: {ex.Message}');", true);
            }
        }

        private DataTable GetFilteredProducts()
        {
            string searchTerm = txtSearch.Text.Trim();
            string category = ddlCategory.SelectedValue;
            string brand = ddlBrand.SelectedValue;
            string priceRange = ddlPriceRange.SelectedValue;

            DataTable dt = new DataTable();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
                           Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                    FROM Products2";
                
                List<string> conditions = new List<string>();
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    conditions.Add("(Product_Name LIKE @searchTerm OR Product_Brand LIKE @searchTerm OR Product_Description LIKE @searchTerm)");
                    parameters.Add(new SqlParameter("@searchTerm", "%" + searchTerm + "%"));
                }

                if (category != "all")
                {
                    conditions.Add("Product_Category = @category");
                    parameters.Add(new SqlParameter("@category", category));
                }

                if (brand != "all")
                {
                    conditions.Add("Product_Brand = @brand");
                    parameters.Add(new SqlParameter("@brand", brand));
                }

                if (priceRange != "all")
                {
                    string[] range = priceRange.Split('-');
                    if (range.Length == 2)
                    {
                        if (range[1] == "+")
                        {
                            conditions.Add("Product_Price >= @minPrice");
                            parameters.Add(new SqlParameter("@minPrice", decimal.Parse(range[0])));
                        }
                        else
                        {
                            conditions.Add("Product_Price >= @minPrice AND Product_Price <= @maxPrice");
                            parameters.Add(new SqlParameter("@minPrice", decimal.Parse(range[0])));
                            parameters.Add(new SqlParameter("@maxPrice", decimal.Parse(range[1])));
                        }
                    }
                }

                if (conditions.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                query += " ORDER BY Product_Brand, Product_Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }

        protected void FilterByCategory(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string category = btn.CommandArgument?.Trim();

            // Store current category in ViewState for sorting to preserve filter
            ViewState["CurrentCategory"] = category ?? "all";

            // Update active tab styling
            ResetTabStyles();
            btn.CssClass = "category-tab active";

            try
            {
                // Debug: Log the category being filtered
                System.Diagnostics.Debug.WriteLine($"Filtering by category: '{category}'");
                
                // Filter products by category using database
                DataTable filteredProducts = GetProductsByCategory(category);
                rptProducts.DataSource = filteredProducts;
                rptProducts.DataBind();
                lblResultsCount.Text = $"Showing {filteredProducts.Rows.Count} products";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "FilterError", 
                    $"alert('Error filtering products: {ex.Message}');", true);
            }
        }

        private DataTable GetProductsByCategory(string category)
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
                           Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                    FROM Products2";
                
                if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
                {
                    query += " WHERE LTRIM(RTRIM(Product_Category)) = @category COLLATE SQL_Latin1_General_CP1_CI_AS";
                }
                
                query += " ORDER BY Product_Brand, Product_Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
                    {
                        string trimmedCategory = category.Trim();
                        cmd.Parameters.Add(new SqlParameter("@category", trimmedCategory));
                        System.Diagnostics.Debug.WriteLine($"Executing query with category filter: '{trimmedCategory}'");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Executing query without category filter (showing all products)");
                    }
                    
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    System.Diagnostics.Debug.WriteLine($"Query returned {dt.Rows.Count} products");
                }
            }
            
            return dt;
        }

        private void ResetTabStyles()
        {
            lnkAllProducts.CssClass = "category-tab";
            
            // Reset all category tab styles
            if (rptCategories.Items != null)
            {
                foreach (RepeaterItem item in rptCategories.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        LinkButton lnkCategory = (LinkButton)item.FindControl("lnkCategory");
                        if (lnkCategory != null)
                        {
                            lnkCategory.CssClass = "category-tab";
                        }
                    }
                }
            }
        }

        protected void SortProducts(object sender, EventArgs e)
        {
            try
            {
                string sortBy = ddlSortBy.SelectedValue;
                // Get the current category filter from ViewState
                string currentCategory = ViewState["CurrentCategory"] as string ?? "all";
                
                // Get filtered products first, then sort them
                DataTable filteredProducts = GetProductsByCategory(currentCategory);
                
                // Apply sorting to the filtered results
                DataTable sortedProducts = SortDataTable(filteredProducts, sortBy);
                rptProducts.DataSource = sortedProducts;
                rptProducts.DataBind();
                lblResultsCount.Text = $"Showing {sortedProducts.Rows.Count} products";
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "SortError", 
                    $"alert('Error sorting products: {ex.Message}');", true);
            }
        }

        private DataTable SortDataTable(DataTable dt, string sortBy)
        {
            DataTable sortedDt = dt.Clone();
            DataRow[] sortedRows;

            switch (sortBy)
            {
                case "price_asc":
                    sortedRows = dt.Select("", "Product_Price ASC");
                    break;
                case "price_desc":
                    sortedRows = dt.Select("", "Product_Price DESC");
                    break;
                case "name_asc":
                    sortedRows = dt.Select("", "Product_Name ASC");
                    break;
                case "date_desc":
                    sortedRows = dt.Select("", "Product_Brand DESC");
                    break;
                default:
                    sortedRows = dt.Select("", "Product_Brand, Product_Name");
                    break;
            }

            foreach (DataRow row in sortedRows)
            {
                sortedDt.ImportRow(row);
            }

            return sortedDt;
        }

        private DataTable GetSortedProducts(string sortBy)
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
                           Product_Category, Product_Price, QuantityOnHand, Picture1, Picture2
                    FROM Products2";
                
                switch (sortBy)
                {
                    case "price_asc":
                        query += " ORDER BY Product_Price ASC";
                        break;
                    case "price_desc":
                        query += " ORDER BY Product_Price DESC";
                        break;
                    case "name_asc":
                        query += " ORDER BY Product_Name ASC";
                        break;
                    case "date_desc":
                        query += " ORDER BY Product_Brand DESC";
                        break;
                    default:
                        query += " ORDER BY Product_Brand, Product_Name";
                        break;
                }

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }

        protected void ViewProductDetails(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string productInfo = btn.CommandArgument.ToString();

            // Redirect to product details page (to be created)
            Response.Redirect($"ProductDetails.aspx?product={productInfo}");
        }

        protected void AddToCart(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string productInfo = btn.CommandArgument.ToString();

                // Parse product information
                string[] parts = productInfo.Split('_');
                if (parts.Length >= 2)
                {
                    string brand = parts[0];
                    string productName = string.Join("_", parts, 1, parts.Length - 1).Replace("%20", " ");

                    // Get product data from database
                    var productData = GetProductFromDatabase(brand, productName);
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
                                    int productId = Convert.ToInt32(productData["Product_ID"]);
                                    CartDatabase.AddItemToCart(cartId, productId, 1, Convert.ToDecimal(productData["Product_Price"]));
                                    Session["Cart_ID"] = cartId;
                                    System.Diagnostics.Debug.WriteLine($"Successfully added item to database cart: CartId={cartId}, ProductId={productId}");
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
                                ProductId = Convert.ToInt32(productData["Product_ID"]).ToString(), // Convert to string for session cart
                                ProductName = productData["Product_Name"].ToString(),
                                Brand = productData["Product_Brand"].ToString(),
                                Category = productData["Product_Category"].ToString(),
                                Price = Convert.ToDecimal(productData["Product_Price"]),
                                Quantity = 1,
                                ImageUrl = GetProductImage(productData["Picture1"]?.ToString(), productData["Picture2"]?.ToString())
                            };
                            
                            AddToCartItem(cartItem);
                        }

                        // Show success message without redirect
                        string successMessage = $"Added {productData["Product_Name"]} to cart!";
                        ScriptManager.RegisterStartupScript(this, GetType(), "AddToCartSuccess",
                            $"showSuccessMessage('{successMessage}'); updateCartCounter();", true);
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

        protected void ToggleWishlist(object sender, EventArgs e)
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

                Button btn = (Button)sender;
                int productId = Convert.ToInt32(btn.CommandArgument);
                string custId = Session["Cust_ID"]?.ToString();
                
                if (!string.IsNullOrEmpty(custId))
                {
                    int customerId = Convert.ToInt32(custId);
                    
                    // Check if item is already in wishlist
                    bool isInWishlist = WishlistDatabase.IsInWishlist(customerId, productId);
                    
                    if (isInWishlist)
                    {
                        // Remove from wishlist
                        // Note: This is a simplified approach - in a real app you'd need to get the WishlistItemId
                        ScriptManager.RegisterStartupScript(this, GetType(), "WishlistRemove",
                            "showSuccessMessage('Item removed from wishlist!');", true);
                    }
                    else
                    {
                        // Add to wishlist
                        WishlistDatabase.AddItemToWishlist(customerId, productId);
                        
                        // Update button appearance
                        btn.Text = "♥";
                        btn.CssClass = "btn-wishlist in-wishlist";
                        
                        ScriptManager.RegisterStartupScript(this, GetType(), "WishlistAdd",
                            "showSuccessMessage('Item added to wishlist!');", true);
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
            // Get existing cart from ViewState
            List<CartItem> cart = ViewState["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

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

            // Save back to ViewState
            ViewState["Cart"] = cart;
            
            // Also save to CartTransfer for cart page
            CartTransfer.SaveCart(Session.SessionID, cart);
         


		}

        private DataRow GetProductFromDatabase(string brand, string productName)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Product_ID, Product_Brand, Product_Name, Product_Description, 
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

        private string GetProductImage(string picture1, string picture2)
        {
            string picture = null;

            if (!string.IsNullOrEmpty(picture1))
                picture = picture1;
            else if (!string.IsNullOrEmpty(picture2))
                picture = picture2;
            else
                return "/Images/Products/placeholder.jpg";

            // Remove any existing path prefixes to avoid double paths
            picture = picture.Replace("Images\\Products\\", "").Replace("Images/Products/", "");

            // If it's already an absolute URL, return as is
            if (picture.StartsWith("/") || picture.StartsWith("http"))
                return picture;

            // Otherwise prepend the absolute path
            return $"/Images/Products/{picture}";
        }

        protected void LoadMoreProducts(object sender, EventArgs e)
        {
            // Implementation for pagination/load more functionality
            ScriptManager.RegisterStartupScript(this, GetType(), "LoadMore",
                "alert('Load more functionality to be implemented with database pagination');", true);
        }

        protected void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;

                // Update the image control
                Image imgProduct = (Image)e.Item.FindControl("imgProduct");
                if (imgProduct != null)
                {
                    string picture1 = row["Picture1"]?.ToString();
                    string picture2 = row["Picture2"]?.ToString();
                    imgProduct.ImageUrl = GetProductImage(picture1, picture2);
                    imgProduct.AlternateText = row["Product_Name"].ToString();
                }

                // Update stock label
                Label lblStock = (Label)e.Item.FindControl("lblStock");
                Button btnAddToCart = (Button)e.Item.FindControl("btnAddToCart");
                
                if (lblStock != null)
                {
                    int qty = 0;
                    int.TryParse(row["QuantityOnHand"]?.ToString() ?? "0", out qty);

                    if (qty > 5)
                    {
                        lblStock.Text = "In stock";
                        lblStock.CssClass = "product-stock"; // green class from CSS
                    }
                    else if (qty <= 5 && qty >= 1)
                    {
                        lblStock.Text = "Limited stock";
                        lblStock.CssClass = "product-stock limited-stock"; // orange styling
                    }
                    else
                    {
                        lblStock.Text = "Out of stock";
                        lblStock.CssClass = "product-stock out-of-stock"; // red styling
                    }
                }
                
                // Disable Add to Cart button if out of stock
                if (btnAddToCart != null)
                {
                    int qty = 0;
                    int.TryParse(row["QuantityOnHand"]?.ToString() ?? "0", out qty);
                    
                    if (qty <= 0)
                    {
                        btnAddToCart.Enabled = false;
                        btnAddToCart.CssClass = "btn-cart btn-disabled";
                        btnAddToCart.Text = "Out of Stock";
                    }
                    else
                    {
                        btnAddToCart.Enabled = true;
                        btnAddToCart.CssClass = "btn-cart";
                        btnAddToCart.Text = "Add to Cart";
                    }
                }
            }

        }

    }
}