using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.TipoVivienda;

namespace Miluc.Server.Servicios.Nomina
{
    public class TipoViviendaService(NominaDbContext _context) : ITipoViviendaService
    {
        public  async Task<List<TipoViviendaReaderDto>> GetTipoViviendaAsync()
        {
            try
            {
                var vivienda = await _context.TipoVivienda.AsNoTracking()
                   .Select(v => new TipoViviendaReaderDto
                   {
                       TipoViviendaId = v.TipoViviendaId,
                       Nombre = v.Nombre,

                   }).ToListAsync();
                return vivienda;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener Tipos de vivienda", ex);
            }
        }
    }
}
