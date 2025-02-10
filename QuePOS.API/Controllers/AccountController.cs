using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Models;
using QuePOS.API.ViewModel;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly POSDbContext context;
        private readonly UserManager<ApplicationUser> manager;

        public AccountController(POSDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.manager = userManager;
        }
        [HttpGet("user_with_role")]
        public async Task<IActionResult> GetUserInfoWithRoles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Fetch the user details from the database
            var id_user = await manager.FindByIdAsync(userId);
            var user = await context.StoreUsers.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            // Get the user's roles
            var roles = await manager.GetRolesAsync(id_user);
            // Map the user details and roles to a view model
            var userViewModel = new ApplicationUserViewModel
            {
                StoreUser = user,
                UserRoles = [.. roles],
            };

            return Ok(userViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Test(string email)
        {
            var user = await manager.FindByEmailAsync(email);
            var x = await manager.DeleteAsync(user);
            return Ok(x.Succeeded);
        }
    }
}
