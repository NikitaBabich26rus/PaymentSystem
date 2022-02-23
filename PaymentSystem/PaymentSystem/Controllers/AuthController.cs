using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AuthController: Controller
{
    private readonly AuthService _authService;
    
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUserByEmail(loginModel.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            
            await AddCookie(user.Id);
            return Redirect("/");
        }
        
        return BadRequest("Error model");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUserByEmail(registerModel.Email);
            if (user != null)
            {
                return BadRequest("Email is used");
            }
            
            var userId = await _authService.CreateUser(registerModel);
            await AddCookie(userId);
            return Redirect("/");
        }
        
        return BadRequest("Error model");
    }
    
    public async Task<IActionResult> Logout()
    {
        await DeleteCookie();
        return Redirect("/");
    }

    private async Task DeleteCookie()
        => await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    
    private async Task AddCookie(int userId)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, userId.ToString())
        };
        var id = new ClaimsIdentity
            (
                claims,
                "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}