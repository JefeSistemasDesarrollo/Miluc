using Miluc.Shared.DTOs.Nomina.ClaseVivienda;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IClaseViviendaService
    {
        public Task<List<ClaseViviendaReaderDto>> GetClaseViviendaAsync();
    }
}
