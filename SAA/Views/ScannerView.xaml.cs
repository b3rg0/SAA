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
                    var resultado = Verificar(result.Text);
                    // Show an alert
                    
                    await DisplayAlert("Scanned Barcode", resultado, "OK");
                    
                    //await Navigation.PopModalAsync();
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

        private string Verificar(string text) {
            string s = "el dato:\n" + text + "\nno posee un formato valido";

            if (text.Contains('-')) {
                string[] r = text.Split('-');
                if (r.Length > 1) {
                    r[0] = r[0].Trim();
                    r[1] = r[1].Trim();
                    s = r[0];
                    r = r[1].Split(' ');
                    if (r.Length == 4) {
                        s += "-" + r[0] + "-" + r[2];
                    }
                    else {
                        s = "el dato:\n" + text + "\nno posee un formato valido";
                    }
                }

            }
            MessagingCenter.Send(this, "", s);
            return s;
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