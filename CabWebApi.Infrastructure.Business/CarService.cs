using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CarWebApi.Infrastructure.Business;
public class CarService : ICarService
{
	private readonly IModelRepository<Car> repository;
	private readonly IDistributedCache cache;
	public IModelRepository<Car> Repository => repository;
	public IDistributedCache Cache => cache;
	public CarService(IModelRepository<Car> repository, IDistributedCache cache)
	{
		this.repository = repository;
		this.cache = cache;
	}

	public Task<List<Car>> GetAwaitingCarsAsync() =>
		repository.GetAllWithAsync(nameof(Car.Status), CarStatus.Awaiting);

	public Task<List<Car>> GetCarsInWorkAsync() =>
		repository.GetAllWithAsync(nameof(Car.Status), CarStatus.InWork);
}