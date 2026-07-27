using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface ITipoUsuarioClienteService
    {
        Task<ResponseAPI<List<TipoUsuarioReadDto>>> GetAllTipoUsuarioAsync();
        Task<ResponseAPI<TipoUsuarioReadDto>> GetByTipoUsuarioIdAsync(int id);
        Task<ResponseAPI<bool>> CreateTipoUsuarioAsync(TipoUsuarioCreateDto dto);
        Task<ResponseAPI<bool>> UpdateTipoUsuarioAsync(TipoUsuarioUpdateDto dto);
        Task<ResponseAPI<bool>> DeleteTipoUsuarioAsync(int id);
    }
}
