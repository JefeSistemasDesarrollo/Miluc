using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.Nomina
{
    public class CajaCompClientService(HttpClient httpClient) : ICajaCompensacionClientService
    {
        public async Task<ResponseAPI<CajaCompensacionReaderDto>> CajaUpdate(CajaUpdate cajaUpdate)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"api/Caja/{cajaUpdate.cajaCompensacionId}", cajaUpdate);


                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = string.IsNullOrWhiteSpace(rawContent)
                        ? null
                        : System.Text.Json.JsonSerializer.Deserialize<ResponseAPI<CajaCompensacionReaderDto>>(rawContent, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return errorResponse ?? new ResponseAPI<CajaCompensacionReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawContent) ? rawContent : "Error al actualizar la Caja."
                    };
                }

                // Respuesta exitosa (200 OK)
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<CajaCompensacionReaderDto>>();

                return result ?? new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Respuesta vacía del servidor."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = ex.Message
                };
            }
        }

        public  async Task<ResponseAPI<CajaCompensacionReaderDto>> CreateCajaAsync(CajaCreateDto cajaCreateDto)
        {
            try
            {
                var responsePeticion = await httpClient.PostAsJsonAsync("/api/CajaCompensacion/CrearCaja", cajaCreateDto);
                if (responsePeticion == null)
                {
                    return new ResponseAPI<CajaCompensacionReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se obtuvo respuesta del servidor",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<CajaCompensacionReaderDto>>();
                return resultado ?? new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear la caja: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<CajaCompensacionReaderDto>> GetBycajaAsync(int id)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<ResponseAPI<CajaCompensacionReaderDto>>($"/api/Caja/{id}");
                return response ?? new ResponseAPI<CajaCompensacionReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se encontró la Caja."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<CajaCompensacionReaderDto>
                {
                    Errores = new List<string> { $"Error: {ex.Message}" },
                    Mensaje = ex.Message,
                    EsCorrecto = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<List<CajaCompensacionReaderDto>>> GetCajaCompensacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina)
        {
            {

                try
                {
                    var responsePeticion = await httpClient.GetAsync($"/api/CajaCompensacion?filtro={Uri.EscapeDataString(textoBusqueda ?? string.Empty)}&page={paginaActual}&cantidad={cantidadPorPagina}");
                   if (!responsePeticion.IsSuccessStatusCode)
                        return new ResponseAPI<List<CajaCompensacionReaderDto>>
                        {
                            EsCorrecto = false,
                            Mensaje = "Error al obtener las cajas de compensación",
                            Valor = null,
                            CantRegistros = 0
                        };
                    

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<CajaCompensacionReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<CajaCompensacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener las cajas: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }


                return new ResponseAPI<List<CajaCompensacionReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "Cajas obtenidas correctamente",
                    Valor = resultado.Valor ,
                    CantRegistros = resultado.CantRegistros
                };
            }
                catch (Exception ex)
                {
                    return new ResponseAPI<List<CajaCompensacionReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las cajas de compensación: {ex.Message}",
                        Valor = null,
                        CantRegistros = 0
                    };
                }
            }
        }
    }
}

