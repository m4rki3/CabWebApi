using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
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
        Repository.GetAll();

    Task<List<TModel>> GetAllAsync(string propertyName, object? value) =>
        Repository.GetAll(propertyName, value);

    ValueTask<TModel?> GetAsync(int id) =>
        Repository.Get(id);

    Task UpdateAsync(TModel model)
    {
        Repository.Update(model);
        return Repository.SaveChangesAsync();
    }
    Task DeleteAsync(TModel model)
    {
        Repository.Delete(model);
        return Repository.SaveChangesAsync();
    }
    void Create(TModel model)
    {
        //int? modelId = (int?)model.GetType()
        //                          .GetProperty("Id", typeof(int))?
        //                          .GetValue(model);
        //int toCreateId = modelId ?? -1;
        //if (toCreateId == -1 || Repository.Get(toCreateId) is null)
        //{
        //    Repository.Create(model);
        //    Repository.SaveChanges();
        //    return true;
        //}
        //return false;
        Repository.Create(model);
        Repository.SaveChangesAsync();
    }
}
