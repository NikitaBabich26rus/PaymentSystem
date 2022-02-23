using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class AuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly RolesService _rolesService;
    
    public AuthService(IAuthRepository authRepository, RolesService rolesService)
    {
        _authRepository = authRepository;
        _rolesService = rolesService;
    }

    public async Task<UserRecord?> GetUserById(int userId)
        => await _authRepository.GetUserById(userId);

    public async Task<UserRecord?> GetUserByEmail(string email)
        => await _authRepository.GetUserByEmail(email);
    
    public async Task<int> CreateUser(RegisterModel registerModel)
    {
        var newUser = new UserRecord()
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            Password = registerModel.Password,
            RegisteredAt = DateTime.UtcNow
        };
        
        var userId = await _authRepository.CreateUser(newUser);
        await _rolesService.AddUserRole(userId, 1);
        return userId;
    }
    
    public async Task DeleteUser(UserRecord userRecord)
        => await _authRepository.DeleteUser(userRecord);

    public async Task UpdateUser(UserRecord userRecord)
        => await _authRepository.UpdateUser(userRecord);
}