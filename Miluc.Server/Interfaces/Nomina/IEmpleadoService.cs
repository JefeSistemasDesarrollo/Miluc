using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
//using Miluc.Shared.DTOs.Nomina.PaginacionNomina;
using Miluc.Shared.Models.Response;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IEmpleadoService
    {
        Task<ResponseAPI<List<EmpleadoReaderDto>>> GetEmpleadosAsync(string? filtro = null, int page = 1, int? cantidad = null);
        Task<EmpleadoReaderDto> GetEmpleadoByIdAsync(int id);
        Task<EmpleadoReaderDto> CreateEmpleadosAsync(EmpleadoCreateDto dto);
        Task<EmpleadoReaderDto> UpdateEmpleadosAsync(EmpleadoUpdateDto updateDto);
        Task<bool> DeleteEmpleadoAsync(int id);
    }
}
