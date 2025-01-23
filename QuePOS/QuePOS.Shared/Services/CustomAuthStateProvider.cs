using Microsoft.AspNetCore.Components.Authorization;
using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Shared.Services 
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser;
        public CustomAuthStateProvider()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(new AuthenticationState(_currentUser));
        }
        public void NotifyNotificationState(ApplicationUserViewModel vm)
        {

            if (vm != null)
            {
                var applicationUser = vm.StoreUser;
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, applicationUser.FirstName),
                    new(ClaimTypes.Surname, applicationUser.LastName),
                    new(ClaimTypes.NameIdentifier, applicationUser.Id.ToString()),
                    new(ClaimTypes.MobilePhone, applicationUser.PhoneNumber),


                };
                foreach (var role in vm.UserRoles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }
                var identity = new ClaimsIdentity(claims, "CustomAuth");

                _currentUser = new ClaimsPrincipal(identity);

            }
            else
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}

