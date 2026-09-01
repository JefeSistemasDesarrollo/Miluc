using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.NewFolder;
using Miluc.Shared.DTOs.Nomina.PaisDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Miluc.Server.Servicios.Nomina
{
    public class PaisService(NominaDbContext _context) : IPaisService
    {

        public async Task<List<PaisReaderDto>> GetPaisAsync()
        {
            try
            {


                var pais = await _context.Pais.AsNoTracking()
           .Select(p => new PaisReaderDto
           {
               PaisId = p.PaisId,
               Codigo = p.Codigo,
               pais = p.pais

           }).ToListAsync();


                return pais;

            }
            catch (Exception ex)

            {
                throw new Exception($"Error al obtener paises{ex.Message}");
            }




        }
    }
}

