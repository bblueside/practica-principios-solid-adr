public class SMS : INotificacion
{
    readonly private string _twilioApiKey = "TWILIO_SECRET_KEY_998877";
    public void Notificar(string mensaje)
    {
        Console.WriteLine($"Enviando SMS: {mensaje}");
    }
}