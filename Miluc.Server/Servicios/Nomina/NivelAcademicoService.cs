using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.NivelAcademico;

namespace Miluc.Server.Servicios.Nomina
{
    public class NivelAcademicoService(NominaDbContext _context) : INivelAcademicoService
    {
        public async Task<List<NivelAcademicoReaderDto>> GetNivelAcademicosAsync()
        {
            try {
                var nivel = await _context.NivelAcademico.AsNoTracking()
                    .Select(n => new NivelAcademicoReaderDto
                    {
                        NivelAcademicoId = n.NivelAcademicoId,
                         Nombre = n.Nombre,

                    }).ToListAsync();
                return nivel;
            }
            
            catch (Exception ex)
            
            
            {
                throw new Exception("Error al obtener nivel academico",ex);
            }
        }
    }
}
