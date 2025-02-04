using System.Linq.Expressions;

namespace QuePOS.API.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Add(T entity);
        public Task<T> Get(int id);
        public Task Delete(int id);
        public Task Update(int id, T entity);
        public Task<List<T>> GetList();
        public Task<List<T>> GetList(params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
    }
}
