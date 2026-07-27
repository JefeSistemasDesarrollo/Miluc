using Miluc.Shared.DTOs.Nomina.CondicionMedica;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface ICondicionMedicaService
    {
        public Task<List<CondicionMedicaReaderDto>> GetCondicionMedicaAsync();
    }
}
