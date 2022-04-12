using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Controllers;

public class AccountController: Controller
{
    private readonly AccountService _accountService;
    private readonly IRolesRepository _rolesRepository;

    public AccountController(
        AccountService accountService,
        IRolesRepository rolesRepository)
    {
        _accountService = accountService;
        _rolesRepository = rolesRepository;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var id = GetUserId();
        var userProfile = await _accountService.GetUserProfileAsync(id);
        return View("Account", userProfile);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UpdateUserAccountModel updateUserAccountModel)
    {
        var userId = GetUserId();
        var userProfile = await _accountService.GetUserProfileAsync(userId);
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            return View("Account", userProfile);
        }
        
        var user = await _accountService.GetUserByIdAsync(userId);
        if (String.CompareOrdinal(user!.Password, updateUserAccountModel.OldPassword) != 0)
        {
            ViewBag.Error = "Password error";
            return View("Account", userProfile);
        }
        
        await _accountService.UpdateUserAccountAsync(updateUserAccountModel, user);
        return Redirect("/");
    }

    [HttpGet]
    [Authorize(Policy = Roles.AdminRole)]
    public async Task<IActionResult> Users()
    {
        var usersProfiles = await _accountService.GetUsersProfiles();
        return View("Users", usersProfiles);
    }

    [HttpGet]
    [Authorize(Policy = Roles.AdminRole)]
    public async Task<IActionResult> UserProfile(string userId)
    {
        Int32.TryParse(userId, out var id);
        var editUserProfile = await GetEditUserProfileModel(id);
        
        if (GetUserId() == id)
        {
            return View("Account", editUserProfile.UserProfile);
        }
        
        return View("UserProfile", editUserProfile);
    }
    
    [HttpPost]
    [Authorize(Policy = Roles.AdminRole)]
    public async Task<IActionResult> UpdateUserProfile(UpdateUserProfileModel userProfile, string id)
    {
        Int32.TryParse(id, out var userId);
        if (!ModelState.IsValid)
        {
            var messages = string.Join(" ", ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage));
            
            ViewBag.Error = messages;
            var editUserProfile = await GetEditUserProfileModel(userId);
            return View("UserProfile", editUserProfile);
        }

        await _accountService.UpdateUserProfileByAdminAsync(userId, userProfile);
        return Redirect("/");
    }

    private async ValueTask<EditUserProfileModel> GetEditUserProfileModel(int userId)
    {
        var userProfile = await _accountService.GetUserProfileAsync(userId);
        var roles = await _rolesRepository.GetRolesAsync();
        var editUserProfile = new EditUserProfileModel()
        {
            UserProfile = userProfile,
            Roles = roles
        };
        return editUserProfile;
    }

    private int GetUserId()
    {
        Int32.TryParse(HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value, out var id);
        return id;
    }
}