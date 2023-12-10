using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DriverController : ControllerBase
{
	private readonly IModelService<Driver> modelService;
	private readonly IAccountService<User> userService;
	private readonly IAccountService<Driver> driverService;
	public DriverController(
		IModelService<Driver> modelService,
		IAccountService<User> userService,
		IAccountService<Driver> driverService)
	{
		this.modelService = modelService;
		this.userService = userService;
		this.driverService = driverService;
	}

	[HttpGet]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(List<Driver>), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get()
	{
		List<Driver> dbDrivers = await modelService.GetAllAsync();

		if (dbDrivers.Count == 0)
			return NotFound("Drivers are not found in database");

		return Ok(dbDrivers);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	public async Task<IActionResult> Get(int id)
	{
		Driver? dbDriver = await modelService.GetAsync(id);

		if (dbDriver is null)
			return NotFound("Driver with requested id is not found in database");

		return Ok(dbDriver);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Driver), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, DriverModel model)
	{
		Driver? dbDriver = await modelService.GetAsync(id);

		if (dbDriver is null)
			return NotFound("Driver with requested id is not found in database");

		if (model.PhoneNumber != dbDriver.PhoneNumber)
		{
			bool isRegistered = await userService.IsRegisteredWith(
									nameof(Domain.Core.User.PhoneNumber), model.PhoneNumber)
								||
								await driverService.IsRegisteredWith(
									nameof(Driver.PhoneNumber), model.PhoneNumber);
			if (isRegistered)
				return BadRequest(
					"User or driver with requested phone number had been already registered");
		}
		if (model.DrivingLicense != dbDriver.DrivingLicense)
		{
			bool isRegistered = await driverService.IsRegisteredWith(
				nameof(Driver.DrivingLicense), model.DrivingLicense);

			if (isRegistered)
				return BadRequest("Driver with requested license had been already registered");
		}

		DriverBuilder builder = new(dbDriver);
		builder.Named(model.Name)
			   .HasPhoneNumber(model.PhoneNumber)
			   .HasEmail(model.Email)
			   .HasBirthDate(model.BirthDate)
			   .Earns(model.Salary)
			   .HasLicense(model.DrivingLicense);
		dbDriver = builder.Build();
		await modelService.UpdateAsync(dbDriver);

		return Ok(dbDriver);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	public async Task<IActionResult> Delete(int id)
	{
		Driver? dbDriver = await modelService.GetAsync(id);

		if (dbDriver is null)
			return NotFound("Driver with requested id is not found in database");

		await modelService.DeleteAsync(dbDriver);

		return Ok("Driver is deleted successfully");
	}
}