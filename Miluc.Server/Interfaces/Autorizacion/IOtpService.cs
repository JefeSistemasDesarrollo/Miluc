namespace Miluc.Server.Interfaces.Autorizacion
{
    public interface IOtpService
    {

        Task GenerarYEnviarOtpAsync(int idUsuario, string email, string ip, string userAgent);

        Task<bool> ValidarOtpAsync(int idUsuario, string codigoIngresado, string ip, string userAgent);

        Task InvalidarOtpsActivosAsync(int idUsuario);
    }
}
