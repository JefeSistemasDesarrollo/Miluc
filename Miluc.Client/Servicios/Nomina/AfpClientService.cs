using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class AfpClientService(HttpClient httpClient) : IAfpClientService
    {
        public async Task<ResponseAPI<AfpCreateDto>> CreateAfpAsync(AfpCreateDto afpCreateDto)
        {
            try
            {
                var responsePeticion = await httpClient.PostAsJsonAsync("/api/Afp/CrearAfp", afpCreateDto);
                if (responsePeticion == null)
                {
                    return new ResponseAPI<AfpCreateDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se obtuvo respuesta del servidor",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<AfpCreateDto>>();
                return resultado ?? new ResponseAPI<AfpCreateDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<AfpCreateDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear la AFP: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<bool>> DeleteAfpAsync(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Afp/{id}");
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return result ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }


public async Task<ResponseAPI<List<AfpReaderDto>>> GetAfpAsyc(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {
                // Codificar la cadena de búsqueda para manejo seguro de URL
                var busquedaSegura = Uri.EscapeDataString(textoBusqueda ?? string.Empty);


                var url = $"/api/Afp/Afp?textoBusqueda={busquedaSegura}&paginaActual={paginaActual}&cantidadPorPagina={cantidadPorPagina}";

                var responsePeticion = await httpClient.GetAsync(url);

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<AfpReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las AFP (Status: {responsePeticion.StatusCode})",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<AfpReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<AfpReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las AFP: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<AfpReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "AFP obtenidas correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<AfpReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener las AFP: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<AfpReaderDto>> GetByAfpAsync(int id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ResponseAPI<AfpReaderDto>>($"/api/Afp/{id}");
                return response ?? new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se encontró la AFp."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<AfpReaderDto>
                {
                    Errores = new List<string> { $"Error: {ex.Message}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false,
                    CantRegistros = 0
                };
            }
        }

        public  async Task<ResponseAPI<AfpReaderDto>> UpdateAfpAsync(UpdateAfp updateAfp)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Afp/{updateAfp.AfpId}", updateAfp);


                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = string.IsNullOrWhiteSpace(rawContent)
                        ? null
                        : System.Text.Json.JsonSerializer.Deserialize<ResponseAPI<AfpReaderDto>>(rawContent, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return errorResponse ?? new ResponseAPI<AfpReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawContent) ? rawContent : "Error al actualizar la AFP."
                    };
                }

                // Respuesta exitosa (200 OK)
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<AfpReaderDto>>();

                return result ?? new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<AfpReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
    } 
}