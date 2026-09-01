using Miluc.Client.Servicios.Nomina;
using Miluc.Shared.DTOs.Nomina.ClaseVivienda;
using Miluc.Shared.DTOs.Nomina.TipoVivienda;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IClaseViviendaClientService
    {
        Task<ResponseAPI<List<ClaseViviendaReaderDto>>>GetClaseViviendaAsync();
    }
}
