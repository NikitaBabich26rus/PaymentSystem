using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class KycController: Controller
{
    private readonly IVerificationRepository _verificationRepository;
    private readonly AccountService _accountService;
    
    public KycController(
        IVerificationRepository verificationRepository,
        AccountService accountService)
    {
        _accountService = accountService;
        _verificationRepository = verificationRepository;
    }

    [HttpGet]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> Verification()
    {
        var userId = GetUserId();
        var userVerification = await _verificationRepository.GetVerificationRequestByUserIdAsync(userId);

        return View("Verification", userVerification);
    }

    [HttpPost]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> SendVerificationRequest(string passportData)
    {
        var userId = GetUserId();
        if (!Int64.TryParse(passportData, out _))
        {
            var userVerification = await _verificationRepository.GetVerificationRequestByUserIdAsync(userId);
            ViewBag.Error = "Incorrect passport data.";
            return View("Verification", userVerification);
        }
        
        await _verificationRepository.SendVerificationRequestAsync(userId, passportData);
        return Redirect("/");
    }
    
    
    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> GetVerificationRequests()
    {
        var verifications = await _verificationRepository.GetVerificationRequestsAsync();
        return View("GetVerificationRequests", verifications);
    }

    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> AcceptUserVerificationRequest(int verificationId)
    {
        var kycManagerId = GetUserId();
        await _verificationRepository.AcceptUserVerificationAsync(verificationId, kycManagerId);
        return Redirect("/Kyc/VerifyUsers");
    }
    
    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> RejectUserVerificationRequest(int verificationId)
    {   
        await _verificationRepository.RejectUserVerificationRequestAsync(verificationId);
        return Redirect("/Kyc/VerifyUsers");
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.AdminRole}, {Roles.KycManagerRole}")]
    public async Task<IActionResult> GetAcceptedRequestsForVerification()
    {
        var verifications = await _verificationRepository
            .GetAcceptedRequestsForVerificationAsync()
            .ToListAsync();
        
        return View("GetAcceptedRequestsForVerification", verifications);
    }
    
    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}