using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;

namespace QuePOS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepository<Category> repository;
        private readonly POSDbContext _context;

        public CategoryController(IRepository<Category> repository, POSDbContext context)
        {
            this.repository = repository;
            this._context = context;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(Category category)
        {
            return Ok(await repository.Add(category));
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await repository.GetList());
        }
        [HttpGet("set")]
        [AllowAnonymous]
        public async Task<IActionResult> Set()
        {
            var items = await _context.ProductItems.Select(x => x.Cat).Distinct().ToListAsync();
            var cat = new List<Category>();
            foreach (var item in items)
            {
                cat.Add(new Category() { CategoryName = item });
            }
            await _context.AddRangeAsync(cat);
            await _context.SaveChangesAsync();
            return Ok(await repository.GetList());
        }
    }
}
