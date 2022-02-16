using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class AuthRepository: IAuthRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    
    public AuthRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext =  paymentSystemContext;
    }
    
    // TODO это наверное должно быть в репозитории для акаунта
    public async Task<User?> GetUserByEmail(string email)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<User?> GetUserById(int userId)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

    public async Task<int> CreateUser(User newUser)
    {
        var user = await _paymentSystemContext.Users.AddAsync(newUser);
        await _paymentSystemContext.SaveChangesAsync();
        return user.Entity.Id;
    }

    public async Task DeleteUser(User user)
    {
        _paymentSystemContext.Users.Remove(user);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _paymentSystemContext.Entry(user).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }
}