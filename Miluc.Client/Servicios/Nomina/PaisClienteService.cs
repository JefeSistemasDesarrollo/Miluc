using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.DTOs.Nomina.PaisDto;
using Miluc.Shared.Models.Response;

using System.Net.Http.Json;



namespace Miluc.Client.Servicios.Nomina


{
    public class PaisClienteService(HttpClient _httpClient) : IPaisClientService
    {
        public async Task<ResponseAPI<List<PaisReaderDto>>> GetPaisAsync()
        {

            try
            {
                var response = await _httpClient.GetAsync("api/TablasMatriz/Pais");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<PaisReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<PaisReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los países",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<PaisReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los países: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}
