using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using QuePOS.API.ViewModel;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private IRepository<Sale> repository;
        private POSDbContext context;

        public SalesController(IRepository<Sale> repository, POSDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add(Sale entity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            entity.SaleDate = DateTime.Now;
            entity.StoreID = user.StoreID;
            entity.StoreUserId = user.Id;
            entity.Store = null;
            var sale = await repository.Add(entity);
            if (sale != null)
            {
                foreach (var item in entity.SaleDetails)
                {
                    var product = await context.Products.FindAsync(item.ProductID);

                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        if (product.StockQuantity < 0)
                        {
                            product.StockQuantity = 0;
                        }

                        context.Products.Update(product);
                    }
                }

                await context.SaveChangesAsync();
            }
            return Ok(sale);
        }
        [HttpPost("sales_history")]
        public async Task<IActionResult> Get(FilterViewModel filterView)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
                return Unauthorized("User not found");

            var sales = await context.Sales
                .Where(x => x.StoreID == user.StoreID &&
                            x.SaleDate >= filterView.StartDate &&
                            x.SaleDate <= filterView.EndDate)
                .Include(x => x.SaleDetails)
                .ToListAsync();

            return Ok(sales);
        }
        [HttpGet("get_history")]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
                return Unauthorized("User not found");

            var sales = await context.Sales
                .Where(x => x.StoreID == user.StoreID
                            )
                .Include(x => x.SaleDetails)
                .ToListAsync();

            return Ok(sales);
        }
        [HttpGet("get_counter")]
        public async Task<IActionResult> GetCounter()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
                return Unauthorized("User not found");

            var sales = await context.Sales
                .Where(x => x.StoreID == user.StoreID
                            )
                .Include(x => x.SaleDetails)
                .ToListAsync();
            CounterViewModel counterView = new CounterViewModel()
            {
                TotalCount = sales.Count,
                DayCount = sales.Where(x => x.SaleDate.Date == DateTime.Now.Date).Count(),
            };
            return Ok(counterView);
        }
        [AllowAnonymous]
        [HttpGet("saledetails")]
        public async Task<IActionResult> GetSaleDetails()
        {
            return Ok(await context.Sales.Include(x => x.SaleDetails).ToListAsync());
        }
    }
}
