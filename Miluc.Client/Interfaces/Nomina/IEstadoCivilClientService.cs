using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEstadoCivilClientService
    {

        Task<ResponseAPI<List<EstadoCivilReaderDto>>> GetEstadosCivilesAsync(); 
    }
}
