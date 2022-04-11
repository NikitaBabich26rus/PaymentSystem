using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Tests;
using NUnit.Framework;

[TestFixture]
public class AuthTests
{
    private PaymentSystemContext _paymentSystemContext = null!;
    private AccountService _accountService = null!;
    private RolesRepository _rolesRepository = null!;

    private RegisterModel registerModel = new()
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
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

        _paymentSystemContext = new PaymentSystemContext(options);

        await _paymentSystemContext.Roles.AddRangeAsync(new []
        {
            new RoleRecord() { Id = 1, Name = "User" },
            new RoleRecord() { Id = 2, Name = "Admin" },
            new RoleRecord() { Id = 3, Name = "KYC-Manager" },
            new RoleRecord() { Id = 4, Name = "Funds-Manager" }
        });
        
        _rolesRepository = new RolesRepository(_paymentSystemContext);
        var accountRepository = new AccountRepository(_paymentSystemContext);
        var balanceRepository = new BalanceRepository(_paymentSystemContext);
        var verificationRepository = new VerificationRepository(_paymentSystemContext, accountRepository);

        _accountService =
            new AccountService(accountRepository, _rolesRepository, balanceRepository, verificationRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _paymentSystemContext.Dispose();
    }

    [Test]
    public async Task CreateUser_GetUserById()
    {
        var userId = await _accountService.CreateUserAsync(registerModel);
        var user = await _accountService.GetUserByIdAsync(userId);

        user!.Email.Should().Be(registerModel.Email);
        user.Password.Should().Be(registerModel.Password);
        user.Id.Should().Be(userId);
    }
    
    [Test]
    public async Task CreateUser_GetUserRole_GetUserByEmail()
    {
        var user = await _accountService.GetUserByEmailAsync(registerModel.Email);

        var role = await _rolesRepository.GetUserRoleAsync(user!.Id);
        role.Should().Be("User");
        
        user.Email.Should().Be(registerModel.Email);
        user.Password.Should().Be(registerModel.Password);
    }
}