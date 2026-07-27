using Miluc.Shared.DTOs.Nomina.NewFolder;

namespace Miluc.Server.Models.Nomina
{
    public interface IDepartamentoService
    {
       public Task<List<DepartamentoReaderDto>> GetDepartamentosAsync();
    }
}
