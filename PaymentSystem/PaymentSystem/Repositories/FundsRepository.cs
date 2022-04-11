using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public class FundsRepository: IFundsRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    private readonly IBalanceRepository _balanceRepository;
    
    public FundsRepository(
        PaymentSystemContext paymentSystemContext,
        IBalanceRepository balanceRepository)
    {
        _paymentSystemContext = paymentSystemContext;
        _balanceRepository = balanceRepository;
    }

    public async Task CreateDepositAsync(CardModel card, int createdByUserId, int createdToUserId)
    {
        var fundTransfer = new FundTransferRecord()
        {
            UserId = createdToUserId,
            CardNumber = card.CardNumber,
            CardCvc = card.CardCvc,
            CardDate  = card.CardDate,
            AmountOfMoney = card.AmountOfMoney,
            CreatedBy = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            TransferType = TransferType.Deposit
        };

        await _paymentSystemContext.FundTransfers.AddAsync(fundTransfer);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task CreateWithdrawalAsync(CardModel card, int createdByUserId, int createdToUserId)
    {
        var fundTransfer = new FundTransferRecord()
        {
            UserId = createdToUserId,
            CardNumber = card.CardNumber,
            CardCvc = card.CardCvc,
            CardDate  = card.CardDate,
            AmountOfMoney = card.AmountOfMoney,
            CreatedBy = createdByUserId,
            CreatedAt = DateTime.UtcNow,
            TransferType = TransferType.Withdrawal
        };

        await _paymentSystemContext.FundTransfers.AddAsync(fundTransfer);
        await _paymentSystemContext.SaveChangesAsync();
    }
    
    public async ValueTask<List<FundTransferRecord>> GetFundTransfersOfUser(int userId) 
        => await _paymentSystemContext.FundTransfers
            .Where(f => f.UserId == userId)
            .Include(f => f.ConfirmedByUserRecord)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .OrderBy(f => f.CreatedAt)
            .ToListAsync();

    public async ValueTask<List<FundTransferRecord>> GetUnverifiedFundTransfers()
        => await _paymentSystemContext.FundTransfers
            .Where(f => f.ConfirmedBy == null)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .OrderBy(f => f.CreatedAt)
            .ToListAsync();

    public async ValueTask<List<FundTransferRecord>> GetVerifiedFundTransfers()
        => await _paymentSystemContext.FundTransfers
            .Where(f => f.ConfirmedBy != null)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .Include(f => f.ConfirmedByUserRecord)
            .OrderBy(f => f.CreatedAt)
            .ToListAsync();
    
    public async Task AcceptFundTransfer(int fundTransferId, int fundManagerId)
    {
        var fundTransfer = await GetFundTransferAsync(fundTransferId);
        
        if (fundTransfer == null)
        {
            return;
        }
        
        var userBalance = await _balanceRepository.GetUserBalanceAsync(fundTransfer.UserId);
        
        if (fundTransfer.TransferType == TransferType.Withdrawal
            && userBalance.Amount < fundTransfer.AmountOfMoney)
        {
            await DeleteFundTransferAsync(fundTransferId);
            return;
        }
        
        fundTransfer.ConfirmedAt = DateTime.UtcNow;
        fundTransfer.ConfirmedBy = fundManagerId;

        if (fundTransfer.TransferType == TransferType.Withdrawal)
        {
            userBalance.Amount -= fundTransfer.AmountOfMoney;
            
        }
        else
        {
            userBalance.Amount += fundTransfer.AmountOfMoney;
        }
        
        _paymentSystemContext.Entry(fundTransfer).State = EntityState.Modified;
        _paymentSystemContext.Entry(userBalance).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async ValueTask<FundTransferRecord?> GetFundTransferAsync(int fundTransferId)
        => await _paymentSystemContext.FundTransfers
            .FirstOrDefaultAsync(f => f.Id == fundTransferId);
    
    public async Task DeleteFundTransferAsync(int fundTransferId)
    {
        var fundTransfer = await GetFundTransferAsync(fundTransferId);

        if (fundTransfer == null)
        {
            return;
        }
        _paymentSystemContext.FundTransfers.Remove(fundTransfer);
        await _paymentSystemContext.SaveChangesAsync();
    }
}