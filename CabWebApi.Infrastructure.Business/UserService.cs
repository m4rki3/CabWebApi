
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Security.Claims;

namespace CabWebApi.Infrastructure.Business;
public class UserService : IUserService
{
    private readonly IModelRepository<User> repository;
    public IModelRepository<User> Repository => repository;
    public UserService(IModelRepository<User> repository) =>
        this.repository = repository;

    public Task<bool> IsRegistered(string phoneNumber) =>
        repository.GetAllWithAsync(nameof(User.PhoneNumber), phoneNumber)
                  .ContinueWith(task =>
                  {
                      List<User> dbUsers = task.Result;
                      return dbUsers.Count == 0 ? false : true;
                  });

    public Task<(bool, User?)> TryGetRegistered(string phoneNumber) =>
        repository.GetAllWithAsync(nameof(User.PhoneNumber), phoneNumber)
                  .ContinueWith(task =>
                  {
                      User? dbUser = task.Result.SingleOrDefault();
                      return dbUser is null ? (false, dbUser) : (true, dbUser);
                  });

    public User FromModel(
        string name, string phoneNumber,
        string email, string password, DateTime birthDate) =>
        new User()
        {
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password,
            BirthDate = birthDate
        };

    public ClaimsPrincipal GetPrincipal(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToShortDateString())
        };
        ClaimsIdentity identity = new(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}