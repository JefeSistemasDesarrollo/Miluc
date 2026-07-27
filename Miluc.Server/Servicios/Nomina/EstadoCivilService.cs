using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EstadoCivilDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class EstadoCivilService (NominaDbContext dbcontex): IEstadoCivilService
    {
        public async Task<List<EstadoCivilReaderDto>> GetAllEstadosCivilesAsync()
        {
            try
            {
                var estadosCiviles = await dbcontex.EstadoCivil.AsTracking().Select(e => new EstadoCivilReaderDto
                {
                    EstadoCivilId = e.EstadoCivilId,
                    Nombre = e.Nombre
                }).ToListAsync(); 
                
                

                return estadosCiviles;

            }catch (Exception ex)
            {
                throw new Exception($"Error al obtener los estados civiles: {ex.Message}");
            }
        }
    }
}
