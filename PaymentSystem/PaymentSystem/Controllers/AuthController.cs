using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

[Route("api/auth")]
public class AuthController: Controller
{
    private readonly AuthService _authService;
    
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUserByEmail(loginModel.Email);
            if (user == null)
            {
                return BadRequest();
            }
            
            await AddCookie(user.Id);
            return Ok();
        }
        
        return BadRequest();
    }

    [HttpPost("sign-up")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel registerModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _authService.GetUserByEmail(registerModel.Email);
            if (user != null)
            {
                return BadRequest();
            }
            
            var userId = await _authService.CreateUser(registerModel);
            await AddCookie(userId);
            return Ok();
        }
        
        return BadRequest();
    }
    
    [HttpGet("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await DeleteCookie();
        return Ok();
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