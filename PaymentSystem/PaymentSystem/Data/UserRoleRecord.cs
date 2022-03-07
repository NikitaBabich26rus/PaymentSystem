using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSystem.Data
{
    [Table("user_roles")]
    public class UserRoleRecord
    {
        [Column("id")]
        public int Id { get; set; }
        
        [Column("user_id")]
        [Required]
        public int UserId { get; set; }
        
        [Column("role_id")]
        [Required]
        public int RoleId { get; set; }
        
        public RoleRecord RoleRecord { get; set; }
        
        public UserRecord UserRecord { get; set; }
    }
}
