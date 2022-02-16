using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IAuthRepository
{

    Task<User?> GetUserByEmail(string email);
    
    Task<User?> GetUserById(int userId);
    
    Task<int> CreateUser(User user);
    
    Task DeleteUser(User user);
    
    Task UpdateUser(User user);  
}