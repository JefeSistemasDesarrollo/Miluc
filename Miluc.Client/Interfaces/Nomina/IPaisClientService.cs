using Miluc.Shared.DTOs.Nomina.PaisDto;
using Miluc.Shared.DTOs.Nomina.CondicionMedica;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IPaisClientService
    {
        Task<ResponseAPI<List<PaisReaderDto>>> GetPaisAsync();
    }
}
