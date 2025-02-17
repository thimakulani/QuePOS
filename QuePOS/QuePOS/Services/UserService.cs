using Newtonsoft.Json;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(IHttpClientFactory _httpClient)
        {
            httpClient = _httpClient.CreateClient("api");
        }
        public async Task<AuthToken> Login(UserLogin login)
        {
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/login", content);
            if (response.IsSuccessStatusCode)
            {
                var str_data = await response.Content.ReadAsStringAsync();

                var tkn = JsonConvert.DeserializeObject<AuthToken>(str_data);
                await SecureStorage.SetAsync("accessToken", tkn.AccessToken);
                await SecureStorage.SetAsync("refreshToken", tkn.RefreshToken);
                await SecureStorage.SetAsync("username", login.Email);
                await SecureStorage.SetAsync("password", login.Password);
                return tkn;
            }
            else
            {

                throw new HttpRequestException($"Invalid Email or Password");
            }
        }
        public async Task<ApplicationUserViewModel> GetUserInfo(string _accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            var response = await httpClient.GetAsync("/api/account/user_with_role");
            if (response.IsSuccessStatusCode)
            {
                string str = await response.Content.ReadAsStringAsync();
                var vm = JsonConvert.DeserializeObject<ApplicationUserViewModel>(str);

                return vm;
            }
            else
            {
                string str = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"{response.StatusCode} {str}");
            }
        }

        public async Task<ApplicationUserViewModel> SessionLogin()
        {
            var username = await SecureStorage.GetAsync("username");
            var password = await SecureStorage.GetAsync("password");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                UserLogin userLogin = new UserLogin()
                {
                    Email = username,
                    Password = password
                };
                var results = await Login(userLogin);
                Console.WriteLine("AccessToken " + results.AccessToken);
                return await GetUserInfo(results.AccessToken);
            }
            return null;
        }

        public Task<AuthToken> Login()
        {
            throw new NotImplementedException();
        }
    }
}
