using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAA.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AsistenciaView : ContentPage {
        ViewModels.AsistenciaViewModel avm;
        public AsistenciaView(int a) {
            InitializeComponent();
            avm = new SAA.ViewModels.AsistenciaViewModel(a);
            ListView lv = new ListView();
            lv.ItemsSource = avm.Asistencia;

            Label l1 = new Label() { Text = "Lista de Asistencias:" , FontSize=15, HorizontalOptions=LayoutOptions.CenterAndExpand};

             var AsistenciaDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();
                
                var nombreLabel = new Label { FontAttributes = FontAttributes.Bold };
                var apellidoLabel = new Label { FontAttributes = FontAttributes.Bold };
                var cedulaLabel = new Label();

                nombreLabel.SetBinding(Label.TextProperty, "Nombre");
                apellidoLabel.SetBinding(Label.TextProperty, "Apellido");
                cedulaLabel.SetBinding(Label.TextProperty, "Cedula");

                grid.Children.Add(nombreLabel);
                grid.Children.Add(apellidoLabel,0,1);
                grid.Children.Add(cedulaLabel, 1, 0);

                return new ViewCell { View = grid };
            });

            lv.ItemTemplate = AsistenciaDataTemplate;
            lv.SelectionMode = ListViewSelectionMode.None;
            Button b1 = new Button() {
                Text = "Tomar Asistencias",
                Command = avm.TomarAsistenciaCommand
            };

            Button b2 = new Button() {
                Text = "Reiniciar Asistencias",
                Command = avm.ReiniciarAsistenciaCommand
            };

            this.Content = new StackLayout() {
                Children = { l1, lv, b1, b2 }
            };
        }
        protected override void OnAppearing() {
            avm.refresh();
        }
    }
}