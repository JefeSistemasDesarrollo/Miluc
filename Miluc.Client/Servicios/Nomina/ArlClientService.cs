using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.ArlDto;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class ArlClientService(HttpClient httpClient) : IArlClientService
    {
        public async Task<ResponseAPI<ArlCreateDto>> CreateArlAsync(ArlCreateDto arlCreate)
        {



            try
            {
                var responsePeticion = await httpClient.PostAsJsonAsync("/api/Arl/CrearArl", arlCreate);

                if (responsePeticion == null)
                {
                    return new ResponseAPI<ArlCreateDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se obtuvo respuesta del servidor.",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<ArlCreateDto>>();

                return resultado ?? new ResponseAPI<ArlCreateDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {

                return new ResponseAPI<ArlCreateDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear el eps: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<List<ArlReaderDto>>> GetArlAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            {

                try
                {
                    var response = await httpClient.GetAsync($"/api/Arl/Arl?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}");
                    if (!response.IsSuccessStatusCode)
                        return new ResponseAPI<List<ArlReaderDto>>
                        {
                            EsCorrecto = false,
                            Mensaje = "Error al obtener las cajas de arl",
                            Valor = null,
                            CantRegistros = 0
                        };
                    var resultado = await response.Content.ReadFromJsonAsync<ResponseAPI<List<ArlReaderDto>>>();

                    if (resultado == null || resultado.Valor == null)
                    {
                        return new ResponseAPI<List<ArlReaderDto>>
                        {
                            EsCorrecto = false,
                            Mensaje = "Error al obtener las arl: respuesta nula",
                            Valor = null,
                            CantRegistros = 0
                        };
                    }


                    return new ResponseAPI<List<ArlReaderDto>>
                    {
                        EsCorrecto = true,
                        Mensaje = "arl obtenidas correctamente",
                        Valor = resultado.Valor,
                        CantRegistros = resultado.CantRegistros
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseAPI<List<ArlReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las arl: {ex.Message}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
            }
        }

        public async Task<ResponseAPI<ArlReaderDto>> GetByArlAsync(int id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ResponseAPI<ArlReaderDto>>($"/api/Arl/{id}");
                return response ?? new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se encontró la ARL."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ArlReaderDto>
                {
                    Errores = new List<string> { $"Error: {ex.Message}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<ArlReaderDto>> UpdateArlAsync(ArlUpdate arlUpdate)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Arl/{arlUpdate.ArlId}", arlUpdate);


                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = string.IsNullOrWhiteSpace(rawContent)
                        ? null
                        : System.Text.Json.JsonSerializer.Deserialize<ResponseAPI<ArlReaderDto>>(rawContent, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return errorResponse ?? new ResponseAPI<ArlReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawContent) ? rawContent : "Error al actualizar la Arl."
                    };
                }

                // Respuesta exitosa (200 OK)
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<ArlReaderDto>>();

                return result ?? new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<ArlReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }
    }
}


    