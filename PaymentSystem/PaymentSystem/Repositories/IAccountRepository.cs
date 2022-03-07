using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IAccountRepository
{

    Task<UserRecord?> GetUserByEmailAsync(string email);
    
    Task<UserRecord?> GetUserByIdAsync(int userId);
    
    Task<int> CreateUserAsync(UserRecord userRecord);

    Task DeleteUserAsync(UserRecord userRecord);
    
    Task UpdateUserAsync(UserRecord userRecord);  
}