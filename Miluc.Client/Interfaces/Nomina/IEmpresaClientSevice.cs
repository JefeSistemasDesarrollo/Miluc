using Miluc.Shared.DTOs.Nomina.EmpresaDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEmpresaClientSevice
    {
        Task<ResponseAPI<List<EmpresaReaderDto>>> GetEmpresasAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina, string ? correo = null);
        Task<ResponseAPI<EmpresaReaderDto>> GetEmpresaByIdAsync(int empresaId);
        Task<ResponseAPI<bool>> DeleteEmpresaAsync(int id);
        Task<ResponseAPI<bool>> UpsertEmpresaAsync(UpsertEmpresaDto empresaUpdateDto);


    }
}
