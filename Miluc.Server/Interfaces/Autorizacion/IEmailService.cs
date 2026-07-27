namespace Miluc.Server.Interfaces.Autorizacion
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, bool isHtml = false);

    }
}
