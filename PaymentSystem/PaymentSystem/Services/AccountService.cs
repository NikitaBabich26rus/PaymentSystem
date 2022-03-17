using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly RolesService _rolesService;
    private readonly BalanceService _balanceService;
    
    public AccountService(
        IAccountRepository accountRepository,
        RolesService rolesService,
        BalanceService balanceService)
    {
        _accountRepository = accountRepository;
        _rolesService = rolesService;
        _balanceService = balanceService;
    }

    public async Task<UserProfileModel> GetUserProfile(int userId)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);
        var userRole = await _rolesService.GetUserRoleAsync(userId);
        var userBalance = await _balanceService.GetUserBalanceAsync(userId);

        var userProfile = new UserProfileModel()
        {
            FirstName = user!.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt,
            IsVerified = user.IsVerified,
            Balance = userBalance,
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
        await Task.WhenAll
        (
            _rolesService.AddUserRoleAsync(userId, 1),
            _balanceService.CreateBalanceForUserAsync(userId)
        );
        
        return userId;
    }
    
    public async Task DeleteUserAsync(UserRecord userRecord)
        => await _accountRepository.DeleteUserAsync(userRecord);

    public async Task UpdateUserAsync(UpdateAccountModel updateAccountModel, int userId)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);
        if (String.CompareOrdinal(user!.Password, updateAccountModel.OldPassword) != 0)
        {
            throw new ArgumentException("Password error");
        }

        user.Email = updateAccountModel.Email;
        user.Password = updateAccountModel.NewPassword;
        user.FirstName = updateAccountModel.FirstName;
        user.LastName = updateAccountModel.LastName;
        await _accountRepository.UpdateUserAsync(user);
    }

    public async Task<List<UserProfileModel>> GetUsersProfiles()
    {
        var users = await _accountRepository.GetUsersAsync();
        var usersProfiles = new List<UserProfileModel>();

        foreach (var user in users)
        {
            var userBalance = await _balanceService.GetUserBalanceAsync(user.UserRecord.Id);

            usersProfiles.Add(new UserProfileModel()
            {
                FirstName = user.UserRecord.FirstName,
                LastName = user.UserRecord.LastName,
                Email = user.UserRecord.Email,
                RegisteredAt = user.UserRecord.RegisteredAt,
                IsVerified = user.UserRecord.IsVerified,
                Balance = userBalance,
                Role = user.RoleRecord.Name
            });
        }
        return usersProfiles;
    }
}