using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAspNetApp.Models
{
    public partial class CartItem
    {
        public int Quantity { set; get; }
        public virtual Item item { set; get; } = null!;
    }
}