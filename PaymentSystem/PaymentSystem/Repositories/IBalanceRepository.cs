using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IBalanceRepository
{
    Task CreateBalanceForUserAsync(int userId);

    ValueTask<BalanceRecord> GetUserBalanceAsync(int userId);
}