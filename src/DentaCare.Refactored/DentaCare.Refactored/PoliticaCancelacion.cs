namespace DentaCare.Refactored
{
    public class PoliticaCancelacion : IPoliticaCancelacion
    {
        private readonly TimeProvider _timeProvider;

        public PoliticaCancelacion(TimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public decimal CalcularPenalidad(Cita cita, IEspecialidad especialidad)
        {
            TimeSpan diferenciaTiempo = cita.FechaHora - _timeProvider.GetLocalNow().DateTime;
            decimal penalizacion = 0;

            if (diferenciaTiempo.TotalHours < 24)
            {
                penalizacion = especialidad.RecargoCancelacion;
            }

            return penalizacion;
        }
    }
}
