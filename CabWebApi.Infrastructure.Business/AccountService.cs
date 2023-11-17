using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Business;

public class AccountService<TUser> : IAccountService<TUser>
	where TUser : Person
{
	private readonly IModelRepository<TUser> repository;
	public IModelRepository<TUser> Repository => repository;
	public AccountService(IModelRepository<TUser> repository)
	{
		this.repository = repository;
	}
	public Task<bool> IsRegisteredWith(string propertyName, object? propertyValue) =>
		repository.GetAllWithAsync(propertyName, propertyValue)
				  .ContinueWith(task =>
				  {
					  List<TUser> dbUsers = task.Result;
					  return dbUsers.Count == 0 ? false : true;
				  });

	public Task<TUser?> GetRegisteredWith(string propertyName, object? propertyValue) =>
		repository.GetAllWithAsync(propertyName, propertyValue)
				  .ContinueWith(task => task.Result.FirstOrDefault());

	public ClaimsPrincipal GetPrincipal(TUser user)
	{
		List<Claim> claims = new()
		{
			new Claim(ClaimTypes.Name, user.Name),
			new Claim(ClaimTypes.Email, user.Email),
			new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToShortDateString()),
			new Claim(ClaimTypes.Role, typeof(TUser).Name)
		};
		ClaimsIdentity identity = new(
			claims, CookieAuthenticationDefaults.AuthenticationScheme
		);
		return new ClaimsPrincipal(identity);
	}
}