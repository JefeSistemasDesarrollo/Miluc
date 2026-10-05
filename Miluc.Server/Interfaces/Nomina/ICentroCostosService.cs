using Miluc.Shared.DTOs.Nomina.CentroDeCostos;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ICentroCostosService
    {
        public Task<(List<CentroCostosReaderDto> Data, int TotalRegistros)> GetCentroCostosAsync(string? filtro = null, int page = 1, int? cantidad = null, string? correo = null);
        public Task<CentroCostosReaderDto> GetCentroCostosByIdAsync(int id);
        public Task<bool>UpsertCentoCostosAsync(UpsertCentroCostosDto centroCostos);
        public Task<bool> DeleteCentroCostosAsync(int centroCostoId);
    }
}
