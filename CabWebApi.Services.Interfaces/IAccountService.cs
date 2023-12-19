using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Services.Interfaces;

public interface IAccountService<TPerson> : IModelService<TPerson>
	where TPerson : Person
{
	Task<bool> IsRegisteredWith(string propertyName, object? propertyValue);
	Task<TPerson?> GetRegisteredWith(string propertyName, object? propertyValue);
	ClaimsPrincipal GetPrincipal(TPerson person);
	string HashPassword(string password);
	bool PasswordsMatch(string hashedPassword, string password);
}