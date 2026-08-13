using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.ContratoLaboralDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IContratoLaboralClientService
    {
        Task<ResponseAPI<List<ContratoLabralreaderDto>>> GetContratoLaboralAsync(string textoBusqueda, int paginaActual, int cantidadPorPagina);
        Task<ResponseAPI<ContratoLabralreaderDto>> UpdateContratoLaboralAsync(UpdateContratoLaboralDto contrato);
        Task<ResponseAPI<ContratoLabralreaderDto>> GetContratoLaboralByIdAsync  (int id);
    }
}
