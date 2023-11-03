using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class OrderBuilder
{
	protected readonly Order order;
    public OrderLocationBuilder Route => new OrderLocationBuilder();
    public OrderConfigBuilder MakeWith => new OrderConfigBuilder();
    public OrderBuilder()
    {
        order = new();
    }
    public Order Build() => order;
}
