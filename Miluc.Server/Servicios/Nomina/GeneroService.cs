using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.GeneroDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class GeneroService(NominaDbContext _context) : IGeneroService
    {
        public async Task<List<GeneroReaderDto>> GetGeneroAsync()
        {
            try
            {
                var genero = await _context.Genero.AsNoTracking()
                    .Select(g => new GeneroReaderDto

                    {
                        GeneroId = g.GeneroId,
                        Nombre = g.Nombre,
                    }).ToListAsync();
                return genero;

            }

            catch (Exception ex)
            {
                throw new Exception($"Error al Encontrar Generos{ex.Message}");
            }
        }
    }
}
