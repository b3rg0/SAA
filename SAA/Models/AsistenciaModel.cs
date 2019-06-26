using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SAA.Models
{
    public class AsistenciaModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [NotNull]
        public int id_seccion { get; set; }

        [MaxLength(15), NotNull]
        public string Nombre { get; set; }

        [MaxLength(15), NotNull]
        public string Apellido { get; set; }

        [MaxLength(15), NotNull]
        public string Cedula { get; set; }
                
    }
}
