using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SAA {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            MainPage = new Views.MateriaView();
        }

        static Models.GestorBDD baseDeDatos;

        public static Models.GestorBDD BaseDeDatos {
            get {
                if (baseDeDatos == null) {
                    baseDeDatos = new Models.GestorBDD();
                }
                return baseDeDatos;
            }
        }
        protected override void OnStart() {
            // Handle when your app starts
        }

        protected override void OnSleep() {
            // Handle when your app sleeps
        }

        protected override void OnResume() {
            // Handle when your app resumes
        }
    }
}
