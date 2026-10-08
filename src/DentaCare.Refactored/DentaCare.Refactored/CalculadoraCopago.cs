namespace DentaCare.Refactored
{
    public class CalculadoraCopago : ICalculadoraCopago
    {
        private const decimal CostoConsulta = 100.0m;

        public decimal CalcularCopago(IEspecialidad especialidad, IConvenio convenio, bool esPrimeraVez, bool requiereRadiografia)
        {
            decimal copagoFinal = CostoConsulta * especialidad.CalcularCostoEspecialidad();
            copagoFinal *= convenio.CalcularCostoConvenio();

            if (esPrimeraVez)
            {
                copagoFinal += 20.0m;
            }

            if (requiereRadiografia)
            {
                copagoFinal += 35.0m;
            }

            return copagoFinal;
        }
    }
}
