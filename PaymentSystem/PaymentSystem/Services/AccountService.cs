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

    public async Task<UserProfile> GetUserProfile(int userId)
    {
        var user = await _accountRepository.GetUserByIdAsync(userId);
        var userRoles = await _rolesService.GetUserRoleAsync(userId);
        var userBalance = await _balanceService.GetUserBalance(userId);

        var userProfile = new UserProfile()
        {
            FirstName = user!.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            RegisteredAt = user.RegisteredAt,
            IsVerified = user.IsVerified,
            Balance = userBalance,
            Roles = userRoles
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

    public async Task UpdateUserAsync(UserRecord userRecord)
        => await _accountRepository.UpdateUserAsync(userRecord);
}