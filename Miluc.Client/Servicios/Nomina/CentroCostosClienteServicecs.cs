using Miluc.Client.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.CentroDeCostos;

using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CentroCostosClienteService(HttpClient _httpClient) : ICentroCostoClienteService
    {


        public async Task<ResponseAPI<List<CentroCostosReaderDto>>> GetCentroCostosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina, string? correo = null)
        {
            try
            {

                var url = $"/api/CentroCostos/CentroCostos?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina} ";

                var responsePeticion = await _httpClient.GetAsync(url);
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<CentroCostosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener los centros de costos: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<CentroCostosReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<CentroCostosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los centros de costos: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<CentroCostosReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Centros de costos obtenidos correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<CentroCostosReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los Centros de Costos: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            } }

        public async Task<ResponseAPI<CentroCostosReaderDto>> GetCentroCostosByIdAsync(int centroCostoId)
        {
            try
            {
                // 1. Usamos await en lugar de .Result
                // 2. Usamos la ruta REST estándar /api/Cargos/{id}
                var response = await _httpClient.GetAsync($"/api/CentroCostos/{centroCostoId}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseAPI<CentroCostosReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error en la solicitud ({response.StatusCode}): {response.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<CentroCostosReaderDto>>();

                if (resultado == null)
                {
                    return new ResponseAPI<CentroCostosReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener centro de costos: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<CentroCostosReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener centro de costos: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }



        public async Task<ResponseAPI<bool>> UpsertCentroCostosAsync(UpsertCentroCostosDto centroCostosUpdateDto)
        {
            try
            {
                var responsePeticion = await _httpClient.PutAsJsonAsync($"/api/CentroCostos/{centroCostosUpdateDto.CentroCostoId}", centroCostosUpdateDto);
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    var errorApi =
                        await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                    return errorApi ?? new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Ocurrió un error inesperado."
                    };
                }

                var resultado =
                    await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return resultado ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta válida del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error de red: {ex.Message}"
                };
            }
        }

        public async Task<ResponseAPI<bool>> DeleteCentroCostosAsync(int centroCostosId)
        {
            try
            {
                var responsePeticion = await _httpClient.DeleteAsync($"/api/CentroCostos/{centroCostosId}");
                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al eliminar el centro de costos: {responsePeticion.ReasonPhrase}",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                if (resultado == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al eliminar el centro de costos: respuesta nula",
                        Valor = false,
                        CantRegistros = 0
                    };
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al eliminar el centro de costos: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }

    } }
