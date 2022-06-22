using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        try
        {
            var userId = GetUserId();
            await _fundsRepository.CreateDepositAsync(card, userId, userId);

            return Redirect("/");
        }
        catch (Exception e)
        {
            ViewBag.Error = e.Message;
            return View("CreateDeposit");
        }
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

        try
        {
            var userId = GetUserId();
            await _fundsRepository.CreateWithdrawalAsync(card, userId, userId);
            return Redirect("/");
        }
        catch (Exception e)
        {
            ViewBag.Error = e.Message;
            return View("CreateWithdrawal");
        }
    }

    [HttpGet]
    [Authorize(Policy = Roles.UserRole)]
    public async Task<IActionResult> UserFundTransfers()
    {
        try
        {
            var userId = GetUserId();
            var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(userId).ToListAsync();
            return View("UserFundTransfers", fundTransfers);   
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
    [Authorize(Policy = Roles.FundsManagerRole)]
    public async Task<IActionResult> UncheckedFundTransfers()
    {
        try
        {
            var fundTransfers = await _fundsRepository.GetUncheckedFundTransfers().ToListAsync();
            return View("UncheckedFundTransfers", fundTransfers);   
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
    [Authorize(Roles = $"{Roles.FundsManagerRole}, {Roles.AdminRole}")]
    public async Task<IActionResult> AcceptedFundTransfers()
    {
        try
        {
            var fundTransfers = await _fundsRepository.GetAcceptedFundTransfers().ToListAsync();
            return View("AcceptedFundTransfers", fundTransfers);   
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
    [Authorize(Policy = Roles.FundsManagerRole)]
    public async Task<IActionResult> AcceptFundTransfer(int fundTransferId)
    {
        try
        {
            var fundManagerId = GetUserId();
            await _fundsRepository.AcceptFundTransfer(fundTransferId, fundManagerId);
            return Redirect("/Funds/UncheckedFundTransfers");
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
    [Authorize(Roles = $"{Roles.FundsManagerRole}, {Roles.UserRole}")]
    public async Task<IActionResult> RejectFundTransfer(int fundTransferId)
    {
        try
        {
            await _fundsRepository.DeleteFundTransferAsync(fundTransferId);
            var userRole = GetUserRole();
        
            if (userRole == Roles.FundsManagerRole)
            {
                return Redirect("/Funds/UncheckedFundTransfers");   
            }
        
            return Redirect("/Funds/UserFundTransfers");   
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

    private string GetUserRole()
        => HttpContext.User.FindFirst(ClaimTypes.Role)?.Value!;
    
}