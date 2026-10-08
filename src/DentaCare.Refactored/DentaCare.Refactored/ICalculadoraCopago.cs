namespace DentaCare.Refactored
{
    public interface ICalculadoraCopago
    {
        decimal CalcularCopago(IEspecialidad especialidad, IConvenio convenio, bool esPrimeraVez, bool requiereRadiografia);
    }
}
