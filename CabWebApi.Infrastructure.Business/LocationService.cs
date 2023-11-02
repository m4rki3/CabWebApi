using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Infrastructure.Business;
public class LocationService : IModelService<Location>
{
	private readonly IModelRepository<Location> repository;
	public IModelRepository<Location> Repository => repository;
    public LocationService(IModelRepository<Location> repository)
    {
        this.repository = repository;
    }
}