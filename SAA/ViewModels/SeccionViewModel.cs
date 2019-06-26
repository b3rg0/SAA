using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using SAA.Models;
using Xamarin.Forms;

namespace SAA.ViewModels {
    class SeccionViewModel {
        int materia = 0;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ModificarSeccionCommand { get; set; }
        public ICommand VerAsistenciaCommand { get; set; }

        private ObservableCollection<SeccionModel> _seccion;

        public ObservableCollection<SeccionModel> Seccion {
            get { return _seccion; }
            set {
                _seccion = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("listasecciones"));
                }
            }
        }

        public SeccionViewModel(int m) {
            materia = m;
            _seccion = App.BaseDeDatos.ObtenerSecciones(materia);            
            ModificarSeccionCommand = new Command(ModificarSeccion);
            VerAsistenciaCommand = new Command<int>(VerAsistencia);
        }

        public async void ModificarSeccion() {
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.EditarView(2, materia));
        }

        public async void VerAsistencia(int a) {
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.AsistenciaView(a));
        }

        public void refresh() {
            _seccion.Clear();
            ObservableCollection<SeccionModel> aux = App.BaseDeDatos.ObtenerSecciones(materia);
            foreach (var i in aux)
                _seccion.Add(i);
        }
    }
}