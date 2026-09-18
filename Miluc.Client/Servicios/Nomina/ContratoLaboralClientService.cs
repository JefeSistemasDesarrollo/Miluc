using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
using TuProyecto.Client.Services;

namespace Miluc.Client.Servicios.Nomina
{
    public class ContratoLaboralClientService(HttpClient _httpClient) : IContratoLaboralClientService
    {
        public async Task<ResponseAPI<List<ContratoLaboralreaderDto>>> GetContratoLaboralAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // Ruta corregida: api/ContratoLaboral (sin el segmento duplicado)
                var response = await _httpClient.GetAsync($"api/ContratoLaboral?filtro={textoBusqueda}&page={paginaActual}&cantidad={cantidadPorPagina}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error ({response.StatusCode}): {(string.IsNullOrWhiteSpace(error) ? response.ReasonPhrase : error)}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ContratoLaboralreaderDto>>>();

                return resultado ?? new ResponseAPI<List<ContratoLaboralreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al obtener contratos: respuesta nula del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ContratoLaboralreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener contratos: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<List<ContratoLaboralreaderDto>>> GetContratosPorEmpleadoAsync(int empleadoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ContratoLaboral/empleado/{empleadoId}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<List<ContratoLaboralreaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error ({response.StatusCode}): {(string.IsNullOrWhiteSpace(error) ? response.ReasonPhrase : error)}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ContratoLaboralreaderDto>>>();

                return resultado ?? new ResponseAPI<List<ContratoLaboralreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = "No se obtuvo respuesta válida del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<ContratoLaboralreaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener contratos del empleado: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<ContratoLaboralreaderDto>> GetContratoByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ContratoLaboral/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error ({response.StatusCode}): {(string.IsNullOrWhiteSpace(error) ? response.ReasonPhrase : error)}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<ContratoLaboralreaderDto>>();

                return resultado ?? new ResponseAPI<ContratoLaboralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se obtuvo respuesta válida del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ContratoLaboralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener el contrato: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<bool>> CreateContratoLaboralAsync(CreateContratoLaboralDto contratoLaboral)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/ContratoLaboral", contratoLaboral);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error API ({response.StatusCode}): {(string.IsNullOrWhiteSpace(error) ? response.ReasonPhrase : error)}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

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
                    Mensaje = $"Error al crear el contrato: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<ContratoLaboralreaderDto>> UpdateContratoLaboralAsync(UpdateContratoDto update)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/ContratoLaboral/{update.ContratoLaboralId}", update);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContenido = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<ContratoLaboralreaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error servidor ({response.StatusCode}): {(string.IsNullOrWhiteSpace(errorContenido) ? response.ReasonPhrase : errorContenido)}"
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<ContratoLaboralreaderDto>>();

                return resultado ?? new ResponseAPI<ContratoLaboralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta válida del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ContratoLaboralreaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error de red: {ex.Message}"
                };
            }
        }

        public async Task<ResponseAPI<bool>> InhabilitarContratoAsync(InhabilitarContratoDto dto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("api/ContratoLaboral/Inhabilitar", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContenido = await response.Content.ReadAsStringAsync();
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error servidor ({response.StatusCode}): {(string.IsNullOrWhiteSpace(errorContenido) ? response.ReasonPhrase : errorContenido)}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return resultado ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta del servidor al inhabilitar."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al actualizar el estado: {ex.Message}"
                };
            }
        }
    }
}