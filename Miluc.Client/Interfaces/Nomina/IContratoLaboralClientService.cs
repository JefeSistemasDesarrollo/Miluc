using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IContratoLaboralClientService
    {
        Task<ResponseAPI<List<ContratoLabralreaderDto>>> GetContratoAsynk(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}
