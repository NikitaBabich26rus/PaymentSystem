using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IVerificationRepository
{
    Task VerifyUserAsync(int userId, string passportData);

    ValueTask<VerificationTransferRecord?> GetVerificationByUserIdAsync(int userId);

    ValueTask<List<VerificationTransferRecord>> GetVerifyUsersAsync();

    ValueTask<List<VerificationTransferRecord>> GetVerifiedUsers();

    Task AcceptUserVerificationAsync(int verificationId, int kycManagerId);

    Task RejectUserVerificationAsync(int verificationId);
}