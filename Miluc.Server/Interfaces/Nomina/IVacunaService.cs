using Miluc.Shared.DTOs.Nomina.Vacunacion;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IVacunaService
    {
        Task<List<VacunaReaderDto>> GetVacunasAsync();
    }
}
