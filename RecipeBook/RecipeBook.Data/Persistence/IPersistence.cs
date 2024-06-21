using RecipeBook.Data.Entities;
using System.Linq.Expressions;

namespace RecipeBook.Data.Persistence
{
    public interface IPersistence
    {
        Task Persist<T>(T entity) where T : EntityBase;
        Task<T> GetEntity<T>(string id) where T : EntityBase;
        Task<IEnumerable<T>> GetEntities<T>(Expression<Func<T, bool>> filterExpression) where T : EntityBase;
        Task DeleteEntity<T>(string id) where T : EntityBase;
        Task Update<T>(string id, T entity) where T : EntityBase;
    }
}
