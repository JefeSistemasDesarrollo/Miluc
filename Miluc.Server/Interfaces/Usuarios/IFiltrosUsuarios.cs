using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Roles;
using Miluc.Shared.DTOs.Usuarios;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Usuarios
{
    public interface IFiltrosUsuarios
    {
        Task<(List<AsignarFiltrosReaderDto> Data, int TotalRegistros)> GetFiltrosAsync(string? filtro = null, int page = 1, int? cantidad = null);
        Task<AsignarFiltrosReaderDto> GetByIdFiltrosAsync(int idFiltros);
        Task<bool> CreateFiltrosAsync(AsignarFiltrosCreateDto dtoCreate);
        Task<bool> UpdateFiltrosAsync(AsignarFiltroUpdateDto dtoUpdate);
        Task<bool> DeleteFiltrosAsync(int idFiltros);
        //Task<List<string>> GetModulosAsync();

        Task<(List<PermisosReadDto> Data, int TotalRegistros)> GetAllPermisosModuloAsyncPage(string? buscar = null, int pagina = 1, int? cantidad = null);

    }
}
