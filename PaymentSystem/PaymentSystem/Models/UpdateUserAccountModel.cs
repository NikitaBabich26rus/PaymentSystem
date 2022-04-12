using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class UpdateUserAccountModel
{
    [Required(ErrorMessage = "No first name listed.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "No last name listed.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "No email listed.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email.")]
    public string Email { get; set; }
    
    [StringLength(30, MinimumLength = 6)]
    [Required(ErrorMessage = "No old password listed.")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }
    
    [StringLength(30, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "No new password listed.")]
    public string NewPassword { get; set; }
}