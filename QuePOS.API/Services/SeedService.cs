using Microsoft.AspNetCore.Identity;
using QuePOS.API.Models;

namespace QuePOS.API.Services
{
    public class SeedService
    {
        private UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public async Task SeedAsync()
        {
            string[] roles = { "Admin", "Store Owner", "Store Employee" };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        public void SeedAdmin()
        {
            StoreUser storeUser = new StoreUser()
            {
                CreatedAt = DateTime.UtcNow,
                Email = "thimakulani@gmail.com",
                FirstName = "Thima",
                LastName = "Sigauque",
                Password = "LUna@123",
                PhoneNumber = "0713934923",
                
            };
        }
        public SeedService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
        }


    }
}
