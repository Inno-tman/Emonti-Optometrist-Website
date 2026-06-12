using System;

namespace EmontiOptometrist.Web.Models
{
    [Serializable]
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public decimal Subtotal => Price * Quantity;
    }
}
