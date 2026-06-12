using System;

namespace EmontiOptometrist.Web.Models
{
    public class DatabaseOrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Brand { get; set; }
        public string Product_Category { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
