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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IRepository<Product> _productsRepository;
        private IRepository<StoreUser> _storeRepository;
        private CloudinaryService cloudinaryService;
        public ProductsController(IRepository<Product> productsRepository, IRepository<StoreUser> storeRepository, CloudinaryService cloudinaryService)
        {
            _productsRepository = productsRepository;

            _storeRepository = storeRepository;
            this.cloudinaryService = cloudinaryService;
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (product.Base64Url != null)
            {
                var url = await cloudinaryService.UploadImageAsync(product.Base64Url, Guid.NewGuid().ToString());
                product.ImageUrl = url.Url.ToString();
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
        [HttpGet("all/store")]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
            var prod = await _productsRepository.GetWhere(x => x.StoreID == store_user.StoreID);
            return Ok(prod);

        }
    }
}
