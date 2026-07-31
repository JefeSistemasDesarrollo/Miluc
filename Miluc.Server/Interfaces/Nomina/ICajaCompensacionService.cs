using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ICajaCompensacionService
    {
        public Task<(List<CajaCompensacionReaderDto> Data, int TotalRegistros)> GetCajaCompensacionAsync(string? filtro = null, int page = 1, int? cantidad = null);
        public Task<CajaCompensacionReaderDto> CreateCajaAsync(CajaCreateDto cajaCompensacion);
        public Task<CajaCompensacionReaderDto> GetBycajaAsync(int id);
        public Task<CajaCompensacionReaderDto> CajaUpdate(CajaUpdate cajaUpdate);
    }
}
