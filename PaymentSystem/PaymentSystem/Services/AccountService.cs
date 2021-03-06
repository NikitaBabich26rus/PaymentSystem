using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly IBalanceRepository _balanceRepository;

    public AccountService(
        IAccountRepository accountRepository,
        IRolesRepository rolesRepository,
        IBalanceRepository balanceRepository)
    {
        _accountRepository = accountRepository;
        _rolesRepository = rolesRepository;
        _balanceRepository = balanceRepository;
    }

    public async ValueTask<UserRecord?> GetUserByIdAsync(int id)
        => await _accountRepository.GetUserByIdAsync(id);

    public async ValueTask<UserProfileModel> GetUserProfileAsync(int userId)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new ArgumentException($"User was not found");
        }
        
        var userRole = await _rolesRepository.GetUserRoleAsync(userId);

        var balance = await _balanceRepository.GetUserBalanceAsync(userId);

        if (balance == null)
        {
            throw new ArgumentException($"No balance was found for the user.");
        }

        var userProfile = new UserProfileModel()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt,
            IsVerified = user.IsVerified,
            IsBlocked = user.IsBlocked,
            Balance = balance.Amount,
            Role = userRole
        };
        
        return userProfile;
    }

    public async Task<UserRecord?> GetUserByEmailAsync(string email)
        => await _accountRepository.GetUserByEmailAsync(email);
    
    public async Task<int> CreateUserAsync(RegisterModel registerModel)
    {
        var newUser = new UserRecord()
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            Password = registerModel.Password,
            RegisteredAt = DateTime.UtcNow
        };
        
        var userId = await _accountRepository.CreateUserAsync(newUser);
        
        return userId;
    }
    
    public async Task DeleteUserAsync(UserRecord userRecord)
        => await _accountRepository.DeleteUserAsync(userRecord);

    public async Task UpdateUserAccountAsync(UpdateUserAccountModel updateUserAccountModel, UserRecord user)
    {
        user.Email = updateUserAccountModel.Email;
        user.Password = updateUserAccountModel.NewPassword;
        user.FirstName = updateUserAccountModel.FirstName;
        user.LastName = updateUserAccountModel.LastName;
        await _accountRepository.UpdateUserAsync(user);
    }

    public async Task UpdateUserProfileByAdminAsync(int userId, UpdateUserProfileModel updateUser)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);

        if (user == null)
        {
            throw new Exception("User not found for update.");
        }
        
        user.Email = updateUser.Email;
        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.IsBlocked = updateUser.Status != UserStatus.ActiveStatus;
        await _rolesRepository.UpdateUserRoleAsync(userId, updateUser.Role);
        await _accountRepository.UpdateUserAsync(user);
    }

    public async ValueTask<List<UserProfileModel>> GetUsersProfiles()
        => await _accountRepository.GetUsersProfilesAsync().ToListAsync();
}