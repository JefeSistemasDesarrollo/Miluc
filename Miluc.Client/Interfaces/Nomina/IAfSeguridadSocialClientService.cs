using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IAfSeguridadSocialClientService
    {
        Task<ResponseAPI<List<AfiliacionSeguridadSocialreaderDto>>> GetAfSeguridadSocialAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<AfiliacionSeguridadSocialreaderDto>> UpdateAfiliacionAsync(UpdateAFiliacionDto afiliacion);
        Task<ResponseAPI<AfiliacionSeguridadSocialreaderDto>> GetAfiliacionSeguridadSocialByIdAsync(int id);
    }
}
