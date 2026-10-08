using System;
using System.Collections.Generic;
using System.Text;

namespace DentaCare.Refactored
{
    public class Paciente
    {
        public string Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Celular { get; set; }
        public int TipoConvenio { get; set; } // 1=Particular, 2=EPS, 3=Prepagada
        public bool EsPrimeraVez { get; set; }
    }
}