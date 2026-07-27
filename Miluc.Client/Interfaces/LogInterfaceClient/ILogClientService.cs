namespace Miluc.Client.Interfaces.LogInterfaceClient
{
    public interface ILogClientService
    {
        Task GuardarErrorAsync(
            string? mensagge = null,
            string? StackTrace = null,
            string? usuario = null,
            string? metodo = null,
            string? ruta = null,
            string? ip = null,
            string? origen = null,
            string ? nivel = null);
    }
}
