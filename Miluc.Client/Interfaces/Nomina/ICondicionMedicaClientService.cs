using Miluc.Shared.DTOs.Nomina.CondicionMedica;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICondicionMedicaClientService
    {
        Task<ResponseAPI<List<CondicionMedicaReaderDto>>>GetCondicionMedicaAsync();
    }
}
