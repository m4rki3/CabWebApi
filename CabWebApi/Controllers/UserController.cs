using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
	private readonly IModelService<User> modelService;
	private readonly IAccountService<User> userService;
	private readonly IAccountService<Driver> driverService;
	public UserController(
		IModelService<User> modelService,
		IAccountService<User> userService,
		IAccountService<Driver> driverService)
	{
		this.modelService = modelService;
		this.userService = userService;
		this.driverService = driverService;
	}

	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get()
	{
		List<User> dbUsers = await modelService.GetAllAsync();

		if (dbUsers.IsNullOrEmpty())
			return NotFound("Users are not found in database");

		return Ok(dbUsers);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(int id)
	{
		User? dbUser = await modelService.GetAsync(id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		return Ok(dbUser);
	}
	[HttpPut("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(int id, UserModel model)
	{
		User? dbUser = await modelService.GetAsync(id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		if (model.PhoneNumber != dbUser.PhoneNumber)
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

		UserBuilder builder = new(dbUser);
		builder.Named(model.Name)
			   .HasPhoneNumber(model.PhoneNumber)
			   .HasEmail(model.Email)
			   .HasPassword(model.Password)
			   .HasBirthDate(model.BirthDate);
		dbUser = builder.Build();
		await modelService.UpdateAsync(dbUser);

		return Ok(dbUser);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> Delete(int id)
	{
		User? dbUser = await modelService.GetAsync(id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		await modelService.DeleteAsync(dbUser);

		return Ok("User is deleted successfully");
	}
}