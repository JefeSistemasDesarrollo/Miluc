using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class PermisosClientService(HttpClient _http) : IPermisoClientService
    {
      


        public async Task<ResponseAPI<List<PermisosReadDto>>> GetPermisosAsync(string? busqueda = null, int pagina = 1, int? cantidad = null)
        {
            try
            {
                var url = $"api/permisos?busqueda={busqueda}&pagina={pagina}&cantidad={cantidad}";
                var response = await _http.GetFromJsonAsync<ResponseAPI<List<PermisosReadDto>>>(url);
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
        public async Task<ResponseAPI<PermisosReadDto>> GetByIdPermisoAsync(int idPermiso)
        {
            try
            {
                var responsePermisos = await _http.GetFromJsonAsync<ResponseAPI<PermisosReadDto>>($"api/Permisos/{idPermiso}");
                return responsePermisos ?? new ResponseAPI<PermisosReadDto>
                {
                    EsCorrecto = false,
                    Mensaje = "Erro al consular los permisos",
                    Errores = new List<string>(),
                    Valor = null,
                };
            }
            catch (HttpRequestException ex)
            {
                return new ResponseAPI<PermisosReadDto>().ErroresResponse(false, "Error de conexión con la API.", new List<string> { ex.Message });
            }
            catch (TaskCanceledException ex)
            {
                return new ResponseAPI<PermisosReadDto>()
                    .ErroresResponse(false, "Tiempo de espera agotado.", new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return new ResponseAPI<PermisosReadDto>().ErroresResponse(false, "Ha ocurrido un error inesperado.", new List<string> { ex.Message });
            }

        }
    


        public async Task<ResponseAPI<bool>> DeletePermisoAsync(int idPermiso)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Permisos/{idPermiso}");

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return result ?? new ResponseAPI<bool> { EsCorrecto = false, Mensaje = "Error al eliminar", Errores=new List<string>() };
            }
            catch (HttpRequestException ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Error de conexión con la API.", new List<string> { ex.Message });
            }
            catch (TaskCanceledException ex)
            {
                return new ResponseAPI<bool>()
                    .ErroresResponse(false, "Tiempo de espera agotado.", new List<string> { ex.Message });
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Ha ocurrido un error inesperado.", new List<string> { ex.Message });
            }

        }

        public async Task<ResponseAPI<bool>> CreatePermisoAsync(PermisosCreateDto createDto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Permisos",createDto);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return result ?? new ResponseAPI<bool> {EsCorrecto=false, Mensaje ="Ocurrio un error " };


            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool> { EsCorrecto = false, Mensaje = ex.Message};

            }

        }

        public async Task<ResponseAPI<bool>> ActualizarPemriso(PermisosUpdateDto updateDto)
        {
            try
            {
                var response = await _http.PutAsJsonAsync("api/Permisos", updateDto);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return result ?? new ResponseAPI<bool> { EsCorrecto = false, Mensaje = result.Mensaje };


            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool> { EsCorrecto = false, Mensaje = ex.Message };

            }
        }
    }
}