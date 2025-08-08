using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuePOS.Web.Services
{
    public class HttpClientService //: IHttpClientService
    {
        //private readonly ISessionStorageService _sessionStorage;
        private readonly ILocalStorageService _sessionStorage;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private string _accessToken;
        private string _refreshToken;

        public HttpClientService(IHttpClientFactory httpClientFactory, IUserService userService, ILocalStorageService sessionStorage)
        {
            _httpClient = httpClientFactory.CreateClient("api");
            _userService = userService;
            _sessionStorage = sessionStorage;
        }

        private async Task LoadTokensAsync()
        {
            _accessToken = await _sessionStorage.GetItemAsync<string>("accessToken");
            _refreshToken = await _sessionStorage.GetItemAsync<string>("refreshToken");
        }

        private async Task<bool> RefreshAccessTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_refreshToken)) return false;

                var refreshPayload = new { RefreshToken = _refreshToken };
                var content = new StringContent(JsonConvert.SerializeObject(refreshPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/refresh", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<AuthToken>(json);

                    if (tokenResponse != null)
                    {
                        _accessToken = tokenResponse.AccessToken;
                        _refreshToken = tokenResponse.RefreshToken;
                        await _sessionStorage.SetItemAsync("accessToken", _accessToken);
                        await _sessionStorage.SetItemAsync("refreshToken", _refreshToken);
                        Console.WriteLine("Token refreshed successfully.");
                        return true;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return await AttemptReauthenticationAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh token: {ex.Message}");
            }
            return false;
        }

        private async Task<bool> AttemptReauthenticationAsync()
        {
            var username = await _sessionStorage.GetItemAsync<string>("username");
            var password = await _sessionStorage.GetItemAsync<string>("password");
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var userLogin = new UserLogin { Email = username, Password = password };
                var token = await _userService.Login(userLogin);
                if (token != null)
                {
                    _accessToken = token.AccessToken;
                    _refreshToken = token.RefreshToken;
                    await _sessionStorage.SetItemAsync("accessToken", _accessToken);
                    await _sessionStorage.SetItemAsync("refreshToken", _refreshToken);
                    return true;
                }
            }
            return false;
        }
        public async Task<byte[]> DownloadFileAsync(string endpoint)
        {
            await LoadTokensAsync();

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
            if (!string.IsNullOrEmpty(_accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && await RefreshAccessTokenAsync())
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                response = await _httpClient.SendAsync(requestMessage); // Retry with refreshed token
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to download file. Error {response.StatusCode}: {errorMessage}");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }

        private async Task<T> SendRequestWithRetry<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            await LoadTokensAsync();
            HttpResponseMessage response = await ExecuteRequest(requestFunc);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && await RefreshAccessTokenAsync())
            {
                response = await ExecuteRequest(requestFunc); // Retry the request after refreshing token
            }

            return await HandleResponse<T>(response);
        }

        private async Task<HttpResponseMessage> ExecuteRequest(Func<Task<HttpResponseMessage>> requestFunc)
        {
            using var request = new HttpRequestMessage();
            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            return await requestFunc();
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {response.StatusCode}: {errorMessage}", inner: null, statusCode: response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
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
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await SendRequestWithRetry<T>(() => _httpClient.PutAsync(endpoint, content));
        }

        public async Task DeleteAsync(string endpoint)
        {
            await SendRequestWithRetry<object>(() => _httpClient.DeleteAsync(endpoint));
        }
        /*public HttpClientService(IHttpClientFactory httpClientFactory, ISessionStorageService sessionStorage)
        {
            _httpClient = httpClientFactory.CreateClient("api");
            _sessionStorage = sessionStorage;
        }

        private async Task AddAuthorizationHeadersAsync()
        {
            _accessToken = await _sessionStorage.GetItemAsync<string>("accessToken");
            _refreshToken = await _sessionStorage.GetItemAsync<string>("refreshToken");
            //_accessToken = HttpContext.Session.SetString("", "");// _sessionStorage.GetItemAsync<string>("accessToken");
            //_refreshToken = await _sessionStorage.GetItemAsync<string>("refreshToken");


            if (!string.IsNullOrEmpty(_accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _accessToken);
            }
        }
        public async Task<byte[]> DownloadFileAsync(string endpoint)
        {
            await LoadTokensAsync();

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
            if (!string.IsNullOrEmpty(_accessToken))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            var response = await _httpClient.SendAsync(requestMessage);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && await RefreshAccessTokenAsync())
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
                response = await _httpClient.SendAsync(requestMessage); // Retry with refreshed token
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to download file. Error {response.StatusCode}: {errorMessage}");
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
        private async Task<bool> RefreshAccessTokenAsync()
        {
            try
            {
                var refreshPayload = new { RefreshToken = _refreshToken };
                var content = new StringContent(JsonSerializer.Serialize(refreshPayload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/refresh", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonSerializer.Deserialize<AuthToken>(json);

                    if (tokenResponse != null)
                    {
                        _accessToken = tokenResponse.AccessToken;
                        _refreshToken = tokenResponse.RefreshToken;

                        await _sessionStorage.SetItemAsync("accessToken", _accessToken);
                        await _sessionStorage.SetItemAsync("refreshToken", _refreshToken);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to refresh token: {ex.Message}");
            }
            return false;
        }

        private async Task<T> SendRequestWithRetryAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
        {
            await AddAuthorizationHeadersAsync();
            var response = await requestFunc();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (await RefreshAccessTokenAsync())
                {
                    await AddAuthorizationHeadersAsync();
                    response = await requestFunc(); // Retry the request
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Request failed with status {response.StatusCode}: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json) ?? throw new InvalidOperationException("Deserialization returned null.");
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await SendRequestWithRetryAsync<T>(() => _httpClient.GetAsync(endpoint));
        }

        public async Task<T> PostAsync<T>(string endpoint, object payload)
        {
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return await SendRequestWithRetryAsync<T>(() => _httpClient.PostAsync(endpoint, content));
        }

        public async Task<T> PutAsync<T>(string endpoint, object payload)
        {
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            return await SendRequestWithRetryAsync<T>(() => _httpClient.PutAsync(endpoint, content));
        }

        public async Task DeleteAsync(string endpoint)
        {
            await SendRequestWithRetryAsync<object>(() => _httpClient.DeleteAsync(endpoint));
        }*/
    }
}
