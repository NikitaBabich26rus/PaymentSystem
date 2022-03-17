using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IBalanceRepository
{
    Task CreateBalanceForUserAsync(BalanceRecord balanceRecord);

    ValueTask<decimal> GetUserBalanceAsync(int userId);
}