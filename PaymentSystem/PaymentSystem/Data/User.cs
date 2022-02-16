using System;
using System.Collections.Generic;

namespace PaymentSystem.Data
{
    public partial class User
    {
        public User()
        {
            FundTransferConfirmedByNavigations = new HashSet<FundTransfer>();
            FundTransferCreatedByNavigations = new HashSet<FundTransfer>();
            FundTransferUsers = new HashSet<FundTransfer>();
            UserRoles = new HashSet<UserRole>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegisteredAt { get; set; }
        public string? PassportData { get; set; }
        public bool? IsVerified { get; set; }
        public string Password { get; set; } = null!;

        public virtual ICollection<FundTransfer> FundTransferConfirmedByNavigations { get; set; }
        public virtual ICollection<FundTransfer> FundTransferCreatedByNavigations { get; set; }
        public virtual ICollection<FundTransfer> FundTransferUsers { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
