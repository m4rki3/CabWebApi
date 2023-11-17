using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CabWebApi.Domain.Interfaces;
public interface IModelRepository<TModel>
	where TModel : class
{
	Task<List<TModel>> GetAllAsync();
	Task<List<TModel>> GetAllWithAsync(string propertyName, object? propertyValue);
	Task<List<TModel>> GetAllWithAsync(string propertyName, IEnumerable<object> propertyValues);
	ValueTask<TModel?> GetAsync(int id);
	ValueTask<EntityEntry<TModel>> CreateAsync(TModel model);
	void Update(TModel model);
	void Delete(TModel model);
	Task<int> SaveChangesAsync();
}