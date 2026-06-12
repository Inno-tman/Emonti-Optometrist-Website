using System;

namespace EmontiOptometrist.Web.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string CustID { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal Order_Total { get; set; }
        public string Order_Status { get; set; }
        public string Delivery_Address { get; set; }
        public string Payment_Method { get; set; }
        public string Payment_Status { get; set; }
        public string Order_Number { get; set; }
        public DateTime? Payment_Date { get; set; }
        public string Notes { get; set; }
    }
}
