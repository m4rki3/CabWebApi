using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Services.Interfaces;
public interface IUserService : IModelService<User>
{
    bool IsRegistered(User user);
    bool TryRegister(User user);
}
