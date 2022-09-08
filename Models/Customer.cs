using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? CustomerAddress { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
