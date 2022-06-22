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

    public async ValueTask<BalanceRecord> GetUserBalanceAsync(int userId)
    {
        var userBalance  = await _paymentSystemContext.Balances
            .SingleOrDefaultAsync(x => x.UserId == userId);

        if (userBalance == null)
        {
            throw new ArgumentException($"Not listed balance for userId: {userId}");
        }

        return userBalance;
    }
    

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