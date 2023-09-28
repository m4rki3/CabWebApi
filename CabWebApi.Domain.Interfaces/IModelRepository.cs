namespace CabWebApi.Domain.Interfaces;
public interface IModelRepository<TModel>
    where TModel : class
{
    IEnumerable<TModel> GetAll();
    IEnumerable<TModel> GetAll(string propertyName, object? value);
    TModel? Get(int id);
    void Create(TModel model);
    void Update(TModel model);
    void Delete(TModel model);
    void SaveChanges();
}