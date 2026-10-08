using System.Data.SqlClient;

namespace DentaCare.Refactored
{
    public class GestorCitasOdontologicas
    {
        private IRepositorio _db;
        private INotificacion _notificador;

        public GestorCitasOdontologicas(IRepositorio db, INotificacion notificador)
        {
            this._db = db;
            this._notificador = notificador;
        }

        public Cita AgendarCita(Cita nuevaCita)
        {           
            _db.GuardarCita(nuevaCita); 
            _notificador.Notificar(nuevaCita.GenerarTextoConfirmacion()); 

            return nuevaCita;
        }

        public decimal CancelarCita(Cita cita, decimal penalizacion)
        {
            _db.ActualizarCita(cita.Id, penalizacion); 
            _notificador.Notificar($"Su cita #{cita.Id} ha sido CANCELADA.");             
            return penalizacion;
        }


    }
}