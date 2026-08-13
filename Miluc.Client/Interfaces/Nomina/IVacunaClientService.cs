
using Miluc.Shared.Models.Response;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
namespace Miluc.Client.Interfaces.Nomina
{
    public interface IVacunaClientService
    {
        Task<ResponseAPI<List<VacunaReaderDto>>> GetVacunaAsync();
    }
}
