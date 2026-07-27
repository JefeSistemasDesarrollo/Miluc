using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class PermisosClientService : IPermisoClientService
    {
        private readonly HttpClient _http;

        public PermisosClientService(HttpClient http)
        {
            _http = http;
        }

        

        public async Task<ResponseAPI<List<PermisosReadDto>>> GetPermisosAsync(string? busqueda = null, int pagina = 1, int? cantidad = null)
        {
            try
            {
                var url =
            $"api/permisos?busqueda={busqueda}&pagina={pagina}&cantidad={cantidad}";

                var response = await _http.GetFromJsonAsync<ResponseAPI<List<PermisosReadDto>>>(url);

                return response!;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<PermisosReadDto>>()
                    .ErroresResponse(false, "Error al consumir API",
                        new List<string> { ex.Message });
            }
        }
        public async Task<ResponseAPI<PermisosReadDto>> GetByIdPermisoAsync(int idPermiso)
        {
            try
            {
                var responsePermisos = await _http.GetFromJsonAsync<ResponseAPI<PermisosReadDto>>($"api/Permisos/{idPermiso}");


                return responsePermisos!;
            }
            catch (Exception ex) 
            {
                return new
                    ResponseAPI<PermisosReadDto>()
                   .ErroresResponse(false, "Error al consumir API",
                       new List<string> { ex.Message });

            }
        }

        public async Task<ResponseAPI<bool>> DeletePermisoAsync(int idPermiso)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Permisos/{idPermiso}");

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return result ?? new ResponseAPI<bool> { EsCorrecto = false, Mensaje = "Error al eliminar" };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool> { EsCorrecto = false, Mensaje = ex.Message };

            }

        }

        public async Task<ResponseAPI<bool>> CreatePermisoAsync(PermisosCreateDto createDto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/Permisos",createDto);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return result ?? new ResponseAPI<bool> {EsCorrecto=false, Mensaje =result.Mensaje };


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