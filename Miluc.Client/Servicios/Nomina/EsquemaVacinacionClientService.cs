using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto;
using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EsquemaVacinacionClientService(HttpClient httpClient) : IEsquemaVacunacionClientService
    {
        public async Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemaAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7222/api/Vacunas/Esquemas");
            {
                var response = await httpClient.GetAsync($"/api/Vacunas/Esquemas?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}");
                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<EsquemaVacunacionReaderDto>>

                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud: {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };

                }
                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<EsquemaVacunacionReaderDto>>>();
                if (resultado == null)
                {
                    return new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener Esquemas de vacunación : respuesta es nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<EsquemaVacunacionReaderDto>>
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
            





       