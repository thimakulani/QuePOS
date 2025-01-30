using Microsoft.AspNetCore.Identity;
using QuePOS.API.Models;
using System.Diagnostics;

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
        public async Task SeedAdmin()
        {
            try
            {
                ApplicationUser storeUser = new ApplicationUser()
                {
                    Email = "thimakulani@gmail.com",
                    PhoneNumber = "0713934923",
                    UserName = "thimakulani@gmail.com"
                };
                if (await userManager.FindByEmailAsync(storeUser.Email) == null)
                {
                    var results = await userManager.CreateAsync(storeUser, "LUna@123");
                    if (results.Succeeded)
                    {
                        var user = await userManager.FindByEmailAsync(storeUser.Email);
                        await userManager.AddToRoleAsync(user, "Admin");

                    }
                    else
                    {
                        Console.Error.WriteLine(results.Errors.Select(x => x.Description));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
        public SeedService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
        }


    }
}
