using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public interface IFundsRepository
{
    Task CreateDepositAsync(CardModel card, int createdByUserId, int createdToUserId);

    IQueryable<FundTransferRecord> GetFundTransfersOfUser(int userId);

    Task CreateWithdrawalAsync(CardModel card, int createdByUserId, int createdToUserId);

    IQueryable<FundTransferRecord> GetUncheckedFundTransfers();

    IQueryable<FundTransferRecord> GetAcceptedFundTransfers();

    Task AcceptFundTransfer(int fundTransferId, int fundManagerId);

    ValueTask<FundTransferRecord?> GetFundTransferAsync(int fundTransferId);

    Task DeleteFundTransferAsync(int fundTransferId);
}