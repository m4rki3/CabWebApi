namespace CabWebApi.Domain.Interfaces;
public interface IModelRepository<TModel>
    where TModel : class
{
    Task<List<TModel>> GetAll();
    Task<List<TModel>> GetAll(string propertyName, object? value);
    ValueTask<TModel?> Get(int id);
    Task Create(TModel model);
    void Update(TModel model);
    void Delete(TModel model);
    Task SaveChangesAsync();
}