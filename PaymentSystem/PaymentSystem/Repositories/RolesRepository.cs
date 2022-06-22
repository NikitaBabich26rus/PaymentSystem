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

    public async ValueTask<string> GetUserRoleAsync(int userId)
    {
        var userRole = await _paymentSystemContext.UserRoles
            .SingleOrDefaultAsync(x => x.UserId == userId);

        if (userRole == null)
        {
            throw new NullReferenceException($"No role was found for the user.");
        }
        
        var role = await _paymentSystemContext.Roles
            .SingleOrDefaultAsync(x => x.Id == userRole.RoleId);

        if (role == null)
        {
            throw new NullReferenceException($"No role was found for the user.");
        }
        
        return role.Name;
    }

    public async Task UpdateUserRoleAsync(int userId, string roleName)
    {
        var roles = await GetRolesAsync().ToListAsync();
        var userRoleRecord = await _paymentSystemContext.UserRoles
            .Include(x => x.RoleRecord)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (userRoleRecord == null)
        {
            throw new ArgumentException("User role not found for update.");
        }
        
        if (userRoleRecord.RoleRecord.Name != roleName)
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

    public IQueryable<RoleRecord> GetRolesAsync()
        => _paymentSystemContext.Roles;
    
}