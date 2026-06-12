using System;
using System.Collections.Generic;
using System.Linq;

namespace EmontiOptometrist.Web.Models
{
    public static class ShoppingCart
    {
        private static List<CartItem> _cartItems = new List<CartItem>();

        public static List<CartItem> GetCartItems()
        {
            return _cartItems;
        }

        public static void AddItem(CartItem item)
        {
            var existingItem = _cartItems.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                _cartItems.Add(item);
            }
        }

        public static void RemoveItem(string productId)
        {
            _cartItems.RemoveAll(x => x.ProductId == productId);
        }

        public static void UpdateQuantity(string productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(productId);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }

        public static void ClearCart()
        {
            _cartItems.Clear();
        }

        public static int GetTotalItems()
        {
            return _cartItems.Sum(x => x.Quantity);
        }

        public static decimal GetTotalPrice()
        {
            return _cartItems.Sum(x => x.Subtotal);
        }
    }
}
