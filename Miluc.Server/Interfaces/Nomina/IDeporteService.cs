using Miluc.Shared.DTOs.Nomina.DeporteRederDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IDeporteService
    {
        public Task<List<DeporteRederDto>> GetDeporteAsync();
    }
}