using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data
{
    [Table("fund_transfers")]
    public class FundTransferRecord
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("user_id")]
        [Required]
        public int UserId { get; set; }
        
        [Column("amount_of_money")]
        [Required]
        public decimal AmountOfMoney { get; set; }
        
        [Column("card_number")]
        [Required]
        public string CardNumber { get; set; }
        
        [Column("card_cvc")]
        [Required]
        public string CardCvc { get; set; }
        
        [Column("card_date")]
        [Required]
        public string CardDate { get; set; }
        
        [Column("created_by")]
        [Required]
        public int CreatedBy { get; set; }
        
        [Column("confirmed_by")]
        public int? ConfirmedBy { get; set; }
        
        [Column("created_at")]
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Column("confirmed_at")]
        public DateTime? ConfirmedAt { get; set; }
        
        [Column("transfer_type")]
        [Required]
        public TransferType TransferType { get; set; }
        
        public virtual UserRecord? ConfirmedByUserRecord { get; set; }
        
        public virtual UserRecord CreatedByUserRecord { get; set; }
        
        public virtual UserRecord UserRecord { get; set; }
    }
}
