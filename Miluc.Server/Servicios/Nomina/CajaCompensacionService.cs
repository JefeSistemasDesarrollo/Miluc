using Microsoft.EntityFrameworkCore;
using Miluc.Server.Data;
using Miluc.Server.Interfaces.Nomina;
using Miluc.Server.Models.Nomina;
using Miluc.Shared.DTOs.Nomina.CajaCompensacionDto;
using Miluc.Shared.DTOs.Nomina.EpsDto;

namespace Miluc.Server.Servicios.Nomina
{
    public class CajaCompensacionService(NominaDbContext _context) : ICajaCompensacionService
    {
        public async Task<CajaCompensacionReaderDto> CajaUpdate(CajaUpdate cajaUpdate)
        {
            try
            {

                var caja = await _context.CajaCompensacion.FirstOrDefaultAsync(e => e.CajaCompensacionId == cajaUpdate.CajaCompensacionId);

                if (caja == null)
                    throw new Exception("La EPS no existe.");


                var nombreFormateado = cajaUpdate.Nombre?.Trim().ToUpper();
                var codigoFormateado = cajaUpdate.Codigo?.Trim().ToUpper();


                var existeCaja = await _context.CajaCompensacion.AnyAsync(e =>
                    e.Codigo!= cajaUpdate.Codigo &&
                    (e.Nombre.ToUpper() == nombreFormateado || e.Codigo.ToUpper() == codigoFormateado)
                );

                if (existeCaja)
                    throw new Exception("Ya existe otra EPS con el mismo nombre o código.");


                caja.Nombre = cajaUpdate.Nombre;
                caja.Codigo = cajaUpdate.Codigo;
                caja.FechaActualizacion = DateTime.Now;
                caja.Activo = cajaUpdate.Activo;


                await _context.SaveChangesAsync();


                return new CajaCompensacionReaderDto
                {
                    CajaCompensacionId = caja.CajaCompensacionId,
                    Nombre = caja.Nombre,
                    Codigo = caja.Codigo,
                    FechaActualizacion = caja.FechaActualizacion,
                    Activo = caja.Activo
                };

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar caja: {ex.Message}");


            }
        }

        public async Task<CajaCompensacionReaderDto> CreateCajaAsync(CajaCreateDto cajaCompensacion)
        {
            if (cajaCompensacion == null)
                throw new ArgumentException("El objeto cajaCompensacion no puede ser nulo.", nameof(cajaCompensacion));

            try
            {
                var existeCaja = await _context.CajaCompensacion.AnyAsync(c => c.Nombre == cajaCompensacion.Nombre || c.Codigo == cajaCompensacion.Codigo);
                if (existeCaja) throw new Exception("Ya existe una caja de compensación con el mismo nombre o código.");

                var nuevaCaja = new CajaCompensacion
                {
                    Nombre = cajaCompensacion.Nombre,
                    Codigo = cajaCompensacion.Codigo,

                    FechaCreacion = DateTime.UtcNow,
                    Activo = cajaCompensacion.Activo
                };
                _context.CajaCompensacion.Add(nuevaCaja);
                await _context.SaveChangesAsync();
                return new CajaCompensacionReaderDto
                {
                    CajaCompensacionId = nuevaCaja.CajaCompensacionId,
                    Nombre = nuevaCaja.Nombre,
                    Codigo = nuevaCaja.Codigo,

                    FechaCreacion = nuevaCaja.FechaCreacion,
                    Activo = nuevaCaja.Activo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear Caja Compensacion: {ex.Message}");
            }
        }

        public async Task<CajaCompensacionReaderDto> GetBycajaAsync(int id)
        {
            try
            {
                var caja = await _context.CajaCompensacion.AsNoTracking()
                    .Where(e => e.CajaCompensacionId == id)
                    .Select(e => new CajaCompensacionReaderDto
                    {
                        CajaCompensacionId = e.CajaCompensacionId,
                        Nombre = e.Nombre,
                        Codigo = e.Codigo,
                        FechaActualizacion = DateTime.Now,
                        FechaCreacion = DateTime.Now,
                        Activo = e.Activo

                    }).FirstOrDefaultAsync();
                if (caja == null) throw new Exception("Eps no encontrado.");

                return caja;
            }



            catch (Exception ex)
            {
                throw new Exception("Error al  obtener  cajas");




            } 
        }

        public async Task<(List<CajaCompensacionReaderDto> Data, int TotalRegistros)> GetCajaCompensacionAsync(string? filtro = null, int page = 1, int? cantidad = null)
        {
            try
            {
                int cantidadtop = cantidad ?? 20;
                var query = _context.CajaCompensacion.AsNoTracking().AsQueryable();


                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query = query.Where(a => a.Nombre.Contains(filtro) || a.Codigo.Contains(filtro));
                }


                int totalRegistros = await query.CountAsync();


                var cajaComp = await query
                    .Select(c => new CajaCompensacionReaderDto
                    {
                        CajaCompensacionId = c.CajaCompensacionId,
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
                throw new Exception($"Error Al obtener Caja Compensacion: {ex.Message}");
            }
        }
    }
}

