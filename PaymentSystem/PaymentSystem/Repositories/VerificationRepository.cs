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
        var verification = new VerificationRecord()
        {
            UserId = userId,
            PassportData = passportData,
            CreatedAt = DateTime.UtcNow,
        };
        
        await _paymentSystemContext.VerificationTransfers.AddAsync(verification);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async ValueTask<VerificationRecord?> GetVerificationByUserIdAsync(int userId)
        => await _paymentSystemContext.VerificationTransfers
            .Include(v => v.User)
            .FirstOrDefaultAsync(v => v.UserId == userId);
    
    public async ValueTask<List<VerificationRecord>> GetVerifyUsersAsync()
        => await _paymentSystemContext.VerificationTransfers
            .Where(v => v.ConfirmedBy == null)
            .Include(v => v.User)
            .ToListAsync();

    public async Task AcceptUserVerificationAsync(int verificationId, int kycManagerId)
    {
        var verification = await GetVerificationByIdAsync(verificationId);

        if (verification == null)
        {
            throw new ArgumentException("Verification not found by id.");
        }

        var user = await _accountRepository.GetUserByIdAsync(verification.UserId);

        if (user == null)
        {
            throw new ArgumentException("Verification user not found.");
        }
        
        verification.ConfirmedBy = kycManagerId;
        verification.ConfirmedAt = DateTime.UtcNow;
        user.IsVerified = true;
        
        _paymentSystemContext.Entry(user).State = EntityState.Modified;
        _paymentSystemContext.Entry(verification).State = EntityState.Modified;
        await _paymentSystemContext.SaveChangesAsync();
    }

    public async Task RejectUserVerificationAsync(int verificationId)
    {
        var verification = await GetVerificationByIdAsync(verificationId);
        
        if (verification == null)
        {
            throw new ArgumentException("Verification not found by id.");
        }
        
        _paymentSystemContext.VerificationTransfers.Remove(verification);
        await _paymentSystemContext.SaveChangesAsync();
    }

    public IQueryable<VerificationRecord> GetVerifiedUsers()
        => _paymentSystemContext.VerificationTransfers
            .Where(v => v.ConfirmedBy != null)
            .Include(v => v.User)
            .Include(v => v.ConfirmedByUser);
    
    private async ValueTask<VerificationRecord?> GetVerificationByIdAsync(int id)
        => await _paymentSystemContext.VerificationTransfers
            .SingleOrDefaultAsync(v => v.Id == id);
}