using Miluc.Shared.DTOs.Nomina.CentroDeCostos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICentroCostoClienteService
    {
        Task<ResponseAPI<List<CentroCostosReaderDto>>> GetCentroCostosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina, string? correo = null);
        Task<ResponseAPI<bool>> UpsertCentroCostosAsync(UpsertCentroCostosDto centroCostosUpdateDto);
        Task<ResponseAPI<bool>> DeleteCentroCostosAsync(int centroCostosId);
        Task<ResponseAPI<CentroCostosReaderDto>> GetCentroCostosByIdAsync(int Id);
    }
}
