using Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IEsquemaVacunacionClientService
    {
        Task<ResponseAPI<List<EsquemaVacunacionReaderDto>>> GetEsquemaAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}
