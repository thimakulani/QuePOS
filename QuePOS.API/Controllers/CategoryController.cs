using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CategoryController(IRepository<Category> repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            return Ok(await repository.Add(category));
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await repository.GetList());
        }
    }
}
