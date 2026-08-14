using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.EsquemaVacunacionDto;


namespace Miluc.Server.Servicios.Nomina
{
    public class EsquemaDeVacunacionService(NominaDbContext _context) : IEsquemaVacunacionService
    {
        public async Task<bool> CreateEsquemaVacunacionAsync(List<CreateEsquemaVacunacionDto> esquemaVacunacion)
        {
            if (esquemaVacunacion == null || !esquemaVacunacion.Any())
                return false;

            //  Validar que la petición corresponda a un solo empleado
            var empleadoIds = esquemaVacunacion.Select(x => x.EmpleadoId).Distinct().ToList();
            if (empleadoIds.Count > 1)
            {
                throw new ArgumentException("El listado recibido contiene registros de múltiples empleados.");
            }

            var empleadoId = empleadoIds.First();

            // 2. Extraer los IDs de vacunas sin duplicados
            var vacunasNuevasIds = esquemaVacunacion.Select(x => x.VacunaId).Distinct().ToList();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Buscar e inactivar los registros anteriores para este empleado y estas vacunas
                var vacunasAnteriores = await _context.EsquemaVacunacion
                    .Where(x => x.EmpleadoId == empleadoId
                             && vacunasNuevasIds.Contains(x.VacunaId)
                             && (x.Activo == true || x.Activo == null))
                    .ToListAsync();

                if (vacunasAnteriores.Any())
                {
                    foreach (var anterior in vacunasAnteriores)
                    {
                        anterior.Activo = false;
                    }
                }

                //  Mapear y agregar los nuevos registros como ACTIVOS
                var nuevosRegistros = esquemaVacunacion.Select(vac => new EsquemaVacunacion
                {
                    EmpleadoId = vac.EmpleadoId,
                    VacunaId = vac.VacunaId,
                    FechaVacuna = vac.FechaVacuna ?? DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                }).ToList();

                _context.EsquemaVacunacion.AddRange(nuevosRegistros);

                //  Guardar todo en la BD en una sola transacción
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error al procesar el esquema de vacunación: {ex.Message}", ex);
            }
        }

        public async Task<(List<EsquemaVacunacionReaderDto> Data, int CantidadRegistros)> GetEsquemasVacunacionAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;

                var query = _context.Empleado.AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(filtro))
                {
                    string filtroMinuscula = filtro.Trim().ToLower();
                    query = query.Where(x => x.PrimerNombre.ToLower().Contains(filtroMinuscula)
                                          || x.PrimerApellido.ToLower().Contains(filtroMinuscula)
                                          || x.Documento.ToLower().Contains(filtroMinuscula));
                }

                var totalRegistros = await query.CountAsync();

                var datosIntermedios = await query
                    .Skip((page - 1) * cantidadtop)
                    .Take(cantidadtop)
                    .Select(x => new
                    {
                        EmpleadoId = x.EmpleadoId,
                        PrimerNombre = x.PrimerNombre,
                        SegundoNombre = x.SegundoNombre,
                        PrimerApellido = x.PrimerApellido,
                        SegundoApellido = x.SegundoApellido,
                        Documento = x.Documento,

                        Esquema = x.EsquemaVacunacion
                            .Where(e => e.Activo != false)
                            .Select(e => new
                            {
                                e.EsquemaVacunacionId,
                                e.VacunaId,
                                NombreVacuna = e.Vacuna != null ? e.Vacuna.VacunaName : null,
                                e.FechaVacuna,
                                e.FechaCreacion,
                                
                                Activo = (bool?)e.Activo
                            })
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                var esquemas = datosIntermedios.Select(x => new EsquemaVacunacionReaderDto
                {
                    EsquemaVacunacionId = x.Esquema?.EsquemaVacunacionId ?? 0,
                    EmpleadoId = x.EmpleadoId,
                    NombreEmpleado = $"{x.PrimerNombre} {x.SegundoNombre} {x.PrimerApellido} {x.SegundoApellido}".Replace("  ", " ").Trim(),
                    Documento = x.Documento,

                    VacunaId = x.Esquema?.VacunaId,
                    NombreVacuna = x.Esquema?.NombreVacuna ?? "Sin Asignar",
                    FechaVacuna = x.Esquema?.FechaVacuna,
                    FechaCreacion = x.Esquema?.FechaCreacion,

                    Activo = x.Esquema != null ? (x.Esquema.Activo ?? true) : false
                }).ToList();

                return (esquemas, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener esquemas de vacunación: {ex.Message}", ex);
            }
        }

        public async Task<List<EsquemaVacunacionReaderDto>> GetEsquemaVacunacionByIdAsync(int id)
        {
            try
            {
                var esquema = await _context.EsquemaVacunacion
                    .AsNoTracking()
                    .IgnoreQueryFilters() 
                    .Include(e => e.Empleado)
                    .Include(e => e.Vacuna)
                    .Where(e => e.EmpleadoId == id)
                    .Select(e => new EsquemaVacunacionReaderDto
                    {
                        EsquemaVacunacionId = e.EsquemaVacunacionId,
                        EmpleadoId = e.EmpleadoId,
                        NombreEmpleado = $"{e.Empleado.PrimerNombre} {e.Empleado.SegundoNombre} {e.Empleado.PrimerApellido} {e.Empleado.SegundoApellido}".Replace("  ", " ").Trim(),
                        Documento = e.Empleado.Documento,
                        VacunaId = e.VacunaId,
                        NombreVacuna = e.Vacuna != null ? e.Vacuna.VacunaName : "Sin Especificar",
                        FechaVacuna = e.FechaVacuna,
                        FechaCreacion = e.FechaCreacion,
                        Activo = e.Activo
                    })
                    .ToListAsync();

                return esquema;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la información de esquemas de vacunación: {ex.Message}");
            }
        }
    }
    }
