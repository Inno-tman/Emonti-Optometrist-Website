using System;
using System.Collections.Generic;
using System.Linq;

namespace Emonti_Optometrist_Website
{
    public static class CartTransfer
    {
        private static Dictionary<string, List<CartItem>> _userCarts = new Dictionary<string, List<CartItem>>();

        public static void SaveCart(string sessionId, List<CartItem> cart)
        {
            _userCarts[sessionId] = cart;
        }

        public static List<CartItem> GetCart(string sessionId)
        {
            if (_userCarts.ContainsKey(sessionId))
            {
                return _userCarts[sessionId];
            }
            return new List<CartItem>();
        }

        public static int GetTotalItems(string sessionId)
        {
            if (_userCarts.ContainsKey(sessionId))
            {
                return _userCarts[sessionId].Sum(item => item.Quantity);
            }
            return 0;
        }

        public static void ClearCart(string sessionId)
        {
            if (_userCarts.ContainsKey(sessionId))
            {
                _userCarts.Remove(sessionId);
            }
        }
    }
}
