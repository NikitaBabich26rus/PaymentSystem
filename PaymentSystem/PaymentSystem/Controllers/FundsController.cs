using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Repositories;

namespace PaymentSystem.Controllers;

public class FundsController: Controller
{
    private readonly IFundsRepository _fundsRepository;

    public FundsController(IFundsRepository fundsRepository)
    {
        _fundsRepository = fundsRepository;
    }

    [HttpGet]
    [Authorize(Policy = Roles.UserRole)]
    public IActionResult CreateDeposit()
    {
        return View("CreateDeposit");
    } 
    
    [HttpPost]
    [Authorize(Roles = Roles.UserRole)]
    public async Task<IActionResult> CreateDeposit(CardModel card)
    {
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            return View("CreateDeposit");
        }
        
        var userId = GetUserId();
        await _fundsRepository.CreateDepositAsync(card, userId, userId);
        
        return Redirect("/");
    }
    
    [HttpGet]
    [Authorize(Policy = Roles.UserRole)]
    public IActionResult CreateWithdrawal()
    {
        return View("CreateWithdrawal");
    } 
    
    [HttpPost]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> CreateWithdrawal(CardModel card)
    {
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            return View("CreateWithdrawal");
        }

        var userId = GetUserId();
        await _fundsRepository.CreateWithdrawalAsync(card, userId, userId);
        
        return Redirect("/");
    }

    [HttpGet]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> UserFundTransfers()
    {
        var userId = GetUserId();
        var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(userId);
        return View("UserFundTransfers", fundTransfers);
    }

    [HttpGet]
    [Authorize(Policy = Roles.FundsManagerRole)]
    public async Task<IActionResult> UncheckedFundTransfers()
    {
        var fundTransfers = await _fundsRepository.GetUncheckedFundTransfers();
        return View("UncheckedFundTransfers", fundTransfers);
    }
    
    [HttpGet]
    [Authorize(Roles = $"{Roles.FundsManagerRole}, {Roles.AdminRole}")]
    public async Task<IActionResult> AcceptedFundTransfers()
    {
        var fundTransfers = await _fundsRepository.GetAcceptedFundTransfers();
        return View("AcceptedFundTransfers", fundTransfers);
    }
    
    [HttpGet]
    [Authorize(Policy = Roles.FundsManagerRole)]
    public async Task<IActionResult> AcceptFundTransfer(int fundTransferId)
    {
        var fundManagerId = GetUserId();
        await _fundsRepository.AcceptFundTransfer(fundTransferId, fundManagerId);
        return Redirect("/Funds/UnverifiedFundTransfers");
    }
    
    [HttpGet]
    [Authorize(Roles = $"{Roles.FundsManagerRole}, {Roles.UserRole}")]
    public async Task<IActionResult> RejectFundTransfer(int fundTransferId)
    {
        await _fundsRepository.DeleteFundTransferAsync(fundTransferId);
        var userRole = GetUserRole();
        
        if (userRole == Roles.FundsManagerRole)
        {
            return Redirect("/Funds/UnverifiedFundTransfers");   
        }
        
        return Redirect("/Funds/UserFundTransfers");
    }

    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }

    private string GetUserRole()
        => HttpContext.User.FindFirst(ClaimTypes.Role)?.Value!;
    
}