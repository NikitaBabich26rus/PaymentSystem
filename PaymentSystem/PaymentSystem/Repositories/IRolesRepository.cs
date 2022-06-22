using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IRolesRepository
{
    Task AddUserRolesAsync(UserRoleRecord userRoleRecord);

    ValueTask<string> GetUserRoleAsync(int userId);

    IQueryable<RoleRecord> GetRolesAsync();

    Task UpdateUserRoleAsync(int userId, string roleName);
}