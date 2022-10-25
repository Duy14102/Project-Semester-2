using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAspNetApp.Models
{
    public partial class CartItem
    {
        [StringLength(99, MinimumLength = 1)]
        public int Quantity { set; get; }
        public virtual Item item { set; get; } = null!;
        public virtual User user {set;get;} = null!;

        public virtual OrderHistory OrderHistory { get; set; } = null!;
    }
}