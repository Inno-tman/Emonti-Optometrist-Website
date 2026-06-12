using System;

namespace EmontiOptometrist.Web.Models
{
    public class WishlistItem : CartItem
    {
        public int WishlistItemId { get; set; }
        public DateTime AddedAt { get; set; }
        public int StockQuantity { get; set; }
        public string StockStatus { get; set; } = "";
        public string Color { get; set; } = "";
    }
}
