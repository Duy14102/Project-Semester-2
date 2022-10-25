using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class History
    {
        public int HistoryID { set; get; }
        public string HistoryName { set; get; } = null!;
        public int HistoryQuantity { set; get; }
        public decimal HistoryPrice { set; get; }
        public int HistoryOrderId { set; get; }
        public virtual OrderHistory OrderHistory { get; set; } = null!;
    }
}