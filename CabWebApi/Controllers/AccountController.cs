using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Infrastructure.Business;
using CabWebApi.Models;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CabWebApi.Controllers
{
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
        public IActionResult GetAll()
        {
            var users = userService.Get();
            return users.FirstOrDefault() is null ? NotFound() : Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var user = userService.Get(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public IActionResult Register([FromForm] UserRegistrationModel model)
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
            User? fromRepository = userService.GetAll(nameof(user.PhoneNumber), user.PhoneNumber)
                                              .FirstOrDefault();
            if (user is not null)
                return BadRequest("User is already registered");
            
            return ValidationProblem();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromForm] UserLoginModel model, string? returnUrl)
        {
            User? user = userService.GetAll(nameof(user.PhoneNumber), model.PhoneNumber)
                                    .FirstOrDefault();
            if (user is null)
                return NotFound("User hasn't been found");
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
            return Redirect(returnUrl ?? "/");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }

        //[HttpPut]
        //public IActionResult Update(User user) =>
        //    userService.TryUpdate(user, user.Id) is false ? BadRequest() : Ok();

        [HttpPut]
        public IActionResult Update(User user)
        {
            if (userService.Get(user.Id) is null)
                return NotFound();
            userService.Update(user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(User user)
        {
            if (userService.Get(user.Id) is null)
                return NotFound();
            userService.Delete(user);
            return Ok();
        }
    }
}