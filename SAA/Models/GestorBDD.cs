using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using SQLite;
using System.Collections.ObjectModel;

namespace SAA.Models{
    public class GestorBDD {
        static object locker = new object();
        SQLiteConnection conexión;

        public GestorBDD() {
            conexión = new SQLiteConnection(Ruta);
            conexión.CreateTable<MateriaModel>();
            conexión.CreateTable<SeccionModel>();
            conexión.CreateTable<AsistenciaModel>();
        }

        protected string Ruta {
            get {
                var nombreBDD = "TallerComMovil.db3";
                var rutaBDD = "";
                if (Device.RuntimePlatform == "iOS") {
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    folder = Path.Combine(folder, "..", "Library");
                    rutaBDD = Path.Combine(folder, nombreBDD);
                }
                else {
                    if (Device.RuntimePlatform == Device.Android) {
                        string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        rutaBDD = Path.Combine(folder, nombreBDD);
                        return rutaBDD;
                    }
                }
                return rutaBDD;
            }
        }

        public ObservableCollection<MateriaModel> ObtenerMaterias() {
            lock (locker) {
                var x = (from i in conexión.Table<MateriaModel>() select i).ToList();
                var oc = new ObservableCollection<MateriaModel>();
                foreach (var e in x)
                    oc.Add(e);
                return oc;
            }
        }

        public MateriaModel ObtenerMateria(int id) {
            lock (locker) {
                return conexión.Table<MateriaModel>().FirstOrDefault(x => x.ID == id);
            }
        }

        public bool ExisteMateria(string nombre) {
            lock (locker) {
                var y = conexión.Table<MateriaModel>().FirstOrDefault(x => x.Nombre == nombre);
                //usar to lowerCase y trim()
                if (y == null) //default ==null ???? stackoverflow
                
                    return false;
                return true;
            }
        }
        public ObservableCollection<SeccionModel> ObtenerSecciones(int MID) {
            lock (locker) {
                var x = (from i in conexión.Table<SeccionModel>() where MID == i.id_materias select i).ToList();
                var oc = new ObservableCollection<SeccionModel>();
                foreach (var e in x)
                    oc.Add(e);
                return oc;
            }
        }

        public SeccionModel ObtenerSeccion(int SID) {
            lock (locker) {
                return conexión.Table<SeccionModel>().FirstOrDefault(x => x.id == SID );
            }
        }

        public bool ExisteSeccion(int NS) {// el num de la seccion
            lock (locker) {
                var y = conexión.Table<SeccionModel>().FirstOrDefault(x => x.id == NS);                
                if (y == null) 
                    return false;
                return true;
            }
        }

        public ObservableCollection<AsistenciaModel> ObtenerAsistencias(int SID) {
            lock (locker) {
                var x = (from i in conexión.Table<AsistenciaModel>() where SID == i.id_seccion select i).ToList();
                var oc = new ObservableCollection<AsistenciaModel>();
                foreach (var e in x)
                    oc.Add(e);
                return oc;
            }
        }

        public bool ExisteAsistencia(int MID, string AID) {
            lock (locker) {
                var y = conexión.Table<AsistenciaModel>().FirstOrDefault(x => x.id_seccion == MID && x.Cedula== AID);
                //usar to lowerCase y trim()
                if (y == null) //default ==null ???? stackoverflow

                    return false;
                return true;
            }
        }

        public int AlmacenarMateria(MateriaModel materia) {
            lock (locker) {
                if (ExisteMateria(materia.Nombre)) {
                    return -1;
                }
                if (materia.ID != 0) {
                    conexión.Update(materia);
                    return materia.ID;
                }
                else {
                    return conexión.Insert(materia);
                }
            }
        }

        public int AlmacenarSeccion(SeccionModel seccion) {
            lock (locker) {       
                if(!ExisteMateria(ObtenerMateria(seccion.id_materias).Nombre)) { // linea 57
                    return -1;
                }                
                if (seccion.id != 0) {
                    conexión.Update(seccion);
                    return seccion.id;
                }
                else {
                    return conexión.Insert(seccion);
                }
            }
        }

        public int AlmacenarAsistencia(AsistenciaModel asistencia) {
            lock (locker) {
                if (ExisteAsistencia(asistencia.id_seccion, asistencia.Cedula)) { // linea 57
                    return -1;
                }                
                if (asistencia.id != 0) {
                    conexión.Update(asistencia);
                    return asistencia.id;
                }
                else {
                    return conexión.Insert(asistencia);
                }
            }

        }

        public int EliminarMateria(int id) {
            lock (locker) {
                var s = ObtenerSecciones(id);
                foreach(var seccion in s){ 
                    //por el locker no se puede llamar directamente a EliminarSeccion
                    var a = ObtenerAsistencias(seccion.id);
                    foreach(var asistencia in a) {
                        conexión.Delete<AsistenciaModel>(asistencia.id);
                    }
                    conexión.Delete<SeccionModel>(seccion.id);
                }
                return conexión.Delete<MateriaModel>(id);
            }
        }

        public int EliminarSeccion(int ns) {
            lock (locker) {                
                var a = ObtenerAsistencias(ns);
                foreach (var asistencia in a) {
                    conexión.Delete<AsistenciaModel>(asistencia.id);
                }                
               return conexión.Delete<SeccionModel>(ns);
            }
        }

        public void EliminarAsistencias(int sid) {
            var a = ObtenerAsistencias(sid);
            foreach (var asistencia in a) {
                conexión.Delete<AsistenciaModel>(asistencia.id);
            }
        }
    }
}
