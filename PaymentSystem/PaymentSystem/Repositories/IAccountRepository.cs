using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public interface IAccountRepository
{

    ValueTask<UserRecord?> GetUserByEmailAsync(string email);
    
    ValueTask<UserRecord?> GetUserByIdAsync(int userId);
    
    Task<int> CreateUserAsync(UserRecord userRecord);

    Task DeleteUserAsync(UserRecord userRecord);
    
    Task UpdateUserAsync(UserRecord userRecord);

    IQueryable<UserProfileModel> GetUsersProfilesAsync();
}