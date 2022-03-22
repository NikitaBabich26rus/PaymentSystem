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

    public async ValueTask<string> GetUserRoleAsync(int userId)
        => await _rolesRepository.GetUserRolesAsync(userId);

    public async ValueTask<List<RoleRecord>> GetRolesAsync()
        => await _rolesRepository.GetRolesAsync();
    
    public async Task AddUserRoleAsync(int userId, int roleId)
    {
        var userRole = new UserRoleRecord()
        {
            UserId = userId,
            RoleId = roleId
        };
        
        await _rolesRepository.AddUserRolesAsync(userRole);
    }

    public async Task UpdateUserRoleAsync(string roleName, int userId)
        => await _rolesRepository.UpdateUserRoleAsync(roleName, userId);

}