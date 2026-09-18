using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.GeneroDto;
using Miluc.Shared.Models.Response;

using System.Net.Http.Json;



namespace Miluc.Client.Servicios.Nomina


{
    public class GeneroClienteService(HttpClient _httpClient) : IGeneroClientService
    {
        public async Task<ResponseAPI<List<GeneroReaderDto>>> GetGeneroAsync()
        {

            try
            {
                var response = await _httpClient.GetAsync("/api/TablasMatriz/Genero");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseAPI<List<GeneroReaderDto>>>();
                    return result;
                }
                else
                {
                    return new ResponseAPI<List<GeneroReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los géneros",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<GeneroReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los géneros: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
}


