using System;
using DataLayer.Abstract;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync([id], cancellationToken);
            if (entity is null) throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} not found.");
            return entity;
        }

        public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is null)throw new ArgumentNullException(nameof(entity));

            var existingEntity = await _dbSet.FindAsync([_context.Entry(entity).Property("Id").CurrentValue], cancellationToken);
            if (existingEntity is not null)
            {
                _dbSet.Remove(existingEntity);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (orderBy is not null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByFilterAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            return await(filter is not null ? _dbSet.CountAsync(filter, cancellationToken) : _dbSet.CountAsync(cancellationToken));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
     
    }
}

