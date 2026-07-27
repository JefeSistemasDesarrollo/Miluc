using Miluc.Shared.DTOs.Nomina.NewFolder;
using Miluc.Shared.DTOs.Nomina.TipoDocumento;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ITipoDocumentoClienteService 
    {
        Task<ResponseAPI<List<TipoDocumentoReaderDto>>> GetTipoDocumentosAsync();
    }
}
