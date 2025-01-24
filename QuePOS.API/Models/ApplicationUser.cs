using Microsoft.AspNetCore.Identity;

namespace QuePOS.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int StoreUserId { get; set; }    
        public StoreUser StoreUser { get; set; }     

    }
}
