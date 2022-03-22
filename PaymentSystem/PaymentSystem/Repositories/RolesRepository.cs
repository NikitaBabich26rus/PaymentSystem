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

    public async ValueTask<string> GetUserRolesAsync(int userId)
    {
        var userRole = await _paymentSystemContext.UserRoles
            .FirstOrDefaultAsync(x => x.UserId == userId);
        var role = await _paymentSystemContext.Roles
            .FirstOrDefaultAsync(x => x.Id == userRole!.RoleId);
        return role!.Name;
    }

    public async Task UpdateUserRoleAsync(string roleName, int userId)
    {
        var roles = await GetRolesAsync();
        var userRoleRecord = await _paymentSystemContext.UserRoles
            .Include(x => x.RoleRecord)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        
        if (userRoleRecord!.RoleRecord.Name != roleName)
        {
            roles.ForEach(r =>
            {
                if (r.Name == roleName)
                {
                    userRoleRecord.RoleId = r.Id;
                }
            });
            
            await _paymentSystemContext.SaveChangesAsync();
        }
    }

    public async ValueTask<List<RoleRecord>> GetRolesAsync()
        => await _paymentSystemContext.Roles.ToListAsync();
    
}