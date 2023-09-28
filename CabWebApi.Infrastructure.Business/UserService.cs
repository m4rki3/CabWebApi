
using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;

namespace CabWebApi.Infrastructure.Business;
public class UserService : IUserService
{
    private readonly IModelRepository<User> repository;
    public IModelRepository<User> Repository => repository;
    public UserService(IModelRepository<User> repository) =>
        this.repository = repository;

    public bool IsRegistered(User user)
    {
        User? repositoryUser = repository.GetAll("PhoneNumber", user.PhoneNumber)
                                         .FirstOrDefault();
        return repositoryUser is not null ? true : false;
    }
    // доделать метод проблема с уникальностью номера телефона
    public bool TryRegister(User user)
    {
        if (repository.Get(user.Id) is null)
        {
            repository.Create(user);
            repository.SaveChanges();
            return true;
        }
        return false;
    }
}
