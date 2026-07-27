using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.PaisDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Miluc.Server.Servicios.Nomina
{
    public class PaisService(NominaDbContext _context) : IPaisService
    {

        public async Task<(List<PaisReaderDto> data, int CantidadRegistros)> GetPaisAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {

                int cantidadTop = cantidad ?? 20;


                var query = _context.Pais.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {

                    query = query.Where(p => p.pais.Contains(filtro));


                }

                int totalCount = await query.CountAsync();

                var dataPais = await query.OrderBy(p => p.pais)
                                .Skip((page - 1) * cantidadTop)
                                .Take(cantidadTop)
                                .Select(u => new PaisReaderDto
                                {
                                    PaisId = u.PaisId,
                                    Codigo = u.Codigo,
                                    pais = u.pais

                                }).ToListAsync();


                return (dataPais, totalCount);

            }
            catch (Exception ex)

            {
                throw new Exception($"Error al obtener paises{ex.Message}");
            }




        }
    }
}

