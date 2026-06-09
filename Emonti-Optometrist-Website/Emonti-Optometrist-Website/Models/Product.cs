using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Emonti_Optometrist_Website.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}