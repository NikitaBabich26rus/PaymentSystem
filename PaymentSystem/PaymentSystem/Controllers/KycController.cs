using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Repositories;

namespace PaymentSystem.Controllers;

public class KycController: Controller
{
    private readonly IVerificationRepository _verificationRepository;
    
    public KycController(IVerificationRepository verificationRepository)
    {
        _verificationRepository = verificationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> VerifyUsers()
    {
        var verifications = await _verificationRepository.GetVerifyUsersAsync();
        return View("VerifyUsers", verifications);
    }

    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> AcceptUserVerification(int verificationId)
    {
        var kycManagerId = GetKycManagerId();
        await _verificationRepository.AcceptUserVerificationAsync(verificationId, kycManagerId);
        return Redirect("/Kyc/VerifyUsers");
    }
    
    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> RejectUserVerification(int verificationId)
    {   
        await _verificationRepository.RejectUserVerificationAsync(verificationId);
        return Redirect("/Kyc/VerifyUsers");
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.AdminRole}, {Roles.KycManagerRole}")]
    public async Task<IActionResult> VerifiedUsers()
    {
        var verifications = await _verificationRepository.GetVerifiedUsers().ToListAsync();
        return View("VerifiedUsers", verifications);
    }
    
    private int GetKycManagerId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}