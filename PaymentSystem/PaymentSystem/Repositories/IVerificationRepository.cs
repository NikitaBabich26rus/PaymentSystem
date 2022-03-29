using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public interface IVerificationRepository
{
    Task VerifyUserAsync(int userId, string passportData);

    ValueTask<VerificationTransferRecord?> GetUserVerificationAsync(int userId);
}