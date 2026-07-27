using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunbacionDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class EsquemaVacunacionService(NominaDbContext _context) : IEsquemaVacunacionService

    {
        public async Task<EsquemaVacunacionReaderDto> CreateEsquemaVacunacionAsync(EsquemaVacunacionCreateDto esquemaVacunacion)
        {
            throw new NotImplementedException();
        }



        public async Task<(List<EsquemaVacunacionReaderDto> data, int CantidadRegitros)> GetEsquemasVacunacionAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadTop = cantidad ?? 20;
                var queri = _context.EsquemaVacunacion.AsNoTracking().AsQueryable();
                if (!string.IsNullOrEmpty(filtro))
                {
                    queri = queri.Where(e => e.Empleado.PrimerNombre.Contains(filtro) || e.Empleado.PrimerApellido.Contains(filtro));
                }
                var totalRegistros = queri.Count();
                var esquema = await queri
                 .Include(v => v.Vacuna)
                .Include(e => e.Empleado)
                .Select(e => new EsquemaVacunacionReaderDto
                {
                    EsquemaVacunacionId = e.EsquemaVacunacionId,
                    EmpleadoId = e.EmpleadoId,
                    VacunaId = e.VacunaId,
                    VacunaName = e.Vacuna.VacunaName,
                    FechaVacuna = e.FechaVacuna,
                    FechaCreacion = DateTime.Now,
                    NombreEmpleado = $"{e.Empleado.PrimerNombre} {e.Empleado.SegundoNombre} {e.Empleado.PrimerApellido} {e.Empleado.SegundoApellido}",
                    //EmpleadoId = e.EmpleadoId,

                }).Skip((page - 1) * cantidadTop)
                    .Take(cantidadTop)
                    .ToListAsync();
                return (esquema, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener esuemas de vacunacion:{ex.Message}");
            }
        }
        public async Task<EsquemaVacunacionReaderDto> GetEsquemaVacunacionByIdAsync(int id)
        {
            var esquema = await _context.EsquemaVacunacion.AsNoTracking()
                .Include(e => e.Vacuna)
                .Include(e => e.Empleado)
                .Where(e => e.EsquemaVacunacionId == id)
                .Select(e => new EsquemaVacunacionReaderDto
                {
                    EsquemaVacunacionId = e.EsquemaVacunacionId,
                    EmpleadoId = e.EmpleadoId,
                    VacunaId = e.VacunaId,
                    VacunaName = e.Vacuna.VacunaName,
                    FechaVacuna = e.FechaVacuna,
                    FechaCreacion = DateTime.Now,
                    NombreEmpleado = $"{e.Empleado.PrimerNombre} {e.Empleado.SegundoNombre} {e.Empleado.PrimerApellido} {e.Empleado.SegundoApellido}",
                })
                .FirstOrDefaultAsync();

            return esquema;
        }
    }
}


