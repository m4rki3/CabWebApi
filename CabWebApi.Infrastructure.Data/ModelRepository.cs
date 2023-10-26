using Microsoft.EntityFrameworkCore;
using CabWebApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CabWebApi.Domain.Core;

namespace CabWebApi.Infrastructure.Data;
// решить проблему с асинхронностью, настроить реализацию
public class ModelRepository<TModel> : IModelRepository<TModel>
    where TModel : class
{
    private readonly DbContext context;
    public ModelRepository(DbContext context)
    {
        this.context = context;
    }

    public ValueTask<EntityEntry<TModel>> CreateAsync(TModel model) =>
        context.Set<TModel>().AddAsync(model);

    public void Delete(TModel model) =>
        context.Set<TModel>().Remove(model);

    public Task<List<TModel>> GetAllAsync() =>
        context.Set<TModel>().ToListAsync();

    public Task<List<TModel>> GetAllWithAsync(string propertyName, params object?[] propertyValues) =>
        context.Set<TModel>()
               .Select(model => model)
               .Where(new Func<TModel, bool>((model) =>
               {
                   var property = model.GetType()
                                       .GetProperty(propertyName);
                   foreach (var propertyValue in propertyValues)
                       if (property?.GetValue(model) == propertyValue)
                           return true;
                   return false;
               }))
               .AsQueryable()
               .ToListAsync();

    public ValueTask<TModel?> GetAsync(int id) =>
        context.Set<TModel>().FindAsync(id);

    public Task<int> SaveChangesAsync() =>
        context.SaveChangesAsync();

    public void Update(TModel model) =>
        context.Set<TModel>().Update(model);
}