using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using Miluc.Shared.DTOs.Nomina.InformacionFamiliarDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IInfoFamiliarClientService
    {
        public Task<ResponseAPI<List<InfoFamiliarReaderDto>>> GetInfoFamiliaresAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        public Task<ResponseAPI<bool>> CreateFamiliarAsync(List<InfoFamiliarCreateDto> informacionFamiliar);
        public Task<ResponseAPI<InfoFamiliarReaderDto>> UpdateFamiliarAsync(InfoFamiliarUpdateDto empleado);
        public Task<ResponseAPI<List<InfoFamiliarReaderDto>>> GetFamiliarByIdAsync(int idEmpleado);
        public Task<ResponseAPI<bool>> DeleteFamiliarAsync(int id);
    }
}
