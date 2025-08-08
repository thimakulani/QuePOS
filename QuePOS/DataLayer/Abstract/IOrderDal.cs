using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Abstract
{
    public interface IOrderDal : IGenericDal<Order>
    {
        // Add custom operations here
    }
}