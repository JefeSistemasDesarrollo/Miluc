using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.NacionalidadDto;
using Miluc.Shared.DTOs.Nomina.PaisDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class NacionalidadClienteService(HttpClient _httpClient) : INacionalidadClienteService
    {
        public async Task<ResponseAPI<List<NacionalidadReaderDto>>> GetNacionalidadesAsync()
        {

            try
            {
                var response = await _httpClient.GetAsync("api/TablasMatriz/Nacionalidad");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<NacionalidadReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<NacionalidadReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las nacionalidades",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<NacionalidadReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener las nacionalidades: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };

            }

        }
    }
}