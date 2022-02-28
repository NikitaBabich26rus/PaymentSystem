using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IRolesRepository
{
    Task AddUserRolesAsync(UserRoleRecord userRoleRecord);

    Task<List<string>> GetUserRolesAsync(int userId);
}