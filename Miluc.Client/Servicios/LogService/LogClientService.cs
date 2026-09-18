using Miluc.Client.Interfaces.LogInterfaceClient;
using Miluc.Shared.DTOs.LogDots;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.LogService
{
    public class LogClientService(HttpClient _http) : ILogClientService
    {
     
        public async Task GuardarErrorAsync(string? mensagge = null, string? StackTrace = null, string? usuario = null, string? metodo = null, string? ruta = null, string? ip = null, string? origen = null, string ?nivel =null)
        {
            var errorDto = new ErrorLogRequest
            {
                Mensaje = string.IsNullOrEmpty(mensagge) ? "Error desconocido" : mensagge,
                StackTrace = StackTrace,
                Ruta = ruta,
                Metodo = metodo,
                Usuario = usuario,
                DireccionIp = ip,
                Origen = origen,
                Nivel =nivel ?? "Error", 
                Fecha = DateTime.Now
            };
            await _http.PostAsJsonAsync("api/logs/registrar-error", errorDto);
        }
    }
}
