using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.AfiliacionSeguridadSocialDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;
using Miluc.Shared.DTOs.Nomina.Afp.Dto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EmpleadoDto;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata;

namespace Miluc.Server.Servicios.Nomina
{
    public class AfiliacionSeguridadSocialService(NominaDbContext _context) : IAfiliacionSeguridadSocialService
    {

        public async Task<(List<AfiliacionSeguridadSocialreaderDto> Data, int CantidadRegistros)> GetAfiliacionSeguridadSocialAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;

                var query = _context.Empleado.AsNoTracking().AsQueryable();

                query = AplicarFiltro(query, filtro);

                var totalRegistros = await query.CountAsync();

             
                var datosIntermedios = await query
                    .Skip((page - 1) * cantidadtop)
                    .Take(cantidadtop)
                    .Select(x => new
                    {
                        x.EmpleadoId,
                        x.PrimerNombre,
                        x.SegundoNombre,
                        x.PrimerApellido,
                        x.SegundoApellido,
                        x.Documento,


                        ActivoAfiliacion = (bool?)x.AfiliacionSeguridadSocial.Activo ?? false,
                        AfiliacionId = (int?)x.AfiliacionSeguridadSocial.AfiliacionId ?? 0,

                        EpsId = (int?)x.AfiliacionSeguridadSocial.EpsId,
                        NombreEps = x.AfiliacionSeguridadSocial.Eps.Nombre,

                        AfpId = (int?)x.AfiliacionSeguridadSocial.AfpId,
                        NombreAfp = x.AfiliacionSeguridadSocial.Afp.Nombre,

                        ArlId = (int?)x.AfiliacionSeguridadSocial.ArlId,
                        NombreArl = x.AfiliacionSeguridadSocial.Arl.Nombre,

                        CajaCompensacionId = (int?)x.AfiliacionSeguridadSocial.CajaCompensacionId,
                        NombreCajaCompensacion = x.AfiliacionSeguridadSocial.CajaCompensacion.Nombre
                    })
                    .ToListAsync();

                // 🔹 Mapeo final en memoria al DTO
                var afiliaciones = datosIntermedios.Select(x => new AfiliacionSeguridadSocialreaderDto
                {
                    AfiliacionId = x.AfiliacionId,
                    EmpleadoId = x.EmpleadoId,
                    NombreEmpleado = $"{x.PrimerNombre} {x.SegundoNombre} {x.PrimerApellido} {x.SegundoApellido}",
                    Documento = x.Documento,

                    EpsId = x.EpsId,
                    NombreEps = x.NombreEps ?? "Sin Asignar",

                    AfpId = x.AfpId,
                    NombreAfp = x.NombreAfp ?? "Sin Asignar",

                    ArlId = x.ArlId,
                    NombreArl = x.NombreArl ?? "Sin Asignar",

                    NombreCajaCompensacion = x.NombreCajaCompensacion ?? "Sin Asignar",

                    Activo = x.ActivoAfiliacion
                }).ToList();

                return (afiliaciones, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al obtener Afiliaciones a seguridad social: {ex.Message}", ex);
            }
        }

       
        private static IQueryable<Empleado> AplicarFiltro(IQueryable<Empleado> query, string? filtro)
        {
            if (string.IsNullOrEmpty(filtro))
                return query;

            

            // Pasa el filtro directamente sin modificar la columna ni la variable
            return query.Where(x => x.PrimerNombre.Contains(filtro)
                                 || x.PrimerApellido.Contains(filtro)
                                 || x.Documento.Contains(filtro));
        }
        public async Task<AfiliacionSeguridadSocialreaderDto> GetAfiliacionSeguridadSocialByIdAsync(int afiliacionId)
        {
            try
            {
                var afiliacion = await _context.AfiliacionSeguridadSocial.AsNoTracking()
                    .Include(a => a.Empleado)
                    .Include(a => a.Eps)
                    .Include(a => a.Afp)
                    .Include(a => a.Arl)
                    .Include(a => a.CajaCompensacion)
                    .Where(a => a.AfiliacionId == afiliacionId)
                    .Select(a => new AfiliacionSeguridadSocialreaderDto
                    {
                        AfiliacionId = a.AfiliacionId,

                        NombreEmpleado = $"{a.Empleado.PrimerNombre} {a.Empleado.SegundoNombre} {a.Empleado.PrimerApellido} {a.Empleado.SegundoApellido}".Replace("  ", " ").Trim(),
                        EmpleadoId = a.EmpleadoId,
                        EpsId = a.EpsId,
                        NombreEps = a.Eps != null ? a.Eps.Nombre : "Sin Asignar",
                        ArlId = a.ArlId,
                        NombreArl = a.Arl != null ? a.Arl.Nombre : "Sin Asignar",
                        AfpId = a.AfpId,
                        NombreAfp = a.Afp != null ? a.Afp.Nombre : "Sin Asignar",
                        CajaCompensacionId = a.CajaCompensacionId,
                        NombreCajaCompensacion = a.CajaCompensacion != null ? a.CajaCompensacion.Nombre : "Sin Asignar",
                        Activo = a.Activo,
                    }).FirstOrDefaultAsync();


                //  Para registros no encontrados usas KeyNotFoundException
                return afiliacion ?? throw new KeyNotFoundException("No se encontró la afiliación especificada.");
            }
            catch (Exception ex)
            {
                //InvalidOperationException Indica que ocurrió una operación que no se pudo realizar correctamente.
                throw new InvalidOperationException($"Error al obtener Afiliacion a seguridad social por ID: {ex.Message}", ex);
            }
        }





        public async Task<bool> UpsertAfiliacionAsync(UpdateAFiliacionDto dto)
        {
            try
            {
                //  Buscar si ya existe por EmpleadoId o por AfiliacionId válido
                var afiliacionExistente = await _context.AfiliacionSeguridadSocial
                    .FirstOrDefaultAsync(x => x.EmpleadoId == dto.EmpleadoId || (dto.AfiliacionId > 0 && x.AfiliacionId == dto.AfiliacionId));

                if (afiliacionExistente == null)
                {
                    // El empleado NO tenía afiliación -> Crear nuevo registro
                    var nuevaAfiliacion = new AfiliacionSeguridadSocial
                    {
                        EmpleadoId = dto.EmpleadoId,
                        EpsId = dto.EpsId ?? 0,
                        ArlId = dto.ArlId ?? 0,
                        AfpId = dto.AfpId ?? 0,
                        CajaCompensacionId = dto.CajaCompensacionId ?? 0,
                        Activo = dto.Activo
                    };

                    await _context.AfiliacionSeguridadSocial.AddAsync(nuevaAfiliacion);
                }
                else
                {
                    // El empleado YA tenía afiliación - Actualizar propiedades
                    
                    afiliacionExistente.EpsId = dto.EpsId ?? 0;
                    afiliacionExistente.ArlId = dto.ArlId ?? 0;
                    afiliacionExistente.AfpId = dto.AfpId ?? 0;
                    afiliacionExistente.CajaCompensacionId = dto.CajaCompensacionId ?? 0;
                    afiliacionExistente.Activo = dto.Activo;
                }

                // 2. Guardar cambios
                var filasAfectadas = await _context.SaveChangesAsync();

                // Si es un UPDATE y no cambiaron valores, SaveChangesAsync devuelve 0, pero la operación fue correcta.
                return filasAfectadas >= 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al procesar la afiliación a seguridad social: {ex.Message}", ex);
            }
        }
    }

    }

