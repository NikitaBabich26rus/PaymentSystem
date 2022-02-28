using PaymentSystem.Data;
using PaymentSystem.Repositories;

namespace PaymentSystem.Services;

public class BalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    
    public BalanceService(IBalanceRepository balanceRepository)
    {
        _balanceRepository = balanceRepository;
    }

    public async Task<decimal> GetUserBalance(int userId)
        => await _balanceRepository.GetUserBalance(userId);

        public async Task CreateBalanceForUserAsync(int userId)
    {
        var balanceRecord = new BalanceRecord()
        {
            UserId = userId
        };
        
        await _balanceRepository.CreateBalanceForUserAsync(balanceRecord);
    }
}