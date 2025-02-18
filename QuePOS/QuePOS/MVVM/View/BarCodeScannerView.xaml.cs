
using ZXing.Net.Maui;

namespace QuePOS.MVVM.View;

public partial class BarCodeScannerView : ContentPage
{
    public BarCodeScannerView()
    {
        InitializeComponent();
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.OneDimensional,
            AutoRotate = true,
            Multiple = true
        };
    }
    private TaskCompletionSource<BarcodeResult[]> scanTask = new TaskCompletionSource<BarcodeResult[]>();
    public Task<BarcodeResult[]> WaitForResultAsync()
    {
        return scanTask.Task;

    }

    private void cameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        Application.Current.Windows[0].Navigation.PopModalAsync();
        scanTask.TrySetResult(e.Results);
    }
}