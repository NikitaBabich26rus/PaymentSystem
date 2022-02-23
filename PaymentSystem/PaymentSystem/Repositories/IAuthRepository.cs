using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IAuthRepository
{

    Task<UserRecord?> GetUserByEmail(string email);
    
    Task<UserRecord?> GetUserById(int userId);
    
    Task<int> CreateUser(UserRecord userRecord);
    
    Task DeleteUser(UserRecord userRecord);
    
    Task UpdateUser(UserRecord userRecord);  
}