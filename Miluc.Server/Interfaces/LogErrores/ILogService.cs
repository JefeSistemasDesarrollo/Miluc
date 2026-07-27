namespace Miluc.Server.Interfaces.LogErrores
{
    public interface ILogService
    {
        Task GuardarErrorAsync(
            string? message = null,
            string? StackTrace = null,
            string? usuario = null,
            string? metodo = null,
            string? ruta = null,
            string? ip = null,
            string? origen = null,
            string nivel = "Error");
       
    }

}
