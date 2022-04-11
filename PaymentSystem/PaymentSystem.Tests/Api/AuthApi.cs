using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PaymentSystem.Models;

namespace PaymentSystem.Tests.Api;

public static class AuthApi
{
    
    public static async Task<HttpResponseMessage> Register(HttpClient client, RegisterModel registerModel)
    {
        var httpContent = new StringContent(JsonConvert.SerializeObject(registerModel), Encoding.UTF8, "application/json");
        var response = await client.PostAsync($"/Auth/Register/", httpContent);
        return response;
    }
    
}