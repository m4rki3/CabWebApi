using CabWebApi.Content.Extensions;
using CabWebApi.Content.Swagger;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Infrastructure.Data;
using CabWebApi.Services.Interfaces;
using CarWebApi.Infrastructure.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
string? dbConnection = builder.Configuration.GetConnectionString("DefaultConnection");
// string? dbConnection = "Server=localhost";
string? watchdogHost = builder.Configuration["WATCHDOG_HOST"];
string? watchdogPortStr = builder.Configuration["WATCHDOG_PORT"];

builder.WebHost.ConfigureServices(services =>
{
	services.AddSwaggerGen(options =>
		// xml demo
		options.SchemaFilter<EnumXmlSchemaFilter>()
		// options.SchemaFilter<EnumSchemaFilter>();
	);
	services.AddStackExchangeRedisCache(options =>
	{
		options.Configuration = builder.Configuration["REDIS_HOST"];
		// options.ConfigurationOptions.Password = builder.Configuration["REDIS_PASSWORD"];
		options.InstanceName = builder.Configuration["REDIS_INSTANCE"];
	});
	services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.AccessDeniedPath = "/api/account/accessdenied";
				options.LogoutPath = "/api/account/logout";
			});

	services.AddControllers()
			.AddJsonOptions(options =>
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
			);

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

	services.AddDbContext<DbContext, CabContext>(options =>
		options.UseSqlServer(dbConnection));
});
var app = builder.Build();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseSwagger();
app.UseSwaggerUI(options =>
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1")
);
app.UseRouting();
app.UseAuthentication();

var rewriteOptions = new RewriteOptions().AddRedirect("(.*)/$", "$1");
app.UseRewriter(rewriteOptions);

app.UseConsoleLogging<Program>(LogLevel.Information);
if (watchdogHost is not null && watchdogPortStr is not null)
{
	app.UseWatchdog(
		watchdogHost,
		int.Parse(watchdogPortStr)
	);
}
app.MapControllers();
app.Run();