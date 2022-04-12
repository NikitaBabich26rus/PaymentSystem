using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Tests;

public class AccountTests
{
    private PaymentSystemContext _paymentSystemContext = null!;
    private AccountService _accountService = null!;

    private readonly RegisterModel _registerUser = new()
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Email = "ivanov@gmail.com",
        Password = "123456789",
        ConfirmPassword = "123456789",
    };
    private int _userId;
    
    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<PaymentSystemContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _paymentSystemContext = new PaymentSystemContext(options);

        await _paymentSystemContext.Roles.AddRangeAsync(new[]
        {
            new RoleRecord() { Id = 1, Name = Roles.UserRole },
            new RoleRecord() { Id = 2, Name = Roles.AdminRole },
            new RoleRecord() { Id = 3, Name = Roles.KycManagerRole },
            new RoleRecord() { Id = 4, Name = Roles.FundsManagerRole }
        });
        await _paymentSystemContext.SaveChangesAsync();

        var rolesRepository = new RolesRepository(_paymentSystemContext);
        var accountRepository = new AccountRepository(_paymentSystemContext);
        var balanceRepository = new BalanceRepository(_paymentSystemContext);

        _accountService = new AccountService(
            accountRepository,
            rolesRepository,
            balanceRepository);

        var userId = await _accountService.CreateUserAsync(_registerUser);
        _userId = userId;
    }

    [TearDown]
    public void TearDown()
    {
        _paymentSystemContext.Dispose();
    }

    [Test]
    public async Task GetUserProfile_Test()
    {
        var userProfile = await _accountService.GetUserProfileAsync(_userId);
        
        userProfile.Email.Should().Be(_registerUser.Email);
        userProfile.FirstName.Should().Be(_registerUser.FirstName);
        userProfile.LastName.Should().Be(_registerUser.LastName);
        userProfile.IsVerified.Should().Be(false);
        userProfile.IsBlocked.Should().Be(false);
        userProfile.Balance.Should().Be(0m);
        userProfile.Role.Should().Be(Roles.UserRole);
    }
    
    [Test]
    public async Task UpdateUserAccount_Test()
    {
        var user = await _accountService.GetUserByIdAsync(_userId);
        var updatedUserAccount = new UpdateUserAccountModel()
        {
            FirstName = "Ivan1",
            LastName = "Ivanov1",
            Email = "ivanov1@gmail.com",
            OldPassword = "123456789",
            NewPassword = "12345678910"
        };

        await _accountService.UpdateUserAccountAsync(updatedUserAccount, user!);
        var updatedUser = await _accountService.GetUserByIdAsync(_userId);
        
        updatedUser!.FirstName.Should().Be(updatedUserAccount.FirstName);
        updatedUser.LastName.Should().Be(updatedUserAccount.LastName);
        updatedUser.Email.Should().Be(updatedUserAccount.Email);
        updatedUser.Password.Should().Be(updatedUserAccount.NewPassword);
    }
    
    [Test]
    public async Task GetUsersProfiles_Test()
    {
        var usersProfiles = await _accountService.GetUsersProfiles();
        
        usersProfiles[0].Email.Should().Be(_registerUser.Email);
        usersProfiles[0].FirstName.Should().Be(_registerUser.FirstName);
        usersProfiles[0].LastName.Should().Be(_registerUser.LastName);
        usersProfiles[0].IsVerified.Should().Be(false);
        usersProfiles[0].IsBlocked.Should().Be(false);
        usersProfiles[0].Balance.Should().Be(0m);
        usersProfiles[0].Role.Should().Be(Roles.UserRole);
    }
    
    [Test]
    public async Task UpdateUserProfileByAdmin_GetUpdatedUserProfile_Test()
    {
        var updatedUserProfile = new UpdateUserProfileModel()
        {
            FirstName = "Ivan2",
            LastName = "Ivanov2",
            Email = "ivanov2@gmail.com",
            Role = Roles.FundsManagerRole,
            Status = UserStatus.BlockedStatus
        };
        
        await _accountService.UpdateUserProfileByAdminAsync(_userId, updatedUserProfile);
        var userProfile = await _accountService.GetUserProfileAsync(_userId);

        userProfile.FirstName.Should().Be(updatedUserProfile.FirstName);
        userProfile.LastName.Should().Be(updatedUserProfile.LastName);
        userProfile.Email.Should().Be(updatedUserProfile.Email);
        userProfile.Role.Should().Be(Roles.FundsManagerRole);
        userProfile.IsBlocked.Should().Be(true);
    }
}