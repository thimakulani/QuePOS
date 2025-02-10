using Blazored.SessionStorage;
using QuePOS.Shared.Services;

namespace QuePOS.Web.Services
{
    public class FormFactor : IFormFactor
    {
        private readonly ISessionStorageService _sessionStorage;

        public FormFactor(ISessionStorageService sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public string GetFormFactor()
        {
            return "Web";
        }

        public string GetPlatform()
        {
            return Environment.OSVersion.ToString();
        }
        public async Task<string> GetSession(string key)
        {
            return await _sessionStorage.GetItemAsync<string>(key);
        }

        public async Task SetSession(string key, string value)
        {
            await _sessionStorage.SetItemAsync(key, value);
        }

        public async Task<string> ShowBarCodeScanner()
        {
            return await Task.FromResult("");
        }
    }
}
