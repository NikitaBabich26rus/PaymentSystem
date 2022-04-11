using PaymentSystem.Data;
using PaymentSystem.Models;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

namespace PaymentSystem.Tests;

public class AccountTests
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
    
    
}