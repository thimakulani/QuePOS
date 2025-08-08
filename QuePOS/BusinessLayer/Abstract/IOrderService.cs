using EntityLayer.Dtos.OrderDtos;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IOrderService : IGenericService<Order, CreateOrderDtos, UpdateOrderDtos,ListOrderDtos>
    {
        // You can add Custom Operations
    }
}
