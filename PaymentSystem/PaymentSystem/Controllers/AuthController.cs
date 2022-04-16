using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AuthController: Controller
{
    private readonly AccountService _accountService;
    private readonly IRolesRepository _rolesRepository;
    
    public AuthController(AccountService accountService, IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
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
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            return View("Login");
        }
        
        var user = await _accountService.GetUserByEmailAsync(loginModel.Email);
            
        if (user == null)
        {
            ViewBag.Error = "Error email";
            return View("Login");
        }
            
        if (user.IsBlocked)
        {
            ViewBag.Error = "Account is blocked";
            return View("Login");
        }

        if (String.CompareOrdinal(user.Password, loginModel.Password) != 0)
        {
            ViewBag.Error = "Error password";
            return View("Login");
        }

        try
        {
            var userRole = await _rolesRepository.GetUserRoleAsync(user.Id);
            await AddCookie(user.Id, userRole);
            return Redirect("/");
        }
        catch (Exception)
        {
            ViewBag.Error = "Sign in error, contact support please.";
            return View("Login");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            return View("Register");
        }
        
        var user = await _accountService.GetUserByEmailAsync(registerModel.Email);
        
        if (user != null)
        {
            ViewBag.Error = "Email is used";
            return View("Register");
        }

        try
        {
            var userId = await _accountService.CreateUserAsync(registerModel);
            var userRole = await _rolesRepository.GetUserRoleAsync(userId);
            await AddCookie(userId, userRole);
            return Redirect("/");
        }
        catch (Exception)
        {
            ViewBag.Error = "Sign up error, contact support please.";
            return View("Register");
        }
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