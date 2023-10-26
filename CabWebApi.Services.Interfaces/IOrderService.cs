using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Services.Interfaces;
public interface IOrderService : IModelService<Order>
{
	Task<List<Order>> GetOrdersInExecution();
	Task<int> GetPriceAsync(
		int avaliableCarsCount, int carsInWorkCount, bool badWeather,
		int travelTime, params int[] travelTimeToClient);
	Order FromModel(int userId, int carId, int departureId, int destinationId, int price);
	Task<(int, int)> CreateLocations(Location departure, Location destination);
}