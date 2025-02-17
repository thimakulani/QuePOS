using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.Services
{
    public interface IUserService
    {
        Task<ApplicationUserViewModel> GetUserInfo(string _accessToken);
        Task<AuthToken> Login(UserLogin login);
        Task<AuthToken> Login();
        Task<ApplicationUserViewModel> SessionLogin();
    }
}
