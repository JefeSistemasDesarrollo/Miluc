using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface IUsuarioClientService
    {
        // Ajustada para devolver el Mensaje que necesitas mostrar en Blazor
        Task<ResponseAPI<List<UsuarioReadDto>>> ListarUsuariosAsync(string? buscar = null, int ? pagina=null, int ?cantidad=null);
        Task<ResponseAPI<UsuarioReadDto>> GetUsuarioByIdAsync(int id);
        Task<ResponseAPI<UsuarioReadDto>> CrearUsuarioAsync(UsuarioCreateDto usuario);
        Task<ResponseAPI<UsuarioReadDto>> ActualizarUsuarioAsync(UsuarioUpdateDto usuario);
        Task<ResponseAPI<bool>> EliminarUsuarioAsync(int id);
    }
}
