using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data
{
    [Table("balances")]
    public class BalanceRecord
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        [Column("user_id")]
        public int UserId { get; set; }
        
        [Column("amount")]
        [DefaultValue(0)]
        public decimal Amount { get; set; }
        
        public UserRecord UserRecord { get; set; }
    }
}
