
using Miluc.Client.Interfaces.Nomina;

using Miluc.Shared.DTOs.Nomina.CondicionMedica;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CondicionMedicaClientservice(HttpClient _httpClient) : ICondicionMedicaClientService
    {
        public async Task<ResponseAPI<List<CondicionMedicaReaderDto>>> GetCondicionMedicaAsync()
        {
            try

            {
                var response = await _httpClient.GetAsync("api/TablasMatriz/CondicionMedica");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<CondicionMedicaReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<CondicionMedicaReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las condiciones médicas",               
                        Valor = null,
                        CantRegistros = 0
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<CondicionMedicaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener las condiciones médicas: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

    }
}