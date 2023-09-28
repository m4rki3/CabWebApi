using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;

namespace CarWebApi.Infrastructure.Business
{
    public class SearchService
    {
        private readonly IModelRepository<Car> cars;
        public SearchService(IModelRepository<Car> cars)
        {
            this.cars = cars;
        }
    }
}