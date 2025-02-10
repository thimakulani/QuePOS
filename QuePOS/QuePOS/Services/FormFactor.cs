using QuePOS.MVVM.View;
using QuePOS.Shared.Services;

namespace QuePOS.Services
{
    public class FormFactor : IFormFactor
    {
        public string GetFormFactor()
        {
            return DeviceInfo.Idiom.ToString();
        }

        public string GetPlatform()
        {
            return DeviceInfo.Platform.ToString() + " - " + DeviceInfo.VersionString;
        }

        public async Task<string> GetSession(string key)
        {
            return await SecureStorage.GetAsync(key);
        }

        public async Task SetSession(string key, string value)
        {
            await SecureStorage.SetAsync(key, value);
        }
        public async Task<string> ShowBarCodeScanner()
        {
            await Application.Current.Windows[0].Navigation.PushModalAsync(new BarCodeScannerView());
            return await Task.FromResult("");
        }
    }
}
