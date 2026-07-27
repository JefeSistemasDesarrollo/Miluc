using Miluc.Shared.DTOs.Nomina.MatrizSocioDemograficaDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IMatrizSociodemograficaClientService
    {
        Task<ResponseAPI<List<MatrizSociodemograficaReaderDto>>> GetMatrizSociodemograficaAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}
