using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.TipoVivienda;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
namespace Miluc.Client.Servicios.Nomina
{
    public class TipoDeViviendaClientService(HttpClient _httpClient) : ITipoViviendaClientService
    {
        public async Task<ResponseAPI<List<TipoViviendaReaderDto>>> GetTipoViviendaAsync()
        {
            try { 
             var responsePeticion = await _httpClient.GetAsync("api/TablasMatriz/TipoVivienda");
            if (!responsePeticion.IsSuccessStatusCode)
            {
                return new ResponseAPI<List<TipoViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los tipos de vivienda: {responsePeticion.ReasonPhrase}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<TipoViviendaReaderDto>>>();
            if (resultado == null || resultado.Valor == null)
            {
                return new ResponseAPI<List<TipoViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al obtener los tipos de vivienda: respuesta nula",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            return new ResponseAPI<List<TipoViviendaReaderDto>>
            {
                EsCorrecto = true,
                Mensaje = "Tipos de vivienda obtenidos correctamente",
                Valor = resultado.Valor,
                CantRegistros = resultado.CantRegistros
            };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<TipoViviendaReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los tipos de vivienda: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };

            }
        }
    } 
}
