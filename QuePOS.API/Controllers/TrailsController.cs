using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        private readonly IRepository<AuditTrail> repository;

        public TrailsController(IRepository<AuditTrail> repository)
        {
            this.repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await repository.GetList());
        }
    }
}
