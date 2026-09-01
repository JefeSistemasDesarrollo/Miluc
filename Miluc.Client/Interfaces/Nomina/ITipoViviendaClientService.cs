using Miluc.Shared.DTOs.Nomina.TipoVivienda;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ITipoViviendaClientService
    {
        Task<ResponseAPI<List<TipoViviendaReaderDto>>> GetTipoViviendaAsync();
    }
}
