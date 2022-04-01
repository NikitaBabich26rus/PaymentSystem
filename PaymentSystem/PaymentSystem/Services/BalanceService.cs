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

    public async ValueTask<BalanceRecord> GetUserBalanceAsync(int userId)
        => await _balanceRepository.GetUserBalanceAsync(userId);
    
    public async Task CreateBalanceForUserAsync(int userId)
    {
        var balanceRecord = new BalanceRecord()
        {
            UserId = userId
        };
        
        await _balanceRepository.CreateBalanceForUserAsync(balanceRecord);
    }
}