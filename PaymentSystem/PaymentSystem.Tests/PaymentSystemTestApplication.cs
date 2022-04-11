using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PaymentSystem.Data;

namespace PaymentSystem.Tests;

public class PaymentSystemTestApplication: WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PaymentSystemContext>));
            services.AddDbContext<PaymentSystemContext>(options =>
                options.UseInMemoryDatabase("Testing", root));
        });
        
        return base.CreateHost(builder);
    }    
}