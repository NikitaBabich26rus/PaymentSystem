using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;

    public AccountRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext = paymentSystemContext;
    }

    public async ValueTask<UserRecord?> GetUserByEmailAsync(string email)
        => await _paymentSystemContext.Users.SingleOrDefaultAsync(user => user.Email == email);

    public async ValueTask<UserRecord?> GetUserByIdAsync(int userId)
        => await _paymentSystemContext.Users.SingleOrDefaultAsync(user => user.Id == userId);

    public async Task<int> CreateUserAsync(UserRecord newUserRecord)
    {
        await _paymentSystemContext.Users.AddAsync(newUserRecord);
        await _paymentSystemContext.SaveChangesAsync();
        var userId = newUserRecord.Id;

        var userRole = new UserRoleRecord()
        {
            UserId = userId,
            RoleId = 1
        };
        await _paymentSystemContext.AddAsync(userRole);
        
        var balanceRecord = new BalanceRecord()
        {
            UserId = userId,
        };
        await _paymentSystemContext.Balances.AddAsync(balanceRecord);
        
        await _paymentSystemContext.SaveChangesAsync();
        return userId;
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

    public IQueryable<UserProfileModel> GetUsersProfilesAsync()
        => from usersRoles in _paymentSystemContext.UserRoles
                .Include(ur => ur.RoleRecord)
                .Include(ur => ur.UserRecord)
            join balances in _paymentSystemContext.Balances
                on usersRoles.UserRecord.Id equals balances.UserId into userProfiles
            from subBalances in userProfiles.DefaultIfEmpty()
            select new UserProfileModel()
            {
                Id = usersRoles.UserRecord.Id,
                FirstName = usersRoles.UserRecord.FirstName,
                LastName = usersRoles.UserRecord.LastName,
                Email = usersRoles.UserRecord.Email,
                RegisteredAt = usersRoles.UserRecord.RegisteredAt,
                IsVerified = usersRoles.UserRecord.IsVerified,
                IsBlocked = usersRoles.UserRecord.IsBlocked,
                Role = usersRoles.RoleRecord.Name,
                Balance = subBalances.Amount,
            };
}