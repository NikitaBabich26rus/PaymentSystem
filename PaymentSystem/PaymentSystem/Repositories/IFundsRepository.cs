using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public interface IFundsRepository
{
    Task CreateDepositAsync(CardModel card, int createdByUserId, int createdToUserId);

    ValueTask<List<FundTransferRecord>> GetFundTransfersOfUser(int userId);

    Task CreateWithdrawalAsync(CardModel card, int createdByUserId, int createdToUserId);

    ValueTask<List<FundTransferRecord>> GetUnverifiedFundTransfers();

    ValueTask<List<FundTransferRecord>> GetVerifiedFundTransfers();

    Task AcceptFundTransfer(int fundTransferId, int fundManagerId);

    ValueTask<FundTransferRecord?> GetFundTransferAsync(int fundTransferId);

    Task DeleteFundTransferAsync(int fundTransferId);
}