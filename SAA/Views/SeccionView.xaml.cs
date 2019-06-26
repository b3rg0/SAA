using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAA.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeccionView : ContentPage {
        ViewModels.SeccionViewModel svm;
        public SeccionView(int a) {
            InitializeComponent();
            svm = new SAA.ViewModels.SeccionViewModel(a);
            ListView lv = new ListView();
            lv.ItemsSource = svm.Seccion;
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, "Nombre");
            lv.ItemTemplate.SetBinding(TextCell.DetailProperty, "id");

            Button b1 = new Button() {
                Text = "Opciones de Seccion",
                Command = svm.ModificarSeccionCommand
            };

            this.Content = new StackLayout() {
                Children = { lv, b1 }
            };

            lv.ItemTapped += async (object sender, ItemTappedEventArgs e) => {

                var seccion = (Models.SeccionModel)lv.SelectedItem;
                await Navigation.PushModalAsync(new AsistenciaView(seccion.id));                               

            };
        }
        protected override void OnAppearing() {
            svm.refresh();
        }
    }
}