using Microsoft.EntityFrameworkCore;
using CabWebApi.Domain.Interfaces;

namespace CabWebApi.Infrastructure.Data
{
    public class ModelRepository<TModel> : IModelRepository<TModel>
        where TModel : class
    {
        private readonly DbContext context;
        public ModelRepository(DbContext context)
        {
            this.context = context;
        }
        public void Create(TModel model)
        {
            context.Set<TModel>().Add(model);
        }
        public void Delete(TModel model)
        {
            context.Set<TModel>().Remove(model);
        }
        public IEnumerable<TModel> GetAll() =>
            context.Set<TModel>().ToList();

        public IEnumerable<TModel> GetAll(string propertyName, object? value) =>
            context.Set<TModel>()
                   .Select(model => model)
                   .Where(new Func<TModel, bool>((model) =>
                   {
                       var property = model.GetType()
                                           .GetProperty(propertyName);
                       return property?.GetValue(model) == value;
                   }));

        public TModel? Get(int id) =>
            context.Set<TModel>().Find(id);

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public void Update(TModel model)
        {
            context.Set<TModel>().Update(model);
        }
    }
}