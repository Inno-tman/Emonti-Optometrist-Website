using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models
{
    public class Customer
    {
        public int Cust_ID;
        public string Customer_Name = "";
        public string Customer_Surname = "";
        public string Customer_DOB = "";
        public string Customer_Gender = "";
        public string Customer_Email = "";
        public string Customer_Phone = "";
        public string Customer_Address = ""; 
        public string Medical_Aid = "";
        public string Medical_Aid_Number = "";

        // Fields for main member
        public string Main_Member_Name = "";
        public string Main_Member_Surname = "";
        public string Main_Member_ID = ""; // 13 digits only

        // Address fields
        public string Street_Number = "";
        public string Street_Name = "";
        public string Complex_Name = "";
        public string Unit_Number = "";
        public string City = "";
        public string Province = "";
        public string Postal_Code = "";
    }
}