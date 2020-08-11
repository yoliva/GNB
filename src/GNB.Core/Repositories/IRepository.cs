using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GNB.Core.Repositories
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicates);
        IQueryable<TEntity> GetAll();
        
        ValueTask<TEntity> GetByIdAsync(TKey id);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task Truncate();
    }
}
