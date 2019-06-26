using SAA.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SAA.Models;
using System.Collections.ObjectModel;

namespace SAA.ViewModels {
   public class MateriaViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ModificarMateriaCommand { get; set; }

        public ICommand VerSeccionesCommand { get; set; }
        
        private ObservableCollection<MateriaModel> _materias;

        public ObservableCollection<MateriaModel> materias {
            get { return _materias; }
            set {
                _materias = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("listamaterias"));
                }
            }
        }

        public MateriaViewModel() {
            _materias = App.BaseDeDatos.ObtenerMaterias();
            ModificarMateriaCommand = new Command(ModificarMateria);
            VerSeccionesCommand = new Command<int>(VerSecciones);
        }

        public async void ModificarMateria() {
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.EditarView(1));

        }

        public async void VerSecciones(int a) {
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.SeccionView(a));
        }

        public void refresh() {
            _materias.Clear();
            ObservableCollection<MateriaModel> aux= App.BaseDeDatos.ObtenerMaterias();
            foreach (var i in aux)
                _materias.Add(i);
        }
    }
}
