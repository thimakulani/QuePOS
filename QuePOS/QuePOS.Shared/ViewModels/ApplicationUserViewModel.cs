using QuePOS.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.ViewModels
{
    public class ApplicationUserViewModel
    {
        public StoreUser StoreUser { get; set; }
        public List<string> UserRoles { get; set; }
    }
    public class UserLogin
    {
        [Required(ErrorMessage = "Username is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public bool IsServiceProvider { get; set; } = true;
    }
}
