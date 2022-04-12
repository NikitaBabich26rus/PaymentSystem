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

public class VerificationTests
{
    private PaymentSystemContext _paymentSystemContext = null!;
    private AccountService _accountService = null!;
    private VerificationRepository _verificationRepository = null!;

    private readonly RegisterModel _registerUser = new()
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Email = "ivanov@gmail.com",
        Password = "123456789",
        ConfirmPassword = "123456789",
    };
    private int _userId;
    
    private readonly RegisterModel _registerKycManager = new()
    {
        FirstName = "kyc",
        LastName = "kyc",
        Email = "kyc@gmail.com",
        Password = "12345678",
        ConfirmPassword = "12345678",
    };
    private int _kycManagerId;
    
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
        _verificationRepository = new VerificationRepository(_paymentSystemContext, accountRepository);

        _accountService = new AccountService(
            accountRepository,
            rolesRepository,
            balanceRepository);

        _userId = await _accountService.CreateUserAsync(_registerUser);
        _kycManagerId = await _accountService.CreateUserAsync(_registerKycManager);

    }

    [TearDown]
    public void TearDown()
    {
        _paymentSystemContext.Dispose();
    }

    [Test]
    public async Task SendVerificationRequest_GetVerificationRequestByUserId_Test()
    {
        var passportData = "1234567890";
        await _verificationRepository.SendVerificationRequestAsync(_userId, passportData);
        
        var verificationRequest = await _verificationRepository
            .GetVerificationRequestByUserIdAsync(_userId);

        var user = await _accountService.GetUserByIdAsync(_userId);

        verificationRequest!.User.Email.Should().Be(user!.Email);
        verificationRequest.User.Password.Should().Be(user.Password);
        verificationRequest.User.FirstName.Should().Be(user.FirstName);
        verificationRequest.User.LastName.Should().Be(user.LastName);
        verificationRequest.PassportData.Should().Be(passportData);
    }

    [Test]
    public async Task SendVerificationRequest_AcceptVerificationRequest_GetUserVerificationStatus_Test()
    {
        var passportData = "1234567890";

        await _verificationRepository.SendVerificationRequestAsync(_userId, passportData);
        var verificationRequest = await _verificationRepository
            .GetVerificationRequestByUserIdAsync(_userId);
        await _verificationRepository.AcceptUserVerificationAsync(verificationRequest!.Id, _kycManagerId);
        

        verificationRequest.User.Id.Should().Be(_userId);
        verificationRequest.User.Email.Should().Be(_registerUser.Email);
        verificationRequest.User.FirstName.Should().Be(_registerUser.FirstName);
        verificationRequest.ConfirmedBy.Should().Be(_kycManagerId);

        var user = await _accountService.GetUserByIdAsync(_userId);
        user!.IsVerified.Should().Be(true);
    }
    
    [Test]
    public async Task SendVerificationRequest_RejectVerificationRequest_GetUserVerificationStatus_Test()
    {
        var passportData = "1234567890";

        await _verificationRepository.SendVerificationRequestAsync(_userId, passportData);
        var verificationRequest = await _verificationRepository
            .GetVerificationRequestByUserIdAsync(_userId);
        await _verificationRepository.RejectUserVerificationRequestAsync(verificationRequest!.Id);

        var rejectedVerificationRequest = await _verificationRepository
            .GetVerificationRequestByUserIdAsync(_userId);

        rejectedVerificationRequest.Should().BeNull();

        var user = await _accountService.GetUserByIdAsync(_userId);
        user!.IsVerified.Should().Be(false);
    }
    
    [Test]
    public async Task SendVerificationRequest_GetVerificationRequests_Test()
    {
        var passportData = "1234567890";
        await _verificationRepository.SendVerificationRequestAsync(_userId, passportData);
        
        var verificationRequests = await _verificationRepository
            .GetVerificationRequestsAsync();

        var user = await _accountService.GetUserByIdAsync(_userId);

        verificationRequests[0].User.Email.Should().Be(user!.Email);
        verificationRequests[0].User.Password.Should().Be(user.Password);
        verificationRequests[0].User.FirstName.Should().Be(user.FirstName);
        verificationRequests[0].User.LastName.Should().Be(user.LastName);
        verificationRequests[0].PassportData.Should().Be(passportData);
    }
    
    [Test]
    public async Task SendVerificationRequest_AcceptVerificationRequest_GetAcceptedVerificationRequests_Test()
    {
        var passportData = "1234567890";

        await _verificationRepository.SendVerificationRequestAsync(_userId, passportData);
        var verificationRequest = await _verificationRepository
            .GetVerificationRequestByUserIdAsync(_userId);

        await _verificationRepository.AcceptUserVerificationAsync(verificationRequest!.Id, _kycManagerId);

        var acceptedVerificationRequests = await _verificationRepository
            .GetAcceptedRequestsForVerificationAsync()
            .ToListAsync();
        
        acceptedVerificationRequests[0].User.Id.Should().Be(_userId);
        acceptedVerificationRequests[0].User.Email.Should().Be(_registerUser.Email);
        acceptedVerificationRequests[0].User.FirstName.Should().Be(_registerUser.FirstName);
        acceptedVerificationRequests[0].ConfirmedBy.Should().Be(_kycManagerId);
    }
}