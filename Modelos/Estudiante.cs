using System;
using System.Collections.Generic;
using System.Text;

namespace S6dchinchin.Modelos
{
    public class Estudiante
    {
        public int codigo { get; set; }
        public int id_estudiante
        {
            get => codigo;
            set => codigo = value;
        }

        public string nombre { get; set; } = string.Empty;
        public string apellido { get; set; } = string.Empty;
        public string edad { get; set; } = string.Empty;
    }
}
