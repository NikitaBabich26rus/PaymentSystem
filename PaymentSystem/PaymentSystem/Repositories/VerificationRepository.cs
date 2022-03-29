using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class VerificationRepository: IVerificationRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;

    public VerificationRepository(PaymentSystemContext paymentSystemContext)
    {
        _paymentSystemContext = paymentSystemContext;
    }

    public async Task VerifyUserAsync(int userId, string passportData)
    {
        var verification = new VerificationTransferRecord()
        {
            UserId = userId,
            PassportData = passportData,
            CreatedAt = DateTime.UtcNow,
        };
        
        await _paymentSystemContext.VerificationTransfers.AddAsync(verification);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async ValueTask<VerificationTransferRecord?> GetUserVerificationAsync(int userId)
        => await _paymentSystemContext.VerificationTransfers
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.UserId == userId);

}