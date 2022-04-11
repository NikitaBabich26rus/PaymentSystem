using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "No first name listed.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "No last name listed.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "No email listed.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email.")]
    public string Email { get; set; }
         
    [Required(ErrorMessage = "No password listed.")]
    [DataType(DataType.Password)]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }
         
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Incorrect password confirmation.")]
    public string ConfirmPassword { get; set; }
}