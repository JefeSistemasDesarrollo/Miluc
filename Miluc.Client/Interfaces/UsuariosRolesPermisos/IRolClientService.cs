using Miluc.Shared.DTOs.Roles;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface IRolClientService
    {
        Task<ResponseAPI<List<RolReadDto>>> GetAllRolesAsync(string? buscar = null, int? pagina = null, int? cantidad = null);
        Task<ResponseAPI<RolReadDto>> GetByIdRolesAsync(int idRole);
        Task<ResponseAPI<bool>> CreateRolAsync(RolCreateDto dtoCreate);
        Task<ResponseAPI<bool>> UpdateRolAsync(RolUpdateDto dtoUpdate);
        Task<ResponseAPI<bool>> DeleteRolAsync(int idRole);
    }
}
