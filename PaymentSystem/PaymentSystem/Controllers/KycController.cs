using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class KycController: Controller
{
    private readonly AccountService _accountService;
    private readonly IVerificationRepository _verificationRepository;
    
    public KycController(AccountService accountService,
        IVerificationRepository verificationRepository)
    {
        _accountService = accountService;
        _verificationRepository = verificationRepository;
    }

    [HttpGet]
    [Authorize(Policy = "KYC-Manager")]
    public async Task<IActionResult> VerifyUsers()
    {
        var verifications = await _verificationRepository.GetVerifyUsersAsync();
        return View("VerifyUsers", verifications);
    }

    [HttpGet]
    [Authorize(Policy = "KYC-Manager")]
    public async Task<IActionResult> AcceptUserVerification(int verificationId)
    {
        var kycManagerId = GetKycManagerId();
        await _verificationRepository.AcceptUserVerificationAsync(verificationId, kycManagerId);
        return Redirect("/Kyc/VerifyUsers");
    }
    
    [HttpGet]
    [Authorize(Policy = "KYC-Manager")]
    public async Task<IActionResult> RejectUserVerification(int verificationId)
    {   
        await _verificationRepository.RejectUserVerificationAsync(verificationId);
        return Redirect("/Kyc/VerifyUsers");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, KYC-Manager")]
    public async Task<IActionResult> VerifiedUsers()
    {
        var verifications = await _verificationRepository.GetVerifiedUsers();
        return View("VerifiedUsers", verifications);
    }
    
    private int GetKycManagerId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}