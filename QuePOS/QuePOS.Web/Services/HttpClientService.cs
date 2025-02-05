using Blazored.SessionStorage;
using QuePOS.Shared.Services;
using QuePOS.Shared.ViewModels;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace QuePOS.Web.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorage;
        private string _accessToken;
        private string _refreshToken;

        public HttpClientService(IHttpClientFactory httpClientFactory, ISessionStorageService sessionStorage)
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
        }
    }
}
