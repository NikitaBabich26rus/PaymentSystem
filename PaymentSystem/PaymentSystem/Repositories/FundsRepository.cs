using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public class FundsRepository: IFundsRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    private readonly IBalanceRepository _balanceRepository;
    private readonly Semaphore _fundsSemaphore = new(1, 1);

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
        var userBalance = await _balanceRepository.GetUserBalanceAsync(createdToUserId);

        if (userBalance.Amount < card.AmountOfMoney)
        {
            throw new ArgumentException("Not enough money to withdraw.");
        }
        
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
    
    public IQueryable<FundTransferRecord> GetFundTransfersOfUser(int userId) 
        => _paymentSystemContext.FundTransfers
            .Where(f => f.UserId == userId)
            .Include(f => f.ConfirmedByUserRecord)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .OrderBy(f => f.CreatedAt);

    public IQueryable<FundTransferRecord> GetUncheckedFundTransfers()
        => _paymentSystemContext.FundTransfers
            .Where(f => f.ConfirmedBy == null)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .OrderBy(f => f.CreatedAt);

    public IQueryable<FundTransferRecord> GetAcceptedFundTransfers()
        => _paymentSystemContext.FundTransfers
            .Where(f => f.ConfirmedBy != null)
            .Include(f => f.UserRecord)
            .Include(f => f.CreatedByUserRecord)
            .Include(f => f.ConfirmedByUserRecord)
            .OrderBy(f => f.CreatedAt);
    
    public async Task AcceptFundTransfer(int fundTransferId, int fundManagerId)
    {
        _fundsSemaphore.WaitOne();
        
        var fundTransfer = await GetFundTransferAsync(fundTransferId);

        if (fundTransfer == null ||
            fundTransfer.ConfirmedBy != null)
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

        _fundsSemaphore.Release();
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