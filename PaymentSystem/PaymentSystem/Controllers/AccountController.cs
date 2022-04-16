using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        try
        {
            var userProfile = await _accountService.GetUserProfileAsync(id);
            return View("Profile", userProfile);
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
            return View("Profile", userProfile);
        }
        
        var user = await _accountService.GetUserByIdAsync(userId);

        if (user == null)
        {
            ViewBag.Error = "User not found for update.";
            return View("Profile", userProfile);
        }
        
        if (String.CompareOrdinal(user.Password, updateUserAccountModel.OldPassword) != 0)
        {
            ViewBag.Error = "Password error";
            return View("Profile", userProfile);
        }

        try
        {
            await _accountService.UpdateUserAccountAsync(updateUserAccountModel, user);
            return Redirect("/");
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
    [Authorize(Policy = Roles.AdminRole)]
    public async Task<IActionResult> Users()
    {
        try
        {
            var usersProfiles = await _accountService.GetUsersProfiles();
            return View("Users", usersProfiles);
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
    [Authorize(Policy = Roles.AdminRole)]
    public async Task<IActionResult> UserProfile(string userId)
    {
        try
        {
            Int32.TryParse(userId, out var id);
            var editUserProfile = await GetEditUserProfileModel(id);

            if (GetUserId() == id)
            {
                return View("Profile", editUserProfile.UserProfile);
            }

            return View("UserProfile", editUserProfile);
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

            try
            {
                var editUserProfile = await GetEditUserProfileModel(userId);
                return View("UserProfile", editUserProfile);
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

        try
        {
            await _accountService.UpdateUserProfileByAdminAsync(userId, userProfile);
            return Redirect("/");   
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

    private async ValueTask<EditUserProfileModel> GetEditUserProfileModel(int userId)
    {
        var userProfile = await _accountService.GetUserProfileAsync(userId);
        var roles = await _rolesRepository.GetRolesAsync().ToListAsync();
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