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
        var userRole = await _rolesRepository.GetUserRoleAsync(userId);
        
        var balance = await _balanceRepository.GetUserBalanceAsync(userId);

        if (balance == null)
        {
            throw new ArgumentException($"No balance was found for the user with id: {userId}.");
        }

        var userProfile = new UserProfileModel()
        {
            Id = user!.Id,
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
        user!.Email = updateUser.Email;
        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.IsBlocked = updateUser.Status != UserStatus.ActiveStatus;
        await _rolesRepository.UpdateUserRoleAsync(updateUser.Role, userId);
        await _accountRepository.UpdateUserAsync(user);
    }

    public async Task<List<UserProfileModel>> GetUsersProfiles()
    {
        var users = await _accountRepository.GetUsersAsync();
        var usersProfiles = new List<UserProfileModel>();

        foreach (var user in users)
        {
            if (user.RoleRecord.Name == Roles.AdminRole)
            {
                continue;   
            }

            var userBalance = await _balanceRepository.GetUserBalanceAsync(user.UserRecord.Id);

            usersProfiles.Add(new UserProfileModel()
            {
                Id = user.UserRecord.Id,
                FirstName = user.UserRecord.FirstName,
                LastName = user.UserRecord.LastName,
                Email = user.UserRecord.Email,
                RegisteredAt = user.UserRecord.RegisteredAt,
                IsVerified = user.UserRecord.IsVerified,
                IsBlocked = user.UserRecord.IsBlocked,
                Balance = userBalance.Amount,
                Role = user.RoleRecord.Name
            });
        }
        return usersProfiles;
    }
}