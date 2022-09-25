using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class CartItem
    {
        public int Quantity { set; get; }
        public virtual Item item { set; get; } = null!;
    }
}