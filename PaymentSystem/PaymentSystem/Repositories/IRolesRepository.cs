using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IRolesRepository
{
    Task AddUserRolesAsync(UserRoleRecord userRoleRecord);

    ValueTask<string> GetUserRolesAsync(int userId);

    ValueTask<List<RoleRecord>> GetRolesAsync();

    Task UpdateUserRoleAsync(string roleName, int userId);
}