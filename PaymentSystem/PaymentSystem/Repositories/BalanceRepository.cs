using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class BalanceRepository: IBalanceRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    
    public BalanceRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext = paymentSystemContext;
    }
    
    public async ValueTask<BalanceRecord?> GetUserBalanceAsync(int userId)
        => await _paymentSystemContext.Balances
            .SingleOrDefaultAsync(x => x.UserId == userId);

    public async Task CreateBalanceForUserAsync(int userId)
    {
        var balanceRecord = new BalanceRecord()
        {
            UserId = userId
        };
        
        await _paymentSystemContext.Balances.AddAsync(balanceRecord);
        await _paymentSystemContext.SaveChangesAsync();
    }
}