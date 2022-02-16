using System;
using System.Collections.Generic;

namespace PaymentSystem.Data
{
    public partial class Balance
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
