using Miluc.Server.Models.Sap;
using Miluc.Shared.DTOs.Sap.Opln;

namespace Miluc.Server.Interfaces.Sap.Opln
{
    public interface ISapOplnService 
    {
        Task<List<OplnReaderDto>> GetListOplnAsync();
    }
}
