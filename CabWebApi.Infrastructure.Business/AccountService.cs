using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Business;

public class AccountService<TPerson> : IAccountService<TPerson>
	where TPerson : Person
{
	private const int hashSize = 0x31;
	private const int saltSize = 0x10;
	private const int keyBufferSize = 0x20;
	private readonly IModelRepository<TPerson> repository;
	public IModelRepository<TPerson> Repository => repository;
	public AccountService(IModelRepository<TPerson> repository)
	{
		this.repository = repository;
	}
	public Task<bool> IsRegisteredWith(string propertyName, object? propertyValue) =>
		repository.GetAllWithAsync(propertyName, propertyValue)
				  .ContinueWith(task =>
				  {
					  List<TPerson> dbPersons = task.Result;
					  return dbPersons.Count == 0 ? false : true;
				  });

	public Task<TPerson?> GetRegisteredWith(string propertyName, object? propertyValue) =>
		repository.GetAllWithAsync(propertyName, propertyValue)
				  .ContinueWith(task => task.Result.FirstOrDefault());

	public ClaimsPrincipal GetPrincipal(TPerson person)
	{
		List<Claim> claims = new()
		{
			new Claim(ClaimTypes.Name, person.Name),
			new Claim(ClaimTypes.Email, person.Email),
			new Claim(ClaimTypes.DateOfBirth, person.BirthDate.ToShortDateString()),
			new Claim(ClaimTypes.Role, typeof(TPerson).Name)
		};
		ClaimsIdentity identity = new(
			claims, CookieAuthenticationDefaults.AuthenticationScheme
		);
		return new ClaimsPrincipal(identity);
	}
	public string HashPassword(string password)
	{
		if (string.IsNullOrWhiteSpace(password))
			throw new ArgumentNullException("Password must not contain only white spaces");

		byte[] salt;
		byte[] keyBuffer;

		using (Rfc2898DeriveBytes rfc = new(password, saltSize, iterations: 0x3e8))
		{
			salt = rfc.Salt;
			keyBuffer = rfc.GetBytes(keyBufferSize);
		}
		byte[] hash = new byte[hashSize];
		Buffer.BlockCopy(salt, 0, hash, 1, saltSize);
		Buffer.BlockCopy(keyBuffer, 0, hash, saltSize + 0x1, keyBufferSize);
		return Convert.ToBase64String(hash);
	}
	public bool PasswordsMatch(string hashedPassword, string password)
	{
		if (string.IsNullOrWhiteSpace(hashedPassword))
			throw new ArgumentNullException("Hashed password must not contain only white spaces");

		if (string.IsNullOrWhiteSpace(password))
			throw new ArgumentNullException("Password must not contain only white spaces");

		byte[] hash = Convert.FromBase64String(hashedPassword);
		if (hash.Length != hashSize || hash[0] != 0)
			return false;

		byte[] salt = new byte[saltSize];
		byte[] hashKeyBuffer = new byte[keyBufferSize];

		Buffer.BlockCopy(hash, 1, salt, 0, saltSize);
		Buffer.BlockCopy(hash, saltSize + 0x1, hashKeyBuffer, 0, keyBufferSize);

		using Rfc2898DeriveBytes rfc = new(password, salt, 0x3e8);
		byte[] passwordKeyBuffer = rfc.GetBytes(keyBufferSize);

		return Enumerable.SequenceEqual(passwordKeyBuffer, hashKeyBuffer);
	}
}