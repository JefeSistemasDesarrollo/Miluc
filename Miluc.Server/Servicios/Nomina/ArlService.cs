using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.AfpDto;
using Miluc.Shared.DTOs.Nomina.Arl.Dto;
using Miluc.Shared.DTOs.Nomina.ArlDto;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;

namespace Miluc.Server.Servicios.Nomina
{

    public class ArlService(NominaDbContext _context) : IArlService

    {
        public async Task<ArlReaderDto> CreateArlAsync(ArlCreateDto arlCreateDto)
        {
            if (arlCreateDto == null)
                throw new ArgumentNullException(nameof(arlCreateDto), "El objeto ArlCreateDto no puede ser nulo.");
            try
            {
                var existeArl = await _context.Arl.AnyAsync
                    (a => a.Nombre.ToLower().Trim() == arlCreateDto.Nombre.ToLower().Trim());
                if (existeArl) throw new Exception("Ya existe una ARL con el mismo nombre.");

                // Validar si el código ya existe (solo si se proporciona un código)
                if (!string.IsNullOrEmpty(arlCreateDto.Codigo))
                {
                    var existeCodigo = await _context.Arl.AnyAsync
                        (a => a.Codigo.ToLower().Trim() == arlCreateDto.Codigo.ToLower().Trim());
                    if (existeCodigo) throw new Exception("Ya existe una ARL con el mismo código.");
                }



                var arl = new Arl
                {
                    Nombre = arlCreateDto.Nombre,
                    Codigo = arlCreateDto.Codigo,

                    FechaCreacion = DateTime.UtcNow,
                    Activo = arlCreateDto.Activo,
                };
                _context.Arl.Add(arl);
                await _context.SaveChangesAsync();
                return new ArlReaderDto
                {
                    ArlId = arl.ArlId,
                    Nombre = arl.Nombre,
                    Codigo = arl.Codigo,

                    FechaCreacion = arl.FechaCreacion,
                    Activo = arl.Activo
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<(List<ArlReaderDto> Data, int TotalRegistros)> GetArlAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {

            try
            {
                int cantidadtop = cantidad ?? 20;
                var query = _context.Arl.AsNoTracking().AsQueryable();


                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(a => a.Nombre.Contains(filtro) || a.Codigo.Contains(filtro));
                }


                int totalRegistros = await query.CountAsync();


                var cajaComp = await query
                    .Select(c => new ArlReaderDto
                    {
                        ArlId = c.ArlId,
                        Nombre = c.Nombre,
                        Codigo = c.Codigo,

                        FechaCreacion = c.FechaCreacion,
                        FechaActualizacion = c.FechaActualizacion,
                        Activo = c.Activo,
                    })
                    .Skip((page - 1) * cantidadtop)
                    .Take(cantidadtop)
                    .ToListAsync();


                return (cajaComp, totalRegistros);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }










        public async Task<ArlReaderDto> GetByArlAsync(int id)
        {
            try
            {
                var arl = await _context.Arl.AsNoTracking()
                    .Where(a => a.ArlId == id)
                    .Select(a => new ArlReaderDto
                    {
                        ArlId = a.ArlId,
                        Nombre = a.Nombre,
                        Codigo = a.Codigo,
                        FechaActualizacion = a.FechaActualizacion,
                        FechaCreacion = a.FechaCreacion,
                        Activo = a.Activo
                    })
                    .FirstOrDefaultAsync();

                return arl ?? throw new KeyNotFoundException($"ARL con ID {id} no encontrada.");
            }
            catch (KeyNotFoundException)
            {


                throw;
            }
            catch (Exception ex)
            {


                throw new InvalidOperationException($"Error al consultar la ARL con ID {id}.", ex);
            }
        }

        public async Task<ArlReaderDto> UpdateArlAsync(ArlUpdate arlUpdate)
        {
            try
            {
                var arl = await _context.Arl.FirstOrDefaultAsync(a => a.ArlId == arlUpdate.ArlId) ?? throw new Exception("ARL no existe");

                // VALIDACIÓN: Si la entidad está activa, pero intentan pasarla a inactiva (Activo = false)
                if (arl.Activo && !arlUpdate.Activo)
                {
                    //Busca en tabla AfiliacionSeguridadSocial si hay empleados  afiliados a esrta arl
                    bool tieneEmpleadosAfiliados = await _context.AfiliacionSeguridadSocial.AnyAsync(a => a.ArlId == arlUpdate.ArlId);
                    // si Tiene afiliaciones Lanza exepcion  ya que tiene afiliados
                    if (tieneEmpleadosAfiliados)
                    {
                        throw new Exception("La ARL no se puede inactivar porque tiene empleados afiliados.");
                    }
                }

                var nombreformateado = arlUpdate.Nombre?.Trim().ToUpper();
                var codigoFormateado = arlUpdate.Codigo?.Trim().ToUpper();

                var existeArl = await _context.Arl.AnyAsync(a =>
                    a.ArlId != arlUpdate.ArlId &&
                    (a.Nombre.ToUpper() == nombreformateado || a.Codigo.ToUpper() == codigoFormateado)
                );

                if (existeArl)
                    throw new Exception("Ya existe otra ARL con el mismo nombre o código.");

                arl.Nombre = arlUpdate.Nombre;
                arl.Codigo = arlUpdate.Codigo;
                arl.FechaActualizacion = DateTime.Now;
                arl.Activo = arlUpdate.Activo;

                await _context.SaveChangesAsync();

                return new ArlReaderDto
                {
                    ArlId = arl.ArlId,
                    Nombre = arl.Nombre,
                    Codigo = arl.Codigo,
                    FechaActualizacion = arl.FechaActualizacion,
                    Activo = arl.Activo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public async Task<bool> DeleteArlAsync(int id)
        {
            try
            {
                var arl = await _context.Arl.FirstOrDefaultAsync(e => e.ArlId == id) ?? throw new Exception("La Arl no existe.");

                // Validar si tiene afiliaciones activas o históricas vinculadas
                bool tieneEmpleadosAfiliados = await _context.AfiliacionSeguridadSocial
                    .AnyAsync(af => af.ArlId == id);

                if (tieneEmpleadosAfiliados)
                {
                    throw new Exception("La ARl no se puede eliminar porque cuenta con registros o empleados vinculados en el sistema.");
                }

                // Si no tiene registros vinculados, procedemos con seguridad
                _context.Arl.Remove(arl);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
