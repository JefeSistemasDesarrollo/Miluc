using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface IPermisoClientService
    {
        Task<ResponseAPI<List<PermisosReadDto>>> GetPermisosAsync(
     string? buscar = null, int pagina = 1, int? cantidad = null);


        Task<ResponseAPI<PermisosReadDto>> GetByIdPermisoAsync(int idPermiso);

        Task<ResponseAPI<bool>> DeletePermisoAsync(int idPermiso);


        Task<ResponseAPI<bool>> CreatePermisoAsync(PermisosCreateDto createDto);
        Task<ResponseAPI<bool>> ActualizarPemriso(PermisosUpdateDto updateDto);
    }
}
