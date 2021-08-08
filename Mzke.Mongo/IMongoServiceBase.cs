using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Mzke.Mongo
{
    public interface IMongoServiceBase<T>
    {
        Task<List<T>> GetAsync();
        Task<List<T>> GetAsync(FilterDefinition<T> filtro);
        Task<T> GetAsync(string id);
        //Task<MongoServiceResultModel<T>> CreateAsync(T model);
        Task<MongoServiceResultModel<T>> SaveAsync(T model);
        //Task<MongoServiceResultModel<T>> UpdateAsync(string id, T modelIn);
        MongoServiceResultModel<T> Delete(T modelIn);
        Task<MongoServiceResultModel<T>> DeleteAsync(string id);
        MongoServiceResultModel<T> Validate(T model);
    }
}
