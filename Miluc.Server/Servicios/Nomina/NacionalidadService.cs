using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.NacionalidadDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class NacionalidadService(NominaDbContext _context) : INacionalidadService

    {
        public async Task<List<NacionalidadReaderDto>> GetNacionalidadesAsync()
        {
            try { 
                
                
                var nacionalidad = await _context.Nacionalidad.AsNoTracking() 
                .Select(n => new NacionalidadReaderDto
                {
                    NacionalidadId = n.NacionalidadId,
                    NacionalidadNombre = n.NacionalidadNombre.Trim().ToUpper(),
                    CodigoIso = n.CodigoIso
                }).OrderBy(n=>n.NacionalidadNombre)
                .ToListAsync();

                return nacionalidad;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener las nacionalidades: {ex.Message}");
            }

        }
    }
}
