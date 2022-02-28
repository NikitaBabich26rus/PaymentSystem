using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AccountController: Controller
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        Int32.TryParse(HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value, out var id);
        var userProfile = await _accountService.GetUserProfile(id);
        return View("Profile", userProfile);
    }
}