using Miluc.Shared.DTOs.Permisos;

namespace Miluc.Server.Interfaces.Permisos
{
    public interface IPermisosService
    {

        //Task<List<PermisosReadDto>> GetAllPermisosAsync();
        Task<(List<PermisosReadDto> Data, int TotalRegistros)> GetAllPermisosAsyncPage(string? buscar = null, int pagina = 1, int? cantidad = null);

        Task<PermisosReadDto> GetByIdPermisoAsync(int idPermiso);

        Task<bool> DeletePermisoAsync(int idPermiso);

        Task<bool> CreatePermisosAsync(PermisosCreateDto permisosCreate);


        Task<bool> UpdatePermisosAsync(PermisosUpdateDto permisosUpdate);
    }
}
