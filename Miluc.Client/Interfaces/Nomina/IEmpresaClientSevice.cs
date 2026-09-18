using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEmpresaClientSevice
    {
        Task<ResponseAPI<List<EmpresaReaderDto>>> GetEmpresasAsync();
    }
}
