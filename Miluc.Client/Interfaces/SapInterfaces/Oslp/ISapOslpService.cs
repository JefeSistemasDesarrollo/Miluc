using Miluc.Shared.DTOs.Sap.Vendedor;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Oslp
{
    public interface ISapOslpService
    {
        public Task<ResponseAPI<List<SapVendedorReaderDto>>> GetListVendedoresAsync();
    }
}
