using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IBalanceRepository
{
    Task CreateBalanceForUserAsync(BalanceRecord balanceRecord);

    ValueTask<BalanceRecord> GetUserBalanceAsync(int userId);
}