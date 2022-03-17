using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;

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

    public async Task<string> GetUserRolesAsync(int userId)
    {
        var userRole = await _paymentSystemContext.UserRoles
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var role = await _paymentSystemContext.Roles
            .FirstOrDefaultAsync(x => x.Id == userRole!.RoleId);
        return role!.Name;
    }
    
}