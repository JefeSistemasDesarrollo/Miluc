using MailKit.Net.Smtp;
using MailKit.Security;
using Miluc.Server.Interfaces.Autorizacion;
using Miluc.Server.Interfaces.LogErrores;
using MimeKit;

namespace Miluc.Server.Servicios.Autorizacion
{
    public class EmailService(IConfiguration _configuration, ILogService logService) : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MimeMessage();

            // Remitente
            message.From.Add(new MailboxAddress("Miluc Sistema", _configuration["Email:From"]));

            // Destinatario
            message.To.Add(new MailboxAddress("", to));

            message.Subject = subject;

            // Cuerpo del mensaje
            var bodyBuilder = new BodyBuilder();
            if (isHtml) bodyBuilder.HtmlBody = body;
            else bodyBuilder.TextBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                // Conexión a Gmail (Puerto 465 con SSL implícito)
                await client.ConnectAsync(
                    _configuration["Email:SmtpHost"],
                    int.Parse(_configuration["Email:SmtpPort"]),
                    SecureSocketOptions.SslOnConnect
                );

                // Autenticación con la Contraseña de Aplicación
                await client.AuthenticateAsync(
                    _configuration["Email:Username"],
                    _configuration["Email:Password"]
                );

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log de error detallado
                await logService.GuardarErrorAsync(
                    message: $"Error Gmail MailKit: {ex.Message}",
                    StackTrace: ex.StackTrace,
                    usuario: "Sistema",
                    metodo: "SendAsync",
                    ruta: "/Servicios.Autorizacion",
                    ip: "",
                    origen: "EmailService"
                );
                throw; // Re-lanzamos para que el método que llama sepa que falló
            }
        }
        }

}
