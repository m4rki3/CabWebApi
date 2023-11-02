using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using CabWebApi.Services.Interfaces;

namespace CarWebApi.Infrastructure.Business;
public class CarService : ICarService
{
    private readonly IModelRepository<Car> repository;
    public IModelRepository<Car> Repository => repository;
	public CarService(IModelRepository<Car> repository)
    {
        this.repository = repository;
    }

	public Task<List<Car>> GetAwaitingCarsAsync() =>
        repository.GetAllWithAsync(nameof(Car.Status), CarStatus.Awaiting);

    public Task<List<Car>> GetCarsInWorkAsync() =>
        repository.GetAllWithAsync(nameof(Car.Status), CarStatus.InWork);
}