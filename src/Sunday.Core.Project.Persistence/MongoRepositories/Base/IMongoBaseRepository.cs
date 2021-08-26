using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sunday.Core.Persistence.MongoRepository
{
    public interface IMongoBaseRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);

        Task<TEntity> GetAsync(int Id);

        Task<List<TEntity>> GetListAsync();
    }
}