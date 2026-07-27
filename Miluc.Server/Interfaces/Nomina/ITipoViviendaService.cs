using Miluc.Shared.DTOs.Nomina.TipoVivienda;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ITipoViviendaService
    {
        public  Task<List<TipoViviendaReaderDto>> GetTipoViviendaAsync();
    }
}
