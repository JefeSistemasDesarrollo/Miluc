using Miluc.Shared.DTOs.Nomina.MedioTransporteDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IMedioTransporteService
    {
        public Task<List<MedioTransporteReaderDto>> GetMedioTransporteAsync();
    }
}
