using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SAA.Models
{
    public class SeccionModel
    { 
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        [NotNull]
        public int id_materias { get; set; }

        [NotNull]
        public int Nombre { get; set; }
    }
}
