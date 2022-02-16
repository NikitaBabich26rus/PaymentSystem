using System;
using System.Collections.Generic;

namespace PaymentSystem.Data
{
    public partial class FundTransfer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; } = null!;
        public string CardCvc { get; set; } = null!;
        public string CardDate { get; set; } = null!;
        public int CreatedBy { get; set; }
        public int? ConfirmedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        public virtual User? ConfirmedByNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
