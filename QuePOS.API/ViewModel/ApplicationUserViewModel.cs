using QuePOS.API.Models;

namespace QuePOS.API.ViewModel
{
    public class ApplicationUserViewModel
    {
        public StoreUser StoreUser { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
