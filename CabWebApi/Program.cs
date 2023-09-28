using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Infrastructure.Data;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.WebHost.ConfigureServices(services =>
{
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = "/login");
    services.AddAuthorization();
    services.AddControllers();
    services.AddTransient<IModelRepository<User>, ModelRepository<User>>()
            .AddTransient<IUserService, UserService>()
            .AddDbContext<DbContext, ApplicationContext>(options =>
                 options.UseSqlServer(
                     "Server: (localdb)\\MSSQLLocalDB;" +
                     "Database: Cab;" +
                     "Trusted_Connection: true;")
             );
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();
app.Run();