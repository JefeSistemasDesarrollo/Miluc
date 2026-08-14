using Miluc.Shared.DTOs.Roles;

namespace Miluc.Server.Interfaces.Roles
{
    public interface IRolService
    {
        Task<(List<RolReadDto> data, int totalRegistros)> GetAllRolesAsync( string ?buscar=null, int ?pagina=null, int ?cantidad=null);
        Task<RolReadDto> GetByIdRolesAsync(int idRole);
        Task<bool> CreateRolAsync(RolCreateDto dtoCreate);
        Task<bool> UpdateRolAsync(RolUpdateDto dtoUpdate);
        Task<bool> DeleteRolAsync(int idRole);


    }
}
