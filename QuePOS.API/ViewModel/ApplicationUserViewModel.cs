using QuePOS.API.Models;

namespace QuePOS.API.ViewModel
{
    public class ApplicationUserViewModel
    {
        public StoreUser StoreUser { get; set; }
        public List<string> UserRoles { get; set; }
    }
    public class SignupViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StoreName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
