using Microsoft.AspNetCore.Identity;
using QuePOS.API.Data;
using QuePOS.API.Models;
using System.Diagnostics;

namespace QuePOS.API.Services
{
    public class SeedService
    {
        private UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly POSDbContext context;
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
                ApplicationUser appUser = new ApplicationUser()
                {
                    Email = "thimakulani@gmail.com",
                    PhoneNumber = "0713934923",
                    UserName = "thimakulani@gmail.com"
                };

                if (await userManager.FindByEmailAsync(appUser.Email) == null)
                {
                    var results = await userManager.CreateAsync(appUser, "LUna@123");
                    if (results.Succeeded)
                    {
                        var user = await userManager.FindByEmailAsync(appUser.Email);
                        StoreUser storeUser = new()
                        {
                            Email = appUser.Email,
                            FirstName = "Thima",
                            LastName = "Sigauque",
                            PhoneNumber = "011111111",
                            CreatedAt = DateTime.Now,
                            UserId = user.Id,
                        };
                        await userManager.AddToRoleAsync(user, "Admin");
                        context.Add(storeUser);
                        context.SaveChanges();

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
        public SeedService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, POSDbContext context)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;
        }


    }
}
