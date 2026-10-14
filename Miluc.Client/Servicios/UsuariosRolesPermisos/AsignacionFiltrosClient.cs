using DocumentFormat.OpenXml.Office2010.Excel;
using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Nomina.Afp;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class AsignacionFiltrosClient(HttpClient _httpClient) : IAsignacionFiltrosClient
    {


        public async Task<ResponseAPI<List<AsignarFiltrosReaderDto>>> GetFiltrosAsync(string ? filtro = null, int ? page = null, int? cantidad = null  )
        {
            try
            {
                // Codificar la cadena de búsqueda para manejo seguro de URL
                var busquedaSegura = Uri.EscapeDataString(filtro ?? string.Empty);


                var url = $"/api/Filtros/Filtro?filtro={busquedaSegura}&page={page}&cantidad={cantidad}";

                var responsePeticion = await _httpClient.GetAsync(url);

                if (responsePeticion.IsSuccessStatusCode==false)
                {
                    return new ResponseAPI<List<AsignarFiltrosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = $"Error al obtener las AFP (Status: {responsePeticion.StatusCode})",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<List<AsignarFiltrosReaderDto>>>();

                if (resultado == null || resultado.Valor == null)
                {
                    return new ResponseAPI<List<AsignarFiltrosReaderDto>>
                    {
                        EsCorrecto = false,
                        Mensaje = "Error al obtener los filtros: respuesta nula",
                        Valor = null,
                        CantRegistros = 0
                    };
                }

                return new ResponseAPI<List<AsignarFiltrosReaderDto>>
                {
                    EsCorrecto = true,
                    Mensaje = "AFP obtenidas correctamente",
                    Valor = resultado.Valor,
                    CantRegistros = resultado.CantRegistros
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<AsignarFiltrosReaderDto>>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al obtener los filtros: {ex.Message}",
                    Valor = null,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<AsignarFiltrosReaderDto>> GetByIdFiltrosAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<AsignarFiltrosReaderDto>>
                    ($"api/Filtros/{id}");

                return response ?? new ResponseAPI<AsignarFiltrosReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "No se encontró el filtro."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<AsignarFiltrosReaderDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al consultar el filtro.",
                    Errores = new List<string> { ex.Message },
                    CantRegistros = 0
                };
            }
        }
        public async Task<ResponseAPI<bool>> CreateFiltrosAsync(AsignarFiltrosCreateDto dtoCreate)
        {
            try
            {
                var responsePeticion = await _httpClient.PostAsJsonAsync("/api/Filtros/CrearFiltro", dtoCreate);
                if (responsePeticion == null)
                {
                    return new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = "No se obtuvo respuesta del servidor",
                        Valor = false,
                        CantRegistros = 0
                    };
                }

                var resultado = await responsePeticion.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return resultado ?? new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = "Error al procesar la respuesta del servidor",
                    Valor = false,
                    CantRegistros = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    EsCorrecto = false,
                    Mensaje = $"Error al crear la filtros: {ex.Message}",
                    Valor = false,
                    CantRegistros = 0
                };
            }
        }

        public async Task<ResponseAPI<bool>> DeleteFiltrosAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Filtros/{id}");
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


        public async Task<ResponseAPI<bool>> UpdateFiltrosAsync(AsignarFiltroUpdateDto dtoUpdate)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/Filtros", dtoUpdate);


                if (!response.IsSuccessStatusCode)
                {
                    var rawContent = await response.Content.ReadAsStringAsync();

                    var errorResponse = string.IsNullOrWhiteSpace(rawContent)
                        ? null
                        : System.Text.Json.JsonSerializer.Deserialize<ResponseAPI<bool>>(rawContent, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return errorResponse ?? new ResponseAPI<bool>
                    {
                        EsCorrecto = false,
                        Mensaje = !string.IsNullOrWhiteSpace(rawContent) ? rawContent : "Error al actualizar la Filtro."
                    };
                }

                // Respuesta exitosa (200 OK)
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
        public async Task<List<string>> GetModulosAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<string>>("api/Filtros/modulos");
                return response ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        public async Task<ResponseAPI<List<PermisosReadDto>>> GetAllPermisosModuloAsyncPage(string? filtro = null, int page = 1, int? cantidad = null)
        {

            try
            {

                var url = $"/api/Filtros/Permiso?filtro={filtro}&page={page}&cantidad={cantidad}";


              //  var url = $"api/Filtros?modulos={filtro}&pagina={page}&cantidad={cantidad}";
                var response = await _httpClient.GetFromJsonAsync<ResponseAPI<List<PermisosReadDto>>>(url);


                return response ?? new ResponseAPI<List<PermisosReadDto>> { Valor = null, Mensaje = "Ocurrio un error ", EsCorrecto = false };
            }
            catch (HttpRequestException ex)
            {
                return new ResponseAPI<List<PermisosReadDto>>().ErroresResponse(false, "Error de conexión con la API.", new List<string> { ex.Message });
            }
            catch (TaskCanceledException ex)
            {
                return new ResponseAPI<List<PermisosReadDto>>().ErroresResponse(false, "Tiempo de espera agotado.", new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<PermisosReadDto>>().ErroresResponse(false, "Ha ocurrido un error inesperado.", new List<string> { ex.Message });
            }


        }
    }
}