using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private IUserService userService;
    public AccountController(IUserService userService)
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
            return NotFound("User with the same Id is not found in database");

        return Ok(dbUser);
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromForm] UserRegistrationModel model)
    {
        // хэширование
        bool userIsRegistered = await userService.IsRegistered(model.PhoneNumber);

        if (userIsRegistered)
            return BadRequest("User with this phone number had been already registered");

		User user = userService.FromModel(
        	model.Name, model.PhoneNumber, model.Email, model.Password, model.BirthDate
        );

		User created = (await userService.CreateAsync(user)).Item1.Entity;

        return CreatedAtAction(nameof(Register), created);
    }

    [HttpPost(Name = "/login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromForm] UserLoginModel model, string? returnUrl)
    {
        var userIsRegistered = await userService.TryGetRegistered(model.PhoneNumber);

        if (!userIsRegistered.Item1)
            return BadRequest("User with this phone number has not been registered yet");

        userIsRegistered.Item2 = null!;
        User dbUser = userIsRegistered.Item2;
        if (dbUser.Password != model.Password)
            return ValidationProblem("Passwords are not match");

        ClaimsPrincipal principal = userService.GetPrincipal(dbUser);
        await HttpContext.SignInAsync(principal);

        return LocalRedirect(returnUrl ?? "/");
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/login");
    }

    [HttpPut]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(User user)
    {
        User? dbUser = await userService.GetAsync(user.Id);

        if (dbUser is null)
            return NotFound("User with the same Id is not found in database");

        // проблема с копированием
        // каждый раз обновлять индекс номера телефона - затраты по производительности
        // но ведь база данных сама решает, делать ли запрос на обновление, получая те же данные
        // значит проблемы перезаписи нет?
        dbUser = user;
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
            return NotFound("User with the same Id is not found in database");

        await userService.DeleteAsync(dbUser);

        return Ok("User is deleted successfully");
    }

    [HttpGet("/accessdenied")]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public IActionResult AccessDenied(string? returnUrl) =>
        StatusCode(statusCode: 403, returnUrl);
}