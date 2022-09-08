using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int? CustomerId { get; set; }
        public int? OrderStatus { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
