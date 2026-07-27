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

                if (responsePeticion == null)
                {
                    return new ResponseAPI<EpsReaderDto>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se obtuvo respuesta del servidor.",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<EpsReaderDto>>();

                return resultado ?? new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor.",
                    Valor = null,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
              
                return new ResponseAPI<EpsReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear el eps: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

    }
        }
    
