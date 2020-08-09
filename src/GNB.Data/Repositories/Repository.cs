using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GNB.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GNB.Data.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly GNBDbContext _context;

        public Repository(GNBDbContext context)
        {
            this._context = context;
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicates) => _context.Set<TEntity>().Where(predicates);

        public IQueryable<TEntity> GetAll() => _context.Set<TEntity>();
        
        public ValueTask<TEntity> GetByIdAsync(TKey id) => _context.FindAsync<TEntity>(id);
     
        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => _context.Set<TEntity>().SingleOrDefaultAsync(predicate);

        public async Task AddAsync(TEntity entity) => await _context.AddAsync(entity);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _context.AddRangeAsync(entities);

        public void Remove(TEntity entity) => _context.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
