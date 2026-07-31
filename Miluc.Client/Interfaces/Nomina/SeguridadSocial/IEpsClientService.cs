using Miluc.Shared.DTOs.Nomina.EpsDto;
namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
    using Miluc.Shared.DTOs.Nomina.EpsDto;
    using Miluc.Shared.Models.Response;

    public interface IEpsClientService
    {
        Task<ResponseAPI<List<EpsReaderDto>>> GetEpsAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<EpsReaderDto>> CreateEpsAsync(EpsCreateDto epsCreateDto);
        Task<ResponseAPI<EpsReaderDto>> GetByEpsAsync(int id);
        Task<ResponseAPI<EpsReaderDto>> UpdateEpsAsync(UpdateEpsDto updateEps);
    }
}