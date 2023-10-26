using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Services.Interfaces;
public interface IUserService : IModelService<User>
{
    Task<bool> IsRegistered(string phoneNumber);
    Task<(bool, User?)> TryGetRegistered(string phoneNumber);
    User FromModel(string name, string phoneNumber,
        string email, string password, DateTime birthDate);
    ClaimsPrincipal GetPrincipal(User user);
}
