using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class LoginModel
{
    [Required(ErrorMessage = "No email listed.")]
    [MaxLength(50)]
    [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email.")]
    public string Email { get; set; }
         
    [Required(ErrorMessage = "No password listed.")]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }
}