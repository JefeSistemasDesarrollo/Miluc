using Miluc.Shared.DTOs.Sap.Vendedor;

namespace Miluc.Server.Interfaces.Sap.Oslp
{
    public interface ISapOslpService
    {
        Task<List<SapVendedorReaderDto>> GetListOslpAsync();
    }
}
