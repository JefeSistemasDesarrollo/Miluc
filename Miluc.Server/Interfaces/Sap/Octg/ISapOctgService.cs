using Miluc.Shared.DTOs.Sap.Credito;

namespace Miluc.Server.Interfaces.Sap.Octg
{
    public interface ISapOctgService
    {
        Task<List<DiasCreditoDto>> GetAllDiasCreditoAsync();
    }
}
