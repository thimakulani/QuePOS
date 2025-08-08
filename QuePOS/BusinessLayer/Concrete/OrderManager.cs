using BusinessLayer.Abstract;
using BusinessLayer.ClientMessages;
using Common.Helpers.TextMethots;
using Common.Response;
using DataLayer.Abstract;
using DataLayer.EntityFramework;
using EntityLayer.Dtos.OrderDtos;
using EntityLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public sealed class OrderManager : IOrderService
    {
        private readonly IOrderDal _OrderDal;
        private readonly TextsCheckMethots _textsCheckMethos;

        public OrderManager(IOrderDal OrderDal, TextsCheckMethots textsCheckMethos)
        {
            _OrderDal = OrderDal;
            _textsCheckMethos = textsCheckMethos;
        }



        public async Task<ResultDto<CreateOrderDtos>> CreateAsync(CreateOrderDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<ListOrderDtos>> DeleteAsync(ListOrderDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListOrderDtos>>> FindAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListOrderDtos>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<ListOrderDtos>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<List<ListOrderDtos>>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Order, bool>>? filter = null, Expression<Func<Order, object>>? orderBy = null, bool ascending = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultDto<UpdateOrderDtos>> UpdateAsync(UpdateOrderDtos entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private async Task<ResultDto<ListOrderDtos>> CheckOrderNameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

