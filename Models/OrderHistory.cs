using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class OrderHistory
    {
        public OrderHistory()
        {
            Histories = new HashSet<History>();
        }

        public int OrderHistoryID { set; get; }
        public DateTime OrderHistoryDate { get; set; }
        public string OrderHistoryUser { set; get; } = null!;
        public int OrderHistoryStatus { set; get; }
        public virtual ICollection<History> Histories { get; set; }
    }
}