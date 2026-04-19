using OMS.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OMS.Repositories
{
    public class CustomerRepository
    {
        private readonly string connectionString = "Data Source=146.230.177.46;Initial Catalog=WstGrp5;User ID=Wstgrp5;Password=87ad5;TrustServerCertificate=True";

        public List<Customer> GetCustomers(bool includeArchived = false)
        {
            var customers = new List<Customer>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Modified query to exclude archived customers by default
                    string sql = includeArchived
                        ? "SELECT * FROM Customer ORDER BY Cust_ID"
                        : "SELECT * FROM Customer WHERE Is_Archive = 0 ORDER BY Cust_ID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = new Customer();

                                // Existing fields (indexes 0-9)
                                customer.Cust_ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                customer.Customer_Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                customer.Customer_Surname = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                customer.Customer_DOB = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                customer.Customer_Gender = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                customer.Customer_Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                customer.Customer_Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                customer.Customer_Address = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                customer.Medical_Aid = reader.IsDBNull(8) ? "" : reader.GetString(8);
                                customer.Medical_Aid_Number = reader.IsDBNull(9) ? "" : reader.GetString(9);

                                // New fields (indexes 10-19)
                                customer.Main_Member_Name = reader.IsDBNull(10) ? "" : reader.GetString(10);
                                customer.Main_Member_Surname = reader.IsDBNull(11) ? "" : reader.GetString(11);
                                customer.Main_Member_ID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                                customer.Street_Number = reader.IsDBNull(13) ? "" : reader.GetString(13);
                                customer.Street_Name = reader.IsDBNull(14) ? "" : reader.GetString(14);
                                customer.Complex_Name = reader.IsDBNull(15) ? "" : reader.GetString(15);
                                customer.Unit_Number = reader.IsDBNull(16) ? "" : reader.GetString(16);
                                customer.City = reader.IsDBNull(17) ? "" : reader.GetString(17);
                                customer.Province = reader.IsDBNull(18) ? "" : reader.GetString(18);
                                customer.Postal_Code = reader.IsDBNull(19) ? "" : reader.GetString(19);

                                // Is_Archive field (index 20)
                                customer.Is_Archive = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetCustomers: " + ex.ToString());
                MessageBox.Show($"Error loading customers: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return customers;
        }

        public List<Customer> GetArchivedCustomers()
        {
            var customers = new List<Customer>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Customer WHERE Is_Archive = 1 ORDER BY Cust_ID";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = new Customer();

                                customer.Cust_ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                customer.Customer_Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                customer.Customer_Surname = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                customer.Customer_DOB = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                customer.Customer_Gender = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                customer.Customer_Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                customer.Customer_Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                customer.Customer_Address = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                customer.Medical_Aid = reader.IsDBNull(8) ? "" : reader.GetString(8);
                                customer.Medical_Aid_Number = reader.IsDBNull(9) ? "" : reader.GetString(9);
                                customer.Main_Member_Name = reader.IsDBNull(10) ? "" : reader.GetString(10);
                                customer.Main_Member_Surname = reader.IsDBNull(11) ? "" : reader.GetString(11);
                                customer.Main_Member_ID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                                customer.Street_Number = reader.IsDBNull(13) ? "" : reader.GetString(13);
                                customer.Street_Name = reader.IsDBNull(14) ? "" : reader.GetString(14);
                                customer.Complex_Name = reader.IsDBNull(15) ? "" : reader.GetString(15);
                                customer.Unit_Number = reader.IsDBNull(16) ? "" : reader.GetString(16);
                                customer.City = reader.IsDBNull(17) ? "" : reader.GetString(17);
                                customer.Province = reader.IsDBNull(18) ? "" : reader.GetString(18);
                                customer.Postal_Code = reader.IsDBNull(19) ? "" : reader.GetString(19);
                                customer.Is_Archive = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetArchivedCustomers: " + ex.ToString());
                MessageBox.Show($"Error loading archived customers: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return customers;
        }

        public Customer GetCustomer(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM Customer WHERE Cust_ID=@custid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Customer customer = new Customer();

                                customer.Cust_ID = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                customer.Customer_Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                customer.Customer_Surname = reader.IsDBNull(2) ? "" : reader.GetString(2);
                                customer.Customer_DOB = reader.IsDBNull(3) ? "" : reader.GetDateTime(3).ToString("yyyy-MM-dd");
                                customer.Customer_Gender = reader.IsDBNull(4) ? "" : reader.GetString(4);
                                customer.Customer_Email = reader.IsDBNull(5) ? "" : reader.GetString(5);
                                customer.Customer_Phone = reader.IsDBNull(6) ? "" : reader.GetString(6);
                                customer.Customer_Address = reader.IsDBNull(7) ? "" : reader.GetString(7);
                                customer.Medical_Aid = reader.IsDBNull(8) ? "" : reader.GetString(8);
                                customer.Medical_Aid_Number = reader.IsDBNull(9) ? "" : reader.GetString(9);
                                customer.Main_Member_Name = reader.IsDBNull(10) ? "" : reader.GetString(10);
                                customer.Main_Member_Surname = reader.IsDBNull(11) ? "" : reader.GetString(11);
                                customer.Main_Member_ID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                                customer.Street_Number = reader.IsDBNull(13) ? "" : reader.GetString(13);
                                customer.Street_Name = reader.IsDBNull(14) ? "" : reader.GetString(14);
                                customer.Complex_Name = reader.IsDBNull(15) ? "" : reader.GetString(15);
                                customer.Unit_Number = reader.IsDBNull(16) ? "" : reader.GetString(16);
                                customer.City = reader.IsDBNull(17) ? "" : reader.GetString(17);
                                customer.Province = reader.IsDBNull(18) ? "" : reader.GetString(18);
                                customer.Postal_Code = reader.IsDBNull(19) ? "" : reader.GetString(19);
                                customer.Is_Archive = reader.IsDBNull(20) ? 0 : reader.GetInt32(20);

                                return customer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in GetCustomer: " + ex.ToString());
                MessageBox.Show($"Error loading customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public void CreateCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO Customer " +
                                 "(Customer_Name, Customer_Surname, Customer_DOB, Customer_Gender, Customer_Email, Customer_Phone, Customer_Address, Medical_Aid, Medical_Aid_Number, " +
                                 "Main_Member_Name, Main_Member_Surname, Main_Member_ID, Street_Number, Street_Name, Complex_Name, Unit_Number, City, Province, Postal_Code, Is_Archive) VALUES " +
                                 "(@name, @surname, @dob, @gender, @email, @phone, @address, @medicalaid, @medicalaidnumber, " +
                                 "@mainname, @mainsurname, @mainid, @streetnumber, @streetname, @complexname, @unitnumber, @city, @province, @postalcode, @isarchive)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Existing parameters
                        command.Parameters.AddWithValue("@name", customer.Customer_Name ?? "");
                        command.Parameters.AddWithValue("@surname", customer.Customer_Surname ?? "");
                        command.Parameters.AddWithValue("@dob", customer.Customer_DOB ?? "");
                        command.Parameters.AddWithValue("@gender", customer.Customer_Gender ?? "");
                        command.Parameters.AddWithValue("@email", customer.Customer_Email ?? "");
                        command.Parameters.AddWithValue("@phone", customer.Customer_Phone ?? "");
                        command.Parameters.AddWithValue("@address", customer.Customer_Address ?? "");
                        command.Parameters.AddWithValue("@medicalaid", customer.Medical_Aid ?? "");
                        command.Parameters.AddWithValue("@medicalaidnumber", customer.Medical_Aid_Number ?? "");

                        // New parameters
                        command.Parameters.AddWithValue("@mainname", customer.Main_Member_Name ?? "");
                        command.Parameters.AddWithValue("@mainsurname", customer.Main_Member_Surname ?? "");
                        command.Parameters.AddWithValue("@mainid", customer.Main_Member_ID ?? "");
                        command.Parameters.AddWithValue("@streetnumber", customer.Street_Number ?? "");
                        command.Parameters.AddWithValue("@streetname", customer.Street_Name ?? "");
                        command.Parameters.AddWithValue("@complexname", customer.Complex_Name ?? "");
                        command.Parameters.AddWithValue("@unitnumber", customer.Unit_Number ?? "");
                        command.Parameters.AddWithValue("@city", customer.City ?? "");
                        command.Parameters.AddWithValue("@province", customer.Province ?? "");
                        command.Parameters.AddWithValue("@postalcode", customer.Postal_Code ?? "");

                        // Set Is_Archive to 0 for new customers (not archived)
                        command.Parameters.AddWithValue("@isarchive", 0);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateCustomer: " + ex.ToString());
                MessageBox.Show($"Error creating customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Customer " +
                                 "SET Customer_Name=@name, Customer_Surname=@surname, Customer_DOB=@dob, Customer_Gender=@gender, Customer_Email=@email, " +
                                 "Customer_Phone=@phone, Customer_Address=@address, Medical_Aid=@medicalaid, Medical_Aid_Number=@medicalaidnumber, " +
                                 "Main_Member_Name=@mainname, Main_Member_Surname=@mainsurname, Main_Member_ID=@mainid, " +
                                 "Street_Number=@streetnumber, Street_Name=@streetname, Complex_Name=@complexname, Unit_Number=@unitnumber, " +
                                 "City=@city, Province=@province, Postal_Code=@postalcode, Is_Archive=@isarchive " +
                                 "WHERE Cust_ID=@custid";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Existing parameters
                        command.Parameters.AddWithValue("@name", customer.Customer_Name ?? "");
                        command.Parameters.AddWithValue("@surname", customer.Customer_Surname ?? "");
                        command.Parameters.AddWithValue("@dob", customer.Customer_DOB ?? "");
                        command.Parameters.AddWithValue("@gender", customer.Customer_Gender ?? "");
                        command.Parameters.AddWithValue("@email", customer.Customer_Email ?? "");
                        command.Parameters.AddWithValue("@phone", customer.Customer_Phone ?? "");
                        command.Parameters.AddWithValue("@address", customer.Customer_Address ?? "");
                        command.Parameters.AddWithValue("@medicalaid", customer.Medical_Aid ?? "");
                        command.Parameters.AddWithValue("@medicalaidnumber", customer.Medical_Aid_Number ?? "");

                        // New parameters
                        command.Parameters.AddWithValue("@mainname", customer.Main_Member_Name ?? "");
                        command.Parameters.AddWithValue("@mainsurname", customer.Main_Member_Surname ?? "");
                        command.Parameters.AddWithValue("@mainid", customer.Main_Member_ID ?? "");
                        command.Parameters.AddWithValue("@streetnumber", customer.Street_Number ?? "");
                        command.Parameters.AddWithValue("@streetname", customer.Street_Name ?? "");
                        command.Parameters.AddWithValue("@complexname", customer.Complex_Name ?? "");
                        command.Parameters.AddWithValue("@unitnumber", customer.Unit_Number ?? "");
                        command.Parameters.AddWithValue("@city", customer.City ?? "");
                        command.Parameters.AddWithValue("@province", customer.Province ?? "");
                        command.Parameters.AddWithValue("@postalcode", customer.Postal_Code ?? "");
                        command.Parameters.AddWithValue("@isarchive", customer.Is_Archive);

                        command.Parameters.AddWithValue("@custid", customer.Cust_ID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in UpdateCustomer: " + ex.ToString());
                MessageBox.Show($"Error updating customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Soft delete - sets Is_Archive to 1 instead of deleting the record
        public void SoftDeleteCustomer(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Customer SET Is_Archive = 1 WHERE Cust_ID = @custid";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Customer not found or already archived.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in SoftDeleteCustomer: " + ex.ToString());
                MessageBox.Show($"Error archiving customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Restore an archived customer
        public void RestoreCustomer(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "UPDATE Customer SET Is_Archive = 0 WHERE Cust_ID = @custid";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Customer not found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in RestoreCustomer: " + ex.ToString());
                MessageBox.Show($"Error restoring customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Keep the original hard delete method for permanent deletion if needed
        public void DeleteCustomer(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "DELETE FROM Customer WHERE Cust_ID=@custid";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DeleteCustomer: " + ex.ToString());
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Check if a customer exists and is not archived
        public bool CustomerExists(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM Customer WHERE Cust_ID = @custid AND Is_Archive = 0";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CustomerExists: " + ex.ToString());
                return false;
            }
        }

        // Check if a customer is archived
        public bool IsCustomerArchived(int Cust_ID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT Is_Archive FROM Customer WHERE Cust_ID = @custid";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@custid", Cust_ID);
                        object result = command.ExecuteScalar();
                        return result != null && Convert.ToInt32(result) == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in IsCustomerArchived: " + ex.ToString());
                return false;
            }
        }
    }
}