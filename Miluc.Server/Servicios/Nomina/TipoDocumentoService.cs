using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.TipoDocumento;

namespace Miluc.Server.Servicios.Nomina
{
    public class TipoDocumentoService(NominaDbContext _context) : ITipoDocumentoService
    {
        public async Task<List<TipoDocumentoReaderDto>> GetAllTipoDocumento()
        {
            var tipoDocumento = await _context.TipoDocumento.AsNoTracking()
                .Include(e => e.Empleado)
                .Select(t => new TipoDocumentoReaderDto
                {
                    TipoDocumentoId = t.TipoDocumentoId,
                    Codigo = t.Codigo,
                    Nombre = t.Nombre,




                }) .ToListAsync();

            return tipoDocumento;
        }
    } }
