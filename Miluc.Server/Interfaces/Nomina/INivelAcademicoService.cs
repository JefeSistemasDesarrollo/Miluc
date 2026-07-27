using Miluc.Shared.DTOs.Nomina.NivelAcademico;

namespace Miluc.Server.Interfaces.Nomina
{
    public interface INivelAcademicoService
    {
        public Task<List<NivelAcademicoReaderDto>> GetNivelAcademicosAsync();
    }
}
