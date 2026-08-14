
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IVacunaService
    {
        Task<List<VacunaReaderDto>> GetVacunaAsync();
       
    }
}
