using Miluc.Client.Interfaces.Nomina.SeguridadSocial;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
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
    }
}