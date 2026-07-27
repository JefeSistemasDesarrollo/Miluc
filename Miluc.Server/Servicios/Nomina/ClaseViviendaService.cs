using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ClaseVivienda;

namespace Miluc.Server.Servicios.Nomina
{
    public class ClaseViviendaService(NominaDbContext _context) : IClaseViviendaService
    {
        public async Task<List<ClaseViviendaReaderDto>> GetClaseViviendaAsync()
        {
            try
            {
                var clase = await _context.ClaseVivienda.AsNoTracking()
                    .Select(c => new ClaseViviendaReaderDto
                    {
                        ClaseDeViviendaId = c.ClaseViviendaId,
                        Nombre = c.Nombre,

                    }).ToListAsync();
                return clase;
            }
            catch (Exception ex)
            {

                throw new Exception("No se encontraron Clase Vivienda ", ex);
            }
        }
    }
}
