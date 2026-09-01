using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;
using Miluc.Shared.DTOs.Nomina.TipoContratoDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ITipoContratoClientService
    {
        Task<ResponseAPI<List<TipoContratoReaderDto>>> GetTipoContratoAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);

    }
}
