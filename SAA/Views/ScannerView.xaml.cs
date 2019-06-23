using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SAA.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerView : ContentPage {
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        public ScannerView() {

            zxing = new ZXingScannerView {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView",
            };

            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () => {
                    zxing.IsAnalyzing = false;
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");
                    await Navigation.PopModalAsync();
                });

            overlay = new ZXingDefaultOverlay {
                TopText = "Mantenga el código dentro de los bordes",
                BottomText = "Escaneando código QR...",
                ShowFlashButton = true, //zxing.HasTorch,
                AutomationId = "zxingDefaultOverlay",
            };

            overlay.FlashButtonClicked += (sender, e) => {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };

            var grid = new Grid {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);        

            Content = grid;
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing() {
            zxing.IsScanning = false;

            base.OnDisappearing();
        }
    }
}