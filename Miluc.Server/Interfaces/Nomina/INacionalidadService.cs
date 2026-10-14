using Miluc.Shared.DTOs.Nomina.NacionalidadDto;
namespace Miluc.Server.Interfaces.Nomina
{
    public interface INacionalidadService
    {
        Task<List<NacionalidadReaderDto>> GetNacionalidadesAsync();
    }
}
