
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using System.Linq.Expressions;

namespace QuePOS.API.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly POSDbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(POSDbContext context)
        {
            this.context = context;
            dbSet = this.context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(int id)
        {
            var entity = await Get(id);
            dbSet.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetList()
        {
            return await dbSet.ToListAsync();
        }
        public async Task<List<T>> GetList(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }
        public async Task Update(int id, T entity)
        {
            dbSet.Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
