using Miluc.Shared.DTOs.Nomina.NacionalidadDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface INacionalidadClienteService
    {
        public Task<ResponseAPI<List<NacionalidadReaderDto>>> GetNacionalidadesAsync();
    }
}
