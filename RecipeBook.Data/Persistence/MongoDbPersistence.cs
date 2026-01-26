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

        private string GetCollectionName<T>() where T : EntityBase
        {
            return typeof(T).Name switch
            {
                nameof(RecipeEntity) => DatabaseConfiguration.RecipesCollectionName,
                nameof(CategoryEntity) => DatabaseConfiguration.CategoriesCollectionName,
                _ => throw new NotSupportedException($"Entity type {typeof(T).Name} is not supported")
            };
        }

        public async Task DeleteEntity<T>(string id, string userId) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            // Delete only if both id AND userId match (security)
            var filter = Builders<T>.Filter.And(
                Builders<T>.Filter.Eq("Id", id),
                Builders<T>.Filter.Eq("UserId", userId)
            );

            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new UnauthorizedAccessException("Entity not found or access denied");
            }
        }

        public async Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filterExpression) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            var entities = await collection.FindAsync(filterExpression);

            return await entities.ToListAsync();
        }

        public async Task<IEnumerable<T2>> GetProjectedEntities<T, T2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T2>> projectionExpression) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            var entities = await collection.Find(filterExpression).Project(projectionExpression).ToListAsync();

            return entities;
        }

        public async Task<T> GetEntity<T>(string id) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            var entity = await collection.FindAsync(x => x.Id.ToString() == id);

            return await entity.FirstOrDefaultAsync();
        }

        public async Task Persist<T>(T entity) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            await collection.InsertOneAsync(entity);
        }

        public async Task Update<T>(string id, T entity) where T : EntityBase
        {
            var collection = Database.GetCollection<T>(GetCollectionName<T>());

            await collection.ReplaceOneAsync(x => x.Id.ToString() == id, entity);
        }
    }
}
