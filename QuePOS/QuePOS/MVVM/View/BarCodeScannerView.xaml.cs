using ZXing.Net.Maui;

namespace QuePOS.MVVM.View;

public partial class BarCodeScannerView : ContentPage
{
    private TaskCompletionSource<BarcodeResult[]> scanTask = new();

    public BarCodeScannerView()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (cameraBarcodeReaderView != null)
        {
            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.OneDimensional,
                AutoRotate = true,
                Multiple = true
            };
        }
    }

    public Task<BarcodeResult[]> WaitForResultAsync()
    {
        scanTask = new TaskCompletionSource<BarcodeResult[]>(); // Reset task
        return scanTask.Task;
    }

    private async void cameraBarcodeReaderView_BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        cameraBarcodeReaderView.IsDetecting = false; // Stop scanning
        await Task.Delay(100); // Prevent UI race conditions
        await this.Navigation.PopModalAsync();
        scanTask.TrySetResult(e.Results);
    }
}
