using PaymentSystem.Data;

namespace PaymentSystem.Models;

public class UserProfileModel
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public DateTime RegisteredAt { get; set; }

    public bool? IsVerified { get; set; }
    
    public bool IsBlocked { get; set; }
    
    public decimal Balance { get; set; }
    
    public string Role { get; set; }
}