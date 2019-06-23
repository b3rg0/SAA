using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SAA.ViewModels{
    class MateriaViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private string materias;

        public string Materias {
            get { return materias; }
            set {
                materias = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Personas"));
                }
            }
        }

        public MateriaViewModel() {
            var listadoMaterias = SAA.App.BaseDeDatos.ObtenerMaterias();
            foreach (SAA.Models.MateriaModel m in listadoMaterias) {
                Materias += m.Nombre+"\n";
            }
        }
    }
}
