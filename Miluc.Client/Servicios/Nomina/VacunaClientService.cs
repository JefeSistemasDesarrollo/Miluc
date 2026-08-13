using Miluc.Client.Interfaces.Nomina;

using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class VacunaClientService(HttpClient httpClient) : IVacunaClientService
    {
        public async Task<ResponseAPI<List<VacunaReaderDto>>> GetVacunaAsync()
        {
            try
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Vacunas/Esquemas");
            {
                var response = await httpClient.GetAsync($"/api/Vacunas");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<VacunaReaderDto>>       
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };

                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<VacunaReaderDto>>>();
                if (resultado == null)
                {
                    return new ResponseAPI<List<VacunaReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener Vacunas : respuesta es nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<VacunaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al Obtener los esquemas de Vacunación{ex.Message}",
                    Valor = null,
                    CantRegistros = 0

                };
            }
        }
    }
}
            





       