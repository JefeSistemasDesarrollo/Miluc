using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEstadoCivilService
    {
        public Task<List<EstadoCivilReaderDto>> GetAllEstadosCivilesAsync();
    }
}
