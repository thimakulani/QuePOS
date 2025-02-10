using Camera.MAUI;

namespace QuePOS.MVVM.View;

public partial class BarCodeScannerView : ContentPage
{
    public BarCodeScannerView()
    {
        InitializeComponent();
    }
    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.NumCamerasDetected > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void cameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            BarCodeText.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
        });
    }
}