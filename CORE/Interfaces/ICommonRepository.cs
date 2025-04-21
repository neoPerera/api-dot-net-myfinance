using System.Linq.Expressions;

namespace CORE.Interfaces
{
    public interface ICommonRepository
    {
        Task<List<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, string>>? joins = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class;
        
        Task SaveAsync<TEntity>(TEntity entity) where TEntity : class;
        
        Task<string> GetSequenceAsync(string sequenceName, int padding);
        
        Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>>? filter = null
        ) where TEntity : class;
    }
}
