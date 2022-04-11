using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Models;

public class UpdateUserProfileModel
{
    [Required(ErrorMessage = "No first name listed.")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "No last name listed.")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "No email listed.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email.")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "No role listed.")]
    public string Role { get; set; }
    
    [Required(ErrorMessage = "No status listed.")]
    public string Status { get; set; }
}