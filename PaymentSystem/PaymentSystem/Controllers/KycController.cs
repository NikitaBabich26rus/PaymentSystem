using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Models;
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
    [Authorize]
    public async Task<IActionResult> VerificationRequest()
    {
        try
        {
            var userId = GetUserId();
            var userVerification = await _verificationRepository.GetVerificationRequestByUserIdAsync(userId);

            return View("VerificationRequest", userVerification);
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendVerificationRequest(string passportData)
    {
        var userId = GetUserId();
        
        if (!Int64.TryParse(passportData, out _))
        {
            ViewBag.Error = "Incorrect passport data.";
            return View("VerificationRequest", null);
        }

        try
        {
            await _verificationRepository.SendVerificationRequestAsync(userId, passportData);
            return Redirect("/");   
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }
    
    
    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> GetVerificationRequests()
    {
        try
        {
            var verifications = await _verificationRepository.GetVerificationRequestsAsync().ToListAsync();
            return View("GetVerificationRequests", verifications);    
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }

    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> AcceptUserVerificationRequest(int verificationId)
    {
        try
        {
            var kycManagerId = GetUserId();
            await _verificationRepository.AcceptUserVerificationAsync(verificationId, kycManagerId);
            return Redirect("/Kyc/GetVerificationRequests");
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }
    
    [HttpGet]
    [Authorize(Policy = Roles.KycManagerRole)]
    public async Task<IActionResult> RejectUserVerificationRequest(int verificationId)
    {
        try
        {
            await _verificationRepository.RejectUserVerificationRequestAsync(verificationId);
            return Redirect("/Kyc/GetVerificationRequests");
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.AdminRole}, {Roles.KycManagerRole}")]
    public async Task<IActionResult> GetAcceptedRequestsForVerification()
    {
        try
        {
            var verifications = await _verificationRepository
                .GetAcceptedRequestsForVerificationAsync()
                .ToListAsync();

            return View("GetAcceptedRequestsForVerification", verifications);
        }
        catch (Exception e)
        {
            var error = new ErrorModel()
            {
                ErrorMessage = e.Message,
            };

            return View("Error", error);
        }
    }
    
    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}