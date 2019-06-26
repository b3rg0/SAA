using SAA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SAA.Views;
using System.Collections.ObjectModel;

namespace SAA.ViewModels {
    public class AsistenciaViewModel {
        int seccion = 0; //id_seccion
        public event PropertyChangedEventHandler PropertyChanged;

        //Gestion de botones
        public ICommand ReiniciarAsistenciaCommand { get; set; }
        public ICommand TomarAsistenciaCommand { get; set; }

        //Propiedades para acceder a la lista de asistencia
        private ObservableCollection<AsistenciaModel> _asistencia;

        public ObservableCollection<AsistenciaModel> Asistencia {
            get { return _asistencia; }
            set {
                _asistencia = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("listaAsistencia"));
                }
            }
        }

        public AsistenciaViewModel(int s) {
            seccion = s;
            _asistencia = App.BaseDeDatos.ObtenerAsistencias(s);
            ReiniciarAsistenciaCommand = new Command(ReiniciarAsistencia);
            TomarAsistenciaCommand = new Command(TomarAsistencia);
        }

        //Limpia la lista de asistencia
        public void ReiniciarAsistencia() {
            App.BaseDeDatos.EliminarAsistencias(seccion);
            _asistencia = App.BaseDeDatos.ObtenerAsistencias(seccion);

        }
        
        //Tomar asistencia. Invoca un objeto ScannerView()
        public async void TomarAsistencia(){
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.ScannerView());

            //Estara pendiente de cuando la clase scannerView haya obtenido un resultado del escaneo.
            MessagingCenter.Subscribe<ScannerView, String>(this, "", async (s, arg) => {
                MessagingCenter.Unsubscribe<ScannerView, String>(this, "");
                
                //Si posee el siguiente substring, la respuesta no es valida
                if (arg.Contains("no posee un formato valido"))
                    return;

                //Se crea un nuevo objeto AsistenciaModel y se agrega a la base de datos.
                string []datos = arg.Split('-');                                
                var asis = new AsistenciaModel();
                asis.Cedula = datos[0];
                asis.Nombre = datos[1];
                asis.Apellido = datos[2];
                asis.id_seccion = seccion;
                App.BaseDeDatos.AlmacenarAsistencia(asis);
                await App.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        public void refresh() {
            _asistencia.Clear();
            ObservableCollection<AsistenciaModel> aux = App.BaseDeDatos.ObtenerAsistencias(seccion);
            foreach (var i in aux)
                _asistencia.Add(i);
        }
    }
}
