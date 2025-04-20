using ZXing.Net.Maui;

namespace QuePOS.Shared.Services
{
    public interface IFormFactor
    {
        public string GetFormFactor();
        public string GetPlatform();
        public Task SetSession(string key, string value);
        public Task<string> GetSession(string key);
        public void RemoveSession(string key);
        public Task<BarcodeResult[]> ShowBarCodeScanner();
    }
}
