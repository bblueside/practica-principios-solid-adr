namespace DentaCare.Refactored
{
    public interface IPoliticaCancelacion
    {
        decimal CalcularPenalidad(Cita cita, IEspecialidad especialidad);
    }
}
