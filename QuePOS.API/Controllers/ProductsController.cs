using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using QuePOS.API.Services;
using System.Data;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productsRepository;
        private readonly IRepository<StoreUser> _storeRepository;
        private readonly CloudinaryService cloudinaryService;
        private readonly POSDbContext _posdbContext;
        public ProductsController(IRepository<Product> productsRepository, IRepository<StoreUser> storeRepository, CloudinaryService cloudinaryService, POSDbContext posdbContext)
        {
            _productsRepository = productsRepository;

            _storeRepository = storeRepository;
            this.cloudinaryService = cloudinaryService;
            _posdbContext = posdbContext;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!string.IsNullOrEmpty(product.Base64Url))
            {
                var url = await cloudinaryService.UploadImageAsync(product.Base64Url, Guid.NewGuid().ToString());
                product.ImageUrl = url.Url.ToString();
            }
            if (product.StoreID == 0)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
                var user = await _posdbContext.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                product.StoreID = user.StoreID;
            }
            var prod = await _productsRepository.Add(product);
            return Ok(prod);

        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {

            var prod = await _productsRepository.Get(id);
            return Ok(prod);

        }
        [HttpGet("all/store/products/{storeId}")]
        public async Task<IActionResult> GetAll(int storeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
            var prod = await _productsRepository.GetWhere(x => x.StoreID == storeId);
            return Ok(prod);

        }
        [HttpGet("all/store")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
            var user = await _posdbContext.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();//.Include(x => x.Store).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var products = _posdbContext.Products.Where(x => x.StoreID == user.StoreID);
            return Ok(products);

        }
    }
}
