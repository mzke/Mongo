using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Mzke.Mongo
{
    public abstract class MongoServiceBase<T> : IMongoServiceBase<T>
    {
        public  readonly IMongoCollection<T> _collection;

        public MongoServiceBase(MongoSettings settings)
        {
            // https://brahimkamel.wordpress.com/2016/05/28/configure-camel-case-resolver-for-mongodb-c-driver/
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);
            MongoClient client = new(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public async Task<List<T>> GetAsync() => await _collection.Find(model => true).ToListAsync();

        public async Task< List<T>> GetAsync(FilterDefinition<T> filtro)
        {
            return await _collection.Find<T>(filtro).ToListAsync();
        }

        public async Task<T> GetAsync(string id)
        {
            var IdField = new StringFieldDefinition<T, string>("Id");
            var idFilter = Builders<T>.Filter.Eq(IdField, id);
            return await _collection.Find<T>(idFilter).FirstOrDefaultAsync();
        }

        private async Task<MongoServiceResultModel<T>> CreateAsync(T model)
        {
            try
            {
                await _collection.InsertOneAsync(model);
                return new MongoServiceResult<T>().Model;
            }
            catch (Exception ex)
            {
                return new MongoServiceResult<T>(ex).Model;
            }
        }
       
        private async Task<MongoServiceResultModel<T>> UpdateAsync(string id, T modelIn)
        {
            var IdField = new StringFieldDefinition<T, string>("Id");
            var idFilter = Builders<T>.Filter.Eq(IdField, id);
            try { 
                await _collection.ReplaceOneAsync(idFilter, modelIn);
                return new MongoServiceResult<T>().Model;
            }
            catch (Exception ex)
            {
                return new MongoServiceResult<T>(ex).Model;
            }
        }

        public async Task<MongoServiceResultModel<T>> SaveAsync(T model)
        {
            
            var val = Validate(model);
            if (val.Sucesso)
            {
                var id = GetId(model);
                if (id is not null)
                {
                    return await UpdateAsync(id, model);
                }
                else
                    return await CreateAsync(model);
            }
            else
                return val;
        }

        public MongoServiceResultModel<T> Delete(T modelIn)
        {
            var result = new MongoServiceResult<T>();
            var id = GetId(modelIn);
            if (id is null)
            {
                result.Fracasso("id é nulo.");
                return result.Model;
            }
            var IdField = new StringFieldDefinition<T, string>("Id");
            var idFilter = Builders<T>.Filter.Eq(IdField, id);
            try
            {
                _collection.DeleteOne(idFilter);
            }
            catch (Exception ex)
            {
                result.Fracasso(ex);
            }
            return result.Model;
        }

        private static string? GetId(T modelIn)
        {
            return modelIn?.GetType().GetProperty("Id")?.GetValue(modelIn, null)?.ToString();
        }

        public async Task<MongoServiceResultModel<T>> DeleteAsync(string id)
        {
            var IdField = new StringFieldDefinition<T, string>("Id");
            var idFilter = Builders<T>.Filter.Eq(IdField, id);
            try
            {
                await _collection.DeleteOneAsync(idFilter);
                return new MongoServiceResult<T>().Model;
            }catch(Exception ex)
            {
                return new MongoServiceResult<T>(ex).Model;
            }
        }

        public virtual MongoServiceResultModel<T> Validate(T model)
        {
            if (model is null)
            {
                var failResult = new MongoServiceResult<T>("Model não pode ser nulo");
                return failResult.Model;
            }
            return new MongoServiceResult<T>().Model;
        }
    }
}

