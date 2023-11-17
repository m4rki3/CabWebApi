using CabWebApi.Content.Extensions;
using CabWebApi.Content.Swagger;
using CabWebApi.Controllers;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Infrastructure.Data;
using CabWebApi.Services.Interfaces;
using CarWebApi.Infrastructure.Business;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.WebHost.ConfigureServices(services =>
{
	services.AddSwaggerGen(options =>
		// xml demo
		options.SchemaFilter<EnumXmlSchemaFilter>()
	);
	services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.AccessDeniedPath = "/api/account/accessdenied";
				options.LogoutPath = "/api/account/userlogin";
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

	string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
	services.AddDbContext<DbContext, CabContext>(options =>
		options.UseSqlServer(connection));
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
app.MapControllers();
app.Run();