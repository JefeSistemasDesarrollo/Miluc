using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;
using Miluc.Shared.DTOs.Nomina.Vacunacion;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IContratoLaboralDetalleClientService
    {
        Task<ResponseAPI<List<ContratoLaboralDetalleDto>>> GetContratoLaboralDetallesAsync();

    }
}
