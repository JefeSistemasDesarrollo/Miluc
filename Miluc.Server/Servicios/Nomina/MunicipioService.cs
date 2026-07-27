using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.MunicipioDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class MunicipioService(NominaDbContext _context) : IMunicipioService
    {
        public async Task<List<MunicipioReaderDto>> GetMunicipiosAsync()
        {
            try
            {
                var municipio = await _context.Municipio.AsNoTracking()
                    .Include(d => d.Departamento)
                    .Select(m => new MunicipioReaderDto
                    {
                        Codigo = m.Codigo,
                        CodigoDpto = m.CodigoDpto,
                        Nombre = m.Nombre,
                        Activo = m.Activo,
                        NombreDepartamento= m.Departamento.Nombre
                   
                    }).ToListAsync();
                return municipio;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Municipios:{ex.Message}");
            }
        }
    }
}
