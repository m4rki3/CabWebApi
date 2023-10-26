using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;

namespace CabWebApi.Services.Interfaces;
public interface ICarService
{
	Task<List<Car>> GetAwaitingCarsAsync();
	Task<List<Car>> GetCarsInWorkAsync();
}