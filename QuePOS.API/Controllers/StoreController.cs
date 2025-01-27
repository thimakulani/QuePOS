using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private IRepository<Store> _storeRepository;

        public StoreController(IRepository<Store> storeRepository)
        {
            _storeRepository = storeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Store store)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            store.StoreUserId = userId;
            var new_store = await _storeRepository.Add(store);
            return Ok(new_store);
        }
    }
}
