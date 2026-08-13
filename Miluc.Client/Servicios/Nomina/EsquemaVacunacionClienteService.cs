using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EsquemaVacunacionClienteService(HttpClient _httpClient) : IEsquemaVacunacionClienteService
    {
        public async Task<ResponseAPI<bool>> CreateEsquemaVacunacionAsync(List<CreateEsquemaVacunacionDto> esquemaVacunacion)
        { 
        try
            {
                //https://localhost:7222/api/EsquemaVacunacion/CreateEsquemaVacunacion
                var response = await _httpClient.PostAsJsonAsync("/api/EsquemaVacunacion/CreateEsquemaVacunacion", esquemaVacunacion);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();

                    return new ResponseAPI<bool>
                    {
            EsCorrecto = false,
                        Mensaje = $"Error API ({response.StatusCode}): {error}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<bool>>();

                return resultado ?? new ResponseAPI<bool>
                {
        EsCorrecto = false,
                    Mensaje = "La API no devolvió información.",
                    Valor = false,
                    CantRegistros = 0
                };
}
            catch (Exception ex)
            {
    return new ResponseAPI<bool>
    {
        EsCorrecto = false,
        Mensaje = ex.ToString(), // temporal para depuración
        Valor = false,
        CantRegistros = 0
    };
}
        }

        public async Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemasVacunacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {

                var response = await _httpClient.GetAsync($"/api/EsquemaVacunacion/Esquemas?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}");
                if (!response.IsSuccessStatusCode)
                { // Manejar el error de la solicitud
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
                        Mensaje = "Error al obtener los esquemas de vacunación:  respuesta nula",
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
                    Mensaje = $"Error al obtener los esquemas de vacunación: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0

                };
            }



        }

        public async Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemaVacunacionByIdAsync(int id)
        {
            try
            {
               
                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<EsquemaVacunacionReaderDto>>>($"/api/EsquemaVacunacion/{id}");

                return response ?? new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "No se obtuvo respuesta válida del servidor.",
                    Valor = new List<EsquemaVacunacionReaderDto>(),
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al deserializar o consultar API: {ex.Message}");
                return new ResponseAPI<List<EsquemaVacunacionReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener el esquema de vacunación: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }
    }
        }
    
