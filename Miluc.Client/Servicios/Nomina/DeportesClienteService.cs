using Miluc.Client.Interfaces;
using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.DeporteRederDto;
using Miluc.Shared.Models.Response;

using System.Net.Http.Json;



namespace Miluc.Client.Servicios.Nomina
{
    public class DeportesClienteService(HttpClient _httpClient) : IDeportesClientService
    {
        public async Task<ResponseAPI<List<DeporteRederDto>>> GetDeportesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/TablasMatriz/Deportes");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<DeporteRederDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<DeporteRederDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los deportes",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<DeporteRederDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los deportes: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}
