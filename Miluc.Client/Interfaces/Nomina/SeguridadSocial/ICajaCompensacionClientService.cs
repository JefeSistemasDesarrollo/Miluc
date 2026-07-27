using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina.SeguridadSocial
{
    public interface ICajaCompensacionClientService
    {
        public Task<ResponseAPI<List<CajaCompensacionReaderDto>>> GetCajaCompensacionAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}
