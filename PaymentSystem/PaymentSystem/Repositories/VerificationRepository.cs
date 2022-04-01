using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;

namespace PaymentSystem.Repositories;

public class VerificationRepository: IVerificationRepository
{
    private readonly PaymentSystemContext _paymentSystemContext;
    private readonly IAccountRepository _accountRepository;

    public VerificationRepository(
        PaymentSystemContext paymentSystemContext,
        IAccountRepository accountRepository)
    {
        _paymentSystemContext = paymentSystemContext;
        _accountRepository = accountRepository;
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

    public async ValueTask<VerificationTransferRecord?> GetVerificationByUserIdAsync(int userId)
        => await _paymentSystemContext.VerificationTransfers
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.UserId == userId);
    
    public async ValueTask<List<VerificationTransferRecord>> GetVerifyUsersAsync()
        => await _paymentSystemContext.VerificationTransfers
            .Where(v => v.ConfirmedBy == null)
            .Include(v => v.User)
            .ToListAsync();

    public async Task AcceptUserVerificationAsync(int verificationId, int kycManagerId)
    {
        var verification = await GetVerificationByIdAsync(verificationId);
        verification!.ConfirmedBy = kycManagerId;
        verification.ConfirmedAt = DateTime.UtcNow;
        
        var user = await _accountRepository.GetUserByIdAsync(verification.UserId);
        user!.IsVerified = true;
        
        _paymentSystemContext.Entry(user).State = EntityState.Modified;
        _paymentSystemContext.Entry(verification).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task RejectUserVerificationAsync(int verificationId)
    {
        var verification = await GetVerificationByIdAsync(verificationId);
        _paymentSystemContext.VerificationTransfers.Remove(verification!);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async ValueTask<List<VerificationTransferRecord>> GetVerifiedUsers()
        => await _paymentSystemContext.VerificationTransfers
            .Where(v => v.ConfirmedBy != null)
            .Include(v => v.User)
            .Include(v => v.ConfirmedByUser)
            .ToListAsync();
    private async ValueTask<VerificationTransferRecord?> GetVerificationByIdAsync(int id)
        => await _paymentSystemContext.VerificationTransfers
            .FirstOrDefaultAsync(v => v.Id == id);
}