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
    
    public async Task<UserRecord?> GetUserByEmail(string email)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<UserRecord?> GetUserById(int userId)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

    public async Task<int> CreateUser(UserRecord newUserRecord)
    {
        await _paymentSystemContext.Users.AddAsync(newUserRecord);
        await _paymentSystemContext.SaveChangesAsync();
        return newUserRecord.Id;
    }

    public async Task DeleteUser(UserRecord userRecord)
    {
        _paymentSystemContext.Users.Remove(userRecord);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task UpdateUser(UserRecord userRecord)
    {
        _paymentSystemContext.Entry(userRecord).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }
}