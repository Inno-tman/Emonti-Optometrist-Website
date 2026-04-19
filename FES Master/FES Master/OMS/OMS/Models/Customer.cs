using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Models
{
    public class Customer
    {
        public int Cust_ID { get; set; }
        public string Customer_Name { get; set; } = "";
        public string Customer_Surname { get; set; } = "";
        public string Customer_DOB { get; set; } = "";
        public string Customer_Gender { get; set; } = "";
        public string Customer_Email { get; set; } = "";
        public string Customer_Phone { get; set; } = "";
        public string Customer_Address { get; set; } = "";
        public string Medical_Aid { get; set; } = "";
        public string Medical_Aid_Number { get; set; } = "";

        // Fields for main member
        public string Main_Member_Name { get; set; } = "";
        public string Main_Member_Surname { get; set; } = "";
        public string Main_Member_ID { get; set; } = ""; // 13 digits only

        // Address fields
        public string Street_Number { get; set; } = "";
        public string Street_Name { get; set; } = "";
        public string Complex_Name { get; set; } = "";
        public string Unit_Number { get; set; } = "";
        public string City { get; set; } = "";
        public string Province { get; set; } = "";
        public string Postal_Code { get; set; } = "";

        // Soft delete field
        public int Is_Archive { get; set; } = 0; // 0 = not archived, 1 = archived

        // Helper property to check if customer is archived
        public bool IsArchived => Is_Archive == 1;

        // Helper property to get full name
        public string FullName => $"{Customer_Name} {Customer_Surname}".Trim();

        // Helper property to get full address
        public string FullAddress
        {
            get
            {
                var addressParts = new List<string>();

                if (!string.IsNullOrEmpty(Street_Number) || !string.IsNullOrEmpty(Street_Name))
                    addressParts.Add($"{Street_Number} {Street_Name}".Trim());

                if (!string.IsNullOrEmpty(Complex_Name))
                {
                    if (!string.IsNullOrEmpty(Unit_Number))
                        addressParts.Add($"{Complex_Name}, Unit {Unit_Number}");
                    else
                        addressParts.Add(Complex_Name);
                }

                if (!string.IsNullOrEmpty(City))
                    addressParts.Add(City);

                if (!string.IsNullOrEmpty(Province))
                    addressParts.Add(Province);

                if (!string.IsNullOrEmpty(Postal_Code))
                    addressParts.Add(Postal_Code);

                return string.Join(", ", addressParts.Where(p => !string.IsNullOrEmpty(p)));
            }
        }
    }
}