using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.Models.Response;

namespace TuProyecto.Client.Services
{
    public interface IContratoLaboralClientService
    {
        Task<ResponseAPI<List<ContratoLaboralreaderDto>>> GetContratoLaboralAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);

        Task<ResponseAPI<List<ContratoLaboralreaderDto>>> GetContratosPorEmpleadoAsync(int empleadoId);

        Task<ResponseAPI<ContratoLaboralreaderDto>> GetContratoByIdAsync(int id);

        Task<ResponseAPI<bool>> CreateContratoLaboralAsync(CreateContratoLaboralDto contratoLaboral);

        Task<ResponseAPI<ContratoLaboralreaderDto>> UpdateContratoLaboralAsync(UpdateContratoDto update);

        Task<ResponseAPI<bool>> InhabilitarContratoAsync(InhabilitarContratoDto dto);
    }
}