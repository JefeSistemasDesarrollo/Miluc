using Miluc.Shared.DTOs.Nomina.MedioTransporteDto;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface IMedioTransporteClientService
    {
        Task<ResponseAPI<List<MedioTransporteReaderDto>>> GetMedioTransporteAsync();
    }
}
