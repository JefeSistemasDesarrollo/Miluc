using Miluc.Shared.DTOs.Nomina.DeporteRederDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IDeportesClientService
    {
        Task<ResponseAPI<List<DeporteRederDto>>> GetDeportesAsync();
    }
}
