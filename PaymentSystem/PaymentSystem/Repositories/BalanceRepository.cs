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
        => await _paymentSystemContext.Balances.FirstOrDefaultAsync(x => x.UserId == userId);
        

        public async Task CreateBalanceForUserAsync(BalanceRecord balanceRecord)
    {
        await _paymentSystemContext.Balances.AddAsync(balanceRecord);
        await _paymentSystemContext.SaveChangesAsync();
    }
}