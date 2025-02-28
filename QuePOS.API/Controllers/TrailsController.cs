using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TrailsController : ControllerBase
    {
        private readonly IRepository<AuditTrail> repository;
        private readonly POSDbContext context;
        public TrailsController(IRepository<AuditTrail> repository, POSDbContext context)
        {
            this.repository = repository;
            this.context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await repository.GetList());
        }
        [HttpGet("store")]
        public async Task<IActionResult> GetStore()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = context.StoreUsers.Where(x => x.UserId == userId).FirstOrDefault();
            var trail = await context.AuditTrails.Where(x => x.StoreId == user.StoreID).ToListAsync();
            return Ok(trail);
        }
    }
}
