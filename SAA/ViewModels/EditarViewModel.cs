using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SAA.Models;

namespace SAA.ViewModels {
    class EditarViewModel {
        int a;
        public int materia;
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand GuardarElementoCommand { get; set; }
        public ICommand EliminarElementoCommand { get; set; }

        private string _texto;

        public string Texto {
            get { return _texto; }
            set {
                _texto = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("texto"));
                }
            }
        }

        private bool _visible;

        public bool Visible {
            get { return _visible; }
            set {
                _visible = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("estado"));
                }
            }
        }

        private MateriaModel _mat;

        public MateriaModel mat {
            get { return _mat; }
            set {
                _mat = value;
                OnPropertyChanged("mat");
            }
        }



        public EditarViewModel(int a) {//para materias
            _mat = new MateriaModel();
            this.a = a;
            if (a == 1) {
                _visible = true;
                _texto = "ingrese parametro de entrada";
            }
            else {
                _texto = "ingrese la materia a eliminar";
                _visible = false;
            }
            GuardarElementoCommand = new Command(GuardarElemento);
            EliminarElementoCommand = new Command(EliminarElemento);
        }

        public EditarViewModel(int a, int m) { //para seccion
            materia = m;
            _mat = new MateriaModel();
            this.a = a;
            if (a == 1) {
                _visible = true;
                _texto = "ingrese parametro de entrada";
            }
            else {
                _texto = "ingrese la seccion a eliminar";
                _visible = false;
            }
            GuardarElementoCommand = new Command(GuardarElemento);
            EliminarElementoCommand = new Command(EliminarElemento);
        }

        private void OnPropertyChanged(string propertyName = null) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public async void GuardarElemento() {
            if (a == 1) {
                SAA.App.BaseDeDatos.AlmacenarMateria(mat);                
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else {
                SeccionModel s = new SeccionModel();
                s.id_materias = materia;
                s.Nombre = int.Parse(mat.Nombre);
                SAA.App.BaseDeDatos.AlmacenarSeccion(s);
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }

        public async void EliminarElemento() {
            try {
                int.Parse(mat.Nombre);
            } catch (Exception) {
                await App.Current.MainPage.DisplayAlert("Error", "ingrese el numero de identificacion una vez mas", "OK");
                return;
            }
            if (a == 1) {
                SAA.App.BaseDeDatos.EliminarMateria(int.Parse(mat.Nombre));
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else {
                SAA.App.BaseDeDatos.EliminarSeccion(int.Parse(mat.Nombre));
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
        }
                

    }
}
