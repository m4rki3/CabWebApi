using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;

namespace CabWebApi.Domain.Interfaces;
public interface IModelService<TModel>
	where TModel : class
{
	IModelRepository<TModel> Repository { get; }
	IDistributedCache Cache { get; }
	Task<List<TModel>> GetAllAsync() =>
		Repository.GetAllAsync();

	Task<List<TModel>> GetAllWithAsync(string propertyName, object? propertyValue) =>
		Repository.GetAllWithAsync(propertyName, propertyValue);

	Task<List<TModel>> GetAllWithAsync(string propertyName, IEnumerable<object> propertyValues) =>
		Repository.GetAllWithAsync(propertyName, propertyValues);

	ValueTask<TModel?> GetAsync(int id) =>
		Repository.GetAsync(id);

	ValueTask<TModel?> GetFromCacheAsync(int id)
	{
		char[] modelNameId = typeof(TModel).Name.ToCharArray();
		string cacheModelId = string.Empty;
	
		for (int i = 0; i < modelNameId.Length; i++)
			cacheModelId += ((int)modelNameId[i]).ToString();
	
		cacheModelId += id.ToString();
	
		string? cacheModel = Cache.GetString(cacheModelId);
		if (cacheModel is not null)
		{
			byte[] modelBytes = Convert.FromBase64String(cacheModel);
			using MemoryStream stream = new(modelBytes);
			return JsonSerializer.DeserializeAsync<TModel>(stream);
		}
		TModel? dbModel = Repository.GetAsync(id).Result;
		if (dbModel is not null)
		{
			string serializedModel = JsonSerializer.Serialize(
				dbModel,
				new JsonSerializerOptions()
				{ ReferenceHandler = ReferenceHandler.IgnoreCycles }
			);
			Cache.SetString(
				cacheModelId,
				serializedModel,
				new DistributedCacheEntryOptions()
				{ SlidingExpiration = TimeSpan.FromMinutes(5) }
			);
		}
		return ValueTask.FromResult(dbModel);
	}
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