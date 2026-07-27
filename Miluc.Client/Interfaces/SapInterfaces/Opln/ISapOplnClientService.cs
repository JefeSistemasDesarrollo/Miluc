using Miluc.Shared.DTOs.Sap.Opln;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Opln
{
    public interface ISapOplnClientService
    {
        Task<ResponseAPI<List<OplnReaderDto>>> GetListOplnAsync();
    }
}
