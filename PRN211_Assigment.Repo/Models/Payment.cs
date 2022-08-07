using System;
using System.Collections.Generic;

#nullable disable

namespace PRN211_Assigment.Repo.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public DateTime? PayTime { get; set; }
        public double? Amount { get; set; }
        public string PayType { get; set; }
        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
