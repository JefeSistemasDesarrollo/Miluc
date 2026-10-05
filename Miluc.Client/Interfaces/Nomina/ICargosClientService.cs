
using Miluc.Shared.DTOs.Nomina.CargosDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICargosClientService
    {
        Task<ResponseAPI<List<CargosReaderDto>>> GetCargosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina,string? correo = null);
        Task<ResponseAPI<bool>> UpsertCargoAsync(UpsertCargos cargoUpdateDto);
        Task<ResponseAPI<bool>> DeleteCargoAsync(int cargoId);
        Task<ResponseAPI<CargosReaderDto>> GetCargoByIdAsync(int cargoId);
    }
}