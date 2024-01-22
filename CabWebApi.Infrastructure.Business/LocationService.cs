using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CabWebApi.Infrastructure.Business;
public class LocationService : IModelService<Location>
{
	private readonly IModelRepository<Location> repository;
	private readonly IDistributedCache cache;
	public IModelRepository<Location> Repository => repository;
	public IDistributedCache Cache => cache;

	public LocationService(IModelRepository<Location> repository, IDistributedCache cache)
	{
		this.repository = repository;
		this.cache = cache;
	}
}