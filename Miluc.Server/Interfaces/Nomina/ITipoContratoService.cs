using Miluc.Shared.DTOs.Nomina.TipoContratoDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ITipoContratoService
    {
        public Task<List<TipoContratoReaderDto>>GetAllTipoContratoAsync();

    }
}
