namespace DentaCare.Refactored
{
    public interface IRepositorio
    {
        void GuardarCita(Cita cita);

        void ActualizarCita(String citaId, decimal penalizacion);
    }
}
