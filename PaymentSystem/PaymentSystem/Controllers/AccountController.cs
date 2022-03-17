using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        var userId = GetUserId();
        if (!ModelState.IsValid)
        {
            var userProfile = await _accountService.GetUserProfile(userId);
            ViewBag.Error = "Validation error";
            return View("Profile", userProfile);
        }
        
        try
        {
            await _accountService.UpdateUserAsync(updateAccountModel, userId);
        }
        catch (ArgumentException e)
        {
            ViewBag.Error = $"{e.Message}";
            var userProfile = await _accountService.GetUserProfile(userId);
            return View("Profile", userProfile);
        }
        return Redirect("/");
    }

    [HttpGet]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Users()
    {
        var usersProfiles = await _accountService.GetUsersProfiles();

        return View("Users", usersProfiles);
    }

    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User?.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}