using Miluc.Shared.DTOs.Nomina.TipoContratoDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ITipoContratoService
    {
        public Task<(List<TipoContratoReaderDto> Data, int TotalRegistros)>GetTipoContratoAsync(string? filtro = null, int page = 1, int? cantidad = null);

    }
}
