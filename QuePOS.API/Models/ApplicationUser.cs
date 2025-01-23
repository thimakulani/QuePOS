using Microsoft.AspNetCore.Identity;
using static QuePOS.API.Models.Entities;

namespace QuePOS.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int StoreUserId { get; set; }    
        public StoreUser StoreUser { get; set; }     

    }
}
