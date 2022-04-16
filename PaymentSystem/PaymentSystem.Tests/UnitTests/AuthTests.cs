using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Tests.UnitTests;

[TestFixture]
public class AuthTests
{
    private PaymentSystemContext _paymentSystemContext = null!;
    private AccountService _accountService = null!;
    private RolesRepository _rolesRepository = null!;

    private readonly RegisterModel _registerModel = new()
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Email = "ivanov@gmail.com",
        Password = "123456789",
        ConfirmPassword = "123456789"
    };

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<PaymentSystemContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _paymentSystemContext = new PaymentSystemContext(options);

        await _paymentSystemContext.Roles.AddRangeAsync(new []
        {
            new RoleRecord() { Id = 1, Name = Roles.UserRole },
            new RoleRecord() { Id = 2, Name = Roles.AdminRole },
            new RoleRecord() { Id = 3, Name = Roles.KycManagerRole },
            new RoleRecord() { Id = 4, Name = Roles.FundsManagerRole }
        });
        await _paymentSystemContext.SaveChangesAsync();

        _rolesRepository = new RolesRepository(_paymentSystemContext);
        var accountRepository = new AccountRepository(_paymentSystemContext);
        var balanceRepository = new BalanceRepository(_paymentSystemContext);

        _accountService = new AccountService(accountRepository, _rolesRepository, balanceRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _paymentSystemContext.Dispose();
    }

    [Test]
    public async Task CreateUser_GetUserById_Test()
    {
        var userId = await _accountService.CreateUserAsync(_registerModel);
        var user = await _accountService.GetUserByIdAsync(userId);

        user!.Email.Should().Be(_registerModel.Email);
        user.Password.Should().Be(_registerModel.Password);
        user.Id.Should().Be(userId);
    }
    
    [Test]
    public async Task GetUserRole_GetUserByEmail_Test()
    {
        var userId = await _accountService.CreateUserAsync(_registerModel);
        var user = await _accountService.GetUserByEmailAsync(_registerModel.Email);

        var role = await _rolesRepository.GetUserRoleAsync(userId);
        role.Should().Be(Roles.UserRole);
        
        user!.Email.Should().Be(_registerModel.Email);
        user.Password.Should().Be(_registerModel.Password);
    }
}