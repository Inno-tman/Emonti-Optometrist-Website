using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace Emonti_Optometrist_Website
{
    public partial class TestDatabaseSchema : Page
    {
        private string connectionString = System.Configuration.ConfigurationManager
            .ConnectionStrings["ProductConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnTestConnection_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    sb.AppendLine("<div class='success'>");
                    sb.AppendLine("<h2>✓ Connection Successful!</h2>");
                    sb.AppendLine($"<p>Database: {conn.Database}</p>");
                    sb.AppendLine($"<p>Server: {conn.DataSource}</p>");
                    sb.AppendLine($"<p>Connection State: {conn.State}</p>");
                    sb.AppendLine("</div>");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("<div class='error'>");
                sb.AppendLine("<h2>✗ Connection Failed!</h2>");
                sb.AppendLine($"<p>Error: {ex.Message}</p>");
                sb.AppendLine("</div>");
            }
            litResults.Text = sb.ToString();
        }

        protected void btnGetSchema_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    // Get table schema
                    string query = @"
                        SELECT 
                            COLUMN_NAME, 
                            DATA_TYPE, 
                            IS_NULLABLE, 
                            CHARACTER_MAXIMUM_LENGTH,
                            COLUMN_DEFAULT
                        FROM INFORMATION_SCHEMA.COLUMNS 
                        WHERE TABLE_NAME = 'customer'
                        ORDER BY ORDINAL_POSITION";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            sb.AppendLine("<div class='success'>");
                            sb.AppendLine("<h2>Customer Table Schema</h2>");
                            sb.AppendLine("<table>");
                            sb.AppendLine("<tr><th>Column Name</th><th>Data Type</th><th>Nullable</th><th>Max Length</th><th>Default</th></tr>");

                            while (reader.Read())
                            {
                                string columnName = reader["COLUMN_NAME"].ToString();
                                string dataType = reader["DATA_TYPE"].ToString();
                                string isNullable = reader["IS_NULLABLE"].ToString();
                                string maxLength = reader["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? "N/A" : reader["CHARACTER_MAXIMUM_LENGTH"].ToString();
                                string defaultValue = reader["COLUMN_DEFAULT"] == DBNull.Value ? "None" : reader["COLUMN_DEFAULT"].ToString();

                                sb.AppendLine($"<tr><td><strong>{columnName}</strong></td><td>{dataType}</td><td>{isNullable}</td><td>{maxLength}</td><td>{defaultValue}</td></tr>");
                            }

                            sb.AppendLine("</table>");
                            sb.AppendLine("</div>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("<div class='error'>");
                sb.AppendLine("<h2>✗ Schema Query Failed!</h2>");
                sb.AppendLine($"<p>Error: {ex.Message}</p>");
                sb.AppendLine($"<p>Stack Trace: {ex.StackTrace}</p>");
                sb.AppendLine("</div>");
            }
            litResults.Text = sb.ToString();
        }

        protected void btnTestInsert_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO customer (
                            Customer_Name, Customer_Surname, Customer_DOB, Customer_Gender,
                            Customer_Email, Customer_Phone, Customer_Address,
                            Medical_Aid, Medical_Aid_Number,
                            Main_Member_Name, Main_Member_Surname, Main_Member_ID,
                            Street_Number, Street_Name, Complex_Name, Unit_Number,
                            City, Province, Postal_Code, Is_Archive, Customer_Password
                        ) VALUES (
                            @Customer_Name, @Customer_Surname, @Customer_DOB, @Customer_Gender,
                            @Customer_Email, @Customer_Phone, @Customer_Address,
                            @Medical_Aid, @Medical_Aid_Number,
                            @Main_Member_Name, @Main_Member_Surname, @Main_Member_ID,
                            @Street_Number, @Street_Name, @Complex_Name, @Unit_Number,
                            @City, @Province, @Postal_Code, 0, @Customer_Password
                        )";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Test with minimal data
                        cmd.Parameters.AddWithValue("@Customer_Name", "Test");
                        cmd.Parameters.AddWithValue("@Customer_Surname", "User");
                        cmd.Parameters.AddWithValue("@Customer_Email", $"test{DateTime.Now.Ticks}@example.com");
                        cmd.Parameters.AddWithValue("@Customer_Phone", "0123456789");
                        cmd.Parameters.AddWithValue("@Customer_Password", "test123");
                        
                        // Optional fields as NULL
                        cmd.Parameters.AddWithValue("@Customer_DOB", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Customer_Gender", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Customer_Address", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Medical_Aid", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Medical_Aid_Number", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Main_Member_Name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Main_Member_Surname", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Main_Member_ID", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Street_Number", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Street_Name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Complex_Name", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Unit_Number", DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Province", DBNull.Value);
                        cmd.Parameters.AddWithValue("@Postal_Code", DBNull.Value);

                        int rows = cmd.ExecuteNonQuery();

                        sb.AppendLine("<div class='success'>");
                        sb.AppendLine("<h2>✓ Test Insert Successful!</h2>");
                        sb.AppendLine($"<p>Rows affected: {rows}</p>");
                        sb.AppendLine("<p>A test user was created successfully.</p>");
                        sb.AppendLine("</div>");
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("<div class='error'>");
                sb.AppendLine("<h2>✗ Test Insert Failed!</h2>");
                sb.AppendLine($"<p><strong>Error Message:</strong> {ex.Message}</p>");
                
                if (ex.InnerException != null)
                {
                    sb.AppendLine($"<p><strong>Inner Exception:</strong> {ex.InnerException.Message}</p>");
                }
                
                sb.AppendLine($"<p><strong>Stack Trace:</strong></p>");
                sb.AppendLine($"<pre style='background:#f9f9f9; padding:10px; overflow:auto;'>{ex.StackTrace}</pre>");
                sb.AppendLine("</div>");
            }
            litResults.Text = sb.ToString();
        }
    }
}

