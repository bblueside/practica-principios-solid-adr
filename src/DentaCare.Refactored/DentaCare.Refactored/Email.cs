using System.Net.Mail;

namespace DentaCare.Refactored
{
    public class Email : INotificacion
    {
        readonly private string _servidorSmtp = "smtp.dentacare.com";
        readonly SmtpClient client;
        readonly MailMessage mail;

        public Email(String correoPaciente)
        {
            this.client = new SmtpClient(_servidorSmtp);
            this.mail = new MailMessage("citas@dentacare.com", correoPaciente);
        }

        public void Notificar(String mensaje)
        {
            this.mail.Body = mensaje;
            client.Send(mail);
        }
    }
}
