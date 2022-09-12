using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class Item
    {
        public Item()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
        public int Amount { get; set; }
        public sbyte ItemStatus { get; set; }
        public string? Category { get; set; }
        public string? ItemStory { get; set; }
        public string? ItemDescription { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
