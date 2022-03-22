using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AuthController: Controller
{
    private readonly AccountService _accountService;
    private readonly RolesService _rolesService;
    
    public AuthController(AccountService accountService, RolesService rolesService)
    {
        _rolesService = rolesService;
        _accountService = accountService;
    }
    
    public IActionResult Register()
    {
        return View("Register");
    }
    
    public IActionResult Login()
    {
        return View("Login");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _accountService.GetUserByEmailAsync(loginModel.Email);
            if (user == null)
            {
                ViewBag.Error = "User not found";
                return View("Login");
            }
            if (user.IsBlocked)
            {
                ViewBag.Error = "Account is blocked";
                return View("Login");
            }
            
            var userRole = await _rolesService.GetUserRoleAsync(user.Id);
            await AddCookie(user.Id, userRole);
            return Redirect("/");
        }
        
        ViewBag.Error = "Error email or password";
        return View("Login");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _accountService.GetUserByEmailAsync(registerModel.Email);
            if (user != null)
            {
                ViewBag.Error = "Email is used";
                return View("Register");
            }
            
            var userId = await _accountService.CreateUserAsync(registerModel);
            var userRole = await _rolesService.GetUserRoleAsync(userId);
            await AddCookie(userId, userRole);
            return Redirect("/");
        }
        ViewBag.Error = "Validation error";
        return View("Register");
    }
    
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await DeleteCookie();
        return Redirect("/");
    }

    private async Task DeleteCookie()
        => await HttpContext.SignOutAsync("Cookie");
    
    private async Task AddCookie(int userId, string userRole)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, userId.ToString()),
            new Claim(ClaimTypes.Role, userRole)
        };
        var id = new ClaimsIdentity(claims, "Cookie");
        await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(id));
    }
}