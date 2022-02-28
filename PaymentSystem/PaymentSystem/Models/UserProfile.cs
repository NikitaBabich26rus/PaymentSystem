using PaymentSystem.Data;

namespace PaymentSystem.Models;

public class UserProfile
{
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public DateTime RegisteredAt { get; set; }

    public bool? IsVerified { get; set; }
    
    public decimal Balance { get; set; }
    
    public List<string> Roles { get; set; }
}