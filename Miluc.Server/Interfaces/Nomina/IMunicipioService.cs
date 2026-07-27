using Miluc.Shared.DTOs.Nomina.MunicipioDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IMunicipioService
    {

        public Task<List<MunicipioReaderDto>> GetMunicipiosAsync();

    }
}
