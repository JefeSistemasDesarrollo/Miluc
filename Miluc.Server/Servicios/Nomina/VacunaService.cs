using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.Vacunacion;

namespace Miluc.Server.Servicios.Nomina
{
    public class VacunaService (NominaDbContext _dbContext) : IVacunaService
    {
      
        public async Task<List<VacunaReaderDto>> GetVacunaAsync()
        {
            try
            {
                var vacunas = await _dbContext.Vacunas.AsNoTracking().
                    Select(v => 
                    new VacunaReaderDto 
                    { VacunaId = v.VacunaId,
                        VacunaName = v.VacunaName, 
                        })
                    .ToListAsync();
             

                return vacunas;
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error al obtener las vacunas {ex.Message}", ex);


            }
        }
    }
}
