using System;
using System.Collections.Generic;
using System.Text;

namespace DentaCare.Refactored
{
    public class Cita
    {
        public string Id { get; set; }
        public Paciente Paciente { get; set; }
        public Odontologo Odontologo { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal CopagoCalculado { get; set; }
        public string Estado { get; set; } // 'PROGRAMADA', 'CANCELADA'
        public decimal PenalizacionCancelacion { get; set; }

        public Cita( string Id, Paciente Paciente, Odontologo Odontologo, DateTime FechaHora, decimal CopagoCalculado, string Estado, decimal PenalizacionCancelacion)
        {
            this.Id = Id;
            this.Paciente = Paciente;
            this.Odontologo = Odontologo;
            this.FechaHora = FechaHora;
            this.CopagoCalculado = CopagoCalculado;
            this.Estado = Estado;
            this.PenalizacionCancelacion = PenalizacionCancelacion;
        }
        public string GenerarTextoConfirmacion() {
            return $"CITA #{Id} - Paciente: {Paciente?.NombreCompleto}, Fecha: {FechaHora:yyyy-MM-dd HH:mm}, Copago: ${CopagoCalculado}"; 
        }

        public void setEstado(string nuevoEstado)
        {
            this.Estado = nuevoEstado;
        }
    }
}
