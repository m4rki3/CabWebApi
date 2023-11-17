using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Interfaces;
// настроить асинхронность
public interface IModelService<TModel>
	where TModel : class
{
	IModelRepository<TModel> Repository { get; }
	Task<List<TModel>> GetAllAsync() =>
		Repository.GetAllAsync();

	Task<List<TModel>> GetAllWithAsync(string propertyName, object? propertyValue) =>
		Repository.GetAllWithAsync(propertyName, propertyValue);

	Task<List<TModel>> GetAllWithAsync(string propertyName, IEnumerable<object> propertyValues) =>
		Repository.GetAllWithAsync(propertyName, propertyValues);

	ValueTask<TModel?> GetAsync(int id) =>
		Repository.GetAsync(id);

	Task<int> UpdateAsync(TModel model)
	{
		Repository.Update(model);
		return Repository.SaveChangesAsync();
	}
	Task<int> DeleteAsync(TModel model)
	{
		Repository.Delete(model);
		return Repository.SaveChangesAsync();
	}
	Task<(EntityEntry<TModel>, int)> CreateAsync(TModel model) =>
		Repository.CreateAsync(model)
				  .AsTask()
				  .ContinueWith(createTask =>
				  {
					  int savedEntries = Repository.SaveChangesAsync().Result;
					  return (createTask.Result, savedEntries);
				  });
}