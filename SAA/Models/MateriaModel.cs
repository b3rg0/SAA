using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace SAA.Models
{
    public class MateriaModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [MaxLength(15), NotNull]
        public string Nombre { get; set; }
    }
}
