using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class UsuarioClientService(HttpClient _httpclient) : IUsuarioClientService
    {

        public async Task<ResponseAPI<List<UsuarioReadDto>>> ListarUsuariosAsync(string? buscar, int pagina, int cantidad)
        {
            try
            {



                var url = $"api/Usuario?buscar={buscar}&pagina={pagina}&cantidad={cantidad}";
                var responseUsuario = await _httpclient.GetFromJsonAsync<ResponseAPI<List<UsuarioReadDto>>>(url);

                return responseUsuario ??
                    new ResponseAPI<List<UsuarioReadDto>>
                    { Valor = new List<UsuarioReadDto>(), Mensaje = responseUsuario?.Mensaje ?? "Error al obtener datos", Errores = responseUsuario?.Errores };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<UsuarioReadDto>>
                {
                    EsCorrecto = false,
                    Valor = new List<UsuarioReadDto>(),
                    CantRegistros = 0,
                    Mensaje = $"Error al consultar Usuarios: {ex.Message}"
                };
            }
        }
        public async Task<ResponseAPI<UsuarioReadDto>> GetUsuarioByIdAsync(int id)
        {
            try
            {
                ResponseAPI<UsuarioReadDto> response = new ResponseAPI<UsuarioReadDto>();

                var result = await _httpclient.GetFromJsonAsync<ResponseAPI<UsuarioReadDto>>($"api/Usuario/{id}");

                if (result.Valor != null)
                {
                    response.Valor = result.Valor;
                    response.EsCorrecto = result.EsCorrecto;
                    response.Mensaje = result.Mensaje;
                    response.CantRegistros = result.CantRegistros;

                    return response.SuccessResponse(true, "Usuario obtenido con exito", response.Valor, response.CantRegistros);

                }
                else
                {
                    return new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = "No hay respuesta" };
                }
                //return result ?? new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = "No hay respuesta" };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }

        // 3. CREAR
        public async Task<ResponseAPI<UsuarioReadDto>> CrearUsuarioAsync(UsuarioCreateDto usuario)
        {
            try
            {
                var response = await _httpclient.PostAsJsonAsync("api/Usuario", usuario);
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UsuarioReadDto>>();
                return result ?? new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = "Error en el servidor" };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }

        // 4. ACTUALIZAR
        public async Task<ResponseAPI<UsuarioReadDto>> ActualizarUsuarioAsync(UsuarioUpdateDto usuario)
        {
            try
            {
                var response = await _httpclient.PutAsJsonAsync("api/Usuario", usuario);
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<UsuarioReadDto>>();
                return result ?? new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = "Error al actualizar" };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<UsuarioReadDto> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }

        // 5. ELIMINAR Inactivar
        public async Task<ResponseAPI<bool>> EliminarUsuarioAsync(int id)
        {
            try
            {
                var response = await _httpclient.DeleteAsync($"api/Usuario/{id}");
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return result ?? new ResponseAPI<bool> { EsCorrecto = result.EsCorrecto, Mensaje = $"Error al eliminar el usuario", Errores = result?.Errores };
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool> { EsCorrecto = false, Mensaje = ex.Message };
            }
        }

    }
}
