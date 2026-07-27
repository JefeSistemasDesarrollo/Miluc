using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.ParentescoDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class ParentescoService(NominaDbContext _context) : IParentescoService
    {


        public async Task<List<ParentescoReaderDto>> GetAllParentescosAsync()
        {
            try
            { var  parenntescos = await _context.Parentesco.AsNoTracking()
                    .Select(p => new ParentescoReaderDto
                    {
                        ParentescoId = p.ParentescoId,
                        NombreParentesco = p.NombreParentesco,
                        
                    }).ToListAsync();
                return parenntescos;


            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los parentescos: {ex.Message}", ex);
            }
        }
    }
}
