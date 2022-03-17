using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
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
    options.AddPolicy("Admin", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireClaim(ClaimTypes.Role, "Admin");
    });
    options.AddPolicy("User", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireClaim(ClaimTypes.Role, "User");
    });
    options.AddPolicy("KYC-Manager", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireClaim(ClaimTypes.Role, "KYC-Manager");
    });
    options.AddPolicy("Funds-Manager", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireClaim(ClaimTypes.Role, "Funds-Manager");
    });
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<BalanceService>();

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