

using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MedioTransporteDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina

{
    public class MedioTransporteClientService(HttpClient _httpClient) : IMedioTransporteClientService
    {
        public async Task<ResponseAPI<List<MedioTransporteReaderDto>>> GetMedioTransporteAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/TablasMatriz/MedioTransporte");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<MedioTransporteReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<MedioTransporteReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los medios de transporte",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<MedioTransporteReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los medios de transporte: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}
