using Microsoft.AspNetCore.Identity;

namespace QuePOS.API.Services
{
    public class SeedService
    {
            private readonly RoleManager<IdentityRole> _roleManager;
            public async Task SeedAsync()
            {
                string[] roles = { "Admin", "Store" };
                foreach (var roleName in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }

            public SeedService(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }


        }
}
