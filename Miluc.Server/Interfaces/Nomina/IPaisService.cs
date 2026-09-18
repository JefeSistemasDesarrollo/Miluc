using Miluc.Shared.DTOs.Nomina.PaisDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IPaisService
    {
        public Task<List<PaisReaderDto>> GetPaisAsync();   
    }
}
