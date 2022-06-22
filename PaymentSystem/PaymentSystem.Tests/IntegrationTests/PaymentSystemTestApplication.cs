using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PaymentSystem.Data;

namespace PaymentSystem.Tests.IntegrationTests;

public class PaymentSystemTestApplication: WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<PaymentSystemContext>));

            services.AddDbContext<PaymentSystemContext>(options =>
            {
                options.UseInMemoryDatabase("IntegrationTesting", root);
                
            }, ServiceLifetime.Singleton);
            
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var database = scopedServices.GetRequiredService<PaymentSystemContext>();

            database.Roles.AddRange(new []
            {
                new RoleRecord() { Id = 1, Name = Roles.UserRole },
                new RoleRecord() { Id = 2, Name = Roles.AdminRole },
                new RoleRecord() { Id = 3, Name = Roles.KycManagerRole },
                new RoleRecord() { Id = 4, Name = Roles.FundsManagerRole }
            });
            
            database.SaveChanges();
        });
        
        return base.CreateHost(builder);
    }
}