using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
//using Miluc.Shared.DTOs.Nomina.PaginacionNomina;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEmpleadoClientService
    {

    
        Task<ResponseAPI<EmpleadoReaderDto>>createEmpleadosAsync(EmpleadoCreateDto dto);
        Task<ResponseAPI<EmpleadoReaderDto>>UpdateEmpleadosAsync(EmpleadoUpdateDto empleado);
        Task<ResponseAPI<EmpleadoReaderDto>> GetEmpleadoByIdAsync(int idempleado);
        Task<ResponseAPI<List<EmpleadoReaderDto>>> GetEmpleadosAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<bool>> DeleteEmpleadosAsync(int id);
    }
}
