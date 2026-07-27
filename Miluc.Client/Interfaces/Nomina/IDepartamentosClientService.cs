using Miluc.Shared.DTOs.Nomina.MunicipioDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IDepartamentosClientService
    {
        // Método para obtener la lista de departamentos y municipios
        public Task<ResponseAPI<List<MunicipioReaderDto>>> GetMunicipiosAsync();
    }
}
