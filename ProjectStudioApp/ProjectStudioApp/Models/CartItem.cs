using System;

namespace ProjectStudioApp.Models
{
    public class CartItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal ItemCost { get; set; }
        public int Quantity { get; set; } = 1;
        public string ItemImage { get; set; } = string.Empty;
    }
}
