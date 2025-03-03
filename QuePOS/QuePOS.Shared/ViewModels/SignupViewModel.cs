using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.ViewModels
{
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
