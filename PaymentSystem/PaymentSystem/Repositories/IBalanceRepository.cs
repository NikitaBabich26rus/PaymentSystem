using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IBalanceRepository
{
    Task CreateBalanceForUserAsync(BalanceRecord balanceRecord);

    Task<decimal> GetUserBalance(int userId);
}