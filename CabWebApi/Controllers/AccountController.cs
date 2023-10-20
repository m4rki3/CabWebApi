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
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace CabWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private ISearchService searchService;
    private IUserService userService;
    public AccountController(ISearchService searchService, IUserService userService)
    {
        this.searchService = searchService;
        this.userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        var getUsersTask = userService.GetAllAsync();
        await getUsersTask;

        List<User> users = getUsersTask.Result;
        if (users.IsNullOrEmpty())
            return NotFound();

        return Ok(users);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var getUserTask = userService.GetAsync(id);
        await getUserTask;

        User? user = getUserTask.Result;
        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromForm] UserRegistrationModel model)
    {
        // хэширование
        User user = new()
        {
            Name = model.Name,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Password = model.Password,
            BirthDate = model.BirthDate
        };
        var getUsersTask = userService.GetAllAsync(nameof(user.PhoneNumber), user.PhoneNumber);
        await getUsersTask;

        User? userFromDb = getUsersTask.Result.SingleOrDefault();
        if (userFromDb is not null)
            return BadRequest("User is already registered");

        userService.Create(user);
        return Ok();
    }

    [HttpPost(Name = "/login")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Login([FromForm] UserLoginModel model, string? returnUrl)
    {
        //User? user = userService.GetAllAsync(nameof(user.PhoneNumber), model.PhoneNumber)
        //                        .FirstOrDefault();
        User? user;
        var getUserTask = userService.GetAllAsync(nameof(user.PhoneNumber), model.PhoneNumber);
        await getUserTask;

        user = getUserTask.Result.SingleOrDefault();
        if (user is null)
            return NotFound();

        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToShortDateString())
        };
        ClaimsIdentity identity = new(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );
        return LocalRedirect(returnUrl ?? "/");
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status302Found)]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }

    //[HttpPut]
    //public IActionResult Update(User user) =>
    //    userService.TryUpdate(user, user.Id) is false ? BadRequest() : Ok();

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(User user)
    {
        var getUserTask = userService.GetAsync(user.Id);
        await getUserTask;

        User? userFromDb = getUserTask.Result;
        if (userFromDb is null)
            return NotFound();

        userFromDb = user;
        await userService.UpdateAsync(userFromDb);
        return Ok(userFromDb);
    }
    // попробовать реализовать обновление свойства объекта
    //public Task<IActionResult> Update(User user, Expression<Func<User, object?>> update, object? value)

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(User user)
    {
        var getUserTask = userService.GetAsync(user.Id);
        await getUserTask;

        User? userFromDb = getUserTask.Result;
        if (userFromDb is null)
            return NotFound();

        await userService.DeleteAsync(userFromDb);
        return Ok();
    }

    [HttpGet("/accessdenied")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public IActionResult AccessDenied(string? returnUrl) =>
        StatusCode(statusCode: 403, returnUrl);
}