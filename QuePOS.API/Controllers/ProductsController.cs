using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using System.Data;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IRepository<Product> _productsRepository;
        public ProductsController(IRepository<Product> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<IActionResult> Add(Product product)
        {
           var prod = await _productsRepository.Add(product);
           return  Ok(prod);

        }
    }
}
