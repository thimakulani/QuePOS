using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemsController : ControllerBase
    {
        private readonly IRepository<ProductItems> repository;
        private readonly POSDbContext context;

        public ProductItemsController(IRepository<ProductItems> repository, POSDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<ProductItems>> Scan(string barcode)
        {
            var prod = await repository.GetFirstOrDefault(x => x.Bc == barcode);
            if (prod == null)
            {
                return NotFound("BarCode Not Found");
            }
            return Ok(prod);
        }
        [HttpPost("add_list")]
        public async Task<ActionResult> AddBulk(List<ProductItems> prods)
        {
            await context.AddRangeAsync(prods);
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost("add")]
        public async Task<ActionResult> Add(ProductItems prods)
        {
            return Ok(await repository.Add(prods));
        }
    }
}
