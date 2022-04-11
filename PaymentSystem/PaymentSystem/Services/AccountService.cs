using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IRolesRepository _rolesRepository;
    private readonly IBalanceRepository _balanceRepository;
    private readonly IVerificationRepository _verificationRepository;

    public AccountService(
        IAccountRepository accountRepository,
        IRolesRepository rolesRepository,
        IBalanceRepository balanceRepository,
        IVerificationRepository verificationRepository)
    {
        _accountRepository = accountRepository;
        _rolesRepository = rolesRepository;
        _balanceRepository = balanceRepository;
        _verificationRepository = verificationRepository;
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

    public async Task UpdateUserAsync(UpdateProfileModel updateProfileModel, UserRecord user)
    {
        user.Email = updateProfileModel.Email;
        user.Password = updateProfileModel.NewPassword;
        user.FirstName = updateProfileModel.FirstName;
        user.LastName = updateProfileModel.LastName;
        await _accountRepository.UpdateUserAsync(user);
    }

    public async Task UpdateUserByAdminAsync(UpdateUserProfileModel updateUser, int userId)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);
        user!.Email = updateUser.Email;
        user.FirstName = updateUser.FirstName;
        user.LastName = updateUser.LastName;
        user.IsBlocked = updateUser.Status != "Active";
        await _rolesRepository.UpdateUserRoleAsync(updateUser.Role, userId);
        await _accountRepository.UpdateUserAsync(user);
    }

    public async Task VerifyUserAsync(int userId, string passportData)
        => await _verificationRepository.VerifyUserAsync(userId, passportData);

    public async ValueTask<VerificationRecord?> GetUserVerificationAsync(int userId)
        => await _verificationRepository.GetVerificationByUserIdAsync(userId);

    public async Task<List<UserProfileModel>> GetUsersProfiles()
    {
        var users = await _accountRepository.GetUsersAsync();
        var usersProfiles = new List<UserProfileModel>();

        foreach (var user in users)
        {
            if (user.RoleRecord.Name == "Admin")
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