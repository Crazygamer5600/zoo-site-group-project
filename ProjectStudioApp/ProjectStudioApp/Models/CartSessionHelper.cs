using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProjectStudioApp.Models
{
    public static class CartSessionHelper
    {
        private const string CartKey = "Cart";

        public static List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartKey);
            return cartJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetString(CartKey, JsonConvert.SerializeObject(cart));
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartKey);
        }
    }
}
