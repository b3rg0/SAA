using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using SQLite;

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

        public IEnumerable<MateriaModel> ObtenerMaterias() {
            lock (locker) {
                var x = (from i in conexión.Table<MateriaModel>() select i).ToList();
                return x;
            }
        }

        public MateriaModel ObtenerMateria(int id) {
            lock (locker) {
                return conexión.Table<MateriaModel>().FirstOrDefault(x => x.ID == id);
            }
        }

        public bool ExisteMateria(string nombre) {
            lock (locker) {
                var x = conexión.Table<MateriaModel>().FirstOrDefault(x => x.Nombre == nombre);
                //usar to lowerCase y trim()
                if (x == null) //default ==null ???? stackoverflow
                
                    return false;
                return true;
            }
        }
        public IEnumerable<SeccionModel> ObtenerSecciones(int MID) {
            lock (locker) {
                var x = (from i in conexión.Table<SeccionModel>() where MID ==i.idMateria select i).ToList();
                return x;
            }
        }

        public SeccionModel ObtenerSeccion(int SID, int MID) {
            lock (locker) {
                return conexión.Table<SeccionModel>().FirstOrDefault(x => x.nSeccion == SID && x.idMateria==MID);
            }
        }

        public bool ExisteSeccion(int NS) {// el num de la seccion
            lock (locker) {
                var x = conexión.Table<SeccionModel>().FirstOrDefault(x => x.nSeccion == NS);                
                if (x == null) 
                    return false;
                return true;
            }
        }

        public IEnumerable<AsistenciaModel> ObtenerAsistencias(int MID, int SID) {
            lock (locker) {
                var x = (from i in conexión.Table<AsistenciaModel>() where MID == i.idMateria && SID== i.idSeccion select i).ToList();
                return x;
            }
        }

        public bool ExisteAsistencia(int MID, int AID) {
            lock (locker) {
                var x = conexión.Table<AsistenciaModel>().FirstOrDefault(x => x.idMateria == MID && x.idAlumno== AID);
                //usar to lowerCase y trim()
                if (x == null) //default ==null ???? stackoverflow

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
                if(!ExisteMateria(ObtenerMateria(seccion.idMateria).Nombre)) { // linea 57
                    return -1;
                }                
                if (seccion.ID != 0) {
                    conexión.Update(seccion);
                    return seccion.ID;
                }
                else {
                    return conexión.Insert(seccion);
                }
            }
        }

        public int AlmacenarAsistencia(AsistenciaModel asistencia) {
            lock (locker) {
                if (ObtenerAsistencia(asistencia.idMateria, asistencia.idAlumno)) { // linea 57
                    return -1;
                }                
                if (asistencia.ID != 0) {
                    conexión.Update(asistencia);
                    return asistencia.ID;
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
                    var a = ObtenerAsistencias(id, seccion.id);
                    foreach(var asistencia in a) {
                        conexión.Delete<AsistenciaModel>(asistencia.id);
                    }
                    conexión.Delete<SeccionModel>(seccion.id);
                }
                return conexión.Delete<MateriaModel>(id);
            }
        }

        public int EliminarSeccion(int id, int ns) {
            lock (locker) {
                var x = ObtenerSeccion(ns, id);
                var a = ObtenerAsistencias(id, x.id);
                foreach (var asistencia in a) {
                    conexión.Delete<AsistenciaModel>(asistencia.id);
                }                
               return conexión.Delete<SeccionModel>(x.id);
            }
        }

        public void EliminarAsistencias(int mid, int sid) {
            var a = ObtenerAsistencias(mid,sid);
            foreach (var asistencia in a) {
                conexión.Delete<AsistenciaModel>(asistencia.id);
            }
        }
    }
}
