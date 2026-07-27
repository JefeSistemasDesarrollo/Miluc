using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.CondicionMedica;

namespace Miluc.Server.Servicios.Nomina
{
    public class CondicionMedicaService(NominaDbContext _context) : ICondicionMedicaService
    {
        public async Task<List<CondicionMedicaReaderDto>> GetCondicionMedicaAsync()
        {
            try
            {
                var condicion = await _context.CondicionMedica.AsNoTracking()
                    .Select(c => new CondicionMedicaReaderDto
                    {

                        CondicionMedicaId = c.CondicionMedicaId,
                        Nombre = c.Nombre
                    }).ToListAsync();
                return condicion;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener  Condiciones medicas", ex);
            }
        } 
    }
}
