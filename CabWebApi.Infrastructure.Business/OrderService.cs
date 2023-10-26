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
    private readonly IModelService<Location> locationService;
    private const float carsRatio = 0.2f;
    private const int carsRatioPrice = 500;
    private const int travelTimePrice = 9;
    private const int weatherPrice = 50;
    public IModelRepository<Order> Repository => repository;
    public OrderService(IModelRepository<Order> repository, IModelService<Location> locationService)
    {
        this.repository = repository;
        this.locationService = locationService;
    }
    public Task<List<Order>> GetOrdersInExecution()
    {
        var orderStatuses = Enum.GetValues<OrderStatus>();
        List<OrderStatus> inExecutionStatuses = new();

        foreach (var orderStatus in orderStatuses)
            if (orderStatus != OrderStatus.Completed &&
                orderStatus != OrderStatus.Canceled)
                    inExecutionStatuses.Add(orderStatus);
        
        Order order;
		return repository.GetAllWithAsync(nameof(order.Status), inExecutionStatuses);
    }
    public Task<int> GetPriceAsync(
        int avaliableCarsCount, int carsInWorkCount, bool badWeather,
		int travelTime, params int[] travelTimeToClient)
    {
        Func<int> getPrice = () =>
        {
			double averageTimeToClient = travelTimeToClient.Average();
			double carsCoef = (carsRatio - avaliableCarsCount / carsInWorkCount) * carsRatioPrice;
			double timeCoef = (travelTime + averageTimeToClient) * travelTimePrice;
			int weatherCoef = badWeather is true ? weatherPrice : -weatherPrice;

            return (int)(carsCoef + timeCoef + weatherCoef);
        };
        return Task.Run(getPrice);
    }

    public Order FromModel(
        int userId, int carId,
        int departureId, int destinationId, int price) =>
        new Order()
        {
            UserId = userId,
            CarId = carId,
            DepartureId = departureId,
            DestinationId = destinationId,
            Status = OrderStatus.Created,
            Price = price
        };

    public Task<(int, int)> CreateLocations(Location departure, Location destination) =>
        locationService.CreateAsync(departure)
                       .ContinueWith(task =>
                       {
                           int destinationId = locationService.CreateAsync(destination)
                                                              .Result.Item1.Entity.Id;
                           int departureId = task.Result.Item1.Entity.Id;
                           return (departureId, destinationId);
                       });
}