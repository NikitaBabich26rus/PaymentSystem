using PaymentSystem.Data;
using PaymentSystem.Models;

namespace PaymentSystem.Repositories;

public interface IFundsRepository
{
    Task CreateDepositAsync(CardModel card, int createdByUserId, int createdToUserId);

    ValueTask<List<FundTransferRecord>> GetFundTransfersOfUser(int userId);

    Task CreateWithdrawalAsync(CardModel card, int createdByUserId, int createdToUserId);

    ValueTask<List<FundTransferRecord>> GetUncheckedFundTransfers();

    ValueTask<List<FundTransferRecord>> GetAcceptedFundTransfers();

    Task AcceptFundTransfer(int fundTransferId, int fundManagerId);

    ValueTask<FundTransferRecord?> GetFundTransferAsync(int fundTransferId);

    Task DeleteFundTransferAsync(int fundTransferId);
}