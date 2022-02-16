using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class AuthService
{
    private readonly IAuthRepository _authRepository;  
    
    public AuthService(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<User?> GetUserById(int userId)
        => await _authRepository.GetUserById(userId);

    public async Task<User?> GetUserByEmail(string email)
        => await _authRepository.GetUserByEmail(email);
    
    public async Task<int> CreateUser(RegisterModel registerModel)
    {
        var newUser = new User()
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Email = registerModel.Email,
            Password = registerModel.Password,
            RegisteredAt = DateTime.Now
        };
        
        return await _authRepository.CreateUser(newUser);
    }
    
    public async Task DeleteUser(User user)
        => await _authRepository.DeleteUser(user);

    public async Task UpdateUser(User user)
        => await _authRepository.UpdateUser(user);
}