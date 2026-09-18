using Miluc.Shared.DTOs.Nomina.GeneroDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IGeneroClientService
    {
        Task<ResponseAPI<List<GeneroReaderDto>>> GetGeneroAsync();
    }
}
