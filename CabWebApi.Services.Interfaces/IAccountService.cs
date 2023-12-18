using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Services.Interfaces;

public interface IAccountService<TUser> : IModelService<TUser>
	where TUser : Person
{
	Task<bool> IsRegisteredWith(string propertyName, object? propertyValue);
	Task<TUser?> GetRegisteredWith(string propertyName, object? propertyValue);
	ClaimsPrincipal GetPrincipal(TUser user);
	string HashPassword(string password);
	bool PasswordsMatch(string hashedPassword, string password);
}