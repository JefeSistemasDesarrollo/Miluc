using Miluc.Shared.DTOs.Nomina.Cargos;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface ICargos
    {
        Task<ResponseAPI<List<CargosDto>>> GetCargosAsyc(string textoBusqueda, int paginaActual, int cantidadPorPagina);
    }
}