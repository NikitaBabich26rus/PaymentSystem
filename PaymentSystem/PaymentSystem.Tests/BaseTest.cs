using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace PaymentSystem.Tests;

public class BaseTest
{

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}