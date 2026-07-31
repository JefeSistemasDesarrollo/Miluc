using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class EpsClientService(HttpClient httpClient) : IEpsClientService
    {
        public async Task<ResponseAPI<List<EpsReaderDto>>> GetEpsAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            try
            {

                var responsePeticion = await httpClient.GetAsync($"api/Eps?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}");

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    return new ResponseAPI<List<EpsReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las EPS: {responsePeticion.ReasonPhrase}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<EpsReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<EpsReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las EPS: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }


                return new ResponseAPI<List<EpsReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "EPS obtenidas correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<EpsReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener las EPS: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<EpsReaderDto>> CreateEpsAsync(EpsCreateDto eps)
        {
            try
            {
                var responsePeticion = await httpClient.PostAsJsonAsync("/api/Eps/CrearEps", eps);

                if (!responsePeticion.IsSuccessStatusCode)
                {
                    try
                    {
                        var errorDto = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<EpsReaderDto>>();
                        if (errorDto != null && !string.IsNullOrEmpty(errorDto.Mensaje))
                            return errorDto;
                    }
                    catch { }

                    var rawMessage = await responsePeticion.Content.ReadAsStringAsync();
                    return new ResponseAPI<EpsReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawMessage) ? rawMessage : "No se pudo crear la EPS."
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<EpsReaderDto>>();

                return resultado ?? new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear la EPS: {ex.Message}"
                };
            }
        }

        public async Task<ResponseAPI<EpsReaderDto>> GetByEpsAsync(int id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ResponseAPI<EpsReaderDto>>($"/api/Eps/{id}");
                return response ?? new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se encontró la EPS."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EpsReaderDto>
                {
                    Errores = new List<string> { $"Error: {ex.Message}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false,
                    Mensaje = $"Error al crear el eps: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<EpsReaderDto>> UpdateEpsAsync(UpdateEpsDto updateEps)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Eps/{updateEps.EpsId}", updateEps);

               
                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = string.IsNullOrWhiteSpace(rawContent)
                        ? null
                        : System.Text.Json.JsonSerializer.Deserialize<ResponseAPI<EpsReaderDto>>(rawContent, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return errorResponse ?? new ResponseAPI<EpsReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawContent) ? rawContent : "Error al actualizar la EPS."
                    };
                }

                // Respuesta exitosa (200 OK)
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<EpsReaderDto>>();

                return result ?? new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
        }
    
