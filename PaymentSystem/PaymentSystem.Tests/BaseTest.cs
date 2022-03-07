using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using PaymentSystem;

namespace PaymentSystem.Tests;

public class BaseTest
{
    private WebApplicationFactory<Program> _applicationFactory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _applicationFactory = new WebApplicationFactory<Program>();
        _client = _applicationFactory.CreateClient();
    }

    [Test]
    public async Task CheckMainPage()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.AreEqual("text/html; charset=utf-8", 
            response.Content.Headers.ContentType!.ToString());
    }
}