namespace QuePOS.Shared.Services
{
    public interface IFormFactor
    {
        public string GetFormFactor();
        public string GetPlatform();
        public Task SetSession(string key, string value);
        public Task<string> GetSession(string key);
        public Task<string> ShowBarCodeScanner();
    }
}
