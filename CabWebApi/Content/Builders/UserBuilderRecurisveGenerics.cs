using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public abstract class UserBuilder<TUser, TSelf>
    where TUser : User, new()
    where TSelf : UserBuilder<TUser, TSelf>
{
    protected readonly TUser user;
    public UserBuilder()
    {
        user = new();
    }
    public UserBuilder(TUser user)
    {
        this.user = user;
    }
    public TUser Build() => user;
    public TSelf Named(string name)
    {
        user.Name = name;
        return (TSelf)this;
    }
    public TSelf HasPhoneNumber(string phoneNumber)
    {
        user.PhoneNumber = phoneNumber;
        return (TSelf)this;
    }
    public TSelf HasEmail(string email)
    {
        user.Email = email;
        return (TSelf)this;
    }
    public TSelf HasPassword(string password)
    {
        user.Password = password;
        return (TSelf)this;
    }
    public TSelf HasBirthDate(DateTime birthDate)
    {
        user.BirthDate = birthDate;
        return (TSelf)this;
    }
}