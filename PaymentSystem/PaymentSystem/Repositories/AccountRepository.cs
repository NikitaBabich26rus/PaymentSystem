using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;

    public AccountRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext = paymentSystemContext;
    }

    public async Task<UserRecord?> GetUserByEmailAsync(string email)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<UserRecord?> GetUserByIdAsync(int userId)
        => await _paymentSystemContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

    public async Task<int> CreateUserAsync(UserRecord newUserRecord)
    {
        await _paymentSystemContext.Users.AddAsync(newUserRecord);
        await _paymentSystemContext.SaveChangesAsync();
        return newUserRecord.Id;
    }

    public async Task DeleteUserAsync(UserRecord userRecord)
    {
        _paymentSystemContext.Users.Remove(userRecord);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(UserRecord userRecord)
    {
        _paymentSystemContext.Entry(userRecord).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }

    public Task<IEnumerable<UserRoleRecord>> GetUsersAsync()
    {
        var users = _paymentSystemContext.UserRoles
            .Include(ur => ur.RoleRecord)
            .Include(ur => ur.UserRecord)
            .AsEnumerable();
        
        return Task.FromResult(users);
    }
}