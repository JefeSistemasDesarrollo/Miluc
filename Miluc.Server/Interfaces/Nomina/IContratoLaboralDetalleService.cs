using Miluc.Shared.DTOs.Nomina.ContratoLaboralDetalleDto;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface IContratoLaboralDetalleService
    {
        public Task<List<ContratoLaboralDetalleDto>> GetContratoLaboralDetallesAsync();
    }
}
