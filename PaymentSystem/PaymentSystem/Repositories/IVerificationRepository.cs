using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IVerificationRepository
{
    Task SendVerificationRequestAsync(int userId, string passportData);

    ValueTask<VerificationRecord?> GetVerificationRequestByUserIdAsync(int userId);

    ValueTask<List<VerificationRecord>> GetVerificationRequestsAsync();

    IQueryable<VerificationRecord> GetAcceptedRequestsForVerificationAsync();

    Task AcceptUserVerificationAsync(int verificationId, int kycManagerId);

    Task RejectUserVerificationRequestAsync(int verificationId);
}