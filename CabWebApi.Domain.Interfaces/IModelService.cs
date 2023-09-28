using CabWebApi.Domain.Core;
using CabWebApi.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CabWebApi.Domain.Interfaces;
public interface IModelService<TModel>
    where TModel : class
{
    IModelRepository<TModel> Repository { get; }
    IEnumerable<TModel> Get() =>
        Repository.GetAll();

    IEnumerable<TModel> GetAll(string propertyName, object? value) =>
        Repository.GetAll(propertyName, value);

    TModel? Get(int id) =>
        Repository.Get(id);

    void Update(TModel model)
    {
        Repository.Update(model);
        Repository.SaveChanges();
    }
    void Delete(TModel model)
    {
        Repository.Delete(model);
        Repository.SaveChanges();
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
        Repository.SaveChanges();
    }
}
