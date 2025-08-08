using System;
using Common.Response;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T,C,U,L> 
        where T : class
        where C : class
        where U : class
        where L : class
    {
        Task<ResultDto<List<L>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ResultDto<L>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<ResultDto<C>> CreateAsync(C entity, CancellationToken cancellationToken = default);
        Task<ResultDto<U>> UpdateAsync(U entity, CancellationToken cancellationToken = default);
        Task<ResultDto<L>> DeleteAsync(L entity, CancellationToken cancellationToken = default);
        Task<ResultDto<List<L>>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<ResultDto<List<L>>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default);

    }
}

