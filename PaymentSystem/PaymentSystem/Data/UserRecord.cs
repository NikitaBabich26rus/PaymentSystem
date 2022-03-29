using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data
{
    [Table("users")]
    public class UserRecord
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("first_name")]
        [MaxLength(30)]
        [Required]
        public string FirstName { get; set; }
        
        [Column("last_name")]
        [MaxLength(30)]
        [Required]
        public string LastName { get; set; }
        
        [Column("email")]
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        
        [Column("registered_at")]
        [Required]
        public DateTime RegisteredAt { get; set; }

        [Column("is_blocked")]
        [DefaultValue(false)]
        public bool IsBlocked { get; set; }
        
        [Column("is_verified")]
        [DefaultValue(false)]
        public bool IsVerified { get; set; }
        
        [Column("password")]
        [MaxLength(30)]
        [Required]
        public string Password { get; set; }
        
        public BalanceRecord Balance { get; set; }
        
        public VerificationTransferRecord VerificationTransfer { get; set; }
        
        public List<VerificationTransferRecord> VerificationTransfersConfirmedBy { get; set; }
        
        public List<UserRoleRecord> UserRoleRecords { get; set; }
        
        public List<FundTransferRecord> FundTransferConfirmedBy { get; set; }
        
        public List<FundTransferRecord> FundTransferCreatedBy { get; set; }
        
        public List<FundTransferRecord> FundTransfers { get; set; }
    }
}
