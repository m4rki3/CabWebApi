using CabWebApi.Content.Builders;
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly IModelService<User> userService;
    public UserController(IModelService<User> userService)
    {
		this.userService = userService;
    }

    [HttpGet]
	[ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAll()
	{
		List<User> dbUsers = await userService.GetAllAsync();

		if (dbUsers.IsNullOrEmpty())
			return NotFound("Users are not found in database");

		return Ok(dbUsers);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get(int id)
	{
		User? dbUser = await userService.GetAsync(id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		return Ok(dbUser);
	}
	[HttpPut]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
	public async Task<IActionResult> Update(User user)
	{
		User? dbUser = await userService.GetAsync(user.Id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		UserBuilder builder = new(dbUser);
		builder.Named(user.Name)
			   .HasPhoneNumber(user.PhoneNumber)
			   .HasEmail(user.Email)
			   .HasPassword(user.Password)
			   .HasBirthDate(user.BirthDate);
		dbUser = builder.Build();
		await userService.UpdateAsync(dbUser);

		return Ok(dbUser);
	}

	[HttpDelete]
	[ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> Delete(User user)
	{
		User? dbUser = await userService.GetAsync(user.Id);

		if (dbUser is null)
			return NotFound("User with requested id is not found in database");

		await userService.DeleteAsync(dbUser);

		return Ok("User is deleted successfully");
	}
}
