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




                        ActivoAfiliacion = x.AfiliacionSeguridadSocial != null ? x.AfiliacionSeguridadSocial.Activo : false,

                       
                        Afiliacion = x.AfiliacionSeguridadSocial != null ? new
                        {
                            x.AfiliacionSeguridadSocial.AfiliacionId,
                            x.AfiliacionSeguridadSocial.EpsId,
                            NombreEps = x.AfiliacionSeguridadSocial.Eps != null ? x.AfiliacionSeguridadSocial.Eps.Nombre : null,

                            x.AfiliacionSeguridadSocial.AfpId,
                            NombreAfp = x.AfiliacionSeguridadSocial.Afp != null ? x.AfiliacionSeguridadSocial.Afp.Nombre : null,

                            x.AfiliacionSeguridadSocial.ArlId,
                            NombreArl = x.AfiliacionSeguridadSocial.Arl != null ? x.AfiliacionSeguridadSocial.Arl.Nombre : null,

                            x.AfiliacionSeguridadSocial.CajaCompensacionId,
                            NombreCajaCompensacion = x.AfiliacionSeguridadSocial.CajaCompensacion != null ? x.AfiliacionSeguridadSocial.CajaCompensacion.Nombre : null
                        } : null
                    })
                    .ToListAsync();


                var afiliaciones = datosIntermedios.Select(x => new AfiliacionSeguridadSocialreaderDto
                {
                    AfiliacionId = x.Afiliacion?.AfiliacionId ?? 0,
                    EmpleadoId = x.EmpleadoId,
                    NombreEmpleado = $"{x.PrimerNombre} {x.SegundoNombre} {x.PrimerApellido} {x.SegundoApellido}",
                    Documento = x.Documento,

                    EpsId = x.Afiliacion?.EpsId,
                    NombreEps = x.Afiliacion?.NombreEps ?? "Sin Asignar",

                    AfpId = x.Afiliacion?.AfpId,
                    NombreAfp = x.Afiliacion?.NombreAfp ?? "Sin Asignar",

                    ArlId = x.Afiliacion?.ArlId,
                    NombreArl = x.Afiliacion?.NombreArl ?? "Sin Asignar",

                    CajaCompensacionId = x.Afiliacion?.CajaCompensacionId,
                    NombreCajaCompensacion = x.Afiliacion?.NombreCajaCompensacion ?? "Sin Asignar",


                    Activo = x.ActivoAfiliacion
                }).ToList();

                return (afiliaciones, totalRegistros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR CRÍTICO GET AFILIACIONES]: {ex}");
                throw new Exception($"Error al obtener Afiliaciones a seguridad social: {ex.Message}");
            }
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
                        NombreEmpleado = $"{a.Empleado.PrimerNombre} {a.Empleado.SegundoNombre ?? ""}".Trim() + $" {a.Empleado.PrimerApellido} {a.Empleado.SegundoApellido ?? ""}".Trim(),
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



                if (afiliacion == null) throw new Exception($"No se encontró la afiliación con ID ");


                return afiliacion;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener Afiliacion a seguridad social por ID: {ex.Message}");
            }
        }





        public Task<AfiliacionSeguridadSocialreaderDto> UpdateAfiliacionAsync(UpdateAFiliacionDto afiliacion)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpsertAfiliacionAsync(UpdateAFiliacionDto dto)
        {
            try
            {
                
                var afiliacionExistente = await _context.AfiliacionSeguridadSocial
                    .FirstOrDefaultAsync(x => x.AfiliacionId == (dto.AfiliacionId) || x.EmpleadoId == dto.EmpleadoId);

                if (afiliacionExistente == null)
                {
                    //  El empleado NO tenía afiliaciones 
                    var nuevaAfiliacion = new AfiliacionSeguridadSocial
                    {
                        EmpleadoId = dto.EmpleadoId,
                        // 0 para convertir de int? a int de forma segura
                        EpsId = dto.EpsId ?? 0,
                        ArlId = dto.ArlId ?? 0,
                        AfpId = dto.AfpId ?? 0,
                        CajaCompensacionId = dto.CajaCompensacionId ?? 0,
                        Activo = dto.Activo
                        
                    };

                    _context.AfiliacionSeguridadSocial.Add(nuevaAfiliacion);
                }
                else
                {
                   
                    afiliacionExistente.EpsId = dto.EpsId ?? 0;
                    afiliacionExistente.ArlId = dto.ArlId ?? 0;
                    afiliacionExistente.AfpId = dto.AfpId ?? 0;
                    afiliacionExistente.CajaCompensacionId = dto.CajaCompensacionId ?? 0;
                    afiliacionExistente.Activo = dto.Activo ;
                    _context.AfiliacionSeguridadSocial.Update(afiliacionExistente);
                }

                // Guardamos los cambios en la base de datos
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar la afiliación a seguridad social: {ex.Message}");
            }
        }

    }
}

