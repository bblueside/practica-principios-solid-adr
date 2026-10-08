public class Metricas {

    public decimal recaudos_totales { get; private set; }
    public int _totalCitasCanceladas { get; private set; }

    public void RegistrarRecaudo(decimal copagoNuevo)
    {
        this.recaudos_totales += copagoNuevo;
    }

    public void RegistrarCancelacion()
    {
        this._totalCitasCanceladas++;
    }

}
