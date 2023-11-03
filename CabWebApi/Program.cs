using CabWebApi.Content.Extensions;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Infrastructure.Data;
using CabWebApi.Services.Interfaces;
using CarWebApi.Infrastructure.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.WebHost.ConfigureServices(services =>
{
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/login";
            //    options.AccessDeniedPath = "/accessdenied";
            //    options.LogoutPath = "/logout";
            //    options.Cookie.Expiration = TimeSpan.FromDays(10);
            //});

    services.AddControllers();

    services.AddScoped<IModelRepository<User>, ModelRepository<User>>()
            .AddScoped<IModelRepository<Driver>, ModelRepository<Driver>>()
            .AddScoped<IModelRepository<Car>, ModelRepository<Car>>()
            .AddScoped<IModelRepository<Order>, ModelRepository<Order>>()
            .AddScoped<IModelRepository<Location>, ModelRepository<Location>>();

    services.AddTransient<IModelService<User>, AccountService<User>>()
            .AddTransient<IModelService<Driver>, AccountService<Driver>>()
            .AddTransient<IModelService<Car>, CarService>()
            .AddTransient<IModelService<Location>, LocationService>()
            .AddTransient<ICarService, CarService>()
            .AddTransient<IOrderService, OrderService>()
            .AddTransient<IAccountService<User>, AccountService<User>>()
            .AddTransient<IAccountService<Driver>, AccountService<Driver>>();

    string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<DbContext, CabContext>(options =>
        options.UseSqlServer(connection));
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStatusCodePages();
app.UseRouting();
app.UseAuthentication();
//app.UseAuthorization();

//var rewriteOptions = new RewriteOptions().AddRedirect("(.*)/$", "$1");
//app.UseRewriter(rewriteOptions);

app.UseConsoleLogging<Program>(LogLevel.Information);
app.MapControllers();
app.Run();