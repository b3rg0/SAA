using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SAA.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarView : ContentPage {
        public EditarView() {
            InitializeComponent();
            Label l1 = new Label() { Text = "Materia" };
            Label l2 = new Label() { Text = "Sección"};
            Entry e1 = new Entry() { Keyboard=Keyboard.Text};
            Entry e2 = new Entry() { Keyboard=Keyboard.Numeric};
            Button b1 = new Button() { Text = "CREAR", VerticalOptions=LayoutOptions.End};
            Button b2 = new Button() { Text = "ELIMINAR", VerticalOptions = LayoutOptions.End };

            Content = new StackLayout() {
                Children = { l1, e1, l2, e2,b1,b2 }
            };
        }
    }
}