using Newtonsoft.Json;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuePOS.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private string _refreshToken;
        private readonly IUserService userService;

        public HttpClientService(IHttpClientFactory httpClientFactory, IUserService userService)
        {
            _httpClient = httpClientFactory.CreateClient("api");
            this.userService = userService;
        }

        private async Task AddAuthorizationHeaders()
        {
            _accessToken = await SecureStorage.GetAsync("accessToken");
            _refreshToken = await SecureStorage.GetAsync("refreshToken");
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }

        private async Task<bool> RefreshAccessTokenAsync()
        {
            try
            {
                var refreshPayload = new { RefreshToken = _refreshToken };
                var content = new StringContent(JsonConvert.SerializeObject(refreshPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/refresh", content); // Adjust the endpoint as needed

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<AuthToken>(json);

                    if (tokenResponse != null)
                    {
                        _accessToken = tokenResponse.AccessToken;
                        _refreshToken = tokenResponse.RefreshToken;
                        await SecureStorage.SetAsync("accessToken", _accessToken);
                        await SecureStorage.SetAsync("refreshToken", _refreshToken);
                        Console.WriteLine("Token refreshed successfully.");
                        return true;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
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
                        //var results = await Login(userLogin);
                        var token = await userService.Login(userLogin);
                        _accessToken = token.AccessToken;
                        _refreshToken = token.RefreshToken;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh token: {ex.Message}");
            }
            return false;
        }

        private async Task<T> SendRequestWithRetry<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            await AddAuthorizationHeaders();
            var response = await requestFunc();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (await RefreshAccessTokenAsync())
                {
                    await AddAuthorizationHeaders();
                    response = await requestFunc(); // Retry the request
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                // Handle specific status codes
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        throw new HttpRequestException($"Not found: {await response.Content.ReadAsStringAsync()}");

                    case System.Net.HttpStatusCode.BadRequest:
                        var badRequestContent = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Bad Request: {badRequestContent}");

                    case System.Net.HttpStatusCode.Forbidden:
                        throw new HttpRequestException("Access is forbidden. Check your permissions.");

                    case System.Net.HttpStatusCode.InternalServerError:
                        throw new HttpRequestException("Server error. Try again later.");

                    default:
                        throw new HttpRequestException(
                            $"HTTP Request failed with status code {response.StatusCode} and message: {await response.Content.ReadAsStringAsync()}");
                }
            }

            string json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json) ?? throw new InvalidOperationException("Deserialization returned null.");

        }

        public async Task<T> GetAsync<T>(string endpoint)
        {

            return await SendRequestWithRetry<T>(() => _httpClient.GetAsync(endpoint));
        }

        public async Task<T> PostAsync<T>(string endpoint, object payload)
        {
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await SendRequestWithRetry<T>(() => _httpClient.PostAsync(endpoint, content));
        }

        public async Task<T> PutAsync<T>(string endpoint, object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            return await SendRequestWithRetry<T>(() => _httpClient.PutAsync(endpoint, content));
        }

        public async Task DeleteAsync(string endpoint)
        {
            await SendRequestWithRetry<object>(() => _httpClient.DeleteAsync(endpoint));

        }
    }


}

