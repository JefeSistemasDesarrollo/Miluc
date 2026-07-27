using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.DeporteRederDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class DeporteService(NominaDbContext _context) : IDeporteService
    {
        public  async  Task<List<DeporteRederDto>> GetDeporteAsync()
        {
            try
            {
                var deporte = await _context.Deporte.AsNoTracking()
                .Select(d => new DeporteRederDto
                {

                    DeporteId = d.DeporteId,
                    Nombre = d.Nombre,

                }).ToListAsync();
                return deporte;
              
                    




            }
            catch (Exception ex)
            {
                throw new Exception($"Error al encontrar Deportes{ex.Message}");

            }
        }
    }
}
