using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
	private readonly IAccountService<User> userService;
	private readonly IAccountService<Driver> driverService;
	private readonly IModelService<Car> carService;
	public AccountController(
		IAccountService<User> userService,
		IAccountService<Driver> driverService,
		IModelService<Car> carService)
	{
		this.userService = userService;
		this.driverService = driverService;
		this.carService = carService;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(Car), StatusCodes.Status201Created)]
	public async Task<IActionResult> RegisterCar([FromBody] CarModel model)
	{
		Driver? driver = await driverService.GetAsync(model.DriverId);

		if (driver is null)
			return BadRequest(
				$"{nameof(CarModel)}.{nameof(CarModel.DriverId)} is not found in database");

		string registrationPlate = model.SeriesRegistrationNumber + model.RegionCode;
		List<Car> dbCars = await carService.GetAllWithAsync(
			nameof(Car.RegistrationPlate), registrationPlate);

		if (dbCars.Count != 0)
			return BadRequest("Car with posted registration number had been already added");

		CarBuilder builder = new();
		builder.Model(model.ModelName)
			   .RegisteredAs(registrationPlate)
			   .HasDriver(model.DriverId)
			   .InStatus(model.Status);
		Car car = builder.Build();
		Car created = (await carService.CreateAsync(car)).Item1.Entity;

		return CreatedAtAction(nameof(RegisterCar), created);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
	public async Task<IActionResult> RegisterUser([FromForm] UserRegistrationModel model)
	{
		bool userIsRegistered = await userService.IsRegisteredWith(
									nameof(Domain.Core.User.PhoneNumber), model.PhoneNumber)
								||
								await driverService.IsRegisteredWith(
									nameof(Driver.PhoneNumber), model.PhoneNumber);

		if (userIsRegistered)
			return BadRequest("User or driver with entered phone number had been already registered");

		string hashedPassword = userService.HashPassword(model.Password);

		// recursive generics demo
		UserBuilder builder = new();
		builder.Named(model.Name)
			   .HasPhoneNumber(model.PhoneNumber)
			   .HasEmail(model.Email)
			   .HasPassword(hashedPassword)
			   .HasBirthDate(model.BirthDate);
		User user = builder.Build();
		User created = (await userService.CreateAsync(user)).Item1.Entity;

		ClaimsPrincipal principal = userService.GetPrincipal(created);
		await HttpContext.SignInAsync(principal);

		return CreatedAtAction(nameof(RegisterUser), created);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(Driver), StatusCodes.Status201Created)]
	public async Task<IActionResult> RegisterDriver(
		[FromForm] DriverRegistrationModel model, [FromQuery] int salary)
	{
		bool phoneIsRegistered = await driverService.IsRegisteredWith(
									 nameof(Driver.PhoneNumber), model.PhoneNumber)
								 ||
								 await userService.IsRegisteredWith(
									 nameof(Domain.Core.User.PhoneNumber), model.PhoneNumber);

		if (phoneIsRegistered)
			return BadRequest("User or driver with entered phone number had been already registered");

		long drivingLicense = long.Parse(
			model.LicenseSeries.ToString() + model.LicenseNumber.ToString()
		);
		bool licenseIsRegistered = await driverService.IsRegisteredWith(
			nameof(Driver.DrivingLicense), drivingLicense
		);
		if (licenseIsRegistered)
			return BadRequest("Driver with entered driving license had been already registered");

		string hashedPassword = driverService.HashPassword(model.Password);

		// recursive generics demo
		DriverBuilder builder = new();
		builder.Named(model.Name)
			   .HasPhoneNumber(model.PhoneNumber)
			   .HasEmail(model.Email)
			   .HasPassword(hashedPassword)
			   .HasBirthDate(model.BirthDate)
			   .Earns(salary)
			   .HasLicense(drivingLicense);
		Driver driver = builder.Build();
		Driver created = (await driverService.CreateAsync(driver)).Item1.Entity;

		ClaimsPrincipal principal = driverService.GetPrincipal(created);
		await HttpContext.SignInAsync(principal);

		return CreatedAtAction(nameof(RegisterDriver), created);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status301MovedPermanently)]
	public async Task<IActionResult> UserLogin([FromForm] LoginModel model, string? returnUrl)
	{
		User? dbUser = await userService.GetRegisteredWith(
			nameof(Domain.Core.User.PhoneNumber), model.PhoneNumber);

		if (dbUser is null)
			return Unauthorized("User with entered phone number is not registered");

		if (!userService.PasswordsMatch(dbUser.Password, model.Password))
			return Unauthorized("Password is wrong");

		ClaimsPrincipal principal = userService.GetPrincipal(dbUser);
		await HttpContext.SignInAsync(principal);

		return (string.IsNullOrWhiteSpace(returnUrl)) is true ?
				Ok("Driver has signed in") :
				LocalRedirectPermanent(returnUrl);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status301MovedPermanently)]
	public async Task<IActionResult> DriverLogin([FromForm] LoginModel model, string? returnUrl)
	{
		Driver? dbDriver = await driverService.GetRegisteredWith(
			nameof(Driver.PhoneNumber), model.PhoneNumber);

		if (dbDriver is null)
			return Unauthorized("Driver with entered phone number is not registered");

		if (!driverService.PasswordsMatch(dbDriver.Password, model.Password))
			return Unauthorized("Password is wrong");

		ClaimsPrincipal principal = driverService.GetPrincipal(dbDriver);
		await HttpContext.SignInAsync(principal);

		return (string.IsNullOrWhiteSpace(returnUrl)) is true ?
				Ok("Driver has signed in") :
				LocalRedirectPermanent(returnUrl);
	}

	[HttpPost]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	public async Task<IActionResult> Logout()
	{
		await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		return Ok("User has been logged out successfully");
	}

	[HttpHead]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public IActionResult AccessDenied() =>
		StatusCode(StatusCodes.Status403Forbidden);

	[HttpHead]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult IsAuthenticated()
	{
		IIdentity? identity = HttpContext.User.Identity;
		return identity is null || !identity.IsAuthenticated ?
			Unauthorized() : Ok();
	}

	[HttpHead]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	public IActionResult IsDriver() =>
		HttpContext.User.IsInRole(typeof(Driver).Name) is true ?
			Ok() : Forbid();
}