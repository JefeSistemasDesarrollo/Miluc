using Miluc.Shared.DTOs.Nomina.EmpresaDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEmpresaService
    {
        public Task<List<EmpresaReaderDto>> GetEmpresasAsync();
    }
}
