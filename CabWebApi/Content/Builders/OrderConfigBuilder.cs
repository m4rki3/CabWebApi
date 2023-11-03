using CabWebApi.Domain.Core;

namespace CabWebApi.Content.Builders;
public class OrderConfigBuilder : OrderBuilder
{
	public OrderConfigBuilder User(int userId)
	{
		order.UserId = userId;
		return this;
	}
	public OrderConfigBuilder Car(int carId)
	{
		order.CarId = carId;
		return this;
	}
	public OrderConfigBuilder Price(int price)
	{
		order.Price = price;
		return this;
	}
	public OrderConfigBuilder Status(OrderStatus status)
	{
		order.Status = status;
		return this;
	}
}
