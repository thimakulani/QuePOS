using Blazored.SessionStorage;
using Newtonsoft.Json;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace QuePOS.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;
        private readonly ISessionStorageService sessionStorage;
        public UserService(IHttpClientFactory _httpClient, ISessionStorageService sessionStorage)
        {
            httpClient = _httpClient.CreateClient("api");
            this.sessionStorage = sessionStorage;
        }
        public async Task<AuthToken> Login(UserLogin login)
        {
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("login", content);
            if (response.IsSuccessStatusCode)
            {
                var str_data = await response.Content.ReadAsStringAsync();

                var tkn = JsonConvert.DeserializeObject<AuthToken>(str_data);
                await sessionStorage.SetItemAsync("accessToken", tkn.AccessToken);
                await sessionStorage.SetItemAsync("refreshToken", tkn.RefreshToken);
                await sessionStorage.SetItemAsync("username", login.Email);
                await sessionStorage.SetItemAsync("password", login.Password);
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
            var username = await sessionStorage.GetItemAsync<string>("username");
            var password = await sessionStorage.GetItemAsync<string>("password");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                UserLogin userLogin = new UserLogin()
                {
                    Email = username,
                    Password = password
                };
                var results = await Login(userLogin);
                return await this.GetUserInfo(results.AccessToken);
            }
            return null;
        }
    }
}
