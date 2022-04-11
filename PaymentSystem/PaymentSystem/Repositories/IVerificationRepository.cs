using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IVerificationRepository
{
    Task VerifyUserAsync(int userId, string passportData);

    ValueTask<VerificationRecord?> GetVerificationByUserIdAsync(int userId);

    ValueTask<List<VerificationRecord>> GetVerifyUsersAsync();

    IQueryable<VerificationRecord> GetVerifiedUsers();

    Task AcceptUserVerificationAsync(int verificationId, int kycManagerId);

    Task RejectUserVerificationAsync(int verificationId);
}