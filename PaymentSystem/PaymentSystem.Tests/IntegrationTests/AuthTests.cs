using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using PaymentSystem.Models;

namespace PaymentSystem.Tests.IntegrationTests;

[TestFixture]
public class AuthTests
{
    private WebApplicationFactory<Program> _testApplication = null!;
    private HttpClient _client = null!;
    
    private readonly RegisterModel _registerModel = new()
    {
        FirstName = "Ivan",
        LastName = "Ivanov",
        Email = "ivanov@gmail.com",
        Password = "123456789",
        ConfirmPassword = "123456789"
    };

    [OneTimeSetUp]
    public void SetUp()
    {
        _testApplication = new PaymentSystemTestApplication();
        _client = _testApplication.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        _testApplication.Dispose();
        _client.Dispose();
    }

    [Test]
    public async Task RegisterSuccess_Test()
    {
        var httpResponse = await RegisterApi(_registerModel);
        var requestUri = httpResponse.RequestMessage!.RequestUri;

        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        requestUri.Should().Be("http://localhost/");
    }

    private async ValueTask<HttpResponseMessage> RegisterApi(RegisterModel registerModel)
    {
        var registerDictionary = new Dictionary<string, string>
        {
            {"FirstName", registerModel.FirstName},
            {"LastName", registerModel.LastName},
            {"Email", registerModel.Email},
            {"Password", registerModel.Password},
            {"ConfirmPassword", registerModel.ConfirmPassword},
        };
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/Auth/Register/")
        {
            Content = new FormUrlEncodedContent(registerDictionary)
        };
        
        var httpResponse = await _client.SendAsync(requestMessage);
        
        return httpResponse;
    }
}