using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Models;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AccountController: Controller
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var id = GetUserId();
        var userProfile = await _accountService.GetUserProfile(id);
        return View("Profile", userProfile);
    }
    
    [HttpPost]
    public async Task<IActionResult> UpdateAccount(UpdateAccountModel updateAccountModel)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .Select(x => x.Errors)
                .ToArray();
            return BadRequest(" Error model");
        }
        
        var userId = GetUserId();
        try
        {
            await _accountService.UpdateUserAsync(updateAccountModel, userId);
        }
        catch (ArgumentException e)
        {
            return BadRequest($"{e.Message}");
        }
        return Redirect("/");
    }

    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value, out var id);
        return id;
    }
}