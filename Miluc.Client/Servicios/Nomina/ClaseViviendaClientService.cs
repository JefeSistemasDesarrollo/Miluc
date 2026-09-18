using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ClaseVivienda;
using Miluc.Shared.DTOs.Nomina.TipoVivienda;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ClaseViviendaClientService(HttpClient _httpClient) : IClaseViviendaClientService
    {
        public async Task<ResponseAPI<List<ClaseViviendaReaderDto>>> GetClaseViviendaAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/TablasMatriz/ClaseVivienda");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ClaseViviendaReaderDto>>>();
                    return result; 
                }
                else
                {
                    return new ResponseAPI<List<ClaseViviendaReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener la clase de vivienda.",
                        Errores = new List<string> { response.ReasonPhrase },
                        CantRegistros = 0
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ClaseViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al obtener la clase de vivienda.",
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }
    }
}

            
   