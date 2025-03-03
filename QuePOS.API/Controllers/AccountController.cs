using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuePOS.API.Data;
using QuePOS.API.Interfaces;
using QuePOS.API.Models;
using QuePOS.API.ViewModel;
using System.Linq;
using System.Security.Claims;

namespace QuePOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly POSDbContext context;
        private readonly UserManager<ApplicationUser> manager;
        private readonly IRepository<Store> store_repository;
        private readonly IRepository<StoreUser> storeUser_repository;

        public AccountController(POSDbContext context, UserManager<ApplicationUser> userManager, IRepository<Store> store_repository, IRepository<StoreUser> storeUser_repository)
        {
            this.context = context;
            this.manager = userManager;
            this.store_repository = store_repository;
            this.storeUser_repository = storeUser_repository;
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

        [HttpPost("register")]
        public async Task<IActionResult> Add(SignupViewModel signupView)
        {
            var appUser = new ApplicationUser()
            {
                Email = signupView.Email,
                PhoneNumber = signupView.PhoneNumber,
                UserName = signupView.Email
            };
            var results = await manager.CreateAsync(appUser, signupView.Password);
            if (results.Succeeded)
            {
                var user = await manager.FindByEmailAsync(appUser.Email);

                var store = new Store()
                {
                    Phone = signupView.PhoneNumber,
                    StoreName = signupView.StoreName,
                    StoreUserId = user.Id,

                };
                await store_repository.Add(store);
                var storeUser = new StoreUser()
                {
                    FirstName = signupView.Lastname,
                    LastName = signupView.Lastname,
                    PhoneNumber = signupView.PhoneNumber,
                    UserId = user.Id,
                    StoreID = store.Id,
                    Email = signupView.Email,
                };
                await storeUser_repository.Add(storeUser);
                return Ok("Successfully Registered");
            }
            else
            {
                return BadRequest(string.Join("\n", results.Errors.Select(e => e.Description)));
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Test(string email)
        {
            var user = await manager.FindByEmailAsync(email);
            var x = await manager.DeleteAsync(user);
            return Ok(x.Succeeded);
        }
    }
}
