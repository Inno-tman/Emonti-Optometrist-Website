using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Emonti_Optometrist_Website.Admin
{
    public partial class ManageProducts : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IsStaffLoggedIn"] == null || !(bool)Session["IsStaffLoggedIn"])
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }
            if (Session["StaffRole"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Staff/Dashboard.aspx");
                return;
            }
            if (!IsPostBack) LoadProducts(null);
        }

        private void LoadProducts(string search)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT ProductID, ProductName, Brand, Price, Stock, Category, Description, ImageURL FROM Products2";
                if (!string.IsNullOrEmpty(search))
                    sql += " WHERE ProductName LIKE @Search OR Brand LIKE @Search OR Category LIKE @Search";
                sql += " ORDER BY ProductName";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    if (!string.IsNullOrEmpty(search))
                        cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                    var dt = new DataTable();
                    new SqlDataAdapter(cmd).Fill(dt);
                    gvProducts.DataSource = dt;
                    gvProducts.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            string name = txtAddName.Text.Trim();
            string brand = txtAddBrand.Text.Trim();
            decimal price;
            int stock;
            string category = ddlAddCategory.SelectedValue;
            string desc = txtAddDesc.Text.Trim();
            string image = txtAddImage.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(brand) || !decimal.TryParse(txtAddPrice.Text, out price) || !int.TryParse(txtAddStock.Text, out stock))
            {
                lblAddError.Text = "Please fill in all required fields correctly.";
                lblAddError.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"INSERT INTO Products2 (ProductName, Brand, Price, Stock, Category, Description, ImageURL) VALUES (@Name, @Brand, @Price, @Stock, @Category, @Desc, @Image)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : (object)category);
                    cmd.Parameters.AddWithValue("@Desc", string.IsNullOrEmpty(desc) ? DBNull.Value : (object)desc);
                    cmd.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(image) ? DBNull.Value : (object)image);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("ManageProducts.aspx");
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.NewEditIndex].Value);
            e.Cancel = true;

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT ProductID, ProductName, Brand, Price, Stock, Category, Description, ImageURL FROM Products2 WHERE ProductID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hfEditId.Value = reader["ProductID"].ToString();
                            txtEditName.Text = reader["ProductName"].ToString();
                            txtEditBrand.Text = reader["Brand"].ToString();
                            txtEditPrice.Text = Convert.ToDecimal(reader["Price"]).ToString("N2");
                            txtEditStock.Text = reader["Stock"].ToString();
                            if (!reader.IsDBNull(reader.GetOrdinal("Category")))
                                ddlEditCategory.SelectedValue = reader["Category"].ToString();
                            txtEditDesc.Text = reader["Description"]?.ToString() ?? "";
                            txtEditImage.Text = reader["ImageURL"]?.ToString() ?? "";
                        }
                    }
                }
            }
            ClientScript.RegisterStartupScript(GetType(), "showEditModal", "document.getElementById('editModal').classList.add('show');", true);
        }

        protected void btnEditProduct_Click(object sender, EventArgs e)
        {
            int productId;
            if (!int.TryParse(hfEditId.Value, out productId))
                return;

            string name = txtEditName.Text.Trim();
            string brand = txtEditBrand.Text.Trim();
            decimal price;
            int stock;
            string category = ddlEditCategory.SelectedValue;
            string desc = txtEditDesc.Text.Trim();
            string image = txtEditImage.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(brand) || !decimal.TryParse(txtEditPrice.Text, out price) || !int.TryParse(txtEditStock.Text, out stock))
            {
                lblEditError.Text = "Please fill in all required fields correctly.";
                lblEditError.Visible = true;
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"UPDATE Products2 SET ProductName=@Name, Brand=@Brand, Price=@Price, Stock=@Stock, Category=@Category, Description=@Desc, ImageURL=@Image WHERE ProductID=@Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Brand", brand);
                    cmd.Parameters.AddWithValue("@Price", price);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@Category", string.IsNullOrEmpty(category) ? DBNull.Value : (object)category);
                    cmd.Parameters.AddWithValue("@Desc", string.IsNullOrEmpty(desc) ? DBNull.Value : (object)desc);
                    cmd.Parameters.AddWithValue("@Image", string.IsNullOrEmpty(image) ? DBNull.Value : (object)image);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("ManageProducts.aspx");
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productId = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            string connStr = ConfigurationManager.ConnectionStrings["ProductConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM Products2 WHERE ProductID = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", productId);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect("ManageProducts.aspx");
        }
    }
}
