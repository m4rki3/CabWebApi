namespace CabWebApi.Content.Builders;
public class OrderLocationBuilder : OrderBuilder
{
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