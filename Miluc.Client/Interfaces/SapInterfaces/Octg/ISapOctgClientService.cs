using Miluc.Shared.DTOs.Sap.Credito;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.SapInterfaces.Octg
{
    public interface ISapOctgClientService 
    {
        Task<ResponseAPI<List<DiasCreditoDto>>> GetAllOctgAsync();
    }
}
