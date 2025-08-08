using DataLayer.Abstract;
using DataLayer.Context;
using DataLayer.Repositories;
using EntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityFramework
{
    public class EFOrderDal : GenericRepository<Order>, IOrderDal
    {
        public EFOrderDal(AppDbContext context) : base(context)
        {
            // add Ef options
        }
    }
}