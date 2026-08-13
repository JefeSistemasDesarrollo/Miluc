using Microsoft.Extensions.Validation;
using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Roles;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class RoleClientService(HttpClient _http) : IRolClientService
    {
        public async Task<ResponseAPI<bool>> CreateRolAsync(RolCreateDto dtoCreate)
        {

            try
            {

                var response = await _http.PostAsJsonAsync("api/Roles", dtoCreate);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                if (result != null && result.EsCorrecto==true)
                {



                    return result;


                }


                else
                {
                    return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", result.Errores);
                }



            }
            catch (Exception ex)
            {
                
                return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", new List<string> { ex.Message });
                
            }

        }

        public async Task<ResponseAPI<bool>> DeleteRolAsync(int idRole)
        {
            try
            {
                var response = await _http.DeleteAsync($"api/Roles/{idRole}");
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", new List<string> { "Respuesta nula del servidor" });
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", new List<string> { ex.Message });
            }
        }

        public async Task<ResponseAPI<List<RolReadDto>>> GetAllRolesAsync(string? buscar = null, int? pagina = null, int? cantidad = null)
        {
            try
            {
                var url = $"api/Roles?buscar={buscar}&pagina={pagina}&cantidad={cantidad}";

                var response = await _http
                    .GetFromJsonAsync<ResponseAPI<List<RolReadDto>>>(url);

                if (response?.Valor != null)
                {
                    return new ResponseAPI<List<RolReadDto>>
                    {
                        EsCorrecto=response.EsCorrecto,
                        Valor = response.Valor,
                        Mensaje = response.Mensaje,
                        CantRegistros = response.CantRegistros
                    };
                }

                return new ResponseAPI<List<RolReadDto>>()
                    .SuccessResponse(
                        false,
                        "Fallo al obtener los roles",
                        null,
                        0);
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<RolReadDto>>()
                    .ErroresResponse(
                        false,
                        "Fallo al obtener los roles",
                        new List<string> { ex.Message });
            }
        }

        public async Task<ResponseAPI<RolReadDto>> GetByIdRolesAsync(int idRole)
        {
            try
            {
                var response = await _http.GetAsync($"api/Roles/{idRole}");

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<RolReadDto>>();

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new ResponseAPI<RolReadDto>().SuccessResponse(false, "Fallo al obtener el rol", null, 0);
                }
            }
            catch (Exception ex)
            {
                return new ResponseAPI<RolReadDto>().ErroresResponse(false, "Fallo al obtener el rol", new List<string> { ex.Message });
            }
        }

        public async Task<ResponseAPI<bool>> UpdateRolAsync(RolUpdateDto dtoUpdate)
        {
            try
            {
              
                var response = await _http.PutAsJsonAsync("api/Roles", dtoUpdate);

                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", new List<string> { "Respuesta nula del servidor" });
                }

            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Fallo al enviar datos", new List<string> { ex.Message });
            }
        }
    }
}
