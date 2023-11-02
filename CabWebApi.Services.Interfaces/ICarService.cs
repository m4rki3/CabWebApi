using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;

namespace CabWebApi.Services.Interfaces;
public interface ICarService : IModelService<Car>
{
	Task<List<Car>> GetAwaitingCarsAsync();
	Task<List<Car>> GetCarsInWorkAsync();
}