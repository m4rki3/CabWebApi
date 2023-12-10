using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class OrderBuilder
{
	protected readonly Order order;
	public OrderLocationBuilder Route => new OrderLocationBuilder(order);
	public OrderConfigBuilder MakeWith => new OrderConfigBuilder(order);
	public OrderBuilder()
	{
		order = new();
	}
	public OrderBuilder(Order order)
	{
		this.order = order;
	}
	public Order Build() => order;
}
