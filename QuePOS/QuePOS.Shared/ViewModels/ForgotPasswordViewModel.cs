using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.ViewModels
{
    public class ForgotPasswordViewModel
    {

        public string Password { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
