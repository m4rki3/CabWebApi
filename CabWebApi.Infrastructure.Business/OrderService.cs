using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Business;
public class OrderService : IOrderService
{
	private readonly IModelRepository<Order> repository;
	private const float carsRatio = 0.3f;
	private const int carsRatioPrice = 250;
	private const int travelTimePrice = 9;
	private const int weatherPrice = 50;
	public IModelRepository<Order> Repository => repository;
	public OrderService(IModelRepository<Order> repository)
	{
		this.repository = repository;
	}
	public Task<List<Order>> GetOrdersInExecution()
	{
		var orderStatuses = Enum.GetValues<OrderStatus>();
		List<OrderStatus> inExecution = new();

		foreach (var orderStatus in orderStatuses)
			if (orderStatus != OrderStatus.Completed &&
				orderStatus != OrderStatus.Canceled)
				inExecution.Add(orderStatus);

		IEnumerable<object> inExecutionStatuses = inExecution.OfType<object>();
		return repository.GetAllWithAsync(nameof(Order.Status), inExecutionStatuses);
	}
	public Task<int> GetPriceAsync(
		int avaliableCarsCount, int carsInWorkCount, bool badWeather,
		int travelTime, params int[] travelTimeToClient)
	{
		Func<int> getPrice = () =>
		{
			double averageTimeToClient = travelTimeToClient.Average();
			double carsCoef;
			if (carsInWorkCount == 0)
				carsCoef = -100;
			else
			{
				carsCoef = (carsRatio - avaliableCarsCount / carsInWorkCount) * carsRatioPrice;
				if (carsCoef > 100)
					carsCoef = 100;
				else if (carsCoef < -100)
					carsCoef = -100;
			}
			double timeCoef = (travelTime + averageTimeToClient) * travelTimePrice;
			int weatherCoef = badWeather is true ? weatherPrice : -weatherPrice;

			return (int)(carsCoef + timeCoef + weatherCoef);
		};
		return Task.Run(getPrice);
	}
}