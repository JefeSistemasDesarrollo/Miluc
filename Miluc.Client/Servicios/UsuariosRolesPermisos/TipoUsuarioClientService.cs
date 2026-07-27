using Miluc.Client.Interfaces.UsuariosRolesPermisos;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;
using System.Linq;
using System.Net.Http.Json;

namespace Miluc.Client.Servicios.UsuariosRolesPermisos
{
    public class TipoUsuarioClientService(HttpClient _http) : ITipoUsuarioClienteService
    {

        private readonly string _endpoint = "api/TipoUsuario";

        public async Task<ResponseAPI<bool>> CreateTipoUsuarioAsync(TipoUsuarioCreateDto dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_endpoint, dto);
                // Leemos la respuesta sin importar si es 200 u otro código de error
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();

                return result ?? new ResponseAPI<bool>().ErroresResponse(false, "Respuesta vacía del servidor", new());
            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Error de conexión al crear el registro", new List<string> { ex.Message });
            }

        }

        public async Task<ResponseAPI<bool>> DeleteTipoUsuarioAsync(int id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_endpoint}/{id}");
                // Leemos la respuesta sin importar si es 200 u otro código de error
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return result ?? new ResponseAPI<bool>().ErroresResponse(false, "Respuesta vacía del servidor", new());

            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Error de conexión al eliminar el registro", new List<string> { ex.Message });
            }
        }

        public async Task<ResponseAPI<List<TipoUsuarioReadDto>>> GetAllTipoUsuarioAsync()
        {
            try
            {
                ResponseAPI<List<TipoUsuarioReadDto>> responseAPI = new ResponseAPI<List<TipoUsuarioReadDto>>();    

                var response = await _http.GetFromJsonAsync<ResponseAPI<List<TipoUsuarioReadDto>>>(_endpoint);

                if (response != null && response.EsCorrecto)
                {
                    responseAPI.Valor= response.Valor;
                    responseAPI.CantRegistros = response.Valor?.Count ?? 0;
                    responseAPI.Mensaje = $"Se han obtenido {response.CantRegistros} registros de TipoUsuario.";
                    responseAPI.Errores = null; // Limpiamos errores si la respuesta es correcta
                }
                else
                {
                    responseAPI = new ResponseAPI<List<TipoUsuarioReadDto>>().ErroresResponse(false, "Error al obtener datos del servidor", response?.Errores ?? new());
                    responseAPI.Valor = null; // Aseguramos que el valor sea nulo en caso de error
                    responseAPI.CantRegistros = 0; // Aseguramos que la cantidad de registros sea 0 en caso de error
                    responseAPI.Mensaje = "No se pudieron obtener los datos de TipoUsuario."; // Mensaje de error 
                
                }


                    return responseAPI.SuccessResponse(true,"Lista de usuarios obtenida con exito",
                        response.Valor, response.CantRegistros) ;
            }
            catch (Exception ex)
            {
                return new ResponseAPI<List<TipoUsuarioReadDto>>().ErroresResponse(false, "Error de conexión al obtener datos", new List<string> { ex.Message });
            }
        }

        public async Task<ResponseAPI<TipoUsuarioReadDto>> GetByTipoUsuarioIdAsync(int id)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<ResponseAPI<TipoUsuarioReadDto>>($"{_endpoint}/{id}");

                return response ?? new ResponseAPI<TipoUsuarioReadDto>().ErroresResponse(false, "Respuesta vacía del servidor", new());
            }
            catch (Exception ex)
            {
                return new ResponseAPI<TipoUsuarioReadDto>().ErroresResponse(false, "Error de conexión al obtener datos", new List<string> { ex.Message });

            }
        }

        public async Task<ResponseAPI<bool>> UpdateTipoUsuarioAsync(TipoUsuarioUpdateDto dto)
        {
            try
            {
                var response= await _http.PutAsJsonAsync(_endpoint, dto);
                // Leemos la respuesta sin importar si es 200 u otro código de error
                var result = await response.Content.ReadFromJsonAsync<ResponseAPI<bool>>();
                return result ?? new ResponseAPI<bool>().ErroresResponse(false, "Respuesta vacía del servidor", new());

            }
            catch (Exception ex)
            {
                return new ResponseAPI<bool>().ErroresResponse(false, "Error de conexión al actualizar el registro", new List<string> { ex.Message });
            }
        }
    }
}
