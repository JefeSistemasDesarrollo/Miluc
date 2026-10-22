using Miluc.Shared.DTOs.Permisos;
using Miluc.Shared.DTOs.Usuarios.AsignarFiltrosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.UsuariosRolesPermisos
{
    public interface IAsignacionFiltrosClient
    {

        Task<ResponseAPI<List<AsignarFiltrosReaderDto>>> GetFiltrosAsync(string ? textoBusqueda=null, int ? paginaActual=null, int? cantidadPorPagina = null);
        Task<ResponseAPI<AsignarFiltrosReaderDto>> GetByIdFiltrosAsync(int idFiltros);
        Task<ResponseAPI<bool>> CreateFiltrosAsync(AsignarFiltrosCreateDto dtoCreate);
        Task<ResponseAPI<bool>> UpdateFiltrosAsync(AsignarFiltroUpdateDto dtoUpdate);
        //Task<List<string>> GetModulosAsync();
        // Task<(List<PermisosReadDto> Data, int TotalRegistros)> GetAllPermisosModuloAsyncPage(string? buscar = null, int pagina = 1, int? cantidad = null);

        Task<ResponseAPI<List<PermisosReadDto>>> GetAllPermisosModuloAsyncPage(string? filtro = null, int page = 1, int? cantidad = null);


        Task<ResponseAPI<bool>> DeleteFiltrosAsync(int idFiltros);
    }
}
