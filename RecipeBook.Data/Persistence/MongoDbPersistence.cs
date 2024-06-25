using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RecipeBook.Data.Configuration;
using RecipeBook.Data.Entities;
using System.Linq.Expressions;

namespace RecipeBook.Data.Persistence
{
    public class MongoDbPersistence : IPersistence
    {
        IMongoDatabase Database { get; set; }
        RecipeBookDatabaseConfiguration DatabaseConfiguration { get; set; }

        public MongoDbPersistence(IOptions<RecipeBookDatabaseConfiguration> recipeDatabaseConfiguration)
        {
            DatabaseConfiguration = recipeDatabaseConfiguration.Value;

            var client = new MongoClient(DatabaseConfiguration.ConnectionString);
            Database = client.GetDatabase(DatabaseConfiguration.DatabaseName);
        }

        public async Task DeleteEntity<T>(string id) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            await collection.DeleteOneAsync(x => x.Id.ToString() == id);
        }

        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filterExpression) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            var entities = await collection.FindAsync(filterExpression);

            return await entities.ToListAsync();
        }

        public async Task<IEnumerable<T2>> GetProjectedEntities<T, T2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T2>> projectionExpression) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            var entities = await collection.Find(filterExpression).Project(projectionExpression).ToListAsync();

            return entities;
        }

        public async Task<T> GetEntity<T>(string id) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            var entity = await collection.FindAsync(x => x.Id.ToString() == id);

            return await entity.FirstOrDefaultAsync();
        }

        public async Task Persist<T>(T entity) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            await collection.InsertOneAsync(entity);
        }

        public async Task Update<T>(string id, T entity) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(DatabaseConfiguration.RecipesCollectionName);

            await collection.ReplaceOneAsync(x => x.Id.ToString() == id, entity);
        }
    }
}
