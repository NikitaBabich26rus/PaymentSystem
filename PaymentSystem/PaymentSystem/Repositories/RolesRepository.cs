using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class RolesRepository: IRolesRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    
    public RolesRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext = paymentSystemContext;
    }
    
    public async Task AddUserRolesAsync(UserRoleRecord userRoleRecord)
    {
        await _paymentSystemContext.AddAsync(userRoleRecord);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task<List<string>> GetUserRolesAsync(int userId)
        => await _paymentSystemContext.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleRecord.Name)
            .ToListAsync();
}