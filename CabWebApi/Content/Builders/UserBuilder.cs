using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class UserBuilder : UserBuilder<User, UserBuilder>
{
	public UserBuilder() : base() { }
	public UserBuilder(User user) : base(user) { }
}