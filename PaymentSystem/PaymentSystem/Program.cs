using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PaymentSystem;
using PaymentSystem.Data;
using PaymentSystem.Repositories;
using PaymentSystem.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PaymentSystemContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    
}, ServiceLifetime.Transient);
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Cookie")
    .AddCookie("Cookie", options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            context.RedirectUri = "/Home/Login";
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Roles.AdminRole, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, Roles.AdminRole);
    });
    options.AddPolicy(Roles.UserRole, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, Roles.UserRole);
    });
    options.AddPolicy(Roles.KycManagerRole, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, Roles.KycManagerRole);
    });
    options.AddPolicy(Roles.FundsManagerRole, policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, Roles.FundsManagerRole);
    });
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<IFundsRepository, FundsRepository>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();
public partial class Program { }