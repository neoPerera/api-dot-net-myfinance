using MyFinanceService.CORE.Entities;
using MyFinanceService.CORE.Interfaces;
using MyFinanceService.INFRASTRUCTURE.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyFinanceService.INFRASTRUCTURE.Repositories
{
    public class CommonRepository : ICommonRepository 
    {
        private readonly MyFinanceDbContext _context;

        public CommonRepository(MyFinanceDbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(
            Expression<Func<TEntity, bool>>? filter = null,
            Expression<Func<TEntity, string>>? joins = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
            where TEntity : class
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            IQueryable<TEntity> query = dbSet;

            // Apply Includes
            foreach (var include in includes)
                query = query.Include(include);

            // Apply Filtering
            if (filter != null)
                query = query.Where(filter);

            // Apply Ordering
            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }

        public async Task SaveAsync<TEntity>(TEntity entity) where TEntity : class
        {
            DbSet<TEntity> dbSet = _context.Set<TEntity>();
            await dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GetSequenceAsync(string sequenceName, int padding)
        {
            var nextSequenceValue = await _context.Database.SqlQueryRaw<string>("SELECT LPAD(nextval('"+sequenceName+"')::TEXT, "+padding+", '0') as \"Value\"").FirstOrDefaultAsync();
            return nextSequenceValue.ToString();
        }
        public async Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(
            Expression<Func<TEntity, bool>>? filter = null
        ) where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();

            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "A filter expression is required.");

            return await dbSet.FirstOrDefaultAsync(filter);
        }
        public async Task<TEntity?> GetByIdAsync<TEntity>(string id) where TEntity : class
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
        }

    }
}
