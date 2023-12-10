using CabWebApi.Domain.Core;
using System.Runtime.InteropServices;

namespace CabWebApi.Content.Builders;
public class OrderLocationBuilder : OrderBuilder
{
	public OrderLocationBuilder(Order order) : base(order) { }
	public OrderLocationBuilder From(int departureId)
	{
		order.DepartureId = departureId;
		return this;
	}
	public OrderLocationBuilder To(int destinationId)
	{
		order.DestinationId = destinationId;
		return this;
	}
}