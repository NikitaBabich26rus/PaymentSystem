using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data;

[Table("verification_transfers")]
public class VerificationTransferRecord
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("user_id")]
    [Required]
    public int UserId { get; set; }
    
    [Column("confirmed_by")]
    public int? ConfirmedBy { get; set; }
    
    [Column("Passport_data")]
    [Required]
    public string PassportData { get; set; }
    
    [Column("created_at")]
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Column("confirmed_at")]
    public DateTime? ConfirmedAt { get; set; }
    
    public virtual UserRecord User { get; set; }
    
    public virtual UserRecord ConfirmedByUser { get; set; }
}