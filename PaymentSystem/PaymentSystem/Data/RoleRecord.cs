using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data
{
    [Table("roles")]
    public class RoleRecord
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("name")]
        [Required]
        public string Name { get; set; }
        
        public virtual List<UserRoleRecord> UserRoleRecords { get; set; }
    }
}
