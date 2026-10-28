using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface IPermisosRolClientService
    {
        Task<ResponseAPI<List<PermisosRolReaderDto>>> GetPermisosPorRol(int idRol);
        Task<ResponseAPI<bool>> ActualizarPermisosPorRolAsync(int idRol,PermisoActualizarRol dto);


        // Task ActualizarPermisosPorRolAsync(int idRol, List<int> idPermisos);
    }
}
