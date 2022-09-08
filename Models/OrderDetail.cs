using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
