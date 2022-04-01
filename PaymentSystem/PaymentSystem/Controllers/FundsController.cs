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
    [Authorize(Policy = "User")]
    public IActionResult CreateDeposit()
    {
        return View("CreateDeposit");
    } 
    
    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CreateDeposit(CardModel card)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Validation error";
            return View("CreateDeposit");
        }

        if (!IsCardValid(card))
        {
            ViewBag.Error = "Validation error";
            return View("CreateDeposit");
        }

        var userId = GetUserId();
        await _fundsRepository.CreateDepositAsync(card, userId, userId);
        
        return Redirect("/");
    }
    
    [HttpGet]
    [Authorize(Policy = "User")]
    public IActionResult CreateWithdrawal()
    {
        return View("CreateWithdrawal");
    } 
    
    [HttpPost]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> CreateWithdrawal(CardModel card)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Validation error";
            return View("CreateWithdrawal");
        }

        if (!IsCardValid(card))
        {
            ViewBag.Error = "Validation error";
            return View("CreateWithdrawal");
        }

        var userId = GetUserId();
        await _fundsRepository.CreateWithdrawalAsync(card, userId, userId);
        
        return Redirect("/");
    }

    [HttpGet]
    [Authorize(Policy = "User")]
    public async Task<IActionResult> UserFundTransfers()
    {
        var userId = GetUserId();
        var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(userId);
        return View("UserFundTransfers", fundTransfers);
    }

    [HttpGet]
    [Authorize(Policy = "Funds-Manager")]
    public async Task<IActionResult> UnverifiedFundTransfers()
    {
        var fundTransfers = await _fundsRepository.GetUnverifiedFundTransfers();
        return View("UnverifiedFundTransfers", fundTransfers);
    }
    
    [HttpGet]
    [Authorize(Roles = "Funds-Manager, Admin")]
    public async Task<IActionResult> VerifiedFundTransfers()
    {
        var fundTransfers = await _fundsRepository.GetVerifiedFundTransfers();
        return View("VerifiedFundTransfers", fundTransfers);
    }
    
    [HttpGet]
    [Authorize(Policy = "Funds-Manager")]
    public async Task<IActionResult> AcceptFundTransfer(int fundTransferId)
    {
        var fundManagerId = GetUserId();
        await _fundsRepository.AcceptFundTransfer(fundTransferId, fundManagerId);
        return Redirect("/Funds/UnverifiedFundTransfers");
    }
    
    [HttpGet]
    [Authorize(Roles = "Funds-Manager, User")]
    public async Task<IActionResult> RejectFundTransfer(int fundTransferId)
    {
        await _fundsRepository.DeleteFundTransferAsync(fundTransferId);
        
        var userRole = GetUserRole();
        if (userRole == "Funds-Manager")
        {
            return Redirect("/Funds/UnverifiedFundTransfers");   
        }
        
        return Redirect("/Funds/UserFundTransfers");
    }
    
    private bool IsCardValid(CardModel card)
    {
        var isValidCsv = int.TryParse(card.CardCvc, out var csv);
        var isValidCardNumber = long.TryParse(card.CardNumber, out var cardNumber);
        var isValidCardDate = int.TryParse(card.CardDate, out var cardDate);
        var isValidAmountOfMoney = decimal.TryParse(card.CardNumber, out var amountOfMoney);

        return isValidCsv && isValidCardDate && isValidCardNumber && isValidAmountOfMoney;
    }
    
    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }

    private string GetUserRole()
        => HttpContext.User.FindFirst(ClaimTypes.Role)?.Value!;
    
}