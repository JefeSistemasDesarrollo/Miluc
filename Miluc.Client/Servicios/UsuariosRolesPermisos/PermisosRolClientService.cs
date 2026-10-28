using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class PermisosRolClientService(HttpClient _httpclient) : IPermisosRolClientService
    {
        public async Task<ResponseAPI<bool>> ActualizarPermisosPorRolAsync(int idRol, PermisoActualizarRol dto)
        {
            try
            {
                var response = await _httpclient.PutAsJsonAsync($"api/Permisos/rol/{idRol}/permisos",dto);

                var resultado = await response.Content
                    .ReadFromJsonAsync<ResponseAPI<bool>>();

                return resultado ?? new ResponseAPI<bool>
                {
                    Valor = false,
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta de la API."
                };

            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>
                {
                    Mensaje = "",
                    Errores = new List<string> { ex.Message },
                };

            }
        }

        public async Task<ResponseAPI<List<PermisosRolReaderDto>>> GetPermisosPorRol(int idRol)
        {
            try
            {
                var response = await _httpclient.GetFromJsonAsync<ResponseAPI<List<PermisosRolReaderDto>>>($"api/Permisos/rol/{idRol}");

                return response ?? new ResponseAPI<List<PermisosRolReaderDto>>
                {
                    Valor = null,
                    EsCorrecto = false,
                    Mensaje = "No se recibió respuesta de la API."
                };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<PermisosRolReaderDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<PermisosRolReaderDto>(),
                    CantRegistros = 0,
                    Mensaje = $"Error al consultar los permisosAsignados: {ex.Message}"
                };
            }
        }
    }
}
