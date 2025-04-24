using Condorcet.B2.AspnetCore.MVC.Application.Core.Infrastructure;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Repository;
using Condorcet.B2.AspnetCore.MVC.Application.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProjectRepository, DapperProjectRepository>();
builder.Services.AddScoped<IDbConnectionProvider, PGSqlDbConnectionProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, DapperUserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configuration de l'authentification par cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CreateProjectPolicy", 
        policy => policy.RequireRole("Admin", "User"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
