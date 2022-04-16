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

public class FundsTransfersTests
{
    private PaymentSystemContext _paymentSystemContext = null!;
    private AccountService _accountService = null!;
    private IFundsRepository _fundsRepository = null!;
    private IBalanceRepository _balanceRepository = null!;

    private readonly RegisterModel _registerUser = new()
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Email = "ivanov@gmail.com",
        Password = "123456789",
        ConfirmPassword = "123456789",
    };
    private int _userId;
    
    private readonly RegisterModel _registerFundsManager = new()
    {
        FirstName = "kyc",
        LastName = "kyc",
        Email = "kyc@gmail.com",
        Password = "12345678",
        ConfirmPassword = "12345678",
    };
    private int _fundsManagerId;
    
    private readonly CardModel _card = new()
    {
        CardNumber = "111111111111111",
        CardCvc = "444",
        CardDate = "0922",
        AmountOfMoney = 100m
    };

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
        _balanceRepository = new BalanceRepository(_paymentSystemContext);
        _fundsRepository = new FundsRepository(_paymentSystemContext, _balanceRepository);

        _accountService = new AccountService(
            accountRepository,
            rolesRepository,
            _balanceRepository);

        _userId = await _accountService.CreateUserAsync(_registerUser);
        _fundsManagerId = await _accountService.CreateUserAsync(_registerFundsManager);
    }

    [TearDown]
    public void TearDown()
    {
        _paymentSystemContext.Dispose();
    }
    
    [Test]
    public async Task CreateDeposit_AcceptFundTransfer_GetUserBalance_Test()
    {
        await _fundsRepository.CreateDepositAsync(_card, _userId, _userId);
        var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(_userId).ToListAsync();

        fundTransfers[0].CardCvc.Should().Be(_card.CardCvc);
        fundTransfers[0].CardNumber.Should().Be(_card.CardNumber);
        fundTransfers[0].CardDate.Should().Be(_card.CardDate);
        fundTransfers[0].CreatedBy.Should().Be(_userId);
        fundTransfers[0].UserId.Should().Be(_userId);
        
        await _fundsRepository.AcceptFundTransfer(fundTransfers[0].Id, _fundsManagerId);
        var acceptedFundTransfers = await _fundsRepository.GetAcceptedFundTransfers().ToListAsync();
        
        acceptedFundTransfers[0].CardCvc.Should().Be(_card.CardCvc);
        acceptedFundTransfers[0].CardNumber.Should().Be(_card.CardNumber);
        acceptedFundTransfers[0].CardDate.Should().Be(_card.CardDate);
        acceptedFundTransfers[0].UserId.Should().Be(_userId);
        acceptedFundTransfers[0].ConfirmedBy.Should().Be(_fundsManagerId);

        var userBalance = await _balanceRepository.GetUserBalanceAsync(_userId);
        userBalance.Amount.Should().Be(_card.AmountOfMoney);
    }
    
    [Test]
    public async Task CreateDeposit_RejectFundTransfer_GetUserBalance_Test()
    {
        await _fundsRepository.CreateDepositAsync(_card, _userId, _userId);
        var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(_userId).ToListAsync();

        fundTransfers[0].CardCvc.Should().Be(_card.CardCvc);
        fundTransfers[0].CardNumber.Should().Be(_card.CardNumber);
        fundTransfers[0].CardDate.Should().Be(_card.CardDate);
        fundTransfers[0].CreatedBy.Should().Be(_userId);
        fundTransfers[0].UserId.Should().Be(_userId);
        
        await _fundsRepository.DeleteFundTransferAsync(fundTransfers[0].Id);
        var fundTransfer = await _fundsRepository.GetFundTransferAsync(fundTransfers[0].Id);

        fundTransfer.Should().BeNull();
        
        var userBalance = await _balanceRepository.GetUserBalanceAsync(_userId);
        userBalance.Amount.Should().Be(0m);
    }
    
    [Test]
    public async Task CreateWithdrawalWithNoMoneyOnTheBalance_Test()
    {
        await _fundsRepository
            .Invoking(x => x.CreateWithdrawalAsync(_card, _userId, _userId))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("Not enough money to withdraw.");
    }
    
    [Test]
    public async Task CreateDeposit_CreateWithdrawal_AcceptFundsTransfers_GetUserBalance_Test()
    {
        await _fundsRepository.CreateDepositAsync(_card, _userId, _userId);
        var fundTransfers = await _fundsRepository.GetUncheckedFundTransfers().ToListAsync();
        await _fundsRepository.AcceptFundTransfer(fundTransfers[0].Id, _fundsManagerId);

        var userBalance = await _balanceRepository.GetUserBalanceAsync(_userId);
        userBalance.Amount.Should().Be(100m);
        
        await _fundsRepository.CreateWithdrawalAsync(_card, _userId, _userId);
        fundTransfers = await _fundsRepository.GetUncheckedFundTransfers().ToListAsync();
        await _fundsRepository.AcceptFundTransfer(fundTransfers[0].Id, _fundsManagerId);

        userBalance = await _balanceRepository.GetUserBalanceAsync(_userId);
        userBalance.Amount.Should().Be(0m);
    }
    
    [Test]
    public async Task CreateDeposit_GetUncheckedFundsTransfers_AcceptFundsTransfer_GetAcceptedFundsTransfers_Test()
    {
        await _fundsRepository.CreateDepositAsync(_card, _userId, _userId);
        var uncheckedFundTransfers = await _fundsRepository.GetUncheckedFundTransfers().ToListAsync();

        uncheckedFundTransfers.Count.Should().Be(1);
        
        await _fundsRepository.AcceptFundTransfer(uncheckedFundTransfers[0].Id, _fundsManagerId);

        var acceptedFundsTransfers = await _fundsRepository.GetAcceptedFundTransfers().ToListAsync();
        uncheckedFundTransfers = await _fundsRepository.GetUncheckedFundTransfers().ToListAsync();
        
        acceptedFundsTransfers.Count.Should().Be(1);
        uncheckedFundTransfers.Count.Should().Be(0);
    }
    
    [Test]
    public async Task CreateWithdrawal_AcceptFundTransferTwice_GetUserBalance_Test()
    {
        await _fundsRepository.CreateDepositAsync(_card, _userId, _userId);
        var fundTransfers = await _fundsRepository.GetFundTransfersOfUser(_userId).ToListAsync();

        var firstAcceptFundTransfer = _fundsRepository.AcceptFundTransfer(fundTransfers[0].Id, _fundsManagerId);
        var secondAcceptFundTransfer = _fundsRepository.AcceptFundTransfer(fundTransfers[0].Id, _fundsManagerId);
        await Task.WhenAll(firstAcceptFundTransfer, secondAcceptFundTransfer);

        var userBalance = await _balanceRepository.GetUserBalanceAsync(_userId);
        userBalance.Amount.Should().Be(_card.AmountOfMoney);
    }
    
}