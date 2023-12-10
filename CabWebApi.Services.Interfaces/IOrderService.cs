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
}