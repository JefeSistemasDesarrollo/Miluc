using Miluc.Shared.DTOs.Nomina.TipoDocumento;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ITipoDocumentoService
    {

        Task<List<TipoDocumentoReaderDto>> GetAllTipoDocumento();

    }
}
