using Miluc.Shared.DTOs.Nomina.NivelAcademico;
using Miluc.Shared.Models.Response;

namespace Miluc.Client.Interfaces.Nomina
{
    public interface INivelAcademicoClientService
    {
        Task<ResponseAPI<List<NivelAcademicoReaderDto>>> GetNivelAcademicoAsync();
    }
}
