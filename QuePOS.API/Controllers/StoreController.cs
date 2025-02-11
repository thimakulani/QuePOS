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
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IRepository<Store> _storeRepository;
        private readonly POSDbContext context;
        public StoreController(IRepository<Store> storeRepository, POSDbContext context)
        {
            _storeRepository = storeRepository;
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Store store)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            store.StoreUserId = userId;
            var new_store = await _storeRepository.Add(store);
            return Ok(new_store);
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await context.StoreUsers.Where(x => x.UserId == userId)/*.Include(x => x.Store)*/.FirstOrDefaultAsync();
            Console.Error.WriteLine(" UserId " + userId);
            var store = context.Stores.Where(x => x.Id == user.Id).FirstOrDefault();
            return Ok(store);
        }

        [HttpGet("all")]
        public async Task<IActionResult> All()
        {

            var new_store = await _storeRepository.GetList();
            return Ok(new_store);
        }
    }
}
