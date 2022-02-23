using PaymentSystem.Data;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class RolesService
{
    private readonly IRolesRepository _rolesRepository;
    
    public RolesService(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }


    public async Task AddUserRole(int userId, int roleId)
    {
        var userRole = new UserRoleRecord()
        {
            UserId = userId,
            RoleId = roleId
        };
        
        await _rolesRepository.AddUserRolesAsync(userRole);
    }
    
}