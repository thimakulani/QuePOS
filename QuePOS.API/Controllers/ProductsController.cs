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
        [HttpGet("scan/{barcode}")]
        public async Task<IActionResult> Scan(string barcode)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
            var user = await _posdbContext.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();//.Include(x => x.Store).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }
            var product = await _posdbContext.Products
                    .FirstOrDefaultAsync(x => x.StoreID == user.StoreID && x.BarCode == barcode && !x.IsDeleted);
            if(product == null)
            {
                return NotFound("Product not found in store inventory");
            }
            return Ok(product);
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
            //var store_user = await _storeRepository?.GetFirstOrDefault(u => u.UserId == userId);
            var user = await _posdbContext.StoreUsers.Where(x => x.UserId == userId).FirstOrDefaultAsync();//.Include(x => x.Store).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var products = _posdbContext.Products.Where(x => x.StoreID == user.StoreID && !x.IsDeleted);
            return Ok(products);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _posdbContext.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            if (!string.IsNullOrEmpty(product.PublicId))
            {
                await cloudinaryService.DeleteImageAsync(product.PublicId);
            }

            // Perform soft delete by setting IsDeleted to true
            product.IsDeleted = true;

            _posdbContext.Products.Update(product);
            await _posdbContext.SaveChangesAsync();

            return Ok("Product deleted successfully");
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, Product updatedProduct)
        {
            if (updatedProduct == null || id != updatedProduct.Id)
            {
                return BadRequest("Invalid product data");
            }

            var existingProduct = await _posdbContext.Products.FindAsync(id);
            if (existingProduct == null || existingProduct.IsDeleted)
            {
                return NotFound("Product not found");
            }
            if (!string.IsNullOrEmpty(updatedProduct.Base64Url))
            {
                if (!string.IsNullOrEmpty(existingProduct.PublicId))
                {
                    await cloudinaryService.DeleteImageAsync(existingProduct.PublicId);
                }
                var url = await cloudinaryService.UploadImageAsync(updatedProduct.Base64Url, Guid.NewGuid().ToString());
                existingProduct.ImageUrl = url.Url.ToString();
                existingProduct.PublicId = url.PublicId;
            }
            // Update properties
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.StockQuantity = updatedProduct.StockQuantity;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.CategoryID = updatedProduct.CategoryID;

            // Add more fields as needed

            _posdbContext.Products.Update(existingProduct);
            await _posdbContext.SaveChangesAsync();

            return Ok(existingProduct);
        }
    }
}
